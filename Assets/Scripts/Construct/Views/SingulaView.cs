using System;
using UnityEngine;

namespace Construct.Views {
    sealed public class SingulaView : MonoBehaviour {
        public int Id;
        public string Name;
        public Pimple[] Pimples = Array.Empty<Pimple>();
        public Vector3 Slot;
        public bool HasSlot;
        public int EcsEntity;
    }
}
