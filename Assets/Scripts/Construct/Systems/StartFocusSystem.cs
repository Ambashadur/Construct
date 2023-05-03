using System;
using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class StartFocusSystem : IEcsRunSystem {   
        private readonly EcsWorld _world;
        private readonly EcsFilter _singulaStartFocusFilter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<StartFocus> _startFocusPool;

        public StartFocusSystem(EcsWorld world) {
            _world = world;
            _singulaStartFocusFilter = _world.Filter<Singula>().Inc<StartFocus>().End();
            _singulaPool = _world.GetPool<Singula>();
            _startFocusPool = _world.GetPool<StartFocus>();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _singulaStartFocusFilter) {
                ref var singula = ref _singulaPool.Get(entity);

                var isFree = Array.TrueForAll(
                    singula.SlaveSingulaEcsEntities, 
                    slaveSingulaEcsEntity => slaveSingulaEcsEntity == -1);

                if (isFree) singula.SingulaView.GetComponent<MeshRenderer>().material.SetInt("_Outline", 1);

                _startFocusPool.Del(entity);
            }
        }
    }
}