using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class TriggerExitSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _triggerExitFilter;
        private readonly EcsPool<TriggerExit> _triggerExitPool;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<SingulaFrame> _singulaFramePool;

        public TriggerExitSystem(EcsWorld world) {
            _world = world;
            _triggerExitFilter = _world.Filter<Singula>().Inc<TriggerExit>().End();
            _triggerExitPool = _world.GetPool<TriggerExit>();
            _singulaPool = _world.GetPool<Singula>();
            _singulaFramePool = _world.GetPool<SingulaFrame>();
        }   

        public void Run (IEcsSystems systems) {
            foreach (var entity in _triggerExitFilter) {
                ref var singula = ref _singulaPool.Get(entity);
                ref var triggerExit = ref _triggerExitPool.Get(entity);

                if (_singulaFramePool.Has(entity)) {
                    ref var singulaFrame = ref _singulaFramePool.Get(entity);
                    
                    if (singulaFrame.OtherSingulaView.Id == triggerExit.OtherSingulaView.Id) {
                        ref var otherSingula = ref _singulaPool.Get(singulaFrame.OtherSingulaView.EcsEntity);
                        otherSingula.MasterSingulaEcsEntity = null;

                        GameObject.Destroy(singulaFrame.FrameGameObject);
                        _singulaFramePool.Del(entity);
                    }
                }

                _triggerExitPool.Del(entity);
            }
        }
    }
}