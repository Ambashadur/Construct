using System;
using Construct.Model;

namespace Construct.Services {
    [Serializable]
    public class JoinDto {
        public int join_id;
        public int[] next_join_ids;
        public int left_join_id;
        public SingulaJoin[] left_pimples;
        public int right_join_id;
        public SingulaJoin[] right_pimples;
    }
}