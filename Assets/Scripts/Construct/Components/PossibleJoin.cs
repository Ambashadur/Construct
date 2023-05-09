using System.Collections.Generic;
using UnityEngine;

namespace Construct.Components {
    public struct PossibleJoin {
        /// <summary>
        /// Словарь содержащий подходящие для соединения Join.
        /// Key - Уникальный идентификатор <see Join> содержащийся в данной <see Singula>.
        /// Value - Уникальный идентификатор <see Join> содержащийся в <see Singula> в руке у игрока.
        /// </summary>
        public Dictionary<int, int> JoinPairs;
        public int JoinIdSingulaFrame;
        public GameObject SingulaFrame;
    }
}