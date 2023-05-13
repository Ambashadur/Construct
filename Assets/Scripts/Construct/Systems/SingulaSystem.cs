using System.Linq;
using Construct.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Construct.Systems
{
    public sealed class SingulaSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter _inHandSingulafilter;
        private readonly EcsFilter _possibleJoinFilter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<PossibleJoin> _possibleJoinPool;
        private readonly EcsPool<InHand> _inHandPool;

        private readonly Material _greenTransparent;

        private const float nearDistance = 0.75f;

        private NearestPimple _nearestJoin = new();
        private NearestPimple? _oldNearestJoin = null;

        public SingulaSystem(EcsWorld world)
        {
            _world = world;
            _inHandSingulafilter = _world.Filter<Singula>().Inc<InHand>().End();
            _possibleJoinFilter = _world.Filter<Singula>().Inc<PossibleJoin>().End();
            _singulaPool = _world.GetPool<Singula>();
            _possibleJoinPool = _world.GetPool<PossibleJoin>();
            _inHandPool = _world.GetPool<InHand>();

            _greenTransparent = Resources.Load<Material>($"Materials/GreenTransparent");
        }

        public void Run(IEcsSystems systems)
        {
            if (_inHandSingulafilter.GetEntitiesCount() == 0) {
                _oldNearestJoin = null;
                return;
            }

            foreach (var entity in _inHandSingulafilter) {
                ref var singula = ref _singulaPool.Get(entity);
                ref var inHand = ref _inHandPool.Get(entity);
                var singulaTransform = singula.SingulaView.GetComponent<Transform>();

                _nearestJoin.Distance = float.MaxValue;
                _nearestJoin.NearEcsEntity = -1;
                _nearestJoin.NearPimpleId = -1;
                _nearestJoin.NearJoinId = -1;
                _nearestJoin.PimpleId = -1;

                var pimplePositions = singula.Pimples.ToDictionary(
                    kv => kv.Key,
                    kv => singulaTransform.TransformPoint(kv.Value.Position));

                foreach (var possibleJoinEntity in _possibleJoinFilter) {
                    ref var possibleJoinSingula = ref _singulaPool.Get(possibleJoinEntity);
                    ref var possibleJoin = ref _possibleJoinPool.Get(possibleJoinEntity);
                    var possibleJoinSingulaTransform = possibleJoinSingula.SingulaView.GetComponent<Transform>();

                    foreach (var kv in possibleJoin.PimplePairs) {
                        var distance = Vector3.Distance(
                            pimplePositions[kv.Value],
                            possibleJoinSingulaTransform.TransformPoint(possibleJoinSingula.Pimples[kv.Key].Position)
                        );

                        if (possibleJoinSingula.Pimples[kv.Key].JoinId != _nearestJoin.NearJoinId 
                            && distance <= _nearestJoin.Distance) {
                            _nearestJoin.Distance = distance;
                            _nearestJoin.PimpleId = kv.Value;
                            _nearestJoin.NearPimpleId = kv.Key;
                            _nearestJoin.NearJoinId = possibleJoinSingula.Pimples[kv.Key].JoinId;
                            _nearestJoin.NearEcsEntity = possibleJoinEntity;
                        }
                    }
                }

                if (_oldNearestJoin.HasValue 
                    && (_nearestJoin.NearJoinId != _oldNearestJoin.Value.NearJoinId || _nearestJoin.Distance > nearDistance)) {
                    ref var oldPossibleJoin = ref _possibleJoinPool.Get(_oldNearestJoin.Value.NearEcsEntity);
                    oldPossibleJoin.PimpleIdSingulaFrame = -1;
                    GameObject.Destroy(oldPossibleJoin.SingulaFrame);

                    inHand.PossibleJoinEcsEntity = -1;
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
                        possibleJoinSingula.Pimples[_nearestJoin.NearPimpleId].Position 
                        - singula.Pimples[_nearestJoin.PimpleId].Position);

                    singulaFrameTransform.SetParent(possibleJoinSingulaTransform);

                    possibleJoin.SingulaFrame = singulaFrameObject;
                    possibleJoin.PimpleIdSingulaFrame = _nearestJoin.NearPimpleId;
                    inHand.PossibleJoinEcsEntity = _nearestJoin.NearEcsEntity;
                    _oldNearestJoin = _nearestJoin;
                }
            }
        }
    }

    internal struct NearestPimple
    {
        public int PimpleId;
        public int NearPimpleId;
        public int NearJoinId;
        public int NearEcsEntity;
        public float Distance;
    }
}