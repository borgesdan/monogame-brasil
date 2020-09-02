// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework
{
    public interface IUpdate
    {
        /// <summary>Atualiza o objeto.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        void Update(GameTime gameTime);
    }
}
