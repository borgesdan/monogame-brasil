// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma entidade atualizável, com propriedades de transformação, e desenhável em tela.</summary>
    public abstract class Entity2D : IUpdateDrawable, IBoundsable, IDisposable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        protected bool disposed = false;
        /// <summary>Obtém ou define a disponilidade de atualização e desenho da entidade.</summary>
        private Vector2 percentage = Vector2.One;
        protected Polygon poly = new Polygon();

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém as propriedades de transformação.</summary>
        public TransformGroup Transform { get; protected set; }
        /// <summary>Obtém ou define a origem.</summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;
        /// <summary>Obtém ou define a origem no eixo X.</summary>
        public float Xo { get => Origin.X; set => Origin = new Vector2(value, Yo); }
        /// <summary>Obtém ou define a origem no eixo Y.</summary>
        public float Yo { get => Origin.Y; set => Origin = new Vector2(Xo, value); }
        /// <summary>Obtém ou define o LayerDepth para o método Draw.</summary>
        public float LayerDepth { get; set; } = 0;
        /// <summary>Obtém os limites da entidade.</summary>
        public Rectangle Bounds { get; protected set; } = Rectangle.Empty;
        /// <summary>Obtém os limites rotacionados da entidade.</summary>
        public Polygon BoundsR { get; protected set; } = new Polygon();
        /// <summary>Obtém a instância atual da classe Game.</summary>
        public Game Game { get; set; } = null;
        /// <summary>Obtém ou define a disponibilidade de atualização e desenho da entidade.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup();
        /// <summary>Obtém ou define o nome da entidade.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém ou define se a entidade será atualizada fora dos limites de desenho da tela.</summary>
        public bool UpdateOutOfView { get; set; } = true;
        /// <summary>Obtém ou define a tela a qual a entidade está associada.</summary>
        public Screen Screen { get; set; } = null;
        /// <summary>Obtém ou define a lista de componentes da entidade.</summary>
        public ComponentGroup Components { get; private set; } = null;
        /// <summary>Obtém ou define a porcentagem de largura e altura do desenho. De 0f (0%) a 1f (100%).</summary>
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
        /// <summary>Obtém ou define a porcentagem de largura do desenho. De 0f (0%) a 1f (100%).</summary>
        public float XDraw { get => DrawPercentage.X; set => DrawPercentage = new Vector2(value, YDraw); }
        /// <summary>Obtém ou define a porcentagem de altura do desenho. De 0f (0%) a 1f (100%).</summary>
        public float YDraw { get => DrawPercentage.Y; set => DrawPercentage = new Vector2(XDraw, value); }

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
        }        

        /// <summary>Inicializa uma nova instância de Entity2D como cópia de outro Entity2D.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        protected Entity2D(Entity2D source)
        {
            Origin = source.Origin;
            Bounds = source.Bounds;
            Enable = new EnableGroup(source.Enable.IsEnabled, source.Enable.IsVisible);
            Game = source.Game;
            Transform = new TransformGroup(this, source.Transform);
            Name = source.Name;
            Screen = source.Screen;
            UpdateOutOfView = source.UpdateOutOfView;
            Components = new ComponentGroup(this, source.Components);
            DrawPercentage = source.DrawPercentage;
            BoundsR = new Polygon(source.BoundsR);            
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância da entidade quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">A entidade a ser copiada.</param>
        public abstract T Clone<T>(T source) where T : Entity2D;        

        /// <summary>Atualiza a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime)
        {
            Transform.Update();

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

            if (DEBUG.IsEnabled)
            {
                if (DEBUG.ShowBounds)
                {
                    poly.Set(BoundsR);
                    DEBUG.Polygons.Add(new Tuple<Polygon, Color>(BoundsR, DEBUG.BoundsColor));
                }
            }
        }                

        /// <summary>Atualiza os limites da entidade.</summary>
        public virtual void UpdateBounds() { }        

        /// <summary>
        /// Gera uma nova entidade copiada e com uma determinada posição.
        /// </summary>
        /// <typeparam name="T">O tipo da entidade.</typeparam>
        /// <param name="entity">A entidade a ser a origem da cópia.</param>
        /// <param name="position">A posição da entidade.</param>
        public static T Spawn<T>(T entity, Vector2 position) where T : Entity2D
        {
            T spawnEntity = entity.Clone(entity);
            spawnEntity.Transform.SetPosition(position);
            spawnEntity.UpdateBounds();

            return spawnEntity;
        }
        
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