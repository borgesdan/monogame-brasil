//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Enumera o estado de carregamento da tela.</summary>
    public enum ScreenLoadState : byte
    {
        UnLoaded = 0,
        Loaded = 1,
        Loading = 2
    }

    /// <summary>Classe que representa uma tela de jogo com suas entidades.</summary>
    public class Screen : IDisposable, IUpdateDrawable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//
        protected bool disposed = false;
        protected Camera camera = Camera.Create();
        private Polygon3D poly = null;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        /// <summary>Obtém a instância ativa da classe Game.</summary>
        public Game Game { get; private set; } = null;
        /// <summary>Obtém ou define o nome ds tela.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém ou define a instância corrente do gerenciador de telas.</summary>
        public ScreenManager Manager { get; set; } = null;
        /// <summary>Obtém ou define a instância corrente do subgerenciador de telas.</summary>
        public SubScreenManager SubManager { get; set; } = null;
        /// <summary>Obtém ou define a capacidade da tela de ser ativa ou desenhável.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);
        /// <summary>Obtém o valor True se a tela foi carregada.</summary>
        public ScreenLoadState LoadState { get; protected set; } = ScreenLoadState.UnLoaded;
        /// <summary>Obtém ou define a cor de fundo da tela.</summary>
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        /// <summary>Obtém ou define a Viewport da tela.</summary>
        public Viewport Viewport { get; set; }
        /// <summary>Obtém ou define a câmera da tela.</summary>
        public Camera Camera { get => camera; set => camera = value; }

        public List<Tuple<Polygon, Color>> DebugPolygons = new List<Tuple<Polygon, Color>>();

        //-----------------------------------------//
        //-----         EVENTOS               -----//
        //-----------------------------------------//

        /// <summary>Evento chamado no fim do método Update.</summary>
        public event UpdateAction<Screen> OnUpdate;
        /// <summary>Evento chamado no fim do método Draw.</summary>
        public event DrawAction<Screen> OnDraw;

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//
        /// <summary>Inicializa uma nova instância da classe Screen.</summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="name">O nome da tela.</param>
        public Screen(Game game, string name) : this(game, name, true) { }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen.
        /// </summary>
        /// <param name="manager">O gerenciador de telas atual.</param>
        /// <param name="name">O nome da tela.</param>
        /// <param name="loadScreen">True se a tela será carregada.</param>
        public Screen(ScreenManager manager, string name, bool loadScreen) : this(manager.Game, name, loadScreen)
        {
            Manager = manager;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen.
        /// </summary>
        /// <param name="subManager">O subgerenciador de telas associado a uma tela administradora.</param>
        /// <param name="name">O nome da tela.</param>
        /// <param name="loadScreen">True se a tela será carregada.</param>
        public Screen(SubScreenManager subManager, string name, bool loadScreen) : this(subManager.Game, name, loadScreen)
        {
            Manager = subManager.ScreenManager;
            SubManager = subManager;
        }

        /// <summary>Inicializa uma nova instância da classe Screen.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="name">Nome da tela.</param>
        /// <param name="loadScene">True se a tela será carregada.</param>
        public Screen(Game game, string name, bool loadScreen)
        {
            Game = game;
            Name = name;
            Viewport = game.GraphicsDevice.Viewport;

            if (loadScreen)
                Load();
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public Screen(Screen source)
        {
            this.camera = source.Camera;
            this.Enable = source.Enable;
            this.BackgroundColor = source.BackgroundColor;
            this.Game = source.Game;
            this.LoadState = source.LoadState;
            this.Manager = source.Manager;
            this.Viewport = source.Viewport;
            this.Name = source.Name;
            this.OnDraw = source.OnDraw;
            this.OnUpdate = source.OnUpdate;
            this.SubManager = source.SubManager;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Sobrecarregue e chame esse método para carregar sua tela manualmente.
        /// </summary>
        public virtual void Load()
        {
            LoadState = ScreenLoadState.Loaded;
        }

        /// <summary>
        /// Sobrecarregue e chame esse método caso deseje descarregar sua tela sem chamar o método Dispose.
        /// </summary>
        public virtual void Unload()
        {

        }

        /// <summary>
        /// Sobrecarregue e chame esse método para definir sua tela em um estado inicial.
        /// </summary>
        public virtual void Reset()
        {

        }

        /// <summary>Atualiza a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;
            
            //Chama OnEndUpdate
            OnUpdate?.Invoke(this, gameTime);
        }

        /// <summary>Desenha a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            OnDraw?.Invoke(this, gameTime, spriteBatch);

            if(DEBUG.IsEnabled)
            {
                if(poly == null)
                {
                    poly = new Polygon3D(Game, new Polygon(), Color.White);
                }

                foreach(var dp in DebugPolygons)
                {
                    poly.Color = dp.Item2;
                    poly.Poly = dp.Item1;

                    poly.Draw();
                }

                DebugPolygons.Clear();
            }
        }

        internal void CallLoad(ScreenManager manager)
        {
            if (manager != null)
            {
                this.LoadState = ScreenLoadState.Loaded;
                Load();
            }
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//        

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
                Game = null;
                Manager = null;
                Name = null;
            }

            disposed = true;
        }
    }
}