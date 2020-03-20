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

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        /// <summary>Obtém a instância ativa da classe Game.</summary>
        public Game Game { get; private set; } = null;
        /// <summary>Obtém ou define o nome ds tela.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém ou define a instância corrente do gerenciador de cenas.</summary>
        public ScreenManager Manager { get; set; } = null;
        /// <summary>Obtém ou define a capacidade da tela de ser ativa ou desenhável.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup();
        /// <summary>Obtém ou define a lista de entidades disponíveis na tela.</summary>
        public List<Entity2D> Entitys { get; set; } = new List<Entity2D>();        
        /// <summary>Obtém a lista de entidades que serão desenhadas.</summary>
        public List<Entity2D> DrawableEntitys { get; private set; } = new List<Entity2D>();
        /// <summary>Obtém o valor True se a tela foi carregada.</summary>
        public ScreenLoadState LoadState { get; protected set; } = ScreenLoadState.UnLoaded;
        /// <summary>Obtém ou define a cor de fundo da tela.</summary>
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;        
        /// <summary>Obtém ou define a Viewport da tela.</summary>
        public Viewport MainViewport { get; set; }
        /// <summary>Obtém ou define a lista de camadas traseiras.</summary>
        public List<ScreenLayer<Animation>> BackLayers { get; set; } = new List<ScreenLayer<Animation>>();
        /// <summary>Obtém ou define a lista de camadas frontais.</summary>
        public List<ScreenLayer<Animation>> FrontLayers { get; set; } = new List<ScreenLayer<Animation>>();
       /// <summary>Obtém ou degine a câmera da tela.</summary>
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
        /// Inicializa uma nova instância da classe Screen.
        /// </summary>
        /// <param name="manager">O gerenciador de telas atual.</param>
        /// <param name="name">O nome da tela.</param>
        /// <param name="loadScreen">True se a tela será carregada.</param>
        public Screen(ScreenManager manager, string name, bool loadScreen)
        {
            Manager = manager;

            Game = manager.Game;
            Name = name;
            MainViewport = Game.GraphicsDevice.Viewport;

            if (loadScreen)
                Load();
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public Screen(Screen source)
        {
            this.BackgroundColor = source.BackgroundColor;
            this.BackLayers = source.BackLayers;
            this.Camera = source.Camera;
            this.DrawableEntitys = source.DrawableEntitys;
            this.Enable = source.Enable;
            this.Entitys = source.Entitys;
            this.FrontLayers = source.FrontLayers;
            this.Game = source.Game;
            this.LoadState = source.LoadState;
            this.Manager = source.Manager;
            this.MainViewport = source.MainViewport;
            this.Name = source.Name;
            this.OnDraw = source.OnDraw;
            this.OnUpdate = source.OnUpdate;       
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
            
            DrawableEntitys.Clear();

            //Atualiza as entidades.
            UpdateEntitys(gameTime, MainViewport);            

            //Chama OnEndUpdate
            OnUpdate?.Invoke(this, gameTime);
        }

        private void UpdateEntitys(GameTime gameTime, Viewport viewport)
        {
            foreach (var e in Entitys)
            {
                e.Screen = this;

                //Se a entidade é visível em tela.
                if (Util.CheckFieldOfView(Game, Camera, viewport, e.Bounds))
                {
                    //Adiciona-a a lista de entidades desenháveis.
                    DrawableEntitys.Add(e);
                }
            }

            //Atualiza todas as entidades.
            Entitys.ForEach(e => e.Update(gameTime));
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

            //Define a view.
            Game.GraphicsDevice.Viewport = MainViewport;
            //Inicia o spritebatch com a configuração definida pelo usuário.
            spriteBatch.Begin(transformMatrix: Camera.GetTransform());            
            //Desenhas a entidades e chama o evento.
            DrawableEntitys.ForEach(e => e.Draw(gameTime, spriteBatch));            
            OnDraw?.Invoke(this, gameTime, spriteBatch);           
            //Finaliza o spritebatch.
            spriteBatch.End();

            //Desenha as camadas frontais.
            FrontLayers.ForEach(fl => fl.Draw(gameTime, spriteBatch));

            //Define novamente a view.
            Game.GraphicsDevice.Viewport = MainViewport;
        }

        /// <summary>Adiciona entidades a cena.</summary>
        /// <param name="entitys">Lista de entidades a ser adicionada.</param>
        public void Add(params Entity2D[] entitys)
        {
            foreach(var e in entitys)
            {
                Entitys.Add(e);
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

                Entitys.Clear();
                Entitys = null;                

                DrawableEntitys.Clear();
                DrawableEntitys = null;

                Name = null;
                LoadState = ScreenLoadState.UnLoaded;
            }                

            disposed = true;
        }
    }
}