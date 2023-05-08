using System;

namespace Construct.Services {
    [Serializable]
    public class ConventusDto {
        public int conventus_id;
        public string conventus_name;
        public SingulaDto[] singulas;
        public JoinDto[] joins;
    }
}