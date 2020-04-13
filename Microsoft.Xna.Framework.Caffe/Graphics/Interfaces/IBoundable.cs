//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa os limites de uma entidade.</summary>
    public interface IBoundable
    {
        /// <summary>Obtém o retângulo que representa os limites da entidade.</summary>
        Rectangle Bounds { get; }
    }
}