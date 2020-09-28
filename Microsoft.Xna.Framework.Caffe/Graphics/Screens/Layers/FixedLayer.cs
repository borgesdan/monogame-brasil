// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada de uma tela com uma posição fixa, mas que pode
    /// se mover até um determinado limite.
    /// </summary>
    public class FixedLayer : ScreenLayer
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        private Vector2 move = Vector2.Zero;
        private Camera fixedCamera = Camera.Create();
        private Vector2 oldPosition = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private int top, left, right, bottom = 0;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>
        /// Obtém ou define a lista de atores da camada.
        /// </summary>
        public List<Actor> Actors = new List<Actor>();
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
        /// <param name="actors">O atores a serem exibidos na camada.</param>
        public FixedLayer(Screen screen, params Actor[] actors) : this(screen, 0, 0, 0, 0, actors) { }

        /// <summary>
        /// Inicializa uma nova instância da classe FixedLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        /// <param name="up">Define o máximo de deslocamento da camada para cima.</param>
        /// <param name="down">Define o máximo de deslocamento da camada para baixo.</param>
        /// <param name="left">Define o máximo de deslocamento da camada para direita.</param>
        /// <param name="right">Define o máximo de deslocamento da camada para esquerda.</param>
        /// <param name="actors">O atores a serem exibidos na camada.</param>
        public FixedLayer(Screen screen, int up, int down, int left, int right, params Actor[] actors) : base(screen)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;

            if (actors != null)
                Actors.AddRange(actors);
        }

        /// <summary>
        /// Inicializa uma nova instância da classe FixedLayer como cópia de outro FixedLayer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public FixedLayer(FixedLayer source) : base(source)
        {
            source.Actors.ForEach(a => this.Actors.Add(a));
            this.Up = source.Up;
            this.Down = source.Down;
            this.Left = source.Left;
            this.Right = source.Right;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//
        
        /// <summary>Atualiza a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            oldPosition = position;
            position = Screen.Camera.Position;            

            Vector2 diff = position - oldPosition;
            move += diff * Parallax;

            if (move.X > Right)
            {
                fixedCamera.X = Right;
                move.X = Right;
            }                
            else if (move.X < -Left)
            {
                fixedCamera.X = -Left;
                move.X = -Left;
            }                
            else
            {
                fixedCamera.X = move.X;
            }                

            if (move.Y < -Up)
            {
                fixedCamera.Y = -Up;
                move.Y = -Up;
            }                
            else if (move.Y > Down)
            {
                fixedCamera.Y = Down;
                move.Y = Down;
            }                
            else
            {
                fixedCamera.Y = move.Y;
            }
                

            for (int i = 0; i < Actors.Count; i++)
            {
                if (Actors[i].Enable.IsEnabled)
                {
                    Actors[i].Update(gameTime);
                }
            }

            fixedCamera.Zoom = Screen.Camera.Zoom;

                    //for(int i = 0; i < Actors.Count; i++)
                    //{
                    //    if(Actors[i].Enable.IsEnabled)
                    //    {
                    //        //se a câmera da tela se moveu lateralmente
                    //        if (diff.X != 0)
                    //        {
                    //            c.Move(diff.X * Parallax, 0);
                    //            float absDiffX = Math.Abs(diff.X * Parallax);

                    //            //se a câmera da tela se moveu para esquerda
                    //            if (diff.X < 0)
                    //            {
                    //                dright = dright > 0 ? dright + diff.X : 0;

                    //                if (Actors[i].Bounds.Width > View.Width)
                    //                {
                    //                    if (c.X < Actors[i].Transform.Position.X - Left)
                    //                    {
                    //                        c.X = Actors[i].Transform.Position.X - Left;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    //dright -= absDiffX;
                    //                    dleft += absDiffX;

                    //                    //Se passou do limite definido                        
                    //                    if (dleft > Left)
                    //                    {
                    //                        //c.Position.X = Animation.Bounds.X - Left;
                    //                        c.Position.X += absDiffX;
                    //                        dleft = Left;
                    //                        dright = -Right;
                    //                    }
                    //                }
                    //            }

                    //            //se a câmera da tela se moveu para direita
                    //            if (diff.X > 0)
                    //            {
                    //                int cwidth = Screen.Game.Window.ClientBounds.Width;

                    //                if (Actors[i].Bounds.Width > View.Width)
                    //                {
                    //                    if (c.X + cwidth > Actors[i].Bounds.Width + Right)
                    //                    {
                    //                        c.X -= absDiffX;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    dright += absDiffX;

                    //                    //Se passou do limite definido                        
                    //                    if (dright > Right)
                    //                    {
                    //                        c.Position.X -= absDiffX;
                    //                        dright = Right;
                    //                        dleft = -Left;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        //se a câmera da tela se moveu verticalmente
                    //        if (diff.Y != 0)
                    //        {
                    //            c.Move(0, diff.Y * Parallax);
                    //            float absDiffY = Math.Abs(diff.Y * Parallax);

                    //            //se a câmera da tela se moveu para cima
                    //            if (diff.Y < 0)
                    //            {
                    //                ddown = ddown > 0 ? ddown + diff.Y : 0;

                    //                if (Actors[i].Bounds.Height > View.Height)
                    //                {
                    //                    if (c.Y < Actors[i].Transform.Position.Y - Up)
                    //                    {
                    //                        c.Y = Actors[i].Transform.Position.Y - Up;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    //dright -= absDiffX;
                    //                    dup += absDiffY;

                    //                    //Se passou do limite definido                        
                    //                    if (dup > Up)
                    //                    {
                    //                        c.Position.Y += absDiffY;
                    //                        dup = Up;
                    //                        ddown = -Down;
                    //                    }
                    //                }
                    //            }
                    //            //se a câmera da tela se moveu para direita
                    //            if (diff.Y > 0)
                    //            {
                    //                dup = dup > 0 ? dup - diff.Y : 0;

                    //                int cheight = Screen.Game.Window.ClientBounds.Height;

                    //                if (Actors[i].Bounds.Height > View.Height)
                    //                {
                    //                    if (c.Y + cheight > Actors[i].Bounds.Height + Down)
                    //                    {
                    //                        c.Y -= absDiffY;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    ddown += absDiffY;

                    //                    //Se passou do limite definido                        
                    //                    if (ddown > Down)
                    //                    {
                    //                        c.Position.Y -= absDiffY;
                    //                        ddown = Down;
                    //                        dup = -Up;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        layerCamera = c;

                    //        Actors[i].Update(gameTime);
                    //    }                
                    //}            
                }

        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;            

            //Desenhamos na tela.
            spriteBatch.Begin(SpriteBatchConfig.SortMode, SpriteBatchConfig.BlendState, SpriteBatchConfig.Sampler, SpriteBatchConfig.DepthStencil, SpriteBatchConfig.Rasterizer, SpriteBatchConfig.Effect, transformMatrix: fixedCamera.GetTransform());

            for (int i = 0; i < Actors.Count; i++)
            {
                if (Actors[i].Enable.IsVisible)
                {
                    Actors[i].Draw(gameTime, spriteBatch);
                }
            }

            spriteBatch.End();
            
        }
    }
}