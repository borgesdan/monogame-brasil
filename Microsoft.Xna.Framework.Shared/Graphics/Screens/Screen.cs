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
        /// <summary>Obtém ou define o nome do mundo.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Obtém ou define a instância corrente do gerenciador de cenas.</summary>
        public ScreenManager Manager { get; set; } = null;
        /// <summary>Obtém ou define a capacidade da tela de ser ativa ou desenhável.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup();
        /// <summary>Obtém ou define a lista de entidades adicionadas.</summary>
        public List<Entity2D> Entitys { get; set; } = new List<Entity2D>();
        /// <summary>Obtém a lista de entidades que irão sofrer atualização.</summary>
        public List<Entity2D> UpdatableEntitys { get; private set; } = new List<Entity2D>();
        /// <summary>Obtém a lista de entidades que serão desenhadas.</summary>
        public List<Entity2D> DrawableEntitys { get; private set; } = new List<Entity2D>();
        /// <summary>Obtém o valor True se a tela foi carregada.</summary>
        public ScreenLoadState LoadState { get; protected set; } = ScreenLoadState.UnLoaded;
        /// <summary>Obtém ou define a cor de fundo da tela.</summary>
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

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
            this.DrawableEntitys = source.DrawableEntitys;
            this.Enable = source.Enable;
            this.Entitys = source.Entitys;
            this.Game = source.Game;
            this.LoadState = source.LoadState;
            this.Manager = source.Manager;
            this.Name = source.Name;
            this.OnDraw = source.OnDraw;
            this.OnUpdate = source.OnUpdate;
            this.UpdatableEntitys = source.UpdatableEntitys;            
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

            //Recebe a vista e limpa as listas necessárias
            Viewport viewport = Game.GraphicsDevice.Viewport;
            UpdatableEntitys.Clear();
            DrawableEntitys.Clear();

            //Atualiza as entidades.
            UpdateEntitys(gameTime, viewport);            

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

            //Desenha as entidades
            foreach (var e in DrawableEntitys)
            {
                e.Draw(gameTime, spriteBatch);
            }           

            //Chama OnEndDraw
            OnDraw?.Invoke(this, gameTime, spriteBatch);
        }

        private void UpdateEntitys(GameTime gameTime, Viewport viewport)
        {
            foreach (var e in Entitys)
            {
                e.Screen = this;

                if (e.Bounds.Intersects(viewport.Bounds))
                {
                    UpdatableEntitys.Add(e);
                    DrawableEntitys.Add(e);
                }
                else if (e.UpdateOutofView)
                {
                    UpdatableEntitys.Add(e);
                }
            }

            foreach (var ent in UpdatableEntitys)
            {
                ent.Update(gameTime);
            }
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

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//        
        /// <summary>Libera os recursos da classe.</summary>
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
                UpdatableEntitys.Clear();
                DrawableEntitys.Clear();
                Name = null;

                LoadState = ScreenLoadState.UnLoaded;
            }                

            disposed = true;
        }
    }
}