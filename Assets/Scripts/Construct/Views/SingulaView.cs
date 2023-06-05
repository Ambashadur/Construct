using Attributes;
using Construct.Components;
using Construct.Model;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Construct.Views
{
    public sealed class SingulaView : MonoBehaviour
    {
        [ReadOnly] public int Id;
        [ReadOnly] public string Name;
        [ReadOnly] public int EcsEntity;
        [ReadOnly] public Pimple[] Pimples;

        private XRGrabInteractable _xrGrabInteractable;

        public XRGrabInteractable SetXrGrabActions(EcsWorld world, InteractionLayerMask interactionLayerMask)
        {
            if (_xrGrabInteractable == null) _xrGrabInteractable = GetComponent<XRGrabInteractable>();

            _xrGrabInteractable.movementType = XRBaseInteractable.MovementType.Kinematic;
            _xrGrabInteractable.interactionLayers = interactionLayerMask;

            var onHoverEntered = new HoverEnterEvent();
            onHoverEntered.AddListener(args => {
                if (!world.GetPool<StartFocus>().Has(EcsEntity)) world.GetPool<StartFocus>().Add(EcsEntity);
            });

            _xrGrabInteractable.hoverEntered = onHoverEntered;

            var onHoverExited = new HoverExitEvent();
            onHoverExited.AddListener(args => {
                if (!world.GetPool<EndFocus>().Has(EcsEntity)) world.GetPool<EndFocus>().Add(EcsEntity);
            });

            _xrGrabInteractable.hoverExited = onHoverExited;

            var onSelectEnter = new SelectEnterEvent();
            onSelectEnter.AddListener(args => {
                if (!world.GetPool<TakeToHand>().Has(EcsEntity)) world.GetPool<TakeToHand>().Add(EcsEntity);
            });

            _xrGrabInteractable.selectEntered = onSelectEnter;

            var onSelectExit = new SelectExitEvent();
            onSelectExit.AddListener(args => {
                if (!world.GetPool<ReleaseFromHand>().Has(EcsEntity)) world.GetPool<ReleaseFromHand>().Add(EcsEntity);
            });

            _xrGrabInteractable.selectExited = onSelectExit;

            var onActivate = new ActivateEvent();
            onActivate.AddListener(args => {
                if (!world.GetPool<JoinSingula>().Has(EcsEntity)) world.GetPool<JoinSingula>().Add(EcsEntity);
            });

            _xrGrabInteractable.activated = onActivate;

            return _xrGrabInteractable;
        }
    }
}
