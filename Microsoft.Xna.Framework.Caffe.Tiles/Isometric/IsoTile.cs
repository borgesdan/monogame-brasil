// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um tile isométrico.
    /// </summary>
    public class IsoTile : IUpdateDrawable, IDisposable
    {
        /// <summary>Obtém ou define o ator que representa o Tile.</summary>
        public Actor Actor { get; set; } = null;
        /// <summary>Obtém ou define onde o tile se encontra em um mapa.</summary>
        public Point MapPoint { get; set; } = Point.Zero;
        /// <summary>Obtém ou define um valor para o Tile.</summary>
        public short Value { get; set; } = 0;        

        /// <summary>
        /// Inicializa uma nova instância de IsoTile.
        /// </summary>
        public IsoTile() { }

        /// <summary>
        /// Inicializa uma nova instância de IsoTile.
        /// </summary>
        /// <param name="actor">Define o ator do Tile</param>
        public IsoTile(Actor actor)
        {
            Actor = actor;
        }   

        /// <summary>
        /// Inicializa uma nova instância de Tile como cópia de outra instância de Tile.
        /// </summary>
        /// <param name="source">A instância de origem.</param>
        public IsoTile(IsoTile source)
        {
            //Actor = source.Actor;
            Actor = Util.Clone(source.Actor, source.Actor);
            MapPoint = source.MapPoint;
            Value = source.Value;
        }

        /// <summary>
        /// Atualiza o tamanho da animação.
        /// </summary>
        public Rectangle UpdateBounds()
        {
            Actor?.UpdateBounds();
            return Actor.Bounds;
        }

        /// <summary>
        /// Retorna os limites atuais da animação.
        /// </summary>
        public Rectangle GetBounds()
        {
            return Actor.Bounds;
        }

        ///<inheritdoc/>
        public void Update(GameTime gameTime)
        {
            Actor?.Update(gameTime);
        }
        
        ///<inheritdoc/>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Actor?.Draw(gameTime, spriteBatch);
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//

        private bool disposed = false;

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.Actor = null;
            }

            disposed = true;
        }
    }
}