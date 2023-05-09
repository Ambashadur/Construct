using System.Collections.Generic;
using System.Linq;
using Construct.Components;
using Construct.Model;
using Leopotam.EcsLite;
using UnityEngine;

namespace Construct.Systems {
    public sealed class TakeToHandSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _takeToHandFilter;
        private readonly EcsFilter _otherSingulaFilter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<TakeToHand> _takeToHandPool;
        private readonly EcsPool<InHand> _inHandPool;
        private readonly EcsPool<PossibleJoin> _possibleJoinPool;
        private readonly EcsPool<EndFocus> _endFocusPool;

        public TakeToHandSystem(EcsWorld world) {
            _world = world;
            _takeToHandFilter = _world.Filter<Singula>().Inc<TakeToHand>().End();
            _otherSingulaFilter = _world.Filter<Singula>().Exc<TakeToHand>().End();
            _singulaPool = _world.GetPool<Singula>();
            _takeToHandPool = _world.GetPool<TakeToHand>();
            _inHandPool = _world.GetPool<InHand>();
            _possibleJoinPool = _world.GetPool<PossibleJoin>();
            _endFocusPool = _world.GetPool<EndFocus>();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _takeToHandFilter) {
                ref var singula = ref _singulaPool.Get(entity);
                GetNextJoinPairs(singula.SingulaView.Joins, out var nextJoinPairs);

                foreach (var otherEntity in _otherSingulaFilter) {
                    ref var otherSingula = ref _singulaPool.Get(otherEntity);
                    GetNextJoinPairs(otherSingula.SingulaView.Joins, out var otherNextJoinPairs);
                    var commonNextJoinIds = nextJoinPairs.Keys.Intersect(otherNextJoinPairs.Keys);

                    if (commonNextJoinIds.Count() > 0) {
                        ref var possibleJoin = ref _possibleJoinPool.Add(otherEntity);
                        possibleJoin.JoinIdSingulaFrame = -1;
                        possibleJoin.SingulaFrame = null;
                        possibleJoin.JoinPairs = commonNextJoinIds.ToDictionary(
                            nextJoinId => otherNextJoinPairs[nextJoinId],
                            nextJoinId => nextJoinPairs[nextJoinId]);
                    }
                }

                _endFocusPool.Add(entity);
                _inHandPool.Add(entity);
                _takeToHandPool.Del(entity);
            }
        }

        private void GetNextJoinPairs(Dictionary<int, Join> joins, out Dictionary<int, int> result) {
            result = new Dictionary<int, int>();

            foreach (var join in joins) {
                foreach (var nextJoinId in join.Value.NextJoinIds) {
                    result[nextJoinId] = join.Value.Id;
                }
            }
        }
    }
}