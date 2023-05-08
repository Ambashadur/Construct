using System;
using UnityEngine;
using Construct.Model;
using Attributes;

namespace Construct.Views {
    sealed public class SingulaView : MonoBehaviour {
        [ReadOnly] public int Id;
        [ReadOnly] public string Name;
        [ReadOnly] public int EcsEntity;
        public Join[] Joins = Array.Empty<Join>();
    }
}
