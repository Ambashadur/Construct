using Construct.Views;

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

        /// <summary>
        /// Ссылка на сущность, содержащую компонент сборки <see cref="Conventus"/>, частью которой является данная деталь.
        /// </summary>
        public int ConventusEcsEntity;
    }
}