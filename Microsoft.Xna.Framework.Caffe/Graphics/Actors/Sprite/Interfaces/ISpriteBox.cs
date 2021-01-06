// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um box para uso na classe Sprite.
    /// </summary>
    /// <typeparam name="T">T é a estrutura que implementa essa interface.</typeparam>
    public interface ISpriteBox<T> where T : struct
    {
        /// <summary>Obtém um retângulo com a posição e tamanho do box dentro do Sprite.</summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// Obtém a posição relativa da caixa no SpriteFrame independente da escala deste.
        /// </summary>        
        /// <param name="frame">O frame referente ao index do box.</param>
        /// <param name="target">O frame do mesmo index mas com o tamanho final para o calculo.</param>
        /// <param name="scale">O tamanho da escala do target (default Vector.One).</param>
        /// <param name="flip">O SpriteEffect pertencente ao estado atual da entidade (default SpriteEffects.None).</param>
        T GetRelativePosition(Rectangle frame, Rectangle target, Vector2 scale, SpriteEffects flip);
    }
}