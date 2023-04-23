using System;
using UnityEngine;

namespace SingulaSystem {
    [RequireComponent(typeof(Rigidbody))]
    public class Singula : MonoBehaviour {
        [HideInInspector] public Pimple[] Pimples = Array.Empty<Pimple>();
        [SerializeField] private string _name;
        public bool HasSlot = true;
        public Vector3 Slot = Vector3.zero;

        private Rigidbody _rigidbody;
        private Transform _transform;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();

            for (uint index = 0; index < Pimples.Length; index++) {
                Pimples[index].Trigger.OnTriggerEventStart += PimpleTriggered;
                Pimples[index].Trigger.Id = index;
            }
        }

        private void PimpleTriggered(uint index, Collider collider) {
            var otherLimb = collider.GetComponent<Singula>();

            if (otherLimb != null && otherLimb.HasSlot) {
                var joint = collider.GetComponent<FixedJoint>();

                if (joint == null) {
                    var limbTransform = collider.transform;

                    limbTransform.rotation = _transform.rotation;
                    limbTransform.position = _transform.TransformPoint(Pimples[index].Position - otherLimb.Slot);

                    var addedJoint = collider.gameObject.AddComponent<FixedJoint>();
                    addedJoint.connectedBody = _rigidbody;
                } else {
                    joint.connectedBody = _rigidbody;
                }
            }
        }
    }
}
