// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma entidade atualizável, com propriedades de transformação, e desenhável em tela.</summary>
    public class Entity2D : IUpdateDrawable, IDisposable
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
        /// <summary>Obtém a instância atual da classe Game.</summary>
        public Game Game { get; set; } = null;
        /// <summary>Obtém ou define a visibilidade da entidade.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);        
        /// <summary>Obtém ou define o nome da entidade.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém ou define se a entidade será atualizada fora dos limites da Viewport.</summary>
        public bool UpdateOutofView { get; set; } = true;
        /// <summary>Obtém ou define a tela a qual a entidade está associada.</summary>
        public Screen Screen { get; set; } = null;
        /// <summary>Obtém ou define a lista de componentes da entidade.</summary>
        public List<EntityComponent> Components { get; set; } = new List<EntityComponent>();
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
        public Entity2D(Game game, string name)
        {   
            Game = game;
            Transform = new TransformGroup(this);
            Name = name;

            Load();
        }  

        /// <summary>Inicializa uma nova instância de Entity2D.</summary>
        /// <param name="screen">A tela em que a entidade será associada.</param>
        /// <param name="name">Nome da entidade.</param>
        public Entity2D(Screen screen, string name) : this(screen.Game, name)
        {
            screen?.Add(this);
        }

        /// <summary>Inicializa uma nova instância de Entity2D copiando uma outra entidade.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        public Entity2D(Entity2D source)
        {
            Origin = source.Origin;
            Bounds = source.Bounds;
            Enable = source.Enable;
            Game = source.Game;
            Transform = source.Transform;
            Name = source.Name;
            Screen = source.Screen;
            UpdateOutofView = source.UpdateOutofView;
            Components = source.Components;
            DrawPercentage = source.DrawPercentage;
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
            if (!Enable.IsEnabled)
                return;           

            if (Transform.Xv != 0)
                Transform.X += Transform.Velocity.X;
            if(Transform.Yv != 0)
                Transform.Y += Transform.Velocity.Y;
                        
            foreach (var cmp in Components)
            {
                if(cmp.Entity == null)
                    cmp.Entity = this;

                cmp.Update(gameTime);
            }
            
            OnUpdate?.Invoke(this, gameTime);

            UpdateBounds();
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            foreach (var cmp in Components)
            {
                if (cmp.Entity == null)
                    cmp.Entity = this;

                cmp.Draw(gameTime, spriteBatch);
            }

            OnDraw?.Invoke(this, gameTime, spriteBatch);
        }

        //----- COMPONENTES -----//

        /// <summary>Obtém o primeiro componente encontrado seguindo o tipo informado.</summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        /// <returns>Retorna uma lista com todos os componentes encontrados.</returns>
        public List<T> GetAllComponents<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = Components.FindAll(x => x.GetType().Equals(t_type));

            List<T> temp = new List<T>();

            foreach (var f in find)
                temp.Add((T)f);

            return temp;
        }

        /// <summary>Obtém o primeiro componente encontrado seguindo o tipo informado.</summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        /// <returns>Retorna o primeiro componente encontrado na lista de Components.</returns>
        public T GetComponent<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = Components.Find(x => x.GetType().Equals(t_type));

            return (T)find;
        }

        /// <summary>
        /// Obtém o primeiro componente encontrado seguindo o tipo informado.
        /// </summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        /// <param name="internalName">O nome interno do componente. Normalmente utilizando nameof(T) onde 'T' é o tipo dele.</param>
        /// <returns>Retorna um componente através dos parâmetros solicitados.</returns>
        public T GetComponentByName<T>(string internalName) where T : EntityComponent
        {
            var find = Components.Find(x => x.Name.Equals(internalName));

            if (find != null)
                return (T)find;
            else
                return null;
        }

        /// <summary>Adiciona um componente na lista de Componentes.</summary>
        /// <param name="component">Uma instância da classe EntityComponent.</param>
        public void AddComponent(EntityComponent component)
        {
            Components.Add(component);
        }

        /// <summary>Remove um componente na lista de Componentes.</summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        public void RemoveComponent<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = Components.Find(x => x.GetType().Equals(t_type));

            if (find != null)
                Components.Remove(find);
        }

        /// <summary>Remove um componente na lista de Componentes.</summary>
        /// <param name="internalName">O nome interno do componente. Normalmente utilizando nameof(T) onde 'T' é o tipo dele.</param>
        public void RemoveComponentByName(string internalName)
        {
            var find = Components.Find(x => x.Name.Equals(internalName));

            if (find != null)
                Components.Remove(find);
        }

        //----- FIM COMPONENTES -----//

        /// <summary>Atualiza os limites da entidade.</summary>
        public virtual void UpdateBounds() { }

        /// <summary>Cria uma nova instância dessa entidade como uma cópia.</summary>
        public Entity2D Clone() => new Entity2D(this);        

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
                Components.Clear();
                Components = null;
            }

            disposed = true;
        } 
    }
}