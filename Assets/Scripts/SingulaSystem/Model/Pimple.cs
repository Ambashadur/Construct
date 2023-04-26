using System;
using UnityEngine;

namespace SingulaSystem.Model {
    [Serializable]
    public class Pimple {
        public bool IsTaken;
        public Vector3 Position;
        public TriggerEvent Trigger;
    }
}
