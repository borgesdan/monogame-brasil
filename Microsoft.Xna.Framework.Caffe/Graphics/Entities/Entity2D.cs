// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma entidade atualizável, com propriedades de transformação, e desenhável em tela.</summary>
    public abstract class Entity2D : IUpdateDrawable, IBoundable, IDisposable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        protected bool disposed = false;
        private Vector2 percentage = Vector2.One;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        
        /// <summary>Obtém as propriedades de transformação.</summary>
        public TransformGroup Transform { get; protected set; }
        /// <summary>Obtém ou define a origem.</summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;
        /// <summary>Obtém ou define o LayerDepth para o método Draw.</summary>
        public float LayerDepth { get; set; } = 0;
        /// <summary>Obtém os limites da entidade.</summary>
        public Rectangle Bounds { get; protected set; } = Rectangle.Empty;
        /// <summary>Obtém os limites rotacionados da entidade.</summary>
        public Polygon BoundsR { get; protected set; } = new Polygon();
        /// <summary>Obtém a instância atual da classe Game.</summary>
        public Game Game { get; set; } = null;
        /// <summary>Obtém ou define a visibilidade da entidade.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);        
        /// <summary>Obtém ou define o nome da entidade.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém ou define se a entidade será atualizada fora dos limites de desenho da tela.</summary>
        public bool UpdateOutOfView { get; set; } = true;
        /// <summary>Obtém ou define a tela a qual a entidade está associada.</summary>
        public Screen Screen { get; set; } = null;
        /// <summary>Obtém ou define a lista de componentes da entidade.</summary>
        public ComponentGroup Components { get; private set; } = null;
        /// <summary>
        /// Obtém ou define a porcentagem de largura e altura do desenho. De 0F a 1F.
        /// <para>
        /// O valor 1F em X e Y representa 100% do desenho. A aplicação desta propriedade
        /// depende da entidade, de sua implementação e da capacidade em ser desenhada.
        /// </para>
        /// </summary>
        public Vector2 DrawPercentage
        {
            get => percentage;
            set
            {
                float x = MathHelper.Clamp(value.X, 0f, 1f);
                float y = MathHelper.Clamp(value.Y, 0f, 1f);

                percentage = new Vector2(x, y);
            }
        }

        //---------------------------------------//
        //-----         EVENTOS             -----//
        //---------------------------------------//

        /// <summary>Encapsula métodos que serão invocados na função Update.</summary>        
        public event UpdateAction<Entity2D> OnUpdate;
        /// <summary>Encapsula métodos que serão invocados na função Draw.</summary>
        public event DrawAction<Entity2D> OnDraw;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de Entity2D.</summary>
        /// <param name="game">Instância atual da classe Game.</param>
        /// <param name="name">Nome da entidade.</param>
        protected Entity2D(Game game, string name)
        {   
            Game = game;
            Transform = new TransformGroup(this);
            Name = name;
            Components = new ComponentGroup(this);

            Load();
        }

        /// <summary>Inicializa uma nova instância de Entity2D.</summary>
        /// <param name="screen">A tela em que a entidade será associada.</param>
        /// <param name="name">Nome da entidade.</param>
        protected Entity2D(Screen screen, string name) : this(screen.Game, name)
        {
            screen?.Add(this);
        }

        /// <summary>Inicializa uma nova instância de Entity2D copiando uma outra entidade.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        protected Entity2D(Entity2D source)
        {
            Origin = source.Origin;
            Bounds = source.Bounds;
            Enable = source.Enable;
            Game = source.Game;
            Transform = source.Transform;
            Name = source.Name;
            Screen = source.Screen;
            UpdateOutOfView = source.UpdateOutOfView;
            Components = source.Components;
            DrawPercentage = source.DrawPercentage;
            BoundsR = source.BoundsR;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>Carregar essa entidade com seus valores definidos.</summary>
        public virtual void Load() { }

        /// <summary>Atualiza a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime)
        {
            //Define a velocidade da entidade.

            if (Transform.Xv != 0)
                Transform.X += Transform.Velocity.X;
            if (Transform.Yv != 0)
                Transform.Y += Transform.Velocity.Y;

            //Chama o evento.
            OnUpdate?.Invoke(this, gameTime);

            //Atualiza os componentes.
            Components.Update(gameTime);

            //Atualiza os limites da entidade.
            UpdateBounds();
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            OnDraw?.Invoke(this, gameTime, spriteBatch);
            Components.Draw(gameTime, spriteBatch);            
        }        

        /// <summary>Atualiza os limites da entidade.</summary>
        public virtual void UpdateBounds() { }
        
        /// <summary>Libera os recursos dessa instância.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if(disposing)
            {
                Transform = null;
                Enable = null;
                Game = null;
                Screen = null;
                Name = null;
                Components.List.Clear();
                Components = null;
            }

            disposed = true;
        } 
    }
}