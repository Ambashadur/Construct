using System;
using UnityEngine;

namespace Construct.Model {
    [Serializable]
    public sealed class Join {
        public int Id;
        public Vector3 Position;
        public bool IsTaken;
        public int[] PreviousJoinIds;
        public int[] NextJoinIds; 
    }
}