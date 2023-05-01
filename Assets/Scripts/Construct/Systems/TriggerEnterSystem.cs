using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class TriggerEnterSystem : IEcsRunSystem {        
        private readonly EcsWorld _world;
        private readonly EcsFilter _triggerEnterFilter;
        private readonly EcsPool<TriggerEnter> _triggerEnterPool;
        private readonly EcsPool<Conventus> _conventusPool;
        private readonly EcsPool<Singula> _singulaPool;
        private readonly EcsPool<SingulaFrame> _singulaFramePool;
        private readonly EcsPool<SlaveSingula> _slaveSingulaPool;
        private readonly Material _greenTransparent;
        
        public TriggerEnterSystem(EcsWorld world, Material greenTransparent) {
            _world = world;
            _greenTransparent = greenTransparent;
            _triggerEnterFilter = _world.Filter<Singula>().Inc<TriggerEnter>().End();
            _triggerEnterPool = _world.GetPool<TriggerEnter>();
            _conventusPool = _world.GetPool<Conventus>();
            _singulaPool = _world.GetPool<Singula>();
            _singulaFramePool = _world.GetPool<SingulaFrame>();
            _slaveSingulaPool = _world.GetPool<SlaveSingula>();
        }
        
        public void Run (IEcsSystems systems) {
            foreach (var entity in _triggerEnterFilter) {
                ref var triggerEnter = ref _triggerEnterPool.Get(entity);
                ref var singula = ref _singulaPool.Get(entity);
                ref var conventus = ref _conventusPool.Get(singula.ConventusEcsEntity);

                var isPimpleFree = !singula.SingulaView.Pimples[triggerEnter.PimpleId].IsTaken;
                var isCorrectHirearchy = singula.SingulaView.Id == conventus.Hirearchy[triggerEnter.OtherSingulaView.Id]; 

                if (isPimpleFree && isCorrectHirearchy && triggerEnter.OtherSingulaView.HasSlot && !_singulaFramePool.Has(entity)) {
                    var singulaFrameObject = new GameObject("SingulaFrame");
                    singulaFrameObject.AddComponent<MeshRenderer>().material = _greenTransparent;
                    singulaFrameObject.AddComponent<MeshFilter>().mesh = triggerEnter.OtherSingulaMesh;

                    var singulaFrameTransform = singulaFrameObject.GetComponent<Transform>();
                    var singulaTransform = singula.SingulaView.GetComponent<Transform>();

                    singulaFrameTransform.rotation = singulaTransform.rotation;
                    singulaFrameTransform.position = singulaTransform.TransformPoint(
                        singula.SingulaView.Pimples[triggerEnter.PimpleId].Position - triggerEnter.OtherSingulaView.Slot);

                    singulaFrameTransform.SetParent(singulaTransform);

                    ref var singulaFrame = ref _singulaFramePool.Add(entity);
                    singulaFrame.FrameGameObject = singulaFrameObject;
                    singulaFrame.PimpleId = triggerEnter.PimpleId;
                    singulaFrame.OtherSingulaView = triggerEnter.OtherSingulaView;

                    ref var slaveSingula = ref _slaveSingulaPool.Add(triggerEnter.OtherSingulaView.EcsEntity);
                    slaveSingula.MasterSingulaEcsEntity = entity;
                }

                _triggerEnterPool.Del(entity);
            }
        }
    }
}