using System;
using UnityEngine;

namespace SingulaSystem.View {
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerEvent : MonoBehaviour {
        public uint Id;
        public event Action<uint, Collider> OnTriggerEventStart;
        public event Action<uint, Collider> OnTriggerEventEnd; 

        private void OnEnable() => GetComponent<BoxCollider>().isTrigger = true;
        private void OnTriggerEnter(Collider other) => OnTriggerEventStart?.Invoke(Id, other);
        private void OnTriggerExit(Collider other) => OnTriggerEventEnd?.Invoke(Id, other);
    }
}
