using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class EndFocusSystem : IEcsRunSystem {    
        private readonly EcsWorld _world;
        private readonly EcsFilter _singulaEndFocusFilter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<EndFocus> _endFocusPool;

        public EndFocusSystem(EcsWorld world) {
            _world = world;
            _singulaEndFocusFilter = _world.Filter<Singula>().Inc<EndFocus>().End();
            _singulaPool = _world.GetPool<Singula>();
            _endFocusPool = _world.GetPool<EndFocus>();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _singulaEndFocusFilter) {
                ref var singula = ref _singulaPool.Get(entity);
                singula.SingulaView.GetComponent<MeshRenderer>().material.SetInt("_Outline", 0);
                _endFocusPool.Del(entity);
            }
        }
    }
}