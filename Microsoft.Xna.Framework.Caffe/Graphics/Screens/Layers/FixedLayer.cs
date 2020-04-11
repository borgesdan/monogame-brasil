using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada com uma posição fixa na tela, mas que pode
    /// se mover até um determinado limite.
    /// </summary>
    public class FixedLayer : Layer
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        private Vector2 oldPosition = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private float dleft, dright, dup, ddown = 0;
        private int top, left, right, bottom = 0;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define o máximo de deslocamento da camada para cima.</summary>
        public int Up { get => top; set => top = MathHelper.Clamp(value, 0, int.MaxValue - 1); }
        /// <summary>Obtém ou define o máximo de deslocamento da camada para baixo.</summary>
        public int Down { get => bottom; set => bottom = MathHelper.Clamp(value, 0, int.MaxValue - 1); }
        /// <summary>Obtém ou define o máximo de deslocamento da camada para esquerda.</summary>
        public int Left { get => left; set => left = MathHelper.Clamp(value, 0, int.MaxValue - 1); }
        /// <summary>Obtém ou define o máximo de deslocamento da camada para direita.</summary>
        public int Right { get => right; set => right = MathHelper.Clamp(value, 0, int.MaxValue - 1); }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe FixedLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>        
        public FixedLayer(Screen screen) : base(screen)
        {            
        }

        /// <summary>
        /// Inicializa uma nova instância da classe FixedLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        /// <param name="animation">A animação a ser exibida na camada.</param>
        public FixedLayer(Screen screen, Animation animation) : base(screen, animation)
        {            
        }

        /// <summary>
        /// Inicializa uma nova instância da classe FixedLayer como cópia de outro FixedLayer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public FixedLayer(FixedLayer source) : base(source)
        {
            this.Up = source.Up;
            this.Down = source.Down;
            this.Left = source.Left;
            this.Right = source.Right;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância da camada quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">A entidade a ser copiada.</param>
        public override T Clone<T>(T source)
        {
            if (source is FixedLayer)
                return (T)Activator.CreateInstance(typeof(FixedLayer), source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            //SetDefaultView();

            oldPosition = position;
            position = Screen.Camera.Position;

            Vector2 diff = position - oldPosition;
            Camera c = layerCamera;

            //se a câmera da tela se moveu lateralmente
            if (diff.X != 0)
            {
                c.Move(diff.X * Parallax, 0);
                float absDiffX = Math.Abs(diff.X * Parallax);

                //se a câmera da tela se moveu para esquerda
                if (diff.X < 0)
                {
                    dright = dright > 0 ? dright + diff.X : 0;

                    if (Animation.Bounds.Width > View.Width)
                    {
                        if (c.X < Animation.Position.X - Left)
                        {
                            c.X = Animation.Position.X - Left;
                        }
                    }
                    else
                    {
                        //dright -= absDiffX;
                        dleft += absDiffX;

                        //Se passou do limite definido                        
                        if (dleft > Left)
                        {
                            //c.Position.X = Animation.Bounds.X - Left;
                            c.Position.X += absDiffX;
                            dleft = Left;
                            dright = -Right;
                        }
                    }
                }                

                //se a câmera da tela se moveu para direita
                if (diff.X > 0)
                {
                    int cwidth = Screen.Game.Window.ClientBounds.Width;

                    if (Animation.Bounds.Width > View.Width)
                    {
                        if (c.X + cwidth > Animation.Bounds.Width + Right)
                        {
                            c.X -= absDiffX; 
                        }
                    }
                    else
                    {                        
                        dright += absDiffX;

                        //Se passou do limite definido                        
                        if (dright > Right)
                        {
                            c.Position.X -= absDiffX;
                            dright = Right;
                            dleft = -Left;
                        }
                    }
                }
            }

            //se a câmera da tela se moveu verticalmente
            if (diff.Y != 0)
            {
                c.Move(0, diff.Y * Parallax);
                float absDiffY = Math.Abs(diff.Y * Parallax);

                //se a câmera da tela se moveu para cima
                if (diff.Y < 0)
                {
                    ddown = ddown > 0 ? ddown + diff.Y : 0;                    

                    if (Animation.Bounds.Height > View.Height)
                    {
                        if (c.Y < Animation.Position.Y - Up)
                        {
                            c.Y = Animation.Position.Y - Up;
                        }
                    }
                    else
                    {
                        //dright -= absDiffX;
                        dup += absDiffY;

                        //Se passou do limite definido                        
                        if (dup > Up)
                        {                            
                            c.Position.Y += absDiffY;
                            dup = Up;
                            ddown = -Down;
                        }
                    }
                }
                //se a câmera da tela se moveu para direita
                if (diff.Y > 0)
                {
                    dup = dup > 0 ? dup - diff.Y : 0;

                    int cheight = Screen.Game.Window.ClientBounds.Height;

                    if (Animation.Bounds.Height > View.Height)
                    {
                        if (c.Y + cheight > Animation.Bounds.Height + Down)
                        {
                            c.Y -= absDiffY;
                        }
                    }
                    else
                    {
                        ddown += absDiffY;

                        //Se passou do limite definido                        
                        if (ddown > Down)
                        {
                            c.Position.Y -= absDiffY;
                            ddown = Down;
                            dup = -Up;
                        }
                    }
                }
            }

            layerCamera = c;

            base.Update(gameTime);
        }

        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            GraphicsDevice device = Screen.Game.GraphicsDevice;
            Viewport oldView = device.Viewport;
            device.Viewport = View;

            //Desenhamos na tela.
            spriteBatch.Begin(transformMatrix: layerCamera.GetTransform());
            Animation.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            //Recuperamos a viewport originária da tela.
            device.Viewport = oldView;

            base.Draw(gameTime, spriteBatch);
        }
    }
}