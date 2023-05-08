using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class SingulaSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _filter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<Conventus> _conventusPool;

        public SingulaSystem(EcsWorld world) {
            _world = world;
            _filter = _world.Filter<Singula>().Inc<InHand>().End();
            _singulaPool = _world.GetPool<Singula>();
            _conventusPool = _world.GetPool<Conventus>();
        }

        public void Run(IEcsSystems systems) {
            
        }
    }
}