using System;
using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;
using Construct.Views;

namespace Construct.Systems {
    sealed class SingulaSystem : IEcsInitSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _filter;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<Conventus> _conventusPool;
        private readonly Material _greenOutline;

        private readonly int _conventusEcsEntity;

        public SingulaSystem(EcsWorld world, Material greenOutline) {
            _world = world;
            _filter = _world.Filter<Singula>().End();
            _singulaPool = _world.GetPool<Singula>();
            _conventusPool = _world.GetPool<Conventus>();
            _greenOutline = greenOutline;

            _conventusEcsEntity = _world.NewEntity();
            ref var conventus = ref _conventusPool.Add(_conventusEcsEntity);
            conventus.Hirearchy = new int[] { -1, 0 };
        }

        public void Init(IEcsSystems systems)
        {
            var singulaViews = UnityEngine.Object.FindObjectsOfType<SingulaView>();
            var _conventusPool = _world.GetPool<Conventus>();

            foreach (var singulaView in singulaViews) {
                var entity = _world.NewEntity();
                ref var singula = ref _singulaPool.Add(entity);

                singula.SingulaView = singulaView;
                singula.SingulaView.EcsEntity = entity;
                singula.ConventusEcsEntity = _conventusEcsEntity;
                singula.SlaveSingulaEcsEntities = new int[singula.SingulaView.Pimples.Length];
                Array.Fill(singula.SlaveSingulaEcsEntities, -1);

                var meshRenderer = singula.SingulaView.GetComponent<MeshRenderer>();
                var oldTexture = meshRenderer.material.mainTexture;
                meshRenderer.material = _greenOutline;
                meshRenderer.material.SetTexture("_Texture2D", oldTexture);

                var triggerPimpleViews = singulaView.GetComponentsInChildren<TriggerPimpleView>();

                foreach (var triggerPimpleView in triggerPimpleViews) {
                    triggerPimpleView.EcsSingulaEntityId = entity;
                    triggerPimpleView.TriggerEnterPool = _world.GetPool<TriggerEnter>();
                    triggerPimpleView.TriggerExitPool = _world.GetPool<TriggerExit>();
                }
            }
        }
    }
}