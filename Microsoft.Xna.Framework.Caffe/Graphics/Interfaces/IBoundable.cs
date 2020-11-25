// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa os limites de um objeto.</summary>
    public interface IBoundsable
    {
        /// <summary>Obtém o retângulo que representa os limites da objeto.</summary>
        Rectangle Bounds { get; }
    }
}