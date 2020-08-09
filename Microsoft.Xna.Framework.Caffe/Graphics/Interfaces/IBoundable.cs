// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa os limites de uma entidade.</summary>
    public interface IBoundsable
    {
        /// <summary>Obtém o retângulo que representa os limites da entidade.</summary>
        Rectangle Bounds { get; }
    }
}