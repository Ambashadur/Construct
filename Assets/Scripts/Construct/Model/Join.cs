using System;

namespace Construct.Model {
    [Serializable]
    public sealed class Join {
        public int Id;
        public int[] NextJoinIds;
        public int LeftJoinId;
        public SingulaJoin[] LeftPimples;
        public int RightJoinId;
        public SingulaJoin[] RightPimples;
    }

    [Serializable]
    public struct SingulaJoin {
        public int SingulaId;
        public int PimpleId;
    }
}