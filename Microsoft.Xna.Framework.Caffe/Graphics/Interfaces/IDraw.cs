﻿namespace Microsoft.Xna.Framework.Graphics
{
    public interface IDraw
    {
        /// <summary>Desenha o objeto.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
