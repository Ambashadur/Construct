using System;
using UnityEngine;
using Leopotam.EcsLite;

namespace Construct.Views {
    public class SingulaView : MonoBehaviour {
        public int Id;
        public string Name;
        public Pimple[] Pimples = Array.Empty<Pimple>();
        public Vector3? Slot;
    }
}
