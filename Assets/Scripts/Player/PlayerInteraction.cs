using Construct;
using Construct.Components;
using Construct.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerInteraction : PlayerConnection {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private float _distance = 75.0f;
        [SerializeField] private LayerMask _singulaLayer;
        [SerializeField] private float _distanceToSingula = 2.5f;
        [SerializeField] private float _throwImpulseStrength = 10.0f;

        private bool _isDrag = false;
        private Transform _singulaTransform;
        private Rigidbody _singulaRigidbody;
        private SingulaView _singulaView;

        public void Download(InputAction.CallbackContext _) {
            var conventusEntity = World.NewEntity();
            ref var loadConventus = ref World.GetPool<LoadConventus>().Add(conventusEntity);
            loadConventus.Id = 1;
        }

        private void Update() {
            if (_isDrag) {
                var direction = _playerCamera.position + _playerCamera.forward * _distanceToSingula;
                _singulaTransform.Translate(direction - _singulaTransform.position, Space.World);
                return;
            }

            var center = new Vector3(Screen.width / 2, Screen.width / 2, 0);
            var ray = new Ray(_playerCamera.position, _playerCamera.forward);

            if (Physics.Raycast(ray, out var hit, _distance, _singulaLayer)) {
                var singulaView = hit.transform.GetComponent<SingulaView>();

                if (_singulaView == null) {
                    _singulaView = singulaView;
                    World.GetPool<StartFocus>().Add(_singulaView.EcsEntity);
                } else if (_singulaView.Id != singulaView.Id) {
                    World.GetPool<EndFocus>().Add(_singulaView.EcsEntity);
                    _singulaView = singulaView;
                    World.GetPool<StartFocus>().Add(_singulaView.EcsEntity);
                }
            } else if (_singulaView != null) {
                World.GetPool<EndFocus>().Add(_singulaView.EcsEntity);
                _singulaView = null;
            }
        }

        public void Drag(InputAction.CallbackContext _) {
            if (_singulaView != null && !_isDrag) {
                _singulaRigidbody = _singulaView.GetComponent<Rigidbody>();
                _singulaRigidbody.isKinematic = true;
                _isDrag = true;

                _singulaTransform = _singulaView.transform;
                _singulaTransform.SetParent(transform);

                World.GetPool<TakeToHand>().Add(_singulaView.EcsEntity);
            } else if (_isDrag) {
                _isDrag = false;

                _singulaTransform.SetParent(null);
                _singulaTransform = null;

                _singulaRigidbody.isKinematic = false;
                _singulaRigidbody = null;

                World.GetPool<ReleaseFromHand>().Add(_singulaView.EcsEntity);
            }
        }

        public void Release(InputAction.CallbackContext _) {
            if (_isDrag) {
                _isDrag = false;

                _singulaTransform.SetParent(null);
                _singulaTransform = null;

                _singulaRigidbody.isKinematic = false;
                _singulaRigidbody.AddForce(_playerCamera.forward * _throwImpulseStrength, ForceMode.Impulse);
                _singulaRigidbody = null;
                
                World.GetPool<ReleaseFromHand>().Add(_singulaView.EcsEntity);
            }
        }

        public void RotateSingula(Vector2 input) {
            if (!_isDrag) return;

            _singulaTransform.Rotate(Vector3.up, -input.x, Space.World);
            _singulaTransform.Rotate(Vector3.right, input.y, Space.World);
        }

        public void Join(InputAction.CallbackContext _) {
            if (!_isDrag) return;

            World.GetPool<JoinSingula>().Add(_singulaView.EcsEntity);
        }

        public void Detach(InputAction.CallbackContext _) {
            
        }
    }
}
