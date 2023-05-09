using System.Linq;
using Construct.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Construct.Systems {
    public sealed class SingulaSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _inHandSingulafilter;
        private readonly EcsFilter _possibleJoinFilter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<Conventus> _conventusPool;
        private readonly EcsPool<PossibleJoin> _possibleJoinPool;

        private readonly Material _greenTransparent;

        private const float nearDistance = 0.75f;

        private NearestJoin _nearestJoin = new();
        private NearestJoin? _oldNearestJoin = null;

        public SingulaSystem(EcsWorld world) {
            _world = world;
            _inHandSingulafilter = _world.Filter<Singula>().Inc<InHand>().End();
            _possibleJoinFilter = _world.Filter<Singula>().Inc<PossibleJoin>().End();
            _singulaPool = _world.GetPool<Singula>();
            _conventusPool = _world.GetPool<Conventus>();
            _possibleJoinPool = _world.GetPool<PossibleJoin>();

            _greenTransparent = Resources.Load<Material>($"Materials/GreenTransparent");
        }

        public void Run(IEcsSystems systems) {
            foreach (var entity in _inHandSingulafilter) {
                ref var singula = ref _singulaPool.Get(entity);
                var singulaTransform = singula.SingulaView.GetComponent<Transform>();

                _nearestJoin.Distance = float.MaxValue;
                var joinPositions = singula.SingulaView.Joins.ToDictionary(
                    kv => kv.Key,
                    kv => singulaTransform.TransformPoint(kv.Value.Position)
                );

                foreach (var possibleJoinEntity in _possibleJoinFilter) {
                    ref var possibleJoinSingula = ref _singulaPool.Get(possibleJoinEntity);
                    ref var possibleJoin = ref _possibleJoinPool.Get(possibleJoinEntity);
                    var possibleJoinSingulaTransform = possibleJoinSingula.SingulaView.GetComponent<Transform>();

                    foreach (var kv in possibleJoin.JoinPairs) {
                        var distance = Vector3.Distance(
                            joinPositions[kv.Value],
                            possibleJoinSingulaTransform.TransformPoint(possibleJoinSingula.SingulaView.Joins[kv.Key].Position)
                        );

                        if (distance <= _nearestJoin.Distance) {
                            _nearestJoin.Distance = distance;
                            _nearestJoin.JoinId = kv.Value;
                            _nearestJoin.NearJoinId = kv.Key;
                            _nearestJoin.NearEcsEntity = possibleJoinEntity;
                        }
                    }
                }

                if (_oldNearestJoin.HasValue 
                    && (_nearestJoin.NearJoinId != _oldNearestJoin.Value.NearJoinId || _nearestJoin.Distance > nearDistance)) {
                    ref var oldPossibleJoin = ref _possibleJoinPool.Get(_oldNearestJoin.Value.NearEcsEntity);
                    oldPossibleJoin.JoinIdSingulaFrame = -1;
                    GameObject.Destroy(oldPossibleJoin.SingulaFrame);

                    _oldNearestJoin = null;
                }

                // Если расстояние меньше заданного и после предыдущего условия стаорое ближайшее соединение
                // не заданно, то создаем вспомагательную модель для пользователя, привязывая её к
                // текущей ближайшей детали.
                if (_nearestJoin.Distance <= nearDistance && !_oldNearestJoin.HasValue) {
                    ref var possibleJoin = ref _possibleJoinPool.Get(_nearestJoin.NearEcsEntity);
                    ref var possibleJoinSingula = ref _singulaPool.Get(_nearestJoin.NearEcsEntity);
                    var singulaFrameObject = new GameObject("Singla Frame");
                    singulaFrameObject.AddComponent<MeshRenderer>().material = _greenTransparent;
                    singulaFrameObject.AddComponent<MeshFilter>().mesh = singula.SingulaView.GetComponent<MeshFilter>().mesh;

                    var singulaFrameTransform = singulaFrameObject.GetComponent<Transform>();
                    var possibleJoinSingulaTransform = possibleJoinSingula.SingulaView.GetComponent<Transform>();

                    singulaFrameTransform.rotation = possibleJoinSingulaTransform.rotation;
                    singulaFrameTransform.position = possibleJoinSingulaTransform.TransformPoint(
                        possibleJoinSingula.SingulaView.Joins[_nearestJoin.NearJoinId].Position 
                        - singula.SingulaView.Joins[_nearestJoin.JoinId].Position);

                    singulaFrameTransform.SetParent(possibleJoinSingulaTransform);

                    possibleJoin.SingulaFrame = singulaFrameObject;
                    possibleJoin.JoinIdSingulaFrame = _nearestJoin.NearJoinId;
                    _oldNearestJoin = _nearestJoin;
                }
            }
        }
    }

    internal struct NearestJoin {
        public int JoinId;
        public int NearJoinId;
        public int NearEcsEntity;
        public float Distance;
    }
}