// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa uma entidade que expõe acesso a transformações e outras propriedades de jogo.
    /// </summary>
    public abstract class Actor : IActor<Actor>
    {
        bool offView = false;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        
        /// <summary>Obtém ou define a tela a qual a entidade está associada.</summary>
        public Screen Screen { get; set; } = null;
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
        public ComponentGroup Components { get; set; } = null;
        /// <summary>
        /// Obtém ou define se o ator está habilitado a ser atualizado fora do campo de visão da câmera ou do Viewport.
        /// </summary>
        public bool UpdateOffView { get; set; } = true;

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
            this.Components = new ComponentGroup(this);
            this.Transform = new TransformGroup<Actor>(this);            
        }

        protected Actor(Actor source)
        {
            this.Screen = source.Screen;
            this.Name = source.Name;
            this.Game = source.Game;
            this.Bounds = source.Bounds;
            this.BoundsR = new Polygon(source.BoundsR);
            this.Components = new ComponentGroup(this, source.Components);
            this.Enable = new EnableGroup(source.Enable.IsEnabled, source.Enable.IsVisible);
            this.Transform = new TransformGroup<Actor>(this, source.Transform);
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
            if (!Enable.IsVisible)
                return;

            Components.Draw(gameTime, spriteBatch, ActorComponent.DrawPriority.Back);
            
            if(!offView)
                _Draw(gameTime, spriteBatch);

            OnDraw?.Invoke(this, gameTime, spriteBatch);
            
            Components.Draw(gameTime, spriteBatch, ActorComponent.DrawPriority.Forward);
        }

        protected virtual void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Atualiza o ator.
        /// </summary>
        /// <param name="gameTime">Obtém acesso aos tempos de jogo.</param>
        public void Update(GameTime gameTime)
        {
            //Não prossegue caso o ator não está disponível
            if (!Enable.IsEnabled)
                return;

            //Atualiza os limites do ator e verifica se ele se encontra dentro do campo de visão da tela
            UpdateBounds();
            CheckOffView();            

            //Se não é para atualizar caso o ator não esteja dentro do campo de visão e 
            //ele se encontra nesse estado então não prossegue.
            if (!UpdateOffView && offView)
                return;

            _Update(gameTime);
            OnUpdate?.Invoke(this, gameTime);

            Transform.Update();            
            Components.Update(gameTime);
            
            UpdateBounds();
        }

        protected virtual void _Update(GameTime gameTime)
        {
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
        /// <param name="frame">O recorte da textura.</param>
        /// <param name="data">O array de cores recebido da textura.</param>
        /// <param name="effects">Os efeitos a serem observados.</param>
        public static Color[] GetData(Rectangle frame, Color[] data, SpriteEffects effects)
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

        protected void CheckOffView()
        {
            if (Screen != null && Screen.Camera != null)
                offView = !Util.CheckFieldOfView(Screen.Camera, this.Bounds);
            else
                offView = !Util.CheckFieldOfView(Game.GraphicsDevice.Viewport, this.Bounds);
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
                this.BoundsR = null;
                this.Enable = null;
                this.Transform = null;
                this.OnUpdate = null;
                this.OnDraw = null;
            }

            disposed = true;
        }
    }
}