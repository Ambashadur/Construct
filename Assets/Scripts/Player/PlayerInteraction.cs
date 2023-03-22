using UnityEngine;
using UnityEngine.InputSystem;
using SingulaSystem;

namespace Player {
    public class PlayerInteraction : MonoBehaviour {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private float _distance = 100.0f;

        private bool _isDrag = false;
        private float _distanceToSingula;
        private Transform _singulaPosition;
        private Rigidbody _singulaRigidbody;

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
                var singula = hit.transform.GetComponent<Singula>();
                if (singula == null) return;

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
    }
}
