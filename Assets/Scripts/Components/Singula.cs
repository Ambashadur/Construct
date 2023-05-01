using System.Collections.Generic;
using UnityEngine;
using Construct.Views;

namespace Construct.Components {
    public struct Singula {
        public SingulaView SingulaView;
        public int EcsConventusEntityId;
        public Dictionary<int, GameObject> SlaveSingulaFrames;
    }
}
