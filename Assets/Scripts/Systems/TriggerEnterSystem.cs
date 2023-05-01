using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;
using Construct.Views;

namespace Construct.Systems {
    sealed class TriggerEnterSystem : IEcsRunSystem {        
        private EcsWorld _world;
        private EcsFilter _triggerEnterFilter;
        private EcsPool<TriggerEnter> _triggerEnterPool;
        private EcsPool<Conventus> _conventusPool;
        private EcsPool<Singula> _singulaPool;
        private Material _greenTransparent;
        
        public TriggerEnterSystem(EcsWorld world, Material greenTransparent) {
            _world = world;
            _greenTransparent = greenTransparent;
            _triggerEnterFilter = _world.Filter<Singula>().Inc<TriggerEnter>().End();
            _triggerEnterPool = _world.GetPool<TriggerEnter>();
            _conventusPool = _world.GetPool<Conventus>();
            _singulaPool = _world.GetPool<Singula>();
        }
        
        public void Run (IEcsSystems systems) {
            foreach (var entity in _triggerEnterFilter) {
                ref var triggerEnter = ref _triggerEnterPool.Get(entity);
                ref var singula = ref _singulaPool.Get(entity);
                ref var conventus = ref _conventusPool.Get(singula.EcsConventusEntityId);

                var slaveSingulaView = triggerEnter.otherCollider.GetComponent<SingulaView>();

                var isPimpleFree = !singula.SingulaView.Pimples[triggerEnter.PimpleId].IsTaken;
                var isCorrectHirearchy = singula.SingulaView.Id == conventus.Hirearchy[slaveSingulaView.Id]; 

                if (isPimpleFree && isCorrectHirearchy && slaveSingulaView.HasSlot && !singula.SlaveSingulaFrames.ContainsKey(triggerEnter.PimpleId)) {
                    var singulaFrame = new GameObject("SingulaFrame");
                    singulaFrame.AddComponent<MeshRenderer>().material = _greenTransparent;
                    singulaFrame.AddComponent<MeshFilter>().mesh = triggerEnter.otherCollider.GetComponent<MeshFilter>().mesh;

                    var singulaFrameTransform = singulaFrame.GetComponent<Transform>();
                    var singulaTransform = singula.SingulaView.GetComponent<Transform>();

                    singulaFrameTransform.rotation = singulaTransform.rotation;
                    singulaFrameTransform.position = singulaTransform.TransformPoint(singula.SingulaView.Pimples[triggerEnter.PimpleId].Position - slaveSingulaView.Slot);
                    singulaFrameTransform.SetParent(singulaTransform);

                    singula.SlaveSingulaFrames[triggerEnter.PimpleId] = singulaFrame;
                }

                _triggerEnterPool.Del(entity);
            }
        }
    }
}