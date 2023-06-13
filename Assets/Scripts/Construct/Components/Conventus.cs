using System.Collections.Generic;
using Construct.Model;

namespace Construct.Components
{
    /// <summary>
    /// ��������� ������.
    /// </summary>
    public struct Conventus
    {
        /// <summary>
        /// ���������� ������������� ������.
        /// </summary>
        public int Id;

        /// <summary>
        /// �������� ������.
        /// </summary>
        public string Name;

        /// <summary>
        /// ������� ���������� � ������.
        /// </summary>
        public Dictionary<int, Join> Joins;
    }
}