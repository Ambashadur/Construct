using System;
using UnityEngine;

namespace Construct.Services {
    [Serializable]
    public class JoinDto {
        public int join_id;
        public Vector3 position;
        public int[] next_join_ids;
        public int[] previous_join_ids;
    }
}