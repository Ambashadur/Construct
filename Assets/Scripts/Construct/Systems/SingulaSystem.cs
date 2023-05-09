using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;
using Construct.Model;

namespace Construct.Systems {
    sealed class SingulaSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _filter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<Conventus> _conventusPool;

        public SingulaSystem(EcsWorld world) {
            _world = world;
            _filter = _world.Filter<Singula>().End();
            _singulaPool = _world.GetPool<Singula>();
            _conventusPool = _world.GetPool<Conventus>();
        }

        public void Run(IEcsSystems systems) {
            var positions = new Dictionary<int, JoinPosition>();

            foreach (var entity in _filter) {
                ref var singula = ref _singulaPool.Get(entity);
                ref var conventus = ref _conventusPool.Get(singula.ConventusEcsEntity);

                foreach (var join in singula.SingulaView.Joins) {
                    foreach (var nextJoin in join.NextJoinIds) {
                        if (positions.ContainsKey(nextJoin)) {
                            positions[nextJoin].SecondJoinId = join.Id;
                            positions[nextJoin].SecondJoinPosition = singula.SingulaView.transform.TransformPoint(join.Position);
                        } else {
                            positions[nextJoin] = new JoinPosition() {
                                FirstJoinId = join.Id,
                                FirstJoinPosition = singula.SingulaView.transform.TransformPoint(join.Position)
                            };
                        }
                    }
                }
            }

            foreach (var kv in positions) {
                if (Vector3.Distance(kv.Value.FirstJoinPosition, kv.Value.SecondJoinPosition) <= 1.0f) {
                    Debug.Log($"{kv.Value.FirstJoinId} and {kv.Value.SecondJoinId} are near!");
                }
            }
        }
    }

    public sealed class JoinPosition {
        public int FirstJoinId;
        public Vector3 FirstJoinPosition;
        public int SecondJoinId;
        public Vector3 SecondJoinPosition;
    }
}