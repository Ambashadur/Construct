using System;
using UnityEngine;

namespace Construct.Model {
    [Serializable]
    public struct Join {
        public int Id;
        public Vector3 Position;
        public int[] PreviousJoinIds;
        public int[] NextJoinIds; 
    }
}