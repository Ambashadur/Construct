using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;

namespace Construct.Views {
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerPimpleView : MonoBehaviour {
        [HideInInspector] public int PimpleId;
        [HideInInspector] public int EcsSingulaEntityId { get; set; }
        [HideInInspector] public EcsPool<TriggerEnter> TriggerEnterPool { get; set; }
        [HideInInspector] public EcsPool<TriggerExit> TriggerExitPool { get; set; }

        private void OnEnable() => GetComponent<BoxCollider>().isTrigger = true;

        private void OnTriggerEnter(Collider other) {
            ref var triggerEnter = ref TriggerEnterPool.Add(EcsSingulaEntityId);
            triggerEnter.PimpleId = PimpleId;
            triggerEnter.otherCollider = other;
        }

        private void OnTriggerExit(Collider other) {
            ref var triggerExit = ref TriggerExitPool.Add(EcsSingulaEntityId);
            triggerExit.PimpleId = PimpleId;
            triggerExit.otherCollider = other;
        }
    }
}