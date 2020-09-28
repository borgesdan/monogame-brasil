// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada de uma tela que se repete ao infinito nos eixos X e Y.
    /// </summary>
    class InfiniteLayer : ScreenLayer
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
        /// <summary>Obtém ou define o ator a ser exibido.</summary>
        public Actor Actor { get; set; } = null;
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
        /// Inicializa uma nova instância da classe InfiniteLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        /// <param name="actor">O ator a ser exibido na camada</param>
        public InfiniteLayer(Screen screen, Actor actor) : base(screen)
        {
            Actor = actor;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe InfiniteLayer como cópia de outro InfiniteLayer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public InfiniteLayer(InfiniteLayer source) : base(source)
        {
            this.Actor = source.Actor;
            this.Up = source.Up;
            this.Down = source.Down;
            this.Left = source.Left;
            this.Right = source.Right;
            this.InfinityX = source.InfinityX;
            this.InfinityY = source.InfinityY;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

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
                layerCamera.SetPosition(pos);

                //Recebe os valores para o cálculo.
                Point v1 = new Point(max_x_viewport, 0);
                Point v2 = layerCamera.Position.ToPoint();
                int aw = Actor.Bounds.Width;

                //Cálculos

                //Distância entre a o eixo X da view e o eixo X da câmera, R = (v1.X - v2.X)
                int r = v1.X - v2.X;
                //Quantidade de animações dentro dessa distância, Q = |R| / aw
                int q = Math.Abs(r) / aw;
                //Posição para início do desenho, Ps = (aw * q) - aw
                int ps = Math.Abs(v1.X) - (aw * q);

                infPositionStartX = -ps;

                if (!InfinityY)
                    infPositionStartY = Actor.Transform.Position.Y;

                if (!InfinityY)
                    layerCamera.Y = CalcCamera().Y;
            }
            if (InfinityY)
            {
                var pos = layerCamera.Position;
                pos.Y = Screen.Camera.Position.Y * Parallax;
                layerCamera.SetPosition(pos);

                //Point v1 = view.Bounds.Location;
                Point v1 = new Point(0, max_y_viewport);
                Point v2 = layerCamera.Position.ToPoint();
                int ah = Actor.Bounds.Height;

                //Distância entre a o eixo X da view e o eixo X da câmera, R = (v1.Y - v2.Y)
                int r = v1.Y - v2.Y;
                //Quantidade de animações dentro dessa distância, Q = |R| / ah
                int q = Math.Abs(r) / ah;
                //Posição para início do desenho Ps = (aw * q) - aw
                int ps = Math.Abs(v1.Y) - (ah * q);

                infPositionStartY = -ps;

                if (!InfinityX)
                    infPositionStartX = Actor.Transform.Position.X;

                if (!InfinityX)
                    layerCamera.X = CalcCamera().X;
            }

            Actor.Update(gameTime);
        }

        private Camera CalcCamera()
        {
            oldPosition = position;
            position = Screen.Camera.Position;

            Vector2 diff = position - oldPosition;
            Camera c = layerCamera;

            Viewport view = Screen.Game.GraphicsDevice.Viewport;

            if (!InfinityX)
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

                        if (Actor.Bounds.Width > view.Width)
                        {
                            if (c.X < Actor.Transform.Position.X - Left)
                            {
                                c.X = Actor.Transform.Position.X - Left;
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
                                c.X += absDiffX;
                                dleft = Left;
                                dright = -Right;
                            }
                        }
                    }

                    //se a câmera da tela se moveu para direita
                    if (diff.X > 0)
                    {
                        int cwidth = Screen.Game.Window.ClientBounds.Width;

                        if (Actor.Bounds.Width > view.Width)
                        {
                            if (c.X + cwidth > Actor.Bounds.Width + Right)
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
                                c.X -= absDiffX;
                                dright = Right;
                                dleft = -Left;
                            }
                        }
                    }
                }
            }
            if (!InfinityY)
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

                        if (Actor.Bounds.Height > view.Height)
                        {
                            if (c.Y < Actor.Transform.Position.Y - Up)
                            {
                                c.Y = Actor.Transform.Position.Y - Up;
                            }
                        }
                        else
                        {
                            //dright -= absDiffX;
                            dup += absDiffY;

                            //Se passou do limite definido                        
                            if (dup > Up)
                            {
                                c.Y += absDiffY;
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

                        if (Actor.Bounds.Height > view.Height)
                        {
                            if (c.Y + cheight > Actor.Bounds.Height + Down)
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
                                c.Y -= absDiffY;
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

            Viewport view = Screen.Game.GraphicsDevice.Viewport;

            //Desenhamos na tela.
            spriteBatch.Begin(SpriteBatchConfig.SortMode, SpriteBatchConfig.BlendState, SpriteBatchConfig.Sampler, SpriteBatchConfig.DepthStencil, SpriteBatchConfig.Rasterizer, SpriteBatchConfig.Effect, transformMatrix: layerCamera.GetTransform());

            //O desenho caso seja para o infinito.
            if (InfinityX || InfinityY)
            {
                //Recebe os valores para o infinito em X
                int sw = view.Width;
                int aw = Actor.Bounds.Width;

                int rw = sw / aw;
                var posx = infPositionStartX;

                //Em Y
                int sh = view.Height;
                int ah = Actor.Bounds.Height;
                var posy = infPositionStartY;

                float total = (posy + ah) + sh;

                //Uma correção para não ter animação duplicada.
                if (!InfinityY)
                    total = posy + ah;

                while (posy < total)
                {
                    for (int ix = 0; ix <= rw + 1; ix++)
                    {
                        Actor.Transform.Position = new Vector2(posx, posy);
                        Actor.Draw(gameTime, spriteBatch);
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
                Actor.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}