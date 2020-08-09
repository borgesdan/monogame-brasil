// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada de exibição de uma tela.
    /// </summary>
    public abstract class Layer : IDisposable, IUpdateDrawable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//
        protected bool disposed = false;
        protected Camera layerCamera = Camera.Create();

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        /// <summary>Obtém a animação a ser exibida na camada.</summary>
        public Animation Animation { get; protected set; } = null;
        /// <summary>Obtém a tela em que essa camada está associada.</summary>
        public Screen Screen { get; protected set; } = null;
        /// <summary>Obtém ou define o Viewport de desenho da camada.</summary>
        public Viewport View { get; set; } = new Viewport();
        /// <summary>Obtém ou define o valor do efeito parallax. 1f = 100%.</summary>
        public float Parallax { get; set; } = 1f;
        /// <summary>Define o nome da camada.</summary>
        public string Name { get; set; } = "";
        /// <summary>Define a disponibilidade da camada.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe Layer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        protected Layer(Screen screen)
        {
            Screen = screen;
            View = new Viewport(0, 0, screen.Game.Window.ClientBounds.Width, screen.Game.Window.ClientBounds.Height);
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Layer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        /// <param name="animation">A animação a ser exibida na camada.</param>
        protected Layer(Screen screen, Animation animation) : this(screen)
        {
            AddAnimation(animation);
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Layer como cópia de outro Layer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        protected Layer(Layer source)
        {
            this.Screen = source.Screen;
            this.Animation = new Animation(source.Animation);
            this.layerCamera = source.layerCamera;
            this.Parallax = source.Parallax;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância da camada quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">A entidade a ser copiada.</param>
        public abstract T Clone<T>(T source) where T : Layer;

        /// <summary>Atualiza a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }

        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Adiciona uma animação ao layer.
        /// </summary>
        /// <param name="animation">A animação a ser adicionada.</param>
        public void AddAnimation(Animation animation)
        {
            AddAnimation(animation, true);
        }

        /// <summary>
        /// Adiciona uma animação ao layer.
        /// </summary>
        /// <param name="animation">A animação a ser adicionada.</param>
        /// <param name="updateBounds">True para invocar o método UpdateBounds da animação.</param>
        public void AddAnimation(Animation animation, bool updateBounds)
        {
            Animation = animation;

            if (updateBounds)
                animation.UpdateBounds();
        }

        /// <summary>
        /// Define a viewport com os valores padrão (0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height).
        /// </summary>
        protected void SetDefaultView()
        {
            View = new Viewport(0, 0, Screen.Game.Window.ClientBounds.Width, Screen.Game.Window.ClientBounds.Height);
        }

        /// <summary>
        /// Define a posição da animação.
        /// </summary>
        /// <param name="position">A posição definida.</param>
        public void SetPosition(Vector2 position)
        {
            Animation.Position = position;
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//

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

            if (disposing)
            {
                Animation = null;
                Screen = null;
            }

            disposed = true;
        }
    }
}