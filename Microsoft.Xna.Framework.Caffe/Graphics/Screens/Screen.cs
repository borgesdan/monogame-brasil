// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System.Collections.Generic;
using System;

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
        private bool disposed = false;
        private Viewport staticView = new Viewport();

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
        public EnableGroup Enable { get; set; } = new EnableGroup();        
        /// <summary>Obtém o valor True se a tela foi carregada.</summary>
        public ScreenLoadState LoadState { get; protected set; } = ScreenLoadState.UnLoaded;
        /// <summary>Obtém ou define a cor de fundo da tela.</summary>
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;        
        /// <summary>Obtém ou define a Viewport da tela.</summary>
        public Viewport MainViewport { get; set; }
        /// <summary>Obtém ou define a lista de entidades disponíveis na tela.</summary>
        public List<Entity2D> Entities { get; set; } = new List<Entity2D>();
        /// <summary>Obtém ou define a lista de entidades que serão desenhadas atrás de DrawableEntities e que não serão afetadas pela câmera e nem pela MainView.</summary>
        public List<Entity2D> BackStaticEntities { get; set; } = new List<Entity2D>();
        /// <summary>Obtém ou define a lista de entidades que serão desenhadas a frente de DrawableEntities e que não serão afetadas pela câmera e nem pela MainView.</summary>
        public List<Entity2D> FrontStaticEntities { get; set; } = new List<Entity2D>();
        /// <summary>Obtém a lista de entidades que serão desenhadas.</summary>
        public List<Entity2D> DrawableEntities { get; private set; } = new List<Entity2D>();
        /// <summary>Obtém ou define a lista de camadas traseiras.</summary>
        public List<ScreenLayer<Animation>> BackLayers { get; set; } = new List<ScreenLayer<Animation>>();
        /// <summary>Obtém ou define a lista de camadas frontais.</summary>
        public List<ScreenLayer<Animation>> FrontLayers { get; set; } = new List<ScreenLayer<Animation>>();
       /// <summary>Obtém ou define a câmera da tela.</summary>
        public Camera Camera { get; set; } = Camera.Create();

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
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="name">Nome da tela.</param>
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
            MainViewport = game.GraphicsDevice.Viewport;

            if(loadScreen)
                Load();
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public Screen(Screen source)
        {
            this.BackStaticEntities = source.BackStaticEntities;
            this.BackgroundColor = source.BackgroundColor;
            this.BackLayers = source.BackLayers;
            this.Camera = source.Camera;
            this.DrawableEntities = source.DrawableEntities;
            this.Enable = source.Enable;
            this.Entities = source.Entities;
            this.FrontLayers = source.FrontLayers;
            this.FrontStaticEntities = source.FrontStaticEntities;
            this.Game = source.Game;
            this.LoadState = source.LoadState;
            this.Manager = source.Manager;
            this.MainViewport = source.MainViewport;
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
        /// Sobrecarregue e chame esse método caso deseje definir sua tela em um estado inicial.
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

            staticView = new Viewport(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            
            DrawableEntities.Clear();

            //Atualiza as entidades.
            UpdateEntities(gameTime, MainViewport);            

            //Chama OnEndUpdate
            OnUpdate?.Invoke(this, gameTime);
        }

        private void UpdateEntities(GameTime gameTime, Viewport viewport)
        {
            foreach (var e in Entities)
            {
                e.Screen = this;

                //Se a entidade é visível em tela.
                if (Util.CheckFieldOfView(Game, Camera, viewport, e.Bounds))
                {
                    //Adiciona-a a lista de entidades desenháveis.
                    DrawableEntities.Add(e);
                }
            }

            //Atualiza todas as entidades.
            Entities.ForEach(e => e.Update(gameTime));

            BackStaticEntities.ForEach(e => e.Update(gameTime));
            FrontStaticEntities.ForEach(e => e.Update(gameTime));
        }

        /// <summary>Desenha a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;
            
            //Desenha as camadas traseiras.
            BackLayers.ForEach(bl => bl.Draw(gameTime, spriteBatch));

            //Define a view estática.
            Game.GraphicsDevice.Viewport = staticView;

            //Desenha as entidades não afetadas pela câmera.
            spriteBatch.Begin();
            BackStaticEntities.ForEach(e => e.Draw(gameTime, spriteBatch));
            spriteBatch.End();

            //Define a view principal.
            Game.GraphicsDevice.Viewport = MainViewport;

            //Inicia o spritebatch com a câmera.
            spriteBatch.Begin(transformMatrix: Camera.GetTransform());            
            //Desenhas a entidades e chama o evento.
            DrawableEntities.ForEach(e => e.Draw(gameTime, spriteBatch));            
            OnDraw?.Invoke(this, gameTime, spriteBatch);  
            spriteBatch.End();

            //Define a view estática.
            Game.GraphicsDevice.Viewport = staticView;

            //Desenhas as entidades não afetadas pela câmera [Frente].
            spriteBatch.Begin();
            FrontStaticEntities.ForEach(e => e.Draw(gameTime, spriteBatch));
            spriteBatch.End();

            //Desenha as camadas frontais.
            FrontLayers.ForEach(fl => fl.Draw(gameTime, spriteBatch));

            //Define novamente a view.
            Game.GraphicsDevice.Viewport = MainViewport;
        }

        /// <summary>Adiciona entidades a cena.</summary>
        /// <param name="entities">Lista de entidades a ser adicionada.</param>
        public void Add(params Entity2D[] entities)
        {
            foreach(var e in entities)
            {
                Entities.Add(e);
            }
        }

        public void ChangeState(ScreenManager manager, ScreenLoadState state)
        {
            if(manager != null)
            {
                LoadState = state;
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

                Entities.Clear();
                Entities = null;                

                DrawableEntities.Clear();
                DrawableEntities = null;

                Name = null;
                LoadState = ScreenLoadState.UnLoaded;
            }                

            disposed = true;
        }
    }
}