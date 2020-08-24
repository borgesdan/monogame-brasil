﻿// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um tile isométrico.
    /// </summary>
    public class IsometricTile : IUpdateDrawable, IDisposable
    {
        private static bool defSize = false;
        private static int tWidth = 170;
        private static int tHeight = 85;

        /// <summary>Obtém ou define a largura dos tiles para cálculos posteriores.</summary>
        public static int TileWidth 
        { 
            get => tWidth; 
            set
            {
                tWidth = value;
                defSize = true;
            }
        }
        /// <summary>Obtém ou define a altura dos tiles para cálculos posteriores.</summary>
        public static int TileHeight 
        { 
            get => tHeight; 
            set
            {
                tHeight = value;
                defSize = true;
            }
        } 

        /// <summary>Obtém ou define a animação do topo do Tile.</summary>
        public Animation Animation { get; set; } = null;
        /// <summary>Obtém ou define onde o tile se encontra no mapa.</summary>
        public Point MapPoint { get; set; } = Point.Zero;
        /// <summary>Obtém ou define um valor para o Tile.</summary>
        public short Value { get; set; } = 0;        

        /// <summary>
        /// Inicializa uma nova instância de Tile.
        /// </summary>
        public IsometricTile() { }

        /// <summary>
        /// Inicializa uma nova instância de Tile.
        /// </summary>
        /// <param name="animation">Define a animação do Tile</param>
        public IsometricTile(Animation animation)
        {
            Animation = animation;
            SetSize();               
        }

        /// <summary>
        /// Inicializa uma nova instância de Tile.
        /// </summary>
        /// <param name="game">A instância corrente da classe Game.</param>
        /// <param name="sprite">O objeto sprite a ser utilizado para criar uma animação.</param>
        /// <param name="frames">Os frames do sprite.</param>
        public IsometricTile(Game game, Sprite sprite, params SpriteFrame[] frames)
        {
            Animation anm = new Animation(game, 0, "default");

            if (frames != null && frames.Length > 0)
                anm.AddSprite(sprite, frames);
            else
                anm.AddSprites(sprite);

            Animation = anm;

            SetSize();
        }

        private void SetSize()
        {
            if (!defSize)
            {
                Animation.UpdateBounds();

                TileWidth = Animation.Width;
                TileHeight = Animation.Height;

                defSize = true;
            }
        }

        /// <summary>
        /// Define o tamanho dos tiles para cálculos posteriores.
        /// </summary>
        /// <param name="width">A largura.</param>
        /// <param name="height">A altura.</param>
        public static void SetTileSize(int width, int height)
        {
            TileWidth = width;
            TileHeight = height;
        }

        /// <summary>
        /// Inicializa uma nova instância de Tile como cópia de outra instância de Tile.
        /// </summary>
        /// <param name="source">A instância de origem.</param>
        public IsometricTile(IsometricTile source)
        {
            if(source.Animation != null)
                Animation = new Animation(source.Animation);

            MapPoint = source.MapPoint;
            Value = source.Value;
        }

        /// <summary>
        /// Atualiza o tamanho da animação.
        /// </summary>
        public Rectangle UpdateBounds()
        {
            Animation?.UpdateBounds();
            return Animation.Bounds;
        }

        /// <summary>
        /// Retorna os limites atuais da animação.
        /// </summary>
        public Rectangle GetBounds()
        {
            return Animation.Bounds;
        }

        ///<inheritdoc/>
        public void Update(GameTime gameTime)
        {
            Animation?.Update(gameTime);
        }
        
        ///<inheritdoc/>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Animation?.Draw(gameTime, spriteBatch);
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
                this.Animation = null;
            }

            disposed = true;
        }
    }
}