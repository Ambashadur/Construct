using System.Collections.Generic;
using Construct.Model;

namespace Construct.Components
{
    public struct Conventus
    {
        public int Id;
        public string Name;
        public Dictionary<int, Join> Joins;
    }
}