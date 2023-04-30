using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class SingulaSystem : IEcsInitSystem, IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _filter;
        private readonly EcsPool<Singula> _pool;

        public SingulaSystem(EcsWorld world) {
            _world = world;
            _filter = _world.Filter<Singula>().End();
            _pool = _world.GetPool<Singula>();
        }

        public void Init(IEcsSystems systems)
        {
            throw new System.NotImplementedException();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _filter) {
                
            }
        }
    }
}