using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Systems {
    sealed class JoinSingulaSystem : IEcsRunSystem {
        private readonly EcsWorld _world;
        private readonly EcsFilter _joinSingulaFilter;
        private readonly EcsPool<JoinSingula> _joinSingulaPool;
        private readonly EcsPool<SingulaFrame> _singulaFramePool;
        private readonly EcsPool<Singula> _singulaPool;

        public JoinSingulaSystem(EcsWorld world) {
            _world = world;
            _joinSingulaFilter = _world.Filter<JoinSingula>().End();
            _joinSingulaPool = _world.GetPool<JoinSingula>();
            _singulaFramePool = _world.GetPool<SingulaFrame>();
            _singulaPool = _world.GetPool<Singula>();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _joinSingulaFilter) {
                if (_singulaFramePool.Has(entity)) {
                    ref var singulaFrame = ref _singulaFramePool.Get(entity);
                    ref var singula = ref _singulaPool.Get(entity);

                    singulaFrame.OtherSingulaView.transform.position = singulaFrame.FrameGameObject.transform.position;
                    singulaFrame.OtherSingulaView.transform.rotation = singulaFrame.FrameGameObject.transform.rotation;
                    singulaFrame.OtherSingulaView.transform.SetParent(singula.SingulaView.transform);

                    singula.SingulaView.Pimples[singulaFrame.PimpleId].IsTaken = true;

                    if (singula.SingulaView.TryGetComponent<FixedJoint>(out var fixedJoint)) {
                        fixedJoint.connectedBody = singulaFrame.OtherSingulaView.GetComponent<Rigidbody>();
                    } else {
                        fixedJoint = singula.SingulaView.gameObject.AddComponent<FixedJoint>();
                        fixedJoint.connectedBody = singulaFrame.OtherSingulaView.GetComponent<Rigidbody>();
                    }

                    GameObject.Destroy(singulaFrame.FrameGameObject);
                    _singulaFramePool.Del(entity);
                }

                _joinSingulaPool.Del(entity);
            }
        }
    }
}