using System;
using UnityEngine;

namespace Construct.Services {
    [Serializable]
    public class SingulaDto {
        public string name;
        public int singula_id;
        public string model;
        public Vector3 position;
        public int[] joins;
    }
}