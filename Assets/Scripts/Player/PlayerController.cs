using UnityEngine;

namespace Player {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private float _speed = 5.0f;

        private BasePlayerInputActions _playerInput;
        private Transform _cachedTransform;
        private CharacterController _characterController;

        private void Awake() => _playerInput = new();
        private void OnEnable() => _playerInput.FPSMap.Enable();
        private void OnDisable() => _playerInput.FPSMap.Disable();

        private void Start() {
            _cachedTransform = GetComponent<Transform>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            var input = _playerInput.FPSMap.Movement.ReadValue<Vector2>();

            if (input == Vector2.zero) return;

            input = Vector2.ClampMagnitude(input, _speed);
            _characterController.Move(new Vector3 {
                x = input.x * _speed * Time.deltaTime,
                y = 0,
                z = input.y * _speed * Time.deltaTime
            });
        }

        private void LateUpdate() {
            _playerCamera.position = _cachedTransform.position;
        }
    }
}
