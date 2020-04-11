using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada que se repete ao infinito nos eixos X e Y.
    /// </summary>
    class InfiniteLayer : Layer
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        //Representam a máxima posição onde a câmera pode ir
        private readonly int max_x_viewport = -2000000000;
        private readonly int max_y_viewport = -2000000000;

        private float infPositionStartX = 0;
        private float infPositionStartY = 0;

        private Vector2 oldPosition = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private float dleft, dright, dup, ddown = 0;
        private int top, left, right, bottom = 0;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define se a animação será repetida ao infinito no eixo X.</summary>
        public bool InfinityX { get; set; } = true;
        /// <summary>Obtém ou define se a animação será repetida ao infinito no eixo Y.</summary>
        public bool InfinityY { get; set; } = true;

        /// <summary>Obtém ou define o máximo de deslocamento da camada para cima caso InfinityY seja false.</summary>
        public int Up { get => top; set => top = MathHelper.Clamp(value, 0, int.MaxValue - 1); }
        /// <summary>Obtém ou define o máximo de deslocamento da camada para baixo caso InfinityY seja false.</summary>
        public int Down { get => bottom; set => bottom = MathHelper.Clamp(value, 0, int.MaxValue - 1); }
        /// <summary>Obtém ou define o máximo de deslocamento da camada para esquerda caso InfinityX seja false.</summary>
        public int Left { get => left; set => left = MathHelper.Clamp(value, 0, int.MaxValue - 1); }
        /// <summary>Obtém ou define o máximo de deslocamento da camada para direita caso InfinityX seja false.</summary>
        public int Right { get => right; set => right = MathHelper.Clamp(value, 0, int.MaxValue - 1); }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe FixedLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>        
        public InfiniteLayer(Screen screen) : base(screen)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe InfiniteLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        /// <param name="animation">A animação a ser exibida na camada.</param>
        public InfiniteLayer(Screen screen, Animation animation) : base(screen, animation)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe InfiniteLayer como cópia de outro InfiniteLayer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public InfiniteLayer(InfiniteLayer source) : base(source)
        {
            this.Up = source.Up;
            this.Down = source.Down;
            this.Left = source.Left;
            this.Right = source.Right;
            this.InfinityX = source.InfinityX;
            this.InfinityY = source.InfinityY;
        }

        /// <summary>
        /// Cria uma nova instância da camada quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">A entidade a ser copiada.</param>
        public override T Clone<T>(T source)
        {
            if (source is InfiniteLayer)
                return (T)Activator.CreateInstance(typeof(InfiniteLayer), source);
            else
                throw new InvalidCastException();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            //base.SetDefaultView();

            if (InfinityX)
            {
                //Só recebe o valor da posição da câmera no eixo X.
                var pos = layerCamera.Position;
                pos.X = Screen.Camera.Position.X * Parallax;
                layerCamera.Position = pos;                

                //Recebe os valores para o cálculo.
                Point v1 = new Point(max_x_viewport, 0);
                Point v2 = layerCamera.Position.ToPoint();
                int aw = Animation.Bounds.Width;

                //Cálculos

                //Distância entre a o eixo X da view e o eixo X da câmera, R = (v1.X - v2.X)
                int r = v1.X - v2.X;
                //Quantidade de animações dentro dessa distância, Q = |R| / aw
                int q = Math.Abs(r) / aw;
                //Posição para início do desenho, Ps = (aw * q) - aw
                int ps = Math.Abs(v1.X) - (aw * q);

                infPositionStartX = -ps;

                if (!InfinityY)
                    infPositionStartY = Animation.Position.Y;

                if (!InfinityY)
                    layerCamera.Y = CalCamera().Y;
            }
            if (InfinityY)
            {
                var pos = layerCamera.Position;
                pos.Y = Screen.Camera.Position.Y * Parallax;
                layerCamera.Position = pos;                

                //Point v1 = view.Bounds.Location;
                Point v1 = new Point(0, max_y_viewport);
                Point v2 = layerCamera.Position.ToPoint();
                int ah = Animation.Bounds.Height;

                //Distância entre a o eixo X da view e o eixo X da câmera, R = (v1.Y - v2.Y)
                int r = v1.Y - v2.Y;
                //Quantidade de animações dentro dessa distância, Q = |R| / ah
                int q = Math.Abs(r) / ah;
                //Posição para início do desenho Ps = (aw * q) - aw
                int ps = Math.Abs(v1.Y) - (ah * q);

                infPositionStartY = -ps;

                if(!InfinityX)
                    infPositionStartX = Animation.Position.X;

                if (!InfinityX)
                    layerCamera.X = CalCamera().X;
            }

            base.Update(gameTime);
        }

        private Camera CalCamera()
        {
            oldPosition = position;
            position = Screen.Camera.Position;

            Vector2 diff = position - oldPosition;
            Camera c = layerCamera;

            if(!InfinityX)
            {
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
            }
            if(!InfinityY)
            {
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
            }

            return c;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            GraphicsDevice device = Screen.Game.GraphicsDevice;
            Viewport oldView = device.Viewport;
            device.Viewport = View;

            //Desenhamos na tela.
            spriteBatch.Begin(transformMatrix: layerCamera.GetTransform());

            //O desenho caso seja para o infinito.
            if (InfinityX || InfinityY)
            {
                //Recebe os valores para o infinito em X
                int sw = View.Width;
                int aw = Animation.Bounds.Width;                

                int rw = sw / aw;
                var posx = infPositionStartX;

                //Em Y
                int sh = View.Height;
                int ah = Animation.Bounds.Height;
                var posy = infPositionStartY;

                float total = (posy + ah) + sh;

                //Uma correção para não ter animação duplicada.
                if (!InfinityY)
                    total = posy + ah;

                while (posy < total)
                {
                    for (int ix = 0; ix <= rw + 1; ix++)
                    {
                        Animation.Position = new Vector2(posx, posy);
                        Animation.Draw(gameTime, spriteBatch);
                        posx += aw;

                        //Uma correção para não ter animação duplicada.
                        if (!InfinityX)
                            ix = rw;
                    }

                    posx = infPositionStartX;
                    posy += ah;
                }
            }
            else
            {
                Animation.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            //Recuperamos a viewport originária da tela.
            device.Viewport = oldView;

            base.Draw(gameTime, spriteBatch);
        }
    }
}