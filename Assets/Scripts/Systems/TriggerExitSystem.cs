using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class TriggerExitSystem : IEcsRunSystem {
        private EcsWorld _world;
        private EcsFilter _triggerExitFilter;
        private EcsPool<TriggerExit> _triggerExitPool;

        public TriggerExitSystem(EcsWorld world) {
            _world = world;
            _triggerExitFilter = _world.Filter<Singula>().Inc<TriggerExit>().End();
            _triggerExitPool = _world.GetPool<TriggerExit>();
        }        
        public void Run (IEcsSystems systems) {
            foreach (var entity in _triggerExitFilter) {
                

                _triggerExitPool.Del(entity);
            }
        }
    }
}