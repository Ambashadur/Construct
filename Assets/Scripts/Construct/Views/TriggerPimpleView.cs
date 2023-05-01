using UnityEngine;
using Leopotam.EcsLite;
using Construct.Components;
using Attributes;

namespace Construct.Views {
    [RequireComponent(typeof(BoxCollider))]
    sealed public class TriggerPimpleView : MonoBehaviour {
        [ReadOnly] public int PimpleId;
        [ReadOnly] public int EcsSingulaEntityId;

        [HideInInspector] public EcsPool<TriggerEnter> TriggerEnterPool;
        [HideInInspector] public EcsPool<TriggerExit> TriggerExitPool;

        private void OnEnable() => GetComponent<BoxCollider>().isTrigger = true;

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<SingulaView>(out var otherSingulaView)) {
                ref var triggerEnter = ref TriggerEnterPool.Add(EcsSingulaEntityId);
                triggerEnter.PimpleId = PimpleId;
                triggerEnter.OtherSingulaMesh = other.GetComponent<MeshFilter>().mesh;
                triggerEnter.OtherSingulaView = otherSingulaView;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent<SingulaView>(out var otherSingulaView)) {
                ref var triggerExit = ref TriggerExitPool.Add(EcsSingulaEntityId);
                triggerExit.PimpleId = PimpleId;
                triggerExit.OtherSingulaView = otherSingulaView;
            }
        }
    }
}