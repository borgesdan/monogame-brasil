﻿// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa uma entidade que expõe acesso a transformações e outras propriedades de jogo.
    /// </summary>
    public abstract class Actor : IActor
    {
        protected Rectangle bounds = Rectangle.Empty;
        protected Polygon boundsR = new Polygon();

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        
        /// <summary>Obtém ou define a tela a qual a entidade está associada.</summary>
        public Screen Screen { get; set; } = null;
        /// <summary>Obtém ou define o nome do ator.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém a instância corrente da classe Game.</summary>
        public Game Game { get; protected set; } = null;
        /// <summary>Obtém ou define a disponibilidade do ator.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup();
        /// <summary>Obtém os limites do ator.</summary>
        public Rectangle Bounds 
        { 
            get 
            {
                UpdateBounds();
                return bounds;
            }
        }
        /// <summary>Obtém ou define as transformações do ator.</summary>
        public TransformGroup Transform { get; set; } = new TransformGroup();
        /// <summary>Obtém ou define os limites rotacionados do ator.</summary>
        public Polygon BoundsR
        {
            get
            {
                UpdateBounds();
                return boundsR;
            }
        }
        /// <summary>Obtém ou define os componentes do ator.</summary>
        public ComponentGroup Components { get; set; } = null;       

        //---------------------------------------//
        //-----         EVENTOS             -----//
        //---------------------------------------//

        /// <summary>Encapsula métodos que serão invocados na função Update.</summary>        
        public event Action<Actor, GameTime> OnUpdate;
        /// <summary>Encapsula métodos que serão invocados na função Draw.</summary>
        public event Action<Actor, GameTime, SpriteBatch> OnDraw;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        protected Actor(Game game, string name)
        {
            this.Name = name;
            this.Game = game;
            this.Components = new ComponentGroup();            
        }

        protected Actor(Actor source)
        {
            this.Screen = source.Screen;
            this.Name = source.Name;
            this.Game = source.Game;            
            this.bounds = source.bounds;            
            this.boundsR = new Polygon(source.boundsR);
            this.Components = new ComponentGroup(source.Components);
            this.Enable = new EnableGroup(source.Enable.IsEnabled, source.Enable.IsVisible);
            this.Transform = new TransformGroup(source.Transform);
            this.OnUpdate = source.OnUpdate;
            this.OnDraw = source.OnDraw;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//        

        /// <summary>
        /// Desenha o ator.
        /// </summary>
        /// <param name="gameTime">Obtém acesso aos tempos de jogo.</param>
        /// <param name="spriteBatch">A instância do SpriteBatch para desenho.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Enable.IsVisible)
            {
                Components.Draw(gameTime, spriteBatch, ActorComponent.DrawPriority.Back);
                _Draw(gameTime, spriteBatch);
                OnDraw?.Invoke(this, gameTime, spriteBatch);
                Components.Draw(gameTime, spriteBatch, ActorComponent.DrawPriority.Forward);
            }
        }

        /// <summary>
        /// Atualiza o ator.
        /// </summary>
        /// <param name="gameTime">Obtém acesso aos tempos de jogo.</param>
        public void Update(GameTime gameTime)
        {            
            if (Enable.IsEnabled)
            {
                UpdateBounds();
                _Update(gameTime);
                UpdateBounds();
                OnUpdate?.Invoke(this, gameTime);                
                Transform.Update();
                Components.Update(gameTime);                
            }
        }

        protected abstract void _Update(GameTime gameTime);
        protected abstract void _Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Atualiza os limites do ator.
        /// </summary>
        public abstract void UpdateBounds();

        /// <summary>
        /// Obtém o conteúdo de cores do ator se for disponível ou retorna null.
        /// </summary>
        public abstract Color[] GetData();

        /// <summary>
        /// Método de auxílio que calcula e define os limites do ator através de sua posição, escala e origem na propriedade Transform, como também 
        /// os limites rotacionados através de sua rotação e origem.
        /// </summary>
        /// <param name="width">Informa e define o valor da largura do ator.</param>
        /// <param name="height">Informa e define o valor da altura do ator.</param>
        /// <param name="amountOriginX">Define o valor que deve ser incrementado a propriedade Transform.Origin no eixo X, se necessário.</param>
        /// <param name="amountOriginY">Define o valor que deve ser incrementado a propriedade Transform.Origin no eixo Y, se necessário.</param>
        protected void CalcBounds(int width, int height, float amountOriginX, float amountOriginY)
        {
            Transform.Size = new Point(width, height);

            //Posição
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            //Escala
            float sx = Transform.Xs;
            float sy = Transform.Ys;
            //Origem
            float ox = Transform.Xo;
            float oy = Transform.Yo;

            //Obtém uma matrix com a posição e escala através de sua origem
            Matrix m = Matrix.CreateTranslation(-ox + -amountOriginX, -oy + -amountOriginY, 0)
                * Matrix.CreateScale(sx, sy, 1)
                * Matrix.CreateTranslation(x, y, 0);

            //Os limites finais
            Rectangle rec = new Rectangle((int)m.Translation.X, (int)m.Translation.Y, (int)Transform.ScaledWidth, (int)Transform.ScaledHeight);
            bounds = rec;

            //Os limites rotacionados            
            RotatedRectangle rotated = Rotation.GetRectangle(bounds, new Vector2(bounds.X + ox, bounds.Y + oy), Transform.Rotation);
            Polygon poly = new Polygon();
            poly.Set(rotated);

            boundsR = poly;
        }
        
        /// <summary>
        /// Obtém o conteúdo de cores passando o frame (com o tamanho do data) e o Color data.
        /// </summary>
        /// <param name="frame">O recorte da textura.</param>
        /// <param name="data">O array de cores recebido da textura.</param>
        /// <param name="effects">Os efeitos a serem observados.</param>
        protected static Color[] GetData(Rectangle frame, Color[] data, SpriteEffects effects)
        {
            if (effects == SpriteEffects.None)
            {
                //Se não há transformação retorna o array recebido.                
                return data;
            }
            else 
            {
                //Há transformação
                Color[,] colorArray = new Color[frame.Width, frame.Height]; // Array multidimensional para representar uma imagem
                Vector2 position = Vector2.Zero;                            // Posição do array
                int index = 0;                                              // A posição no array colors

                Color[] final = new Color[data.Length];                   // O array a ser enviado no final
                int finalIndex = 0;                                         // O index para percorrer o array final

                if (effects == (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically)) // | ou & ???
                {
                    position.X = frame.Width - 1;
                    position.Y = frame.Height - 1;
                    for (int h = 0; h < frame.Height; h++)
                    {
                        for (int w = 0; w < frame.Width; w++)
                        {
                            colorArray[(int)position.X, (int)position.Y] = data[index];
                            position.X -= 1;
                            index++;
                        }

                        position.Y -= 1;
                        position.X = frame.Width - 1;
                    }
                }
                else if (effects == SpriteEffects.FlipHorizontally)
                {
                    position.X = frame.Width - 1;
                    for (int h = 0; h < frame.Height; h++)
                    {
                        for (int w = 0; w < frame.Width; w++)
                        {
                            colorArray[(int)position.X, (int)position.Y] = data[index];

                            position.X -= 1;
                            index++;
                        }

                        position.Y += 1;
                        position.X = frame.Width - 1;
                    }
                }
                else if(effects == SpriteEffects.FlipVertically)
                {
                    position.Y = frame.Height - 1;
                    for (int h = 0; h < frame.Height; h++)
                    {
                        for (int w = 0; w < frame.Width; w++)
                        {
                            colorArray[(int)position.X, (int)position.Y] = data[index];

                            position.X += 1;
                            index++;
                        }

                        position.Y -= 1;
                        position.X = 0;
                    }
                }

                //Para cada coluna percorremos as linhas do array multidimensional
                for (int c = 0; c < colorArray.GetLength(1); c++)
                {
                    for (int l = 0; l < colorArray.GetLength(0); l++)
                    {
                        final[finalIndex] = colorArray[l, c];
                        finalIndex++;
                    }
                }

                return final;
            }
        }        

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//
        private bool disposed = false;

        /// <summary>
        /// Libera os recursos dessa instância.
        /// </summary>
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
                this.Screen = null;
                this.Game = null;
                this.Name = null;                
                this.boundsR = null;
                this.Enable = null;
                this.Transform = null;
                this.OnUpdate = null;
                this.OnDraw = null;
            }

            disposed = true;
        }
    }
}