using System;

namespace Microsoft.Xna.Framework.Graphics
{
    public abstract class Actor : IActor<Actor>
    {
        Rectangle oldFrame = new Rectangle();
        SpriteEffects oldEffects = SpriteEffects.None;
        Color[] currentColor = null;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>
        /// Obtém ou define o nome do ator.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Obtém a instância corrente da classe Game.
        /// </summary>
        public Game Game { get; protected set; } = null;
        /// <summary>
        /// Obtém ou define a disponibilidade do ator.
        /// </summary>
        public EnableGroup Enable { get; set; } = new EnableGroup();
        /// <summary>
        /// Obtém os limites do ator.
        /// </summary>
        public Rectangle Bounds { get; set; }
        /// <summary>
        /// Obtém ou define as transformações do ator.
        /// </summary>
        public TransformGroup<Actor> Transform { get; set; } = null;
        /// <summary>
        /// Obtém ou define os limites rotacionados do ator.
        /// </summary>
        public Polygon BoundsR { get; protected set; } = new Polygon();
        /// <summary>
        /// Obtém ou define os componentes do ator.
        /// </summary>
        public ComponentGroup<Actor> Components { get; set; } = null;        

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        public Actor(Game game)
        {
            this.Game = game;
            this.Components = new ComponentGroup<Actor>(this);
            this.Transform = new TransformGroup<Actor>(this);            
        }

        public Actor(Actor source)
        {
            this.Name = source.Name;
            this.Game = source.Game;
            this.Bounds = source.Bounds;
            this.BoundsR = new Polygon(source.BoundsR);
            this.Components = new ComponentGroup<Actor>(this, source.Components);
            this.Enable = new EnableGroup(source.Enable.IsEnabled, source.Enable.IsVisible);
            this.Transform = new TransformGroup<Actor>(this, source.Transform);            
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//        

        /// <summary>
        /// Desenha o ator.
        /// </summary>
        /// <param name="gameTime">Obtém acesso aos tempos de jogo.</param>
        /// <param name="spriteBatch">A instância do SpriteBatch para desenho.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            Components.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Atualiza o ator.
        /// </summary>
        /// <param name="gameTime">Obtém acesso aos tempos de jogo.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            Transform.Update();
            Components.Update(gameTime);
        }

        /// <summary>
        /// Atualiza os limites do ator.
        /// </summary>
        public virtual void UpdateBounds()
        {            
        }

        /// <summary>
        /// Obtém o conteúdo de cores do ator.
        /// </summary>
        public abstract Color[] GetData();
        
        /// <summary>
        /// Obtém o conteúdo de cores passando o frame (com o tamanho do data) e o Color data.
        /// </summary>
        protected Color[] GetDataHelper(Rectangle frame, Color[] data)
        {
            //Retorna o último array de cores caso as propriedades não tenham sido mudadas
            if (currentColor != null
                && oldFrame == frame
                && oldEffects == Transform.SpriteEffects)
            {
                return currentColor;
            }

            if (Transform.SpriteEffects == SpriteEffects.None)
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

                if (Transform.SpriteEffects == (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically))
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
                else if (Transform.SpriteEffects == SpriteEffects.FlipHorizontally)
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
                else if(Transform.SpriteEffects == SpriteEffects.FlipVertically)
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

                currentColor = final;
                oldEffects = Transform.SpriteEffects;
                oldFrame = frame;

                return currentColor;
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
                this.Game = null;
                this.Name = null;
                this.BoundsR = null;
                this.Enable = null;
                this.Transform = null;
                this.currentColor = null;
            }

            disposed = true;
        }
    }
}