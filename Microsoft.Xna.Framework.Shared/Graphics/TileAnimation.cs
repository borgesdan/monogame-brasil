// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

namespace Microsoft.Xna.Framework.Graphics
{
    public class TileAnimation : Animation
    {
        private int xr = 0;
        private int yr = 0;

        /// <summary>Obtém ou define o número de repetições no eixo X.</summary>
        public int XRepeat { get => xr; set => xr = MathHelper.Clamp(value, 0, int.MaxValue); }
        /// <summary>Obtém ou define o número de repetições no eixo Y.</summary>
        public int YRepeat { get => yr; set => yr = MathHelper.Clamp(value, 0, int.MaxValue); }
        
        /// <summary>Inicializa uma nova instância da classe Animation.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="time">O tempo de cada quadro da animação.</param>
        public TileAnimation(Game game, int time, string name) : base(game, time, name) { }

        /// <summary>Inicializa uma nova instância da classe Animation utilizando uma cópia profunda de uma animação como origem.</summary>
        /// <param name="source">A animação a ser copiada.</param>
        public TileAnimation(TileAnimation source) : base(source)
        {
            this.XRepeat = source.XRepeat;
            this.YRepeat = source.YRepeat;
        }

        protected override void UpdateBounds()
        {
            Rectangle currentFrame = CurrentSprite[FrameIndex].Bounds;

            int w = currentFrame.Width * XRepeat;
            int h = currentFrame.Height * YRepeat;

            currentFrame = new Rectangle(currentFrame.X, currentFrame.Y, w, h);

            Size = currentFrame.Size;
            Bounds = currentFrame;

            //Não chama a base
        }

        /// <summary>Método de desenho.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible || CurrentSprite == null)
                return;

            Rectangle _bounds = Bounds;

            if (useDestinationBounds)
                _bounds = destinationBounds;

            //Desenhar no eixo X.
            for (int x = 0; x < XRepeat; x++)
            {
                Vector2 position = Position;
                position.X += ScaledSize.X * x;

                spriteBatch.Draw(
                            texture: CurrentSprite.Texture,
                            position: position,
                            sourceRectangle: _bounds,
                            color: Color,
                            rotation: Rotation,
                            origin: origin,
                            scale: Scale,
                            effects: SpriteEffect,
                            layerDepth: LayerDepth
                            );

                //Desenhar no eixo Y.
                for (int y = 0; y < YRepeat; y++)
                {
                    position.Y += ScaledSize.Y;

                    spriteBatch.Draw(
                                texture: CurrentSprite.Texture,
                                position: position,
                                sourceRectangle: _bounds,
                                color: Color,
                                rotation: Rotation,
                                origin: origin + CurrentSprite.Frames[FrameIndex].OriginCorrection,
                                scale: Scale,
                                effects: SpriteEffect,
                                layerDepth: LayerDepth
                                );
                }
            }

            InvokeOnDraw(this, gameTime, spriteBatch);

            //Não chama a base.
        }
    }
}