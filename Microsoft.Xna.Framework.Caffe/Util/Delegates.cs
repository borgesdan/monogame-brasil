// Danilo Borges Santos, 2020.

using System;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos para o resultado de uma colisão entre entidades.
    /// </summary>
    /// <param name="source">A entidade que implementa este delegate</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="intersection">A área de intersecção entre as duas entidades.</param>
    /// <param name="collidedEntity">A entidade que recebeu a colisão.</param>
    public delegate void CollisionAction(Entity2D source, GameTime gameTime, CollisionResult result, Entity2D collidedEntity);    

    /// <summary>
    /// Encapsula um metodo que tem os seguintes parâmetros definidos e que expõe o resultado final de uma ação.
    /// </summary>
    /// <typeparam name="T">O tipo do resultado.</typeparam>
    /// <param name="source">A entidade que implementa este delegate</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="result">O resultado exposto da ação a ser exposto.</param>
    public delegate void ResultAction<in T>(Entity2D source, GameTime gameTime, T result);

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
