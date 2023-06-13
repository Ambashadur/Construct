using System.Collections.Generic;
using Construct.Model;
using Construct.Views;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Construct.Components
{
    /// <summary>
    /// ��������� ������.
    /// </summary>
    public struct Singula
    {
        /// <summary>
        /// ������ �� ������ �������� ������������� ������.
        /// </summary>
        public SingulaView SingulaView;

        /// <summary>
        /// ������ �� ��������� Transform.
        /// </summary>
        public Transform Transform;

        /// <summary>
        /// ������ �� �������� ������ ������.
        /// </summary>
        public Collider Collider;

        /// <summary>
        /// ������ �� ��������� ��������� ������.
        /// </summary>
        public Outline Outline;

        /// <summary>
        /// ������ �� ��������� �������������� � ������� ������.
        /// </summary>
        public XRGrabInteractable XRGrabInteractable;

        /// <summary>
        /// ���������� ������������� ������.
        /// </summary>
        public int Id;

        /// <summary>
        /// ��� ������.
        /// </summary>
        public string Name;

        /// <summary>
        /// ����� ���������� ������.
        /// </summary>
        public Dictionary<int, Pimple> Pimples;

        /// <summary>
        /// ������ �� ��������, ���������� ��������� ������ <see cref="Conventus"/>, ������ ������� �������� ������ ������.
        /// </summary>
        public int ConventusEcsEntity;
    }
}