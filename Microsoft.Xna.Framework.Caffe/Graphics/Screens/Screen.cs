// Danilo Borges Santos, 2020.

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Enumera o estado de carregamento da tela.</summary>
    public enum ScreenLoadState : byte
    {
        UnLoaded        = 0,
        Loaded          = 1,
        Loading         = 2
    }

    /// <summary>Classe que representa uma tela de jogo com suas entidades.</summary>
    public class Screen : IDisposable, IUpdateDrawable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//        
        protected Camera camera = null;
        private InputManager input = null;
        private bool needInputUpdate = false;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a lista de atores da tela.</summary>
        public List<Actor> Actors { get; set; } = new List<Actor>();
        /// <summary>Obtém a lista de atores que estão visíveis na tela.</summary>
        public List<Actor> VisibleActors { get; private set; } = new List<Actor>();
        /// <summary>Obtém a instância ativa da classe Game.</summary>
        public Game Game { get; private set; } = null;
        /// <summary>Obtém ou define o nome ds tela.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém ou define a instância corrente do gerenciador de telas.</summary>
        public ScreenManager Manager { get; set; } = null;       
        /// <summary>Obtém ou define a capacidade da tela de ser ativa ou desenhável.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);
        /// <summary>Obtém o valor True se a tela foi carregada.</summary>
        public ScreenLoadState LoadState { get; protected set; } = ScreenLoadState.UnLoaded; 
        /// <summary>Obtém ou define a câmera da tela.</summary>
        public Camera Camera { get => camera; set => camera = value; }
        /// <summary>
        /// Obtém o acesso ao InputManager do ScreenManager em que a tela é associada, 
        /// ou a um novo objeto InputManager caso o ScreenManager ou a propriedade Input dele seja null.
        /// </summary>
        public InputManager Input
        {
            get
            {
                if (Manager != null && Manager.Input != null)
                {
                    input = Manager.Input;
                    needInputUpdate = false;
                }
                else
                {
                    if(input == null)
                        input = new InputManager();
                    
                    needInputUpdate = true;
                }

                return input;
            }
        }
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para os atores que serão desenhados.</summary>
        public SpriteBatchBeginConfig DrawConfig { get; set; } = new SpriteBatchBeginConfig();

        //-----------------------------------------//
        //-----         EVENTOS               -----//
        //-----------------------------------------//

        /// <summary>Evento chamado no fim do método Update.</summary>
        public event Action<Screen, GameTime> OnUpdate;
        /// <summary>Evento chamado no fim do método Draw.</summary>
        public event Action<Screen, GameTime, SpriteBatch> OnDraw;

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//
        /// <summary>Inicializa uma nova instância da classe Screen.</summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="name">O nome da tela.</param>
        public Screen(Game game, string name) : this(game, null, name, true) { }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen.
        /// </summary>
        /// <param name="game">A instância corrente da classe Game.</param>
        /// <param name="manager">O gerenciador de telas atual (pode ser null).</param>
        /// <param name="name">O nome da tela.</param>
        /// <param name="loadScreen">True se a tela será carregada.</param>
        public Screen(Game game, ScreenManager manager, string name, bool loadScreen)
        {
            Game = game;
            Manager = manager;
            Name = name;
            camera = new Camera(game);

            if (loadScreen)
                Load();
        }                

        /// <summary>
        /// Inicializa uma nova instância da classe Screen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public Screen(Screen source)
        {
            source.Actors.ForEach(a => this.Actors.Add(a));
            this.input = source.input;
            this.camera = source.Camera;
            this.Enable = source.Enable;
            this.Game = source.Game;
            this.LoadState = source.LoadState;
            this.Manager = source.Manager;
            this.Name = source.Name;
            this.OnDraw = source.OnDraw;
            this.OnUpdate = source.OnUpdate;
            this.camera = new Camera(source.camera);
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Sobrecarregue e chame esse método para carregar sua tela manualmente.
        /// </summary>
        public void Load()
        {
            LoadState = ScreenLoadState.Loading;
            _Load();
            LoadState = ScreenLoadState.Loaded;
        }

        public virtual void _Load() { }

        /// <summary>
        /// Sobrecarregue e chame esse método caso deseje descarregar sua tela sem chamar o método Dispose.
        /// </summary>
        public virtual void Unload() { }

        /// <summary>
        /// Sobrecarregue e chame esse método para definir sua tela em um estado padrão.
        /// </summary>
        public virtual void Reset() { }

        /// <summary>Atualiza a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public void Update(GameTime gameTime)
        {
            if (Enable.IsEnabled)
            {
                if (needInputUpdate)
                    input.Update(gameTime);                

                _Update(gameTime);

                VisibleActors.Clear();
                foreach (var a in Actors)
                {
                    //Se a entidade é visível em tela.
                    if (ViewHelper.CheckFieldOfView(Camera, a.Bounds))
                        VisibleActors.Add(a);
                }

                OnUpdate?.Invoke(this, gameTime);
            }
        }

        /// <summary>Desenha a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Enable.IsVisible)
            {
                _Draw(gameTime, spriteBatch);
                OnDraw?.Invoke(this, gameTime, spriteBatch);
            }   
        }

        /// <summary>
        /// Os atores serão atualizados ao chamar este método (base._Update()).
        /// </summary>
        public virtual void _Update(GameTime gameTime) 
        {
            for (int i = 0; i < Actors.Count; i++)
                Actors[i].Update(gameTime);
        }

        /// <summary>
        /// Os atores serão desenhados ao chamar este método (base._Draw()).
        /// </summary>
        public virtual void _Draw(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            spriteBatch.Begin(DrawConfig.SortMode, DrawConfig.BlendState, DrawConfig.Sampler, DrawConfig.DepthStencil, DrawConfig.Rasterizer, DrawConfig.Effect, Camera.GetTransform());

            for (int i = 0; i < Actors.Count; i++)
                Actors[i].Draw(gameTime, spriteBatch);            

            spriteBatch.End();
        }        

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//        
        private bool disposed = false;
        
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
                Actors.Clear();
                Actors = null;
                VisibleActors.Clear();
                VisibleActors = null;
            }

            disposed = true;
        }
    }
}