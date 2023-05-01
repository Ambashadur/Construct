using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;
using Construct.Views;

namespace Construct.Systems {
    sealed class SingulaSystem : IEcsInitSystem, IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _filter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<Conventus> _conventusPool;

        private readonly int _conventusEcsEntityId;

        public SingulaSystem(EcsWorld world) {
            _world = world;
            _filter = _world.Filter<Singula>().End();
            _singulaPool = _world.GetPool<Singula>();
            _conventusPool = _world.GetPool<Conventus>();

            _conventusEcsEntityId = _world.NewEntity();
            ref var conventus = ref _conventusPool.Add(_conventusEcsEntityId);
            conventus.Hirearchy = new int[] { -1, 0 };
        }

        public void Init(IEcsSystems systems)
        {
            var singulaViews = Object.FindObjectsOfType<SingulaView>();
            var _conventusPool = _world.GetPool<Conventus>();

            foreach (var singulaView in singulaViews) {
                var entity = _world.NewEntity();
                ref var singula = ref _singulaPool.Add(entity);

                singula.SingulaView = singulaView;
                singula.EcsConventusEntityId = _conventusEcsEntityId;
                singula.SlaveSingulaFrames = new Dictionary<int, GameObject>();

                var triggerPimpleViews = singulaView.GetComponentsInChildren<TriggerPimpleView>();

                foreach (var triggerPimpleView in triggerPimpleViews) {
                    triggerPimpleView.EcsSingulaEntityId = entity;
                    triggerPimpleView.TriggerEnterPool = _world.GetPool<TriggerEnter>();
                    triggerPimpleView.TriggerExitPool = _world.GetPool<TriggerExit>();
                }
            }
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _filter) {
                
            }
        }
    }
}