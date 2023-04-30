using UnityEngine;

namespace Construct.Player {
    [RequireComponent(typeof(CharacterController), typeof(PlayerInteraction))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private float _mouseXSensivity = 80.0f;
        [SerializeField] private float _mouseYSensivity = 50.0f;
        [SerializeField] private float _cameraXClamp = 80.0f;
        [SerializeField] private Vector3 _cameraOffset = Vector3.zero;

        private BasePlayerInputActions _playerInput;
        private Transform _cachedTransform;
        private CharacterController _characterController;
        private PlayerInteraction _interaction;

        private void Awake() => _playerInput = new();

        private void OnEnable() {
            _playerInput.FPSMap.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable() {
            _playerInput.FPSMap.Disable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Start() {
            _cachedTransform = GetComponent<Transform>();
            _characterController = GetComponent<CharacterController>();
            _interaction = GetComponent<PlayerInteraction>();

            _playerInput.FPSMap.Drag.started += _interaction.StartDrag;
            _playerInput.FPSMap.Drag.canceled += _interaction.PerfomDrag;
        }

        private void Update() {
            var input = _playerInput.FPSMap.Movement.ReadValue<Vector2>();

            if (input == Vector2.zero) return;

            input = Vector2.ClampMagnitude(input, _speed);
            var motion = new Vector3 {
                x = input.x * _speed * Time.deltaTime,
                y = 0,
                z = input.y * _speed * Time.deltaTime
            };

            _characterController.Move(transform.rotation * motion);
        }

        private void LateUpdate() {
            var input = _playerInput.FPSMap.View.ReadValue<Vector2>();
            _playerCamera.position = transform.position + _cameraOffset;

            var currentXRotation = _playerCamera.rotation.eulerAngles.x;
            if (currentXRotation >= 180.0f - _cameraXClamp) currentXRotation -= 360.0f;

            var xRotation = -input.y * _mouseYSensivity * Time.deltaTime;
            xRotation = Mathf.Clamp(currentXRotation + xRotation, -_cameraXClamp, _cameraXClamp) - currentXRotation;
            var yRotation = input.x * _mouseXSensivity* Time.deltaTime;

            transform.Rotate(Vector3.up, yRotation, Space.World);
            _playerCamera.Rotate(Vector3.up, yRotation, Space.World);
            _playerCamera.Rotate(Vector3.right, xRotation, Space.Self);
        }
    }
}
