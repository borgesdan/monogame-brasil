// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Implementa a propriedade de um polígono rotacionado.
    /// </summary>
    public interface IBoundsR
    {
        /// <summary>
        /// Obtém os limites rotacionado de um objeto na forma de um polígono.
        /// </summary>
        public Polygon BoundsR { get; }
    }
}