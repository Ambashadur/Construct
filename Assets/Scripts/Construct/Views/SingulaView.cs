using System.Collections.Generic;
using Attributes;
using Construct.Model;
using UnityEngine;

namespace Construct.Views {
    sealed public class SingulaView : MonoBehaviour {
        [ReadOnly] public int Id;
        [ReadOnly] public string Name;
        [ReadOnly] public int EcsEntity;
        [ReadOnly] public Dictionary<int, Join> Joins = new();
    }
}
