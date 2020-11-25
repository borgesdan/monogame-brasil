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
        private Camera fixedCamera = null;
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
            fixedCamera = new Camera(screen.Game);

            if (actors != null)
            {                
                foreach(var a in actors)
                {
                    Actors.Add(a);
                }
            }                
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
            this.fixedCamera = new Camera(source.fixedCamera);
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//
        
        /// <summary>Atualiza a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void _Update(GameTime gameTime)
        {
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
                Actors[i].Update(gameTime);
            }

            fixedCamera.Zoom = Screen.Camera.Zoom;
            base._Update(gameTime);
        }


        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Desenhamos na tela.
            spriteBatch.Begin(SpriteBatchConfig.SortMode, SpriteBatchConfig.BlendState, SpriteBatchConfig.Sampler, SpriteBatchConfig.DepthStencil, SpriteBatchConfig.Rasterizer, SpriteBatchConfig.Effect, transformMatrix: fixedCamera.GetTransform());

            for (int i = 0; i < Actors.Count; i++)
            {
                Actors[i].Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
            base._Draw(gameTime, spriteBatch);
        }
    }
}