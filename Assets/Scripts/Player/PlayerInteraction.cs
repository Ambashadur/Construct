using UnityEngine;
using UnityEngine.InputSystem;
using Construct;
using Construct.Views;
using Construct.Components;

namespace Player {
    public class PlayerInteraction : PlayerConnection {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private float _distance = 100.0f;

        private bool _isDrag = false;
        private float _distanceToSingula;
        private Transform _singulaPosition;
        private Rigidbody _singulaRigidbody;
        private SingulaView _singulaView;

        public void Download(InputAction.CallbackContext context) {
            var conventusEntity = World.NewEntity();
            ref var loadConventus = ref World.GetPool<LoadConventus>().Add(conventusEntity);
            loadConventus.Id = 1;
        }

        private void Update() {
            if (_isDrag) {
                var direction = _playerCamera.position + _playerCamera.forward * _distanceToSingula;
                _singulaPosition.Translate(direction - _singulaPosition.position, Space.World);

                return;
            }

            var center = new Vector3(Screen.width / 2, Screen.width / 2, 0);
            var ray = new Ray(_playerCamera.position, _playerCamera.forward);

            if (Physics.Raycast(ray, out var hit, _distance) && hit.transform.TryGetComponent<SingulaView>(out var singulaView)) {
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

        public void StartDrag(InputAction.CallbackContext context) {
            if (_singulaView != null) {
                _singulaRigidbody = _singulaView.GetComponent<Rigidbody>();
                _singulaRigidbody.isKinematic = true;

                _isDrag = true;
                _singulaPosition = _singulaView.transform;
                _distanceToSingula = Vector3.Distance(_singulaPosition.position, _playerCamera.position);
                World.GetPool<InHand>().Add(_singulaView.EcsEntity);
            }
        }

        public void PerfomDrag(InputAction.CallbackContext context) => EndDrag();

        public void Join(InputAction.CallbackContext context) {
            
        }

        public void Detach(InputAction.CallbackContext context) {
            
        }

        private void EndDrag() {
            if (_isDrag) {
                _isDrag = false;
                _singulaPosition = null;
                _singulaRigidbody.isKinematic = false;
                _singulaRigidbody = null;
                World.GetPool<InHand>().Del(_singulaView.EcsEntity);
            }
        }
    }
}
