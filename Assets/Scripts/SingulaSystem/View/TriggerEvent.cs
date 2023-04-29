using System;
using UnityEngine;
using SingulaSystem.Model;

namespace SingulaSystem.View {
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerEvent : MonoBehaviour {
        public int Id;
        public Singula Singula;
        public event Action<Singula, Singula, int> OnTriggerEventStart;
        public event Action<Singula> OnTriggerEventEnd;

        private void OnEnable() => GetComponent<BoxCollider>().isTrigger = true;

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<Singula>(out var slaveSingula)) {
                OnTriggerEventStart?.Invoke(Singula, slaveSingula, Id);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent<Singula>(out var slaveSingula)) {
                OnTriggerEventEnd?.Invoke(slaveSingula);
            }
        }
    }
}
