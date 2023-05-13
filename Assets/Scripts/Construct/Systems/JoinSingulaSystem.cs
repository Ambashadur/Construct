using System.Linq;
using Construct.Components;
using Construct.Model;
using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;

namespace Construct.Systems
{
    sealed class JoinSingulaSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter _joinSingulaFilter;
        private readonly EcsPool<JoinSingula> _joinSingulaPool;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<InHand> _inHandPool;
        private readonly EcsPool<PossibleJoin> _possibleJoinPool;
        private readonly EcsPool<Conventus> _conventusPool;
        private readonly EcsPool<ReleaseFromHand> _releaseFromHandPool;

        public JoinSingulaSystem(EcsWorld world)
        {
            _world = world;
            _joinSingulaFilter = _world.Filter<JoinSingula>().Inc<Singula>().Inc<InHand>().End();
            _joinSingulaPool = _world.GetPool<JoinSingula>();
            _singulaPool = _world.GetPool<Singula>();
            _inHandPool = _world.GetPool<InHand>();
            _possibleJoinPool = _world.GetPool<PossibleJoin>();
            _conventusPool = _world.GetPool<Conventus>();
            _releaseFromHandPool = _world.GetPool<ReleaseFromHand>();
        }

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _joinSingulaFilter) {
                ref var inHand = ref _inHandPool.Get(entity);

                if (inHand.PossibleJoinEcsEntity == -1) continue;

                ref var singula = ref _singulaPool.Get(entity);
                ref var possibleJoin = ref _possibleJoinPool.Get(inHand.PossibleJoinEcsEntity);
                ref var possibleJoinSingula = ref _singulaPool.Get(inHand.PossibleJoinEcsEntity);
                ref var conventus = ref _conventusPool.Get(singula.ConventusEcsEntity);

                singula.SingulaView.transform.position = possibleJoin.SingulaFrame.transform.position;
                singula.SingulaView.transform.rotation = possibleJoin.SingulaFrame.transform.rotation;

                var leftPimpleId = possibleJoin.PimpleIdSingulaFrame;
                var rightPimpleId = possibleJoin.PimplePairs[leftPimpleId];

                var leftJoin = conventus.Joins[possibleJoinSingula.Pimples[leftPimpleId].JoinId];
                var rightJoin = conventus.Joins[singula.Pimples[rightPimpleId].JoinId];

                var nextJoinId = leftJoin.NextJoinIds.Intersect(rightJoin.NextJoinIds).First();
                var nextJoin = conventus.Joins[nextJoinId];

                possibleJoinSingula.Pimples[leftPimpleId].JoinId = nextJoinId;
                singula.Pimples[rightPimpleId].JoinId = nextJoinId;

                // Собираем все точки соединений из левого соединения (не то что в руке)
                if (nextJoin.LeftPimples.Count == 0) {
                    if (leftJoin.LeftJoinId == 0 && leftJoin.RightJoinId == 0) {
                        nextJoin.LeftPimples.Add(new SingulaJoin() {
                            SingulaId = possibleJoinSingula.Id,
                            PimpleId = leftPimpleId
                        });
                    } else {
                        nextJoin.LeftPimples.AddRange(leftJoin.LeftPimples);
                        nextJoin.LeftPimples.AddRange(leftJoin.RightPimples);
                    }
                }

                // Собираем все точки соединений из правого соединения (то что в руке)
                if (nextJoin.RightPimples.Count == 0) {
                    if (rightJoin.LeftJoinId == 0 && rightJoin.RightJoinId == 0) {
                        nextJoin.RightPimples.Add(new SingulaJoin() {
                            SingulaId = singula.Id,
                            PimpleId = rightPimpleId
                        });
                    } else {
                        nextJoin.RightPimples.AddRange(rightJoin.LeftPimples);
                        nextJoin.RightPimples.AddRange(rightJoin.RightPimples);
                    }
                }

                var fixedJoint = possibleJoinSingula.SingulaView.AddComponent<FixedJoint>();
                fixedJoint.connectedBody = singula.SingulaView.GetComponent<Rigidbody>();

                //var gameObject = new GameObject("Join object");
                //possibleJoinSingula.SingulaView.transform.SetParent(gameObject.transform);
                //singula.SingulaView.transform.SetParent(gameObject.transform);

                //var rigidbody = gameObject.AddComponent<Rigidbody>();
                //var leftRigidbody = possibleJoinSingula.SingulaView.GetComponent<Rigidbody>();
                //var rightRigidbody = singula.SingulaView.GetComponent<Rigidbody>();
                //rigidbody.mass = leftRigidbody.mass + rightRigidbody.mass;
                //GameObject.Destroy(leftRigidbody);
                //GameObject.Destroy(rightRigidbody);

                _joinSingulaPool.Del(entity);
                _releaseFromHandPool.Add(entity);
            }
        }
    }
}