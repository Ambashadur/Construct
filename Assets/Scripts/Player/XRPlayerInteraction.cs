using Construct;
using Construct.Components;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Player
{
    public sealed class XRPlayerInteraction : PlayerConnection
    {
        [SerializeField] private InputActionProperty _downloadAction;

        private CharacterController _characterController;
        private CharacterControllerDriver _characterControllerDriver;
        private XROrigin _xrOrigin;

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _characterControllerDriver = GetComponent<CharacterControllerDriver>();
            _xrOrigin = GetComponent<XROrigin>();

            _downloadAction.action.started += _ => 
            {
                var entity = World.NewEntity();
                ref var conventus = ref World.GetPool<LoadConventus>().Add(entity);
                conventus.Id = 1;
            };
        }

        private void Update()
        {
            UpdateCharacterController();
        }

        private void UpdateCharacterController()
        {
            if (_xrOrigin == null || _characterController == null) return;

            var height = Mathf.Clamp(
                _xrOrigin.CameraInOriginSpaceHeight,
                _characterControllerDriver.minHeight,
                _characterControllerDriver.maxHeight);

            var center = _xrOrigin.CameraInOriginSpacePos;
            center.y = height / 2.0f + _characterController.skinWidth;

            _characterController.height = height;
            _characterController.center = center;
        }
    }
}