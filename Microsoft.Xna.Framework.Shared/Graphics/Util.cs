// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe de auxílio.</summary>
    public static class Util
    {
        //---------------------------------------//
        //-----         UTILIDADE           -----//
        //---------------------------------------//

        /// <summary>Obtém o tamanho de de um objeto Point multiplicado por uma escala.</summary>
        public static Vector2 GetScaledSize(Point size, Vector2 scale)
        {
            Vector2 sSize = new Vector2(size.X * scale.X, size.Y * scale.Y);
            return sSize;
        }        
    }

    //---------------------------------------//
    //-----         DELEGATES           -----//
    //---------------------------------------// 

    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos para o resultado de uma colisão entre entidades.
    /// </summary>
    /// <param name="source">A entidade que implementa este delegate</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="intersection">A área de intersecção entre as duas entidades.</param>
    /// <param name="collidedEntity">A entidade que recebeu a colisão.</param>
    public delegate void CollisionAction(Entity2D source, GameTime gameTime, Rectangle intersection, Entity2D collidedEntity);

    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos para ser uma entidade atualizável.
    /// </summary>
    /// <typeparam name="T">Um tipo que implementa este delegate.</typeparam>
    /// <param name="source">Um tipo que implementa este delegate.</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    public delegate void UpdateAction<in T>(T source, GameTime gameTime);

    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos para ser uma entidade desenhável.
    /// </summary>
    /// <typeparam name="T">Um tipo que implementa este delegate.</typeparam>
    /// <param name="source">Um tipo que implementa este delegate.</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="spriteBatch">Um objeto SpriteBatch para desenho do jogo.</param>
    public delegate void DrawAction<in T>(T source, GameTime gameTime, SpriteBatch spriteBatch);
}