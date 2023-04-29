using System;
using UnityEngine;

namespace SingulaSystem.Model {
    public class Singula : MonoBehaviour {
        public int Id;
        public string Name;
        public Pimple[] Pimples = Array.Empty<Pimple>();
        public Vector3? Slot;
    }
}
