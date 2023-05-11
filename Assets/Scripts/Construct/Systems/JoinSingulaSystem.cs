using System.Linq;
using Construct.Components;
using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;

namespace Construct.Systems {
    sealed class JoinSingulaSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _joinSingulaFilter;
        private readonly EcsFilter _possibleJoinFilter;
        private readonly EcsPool<JoinSingula> _joinSingulaPool;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<InHand> _inHandPool;
        private readonly EcsPool<PossibleJoin> _possibleJoinPool;
        private readonly EcsPool<Conventus> _conventusPool;
        private readonly EcsPool<ReleaseFromHand> _releaseFromHandPool;

        public JoinSingulaSystem(EcsWorld world) {
            _world = world;
            _joinSingulaFilter = _world.Filter<JoinSingula>().Inc<Singula>().End();
            _possibleJoinFilter = _world.Filter<PossibleJoin>().Inc<Singula>().End();
            _joinSingulaPool = _world.GetPool<JoinSingula>();
            _singulaPool = _world.GetPool<Singula>();
            _inHandPool = _world.GetPool<InHand>();
            _possibleJoinPool = _world.GetPool<PossibleJoin>();
            _conventusPool = _world.GetPool<Conventus>();
            _releaseFromHandPool = _world.GetPool<ReleaseFromHand>();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _joinSingulaFilter) {
                //ref var singula = ref _singulaPool.Get(entity);
                //ref var inHand = ref _inHandPool.Get(entity);

                //if (inHand.PossibleJoinEcsEntity == -1) continue;

                //ref var possibleJoin = ref _possibleJoinPool.Get(inHand.PossibleJoinEcsEntity);
                //ref var possibleJoinSingula = ref _singulaPool.Get(inHand.PossibleJoinEcsEntity);
                //ref var conventus = ref _conventusPool.Get(singula.ConventusEcsEntity);

                //singula.SingulaView.transform.position = possibleJoin.SingulaFrame.transform.position;
                //singula.SingulaView.transform.rotation = possibleJoin.SingulaFrame.transform.rotation;
                //singula.SingulaView.transform.SetParent(possibleJoinSingula.SingulaView.transform);

                //possibleJoinSingula.SingulaView.Joins[possibleJoin.JoinIdSingulaFrame].IsTaken = true;
                //singula.SingulaView.Joins[possibleJoin.JoinPairs[possibleJoin.JoinIdSingulaFrame]].IsTaken = true;

                //var firstJoin = conventus.Joins[possibleJoin.JoinIdSingulaFrame];
                //var secondJoin = conventus.Joins[possibleJoin.JoinPairs[possibleJoin.JoinIdSingulaFrame]];

                //var nextJoinId = firstJoin.NextJoinIds.Intersect(secondJoin.NextJoinIds).First();

                //possibleJoinSingula.SingulaView.Joins[nextJoinId] = conventus.Joins[nextJoinId];
                //singula.SingulaView.Joins[nextJoinId] = conventus.Joins[nextJoinId];

                //var fixedJoint = possibleJoinSingula.SingulaView.AddComponent<FixedJoint>();
                //fixedJoint.connectedBody = singula.SingulaView.GetComponent<Rigidbody>();

                _releaseFromHandPool.Add(entity);
            }
        }
    }
}