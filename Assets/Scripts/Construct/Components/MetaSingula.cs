using System.Collections.Generic;
using Construct.Views;

namespace Construct.Components
{
    /// <summary>
    /// ��������� ����-������, ������������ ��������� �������.
    /// </summary>
    public struct MetaSingula
    {
        /// <summary>
        /// ������ �� MetaSingulaView.
        /// </summary>
        public MetaSingulaView MetaSingulaView;

        /// <summary>
        /// ������� �������, ������� �������� ����-������.
        /// Key - ���������� ������������� ����� ����������.
        /// Value - ������ ���������� ��������������� �������, ��������� � �����.
        /// </summary>
        public Dictionary<int, List<int>> PimpleSingulaEcsEntities;

        public List<int> SingulaEcsEntities;
    }
}
