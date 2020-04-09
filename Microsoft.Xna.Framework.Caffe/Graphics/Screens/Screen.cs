// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

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
        protected bool disposed = false;
        private Viewport staticView = new Viewport();
        protected Camera camera = Camera.Create();

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
        /// <summary>Obtém ou define a lista de entidades disponíveis na tela.</summary>
        public List<Entity2D> Entities { get; set; } = new List<Entity2D>();
        /// <summary>Obtém ou define a lista de entidades que serão desenhadas atrás de DrawableEntities e que não serão afetadas pela câmera e nem pela Viewport.</summary>
        public List<Entity2D> BackStaticEntities { get; set; } = new List<Entity2D>();
        /// <summary>Obtém ou define a lista de entidades que serão desenhadas a frente de DrawableEntities e que não serão afetadas pela câmera e nem pela Viewport.</summary>
        public List<Entity2D> FrontStaticEntities { get; set; } = new List<Entity2D>();
        /// <summary>Obtém a lista de entidades que serão desenhadas.</summary>
        public List<Entity2D> DrawableEntities { get; private set; } = new List<Entity2D>();
        /// <summary>Obtém ou define a lista de camadas traseiras.</summary>
        public List<Layer> BackLayers { get; set; } = new List<Layer>();
        /// <summary>Obtém ou define a lista de camadas frontais.</summary>
        public List<Layer> FrontLayers { get; set; } = new List<Layer>();
        /// <summary>Obtém ou define a câmera da tela.</summary>
        public Camera Camera { get => camera; set => camera = value; }
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para as entidades traseiras que não são afetadas pela Viewport da tela.</summary>
        public SpriteBatchBeginConfig BackStaticEntitiesConfig { get; set; } = new SpriteBatchBeginConfig();
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para as entidades frontais que não são afetadas pela Viewport da tela.</summary>
        public SpriteBatchBeginConfig FrontStaticEntitiesConfig { get; set; } = new SpriteBatchBeginConfig();
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para as entidades que serão desenhadas na Viewport da tela. (TransformMatrix será ignorado)</summary>
        public SpriteBatchBeginConfig DrawableEntitiesConfig { get; set; } = new SpriteBatchBeginConfig();

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

            if(loadScreen)
                Load();            
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public Screen(Screen source)
        {
            foreach (var bse in source.BackStaticEntities)
                this.BackStaticEntities.Add(bse.Clone(bse));            

            foreach (var bl in source.BackLayers)
                this.BackLayers.Add(bl.Clone(bl));

            foreach (var e in source.Entities)
                this.Entities.Add(e.Clone(e));

            foreach (var de in source.DrawableEntities)
            {
                var index = source.DrawableEntities.IndexOf(de);
                this.DrawableEntities.Add(this.Entities[index]);
            }

            foreach (var fl in source.FrontLayers)
                this.FrontLayers.Add(fl.Clone(fl));

            foreach (var fse in source.FrontStaticEntities)
                this.FrontStaticEntities.Add(fse.Clone(fse));

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

            this.BackStaticEntitiesConfig = source.BackStaticEntitiesConfig;
            this.FrontStaticEntitiesConfig = source.FrontStaticEntitiesConfig;
            this.DrawableEntitiesConfig = source.DrawableEntitiesConfig;
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

            staticView = new Viewport(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            BackLayers.ForEach(bl => bl.Update(gameTime));
            FrontLayers.ForEach(fl => fl.Update(gameTime));
            BackStaticEntities.ForEach(be => be.Update(gameTime));
            FrontStaticEntities.ForEach(fe => fe.Update(gameTime));

            DrawableEntities.Clear();

            //Atualiza as entidades.
            UpdateEntities(gameTime);            

            //Chama OnEndUpdate
            OnUpdate?.Invoke(this, gameTime);
        }

        //Atualiza as entidades.
        private void UpdateEntities(GameTime gameTime)
        {
            foreach (var e in Entities)
            {
                e.Screen = this;

                //Se a entidade é visível em tela.
                if (Util.CheckFieldOfView(Game, Camera, e.Bounds))
                {
                    //Adiciona-a a lista de entidades desenháveis.
                    DrawableEntities.Add(e);
                }
            }

            //Atualiza todas as entidades.
            //Entities.ForEach(e => e.Update(gameTime));
            for(int e = 0; e < Entities.Count; e++)
            {
                Entities[e].Update(gameTime);
            }            

            //BackStaticEntities.ForEach(e => e.Update(gameTime));
            for (int e = 0; e < BackStaticEntities.Count; e++)
            {
                BackStaticEntities[e].Update(gameTime);
            }

            //FrontStaticEntities.ForEach(e => e.Update(gameTime));
            for (int e = 0; e < FrontStaticEntities.Count; e++)
            {
                FrontStaticEntities[e].Update(gameTime);
            }
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
            spriteBatch.Begin(sortMode: BackStaticEntitiesConfig.SortMode, blendState: BackStaticEntitiesConfig.Blend, samplerState: BackStaticEntitiesConfig.Sampler,
                depthStencilState: BackStaticEntitiesConfig.DepthStencil, rasterizerState: BackStaticEntitiesConfig.Rasterizer, effect: BackStaticEntitiesConfig.Effects,
                transformMatrix: BackStaticEntitiesConfig.TransformMatrix);
            BackStaticEntities.ForEach(e => e.Draw(gameTime, spriteBatch));
            spriteBatch.End();

            //Define a view principal.
            Game.GraphicsDevice.Viewport = Viewport;

            //Inicia o spritebatch com a câmera.
            spriteBatch.Begin(sortMode: DrawableEntitiesConfig.SortMode, blendState: DrawableEntitiesConfig.Blend, samplerState: DrawableEntitiesConfig.Sampler,
                depthStencilState: DrawableEntitiesConfig.DepthStencil, rasterizerState: DrawableEntitiesConfig.Rasterizer, effect: DrawableEntitiesConfig.Effects,
                transformMatrix: Camera.GetTransform());             
            //Desenhas a entidades e chama o evento.
            DrawableEntities.ForEach(e => e.Draw(gameTime, spriteBatch));            
            OnDraw?.Invoke(this, gameTime, spriteBatch);  
            spriteBatch.End();

            //Define a view estática.
            Game.GraphicsDevice.Viewport = staticView;

            //Desenhas as entidades não afetadas pela câmera [Frente].
            spriteBatch.Begin(sortMode: FrontStaticEntitiesConfig.SortMode, blendState: FrontStaticEntitiesConfig.Blend, samplerState: FrontStaticEntitiesConfig.Sampler,
                depthStencilState: FrontStaticEntitiesConfig.DepthStencil, rasterizerState: FrontStaticEntitiesConfig.Rasterizer, effect: FrontStaticEntitiesConfig.Effects,
                transformMatrix: FrontStaticEntitiesConfig.TransformMatrix);
            FrontStaticEntities.ForEach(e => e.Draw(gameTime, spriteBatch));
            spriteBatch.End();

            //Desenha as camadas frontais.
            FrontLayers.ForEach(fl => fl.Draw(gameTime, spriteBatch));

            //Define novamente a view.
            Game.GraphicsDevice.Viewport = Viewport;
        }

        /// <summary>Adiciona entidades a cena.</summary>
        /// <param name="entities">Lista de entidades a serem adicionada.</param>
        public void AddEntity(params Entity2D[] entities)
        {
            foreach(var e in entities)
            {
                e.Screen = this;
                Entities.Add(e);
            }
        }

        /// <summary>Adiciona camadas traseiras a cena.</summary>
        /// <param name="entities">Lista de camadas a serem adicionada.</param>
        public void AddBackLayer(params Layer[] backlayers)
        {
            foreach (var e in backlayers)
            {
                BackLayers.Add(e);
            }
        }

        /// <summary>Adiciona camadas frontais a cena.</summary>
        /// <param name="entities">Lista de camadas a serem adicionada.</param>
        public void AddFrontLayer(params Layer[] frontLayers)
        {
            foreach (var e in frontLayers)
            {
                FrontLayers.Add(e);
            }
        }

        /// <summary>Adiciona entidades a cena que não são afetadas pela câmera.</summary>
        /// <param name="entities">Lista de entidades a serem adicionada.</param>
        public void AddBackStaticEntity(params Entity2D[] backEntities)
        {
            foreach (var e in backEntities)
            {
                e.Screen = this;
                BackStaticEntities.Add(e);
            }
        }

        /// <summary>Adiciona entidades a cena que não são afetadas pela câmera.</summary>
        /// <param name="entities">Lista de entidades a serem adicionada.</param>
        public void AddFrontStaticEntity(params Entity2D[] frontEntities)
        {
            foreach (var e in frontEntities)
            {
                e.Screen = this;
                FrontStaticEntities.Add(e);
            }
        } 

        internal void CallLoad(ScreenManager manager)
        {
            if(manager != null)
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