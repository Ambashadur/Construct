using UnityEngine;
using UnityEngine.InputSystem;
using Leopotam.EcsLite;
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

        private void Update() {
            if (_isDrag) {
                var direction = _playerCamera.position + _playerCamera.forward * _distanceToSingula;
                _singulaPosition.Translate(direction - _singulaPosition.position, Space.World);
            }
        }

        public void StartDrag(InputAction.CallbackContext context) {
            var center = new Vector3(Screen.width / 2, Screen.width / 2, 0);

            var ray = new Ray(_playerCamera.position, _playerCamera.forward);
            if (Physics.Raycast(ray, out var hit, _distance)) {
                var singulaView = hit.transform.GetComponent<SingulaView>();
                if (singulaView == null) return;

                _singulaView = singulaView;
                _singulaRigidbody = hit.transform.GetComponent<Rigidbody>();
                _singulaRigidbody.isKinematic = true;

                _isDrag = true;
                _singulaPosition = hit.transform;
                _distanceToSingula = Vector3.Distance(_singulaPosition.position, _playerCamera.position);;
            }
        }

        public void PerfomDrag(InputAction.CallbackContext context) {
            if (_isDrag) {
                _isDrag = false;
                _singulaPosition = null;
                _singulaRigidbody.isKinematic = false;
                _singulaRigidbody = null;
            }
        }

        public void DoAction(InputAction.CallbackContext context) {
            ref var slaveSingula = ref World.GetPool<SlaveSingula>().Get(_singulaView.EcsEntity);
            World.GetPool<JoinSingula>().Add(slaveSingula.MasterSingulaEcsEntity);

            if (_isDrag) {
                _isDrag = false;
                _singulaPosition = null;
                _singulaRigidbody.isKinematic = false;
                _singulaRigidbody = null;
            }
        }
    }
}
