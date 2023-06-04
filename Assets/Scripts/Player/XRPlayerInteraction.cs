using Construct;
using Construct.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class XRPlayerInteraction : PlayerConnection
    {
        [SerializeField] private InputActionProperty _dwonloadAction;

        void Start()
        {
            _dwonloadAction.action.started += _ => 
            {
                var entity = World.NewEntity();
                ref var conventus = ref World.GetPool<LoadConventus>().Add(entity);
                conventus.Id = 1;
            };
        }
    }
}