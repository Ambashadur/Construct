using System.Collections.Generic;
using Construct.Model;
using Construct.Views;
using UnityEngine;

namespace Construct.Components
{
    /// <summary>
    /// Компонент детали.
    /// </summary>
    public struct Singula
    {
        /// <summary>
        /// Ссылка на объект игрового представления детали.
        /// </summary>
        public SingulaView SingulaView;

        public Transform Transform;

        public MeshRenderer Renderer;

        /// <summary>
        /// Уникальный идентификатор детали.
        /// </summary>
        public int Id;

        /// <summary>
        /// Имя детали.
        /// </summary>
        public string Name;

        /// <summary>
        /// Точки соединений детали.
        /// </summary>
        public Dictionary<int, Pimple> Pimples;

        /// <summary>
        /// Ссылка на сущность, содержащую компонент сборки <see cref="Conventus"/>, частью которой является данная деталь.
        /// </summary>
        public int ConventusEcsEntity;
    }
}