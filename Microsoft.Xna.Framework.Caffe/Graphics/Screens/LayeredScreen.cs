//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System.Collections.Generic;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe que representa uma tela de jogo com suas entidades e camadas.</summary>
    public class LayeredScreen : Screen
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//
        private Viewport staticView = new Viewport();

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
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
        
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para as entidades traseiras que não são afetadas pela Viewport da tela.</summary>
        public SpriteBatchBeginConfig BackStaticEntitiesConfig { get; set; } = new SpriteBatchBeginConfig();
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para as entidades frontais que não são afetadas pela Viewport da tela.</summary>
        public SpriteBatchBeginConfig FrontStaticEntitiesConfig { get; set; } = new SpriteBatchBeginConfig();
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para as entidades que serão desenhadas na Viewport da tela. (TransformMatrix será ignorado)</summary>
        public SpriteBatchBeginConfig DrawableEntitiesConfig { get; set; } = new SpriteBatchBeginConfig();
                
        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//
        /// <summary>Inicializa uma nova instância da classe Screen.</summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="name">O nome da tela.</param>
        public LayeredScreen(Game game, string name) : base(game, name, true) { }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen.
        /// </summary>
        /// <param name="manager">O gerenciador de telas atual.</param>
        /// <param name="name">O nome da tela.</param>
        /// <param name="loadScreen">True se a tela será carregada.</param>
        public LayeredScreen(ScreenManager manager, string name, bool loadScreen) : base(manager, name, loadScreen) { }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen.
        /// </summary>
        /// <param name="subManager">O subgerenciador de telas associado a uma tela administradora.</param>
        /// <param name="name">O nome da tela.</param>
        /// <param name="loadScreen">True se a tela será carregada.</param>
        public LayeredScreen(SubScreenManager subManager, string name, bool loadScreen) : base(subManager, name, loadScreen) { }        

        /// <summary>Inicializa uma nova instância da classe Screen.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="name">Nome da tela.</param>
        /// <param name="loadScene">True se a tela será carregada.</param>
        public LayeredScreen(Game game, string name, bool loadScreen) :base(game, name, loadScreen) { }

        /// <summary>
        /// Inicializa uma nova instância da classe Screen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public LayeredScreen(LayeredScreen source) : base(source)
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

            this.BackStaticEntitiesConfig = source.BackStaticEntitiesConfig;
            this.FrontStaticEntitiesConfig = source.FrontStaticEntitiesConfig;
            this.DrawableEntitiesConfig = source.DrawableEntitiesConfig;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Atualiza a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            staticView = new Viewport(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);            

            DrawableEntities.Clear();

            //Atualiza as entidades.
            UpdateEntities(gameTime);

            base.Update(gameTime);
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
            
            for(int e = 0; e < Entities.Count; e++)
            {
                if(Entities[e].Enable.IsEnabled)
                    Entities[e].Update(gameTime);
            }
            
            for (int e = 0; e < BackLayers.Count; e++)
            {
                if(BackLayers[e].Enable.IsEnabled)
                    BackLayers[e].Update(gameTime);
            }
            
            for (int e = 0; e < FrontLayers.Count; e++)
            {
                if (FrontLayers[e].Enable.IsEnabled)
                    FrontLayers[e].Update(gameTime);
            }
            
            for (int e = 0; e < BackStaticEntities.Count; e++)
            {
                if (BackStaticEntities[e].Enable.IsEnabled)
                    BackStaticEntities[e].Update(gameTime);
            }
            
            for (int e = 0; e < FrontStaticEntities.Count; e++)
            {
                if (FrontStaticEntities[e].Enable.IsEnabled)
                    FrontStaticEntities[e].Update(gameTime);
            }
        }

        /// <summary>Desenha a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;
            
            //Desenha as camadas traseiras.           
            foreach(var bl in BackLayers)
            {
                if (bl.Enable.IsVisible)
                    bl.Draw(gameTime, spriteBatch);
            }

            //Define a view estática.
            Game.GraphicsDevice.Viewport = staticView;

            //Desenha as entidades não afetadas pela câmera.
            spriteBatch.Begin(sortMode: BackStaticEntitiesConfig.SortMode, blendState: BackStaticEntitiesConfig.Blend, samplerState: BackStaticEntitiesConfig.Sampler,
                depthStencilState: BackStaticEntitiesConfig.DepthStencil, rasterizerState: BackStaticEntitiesConfig.Rasterizer, effect: BackStaticEntitiesConfig.Effects,
                transformMatrix: BackStaticEntitiesConfig.TransformMatrix);
            foreach (var bse in BackStaticEntities)
            {
                if (bse.Enable.IsVisible)
                    bse.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            //Define a view principal.
            Game.GraphicsDevice.Viewport = Viewport;

            //Inicia o spritebatch com a câmera.
            spriteBatch.Begin(sortMode: DrawableEntitiesConfig.SortMode, blendState: DrawableEntitiesConfig.Blend, samplerState: DrawableEntitiesConfig.Sampler,
                depthStencilState: DrawableEntitiesConfig.DepthStencil, rasterizerState: DrawableEntitiesConfig.Rasterizer, effect: DrawableEntitiesConfig.Effects,
                transformMatrix: Camera.GetTransform());
            //Desenhas a entidades e chama o evento.
            foreach (var de in DrawableEntities)
            {
                if (de.Enable.IsVisible)
                    de.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            //Define a view estática.
            Game.GraphicsDevice.Viewport = staticView;

            //Desenhas as entidades não afetadas pela câmera [Frente].
            spriteBatch.Begin(sortMode: FrontStaticEntitiesConfig.SortMode, blendState: FrontStaticEntitiesConfig.Blend, samplerState: FrontStaticEntitiesConfig.Sampler,
                depthStencilState: FrontStaticEntitiesConfig.DepthStencil, rasterizerState: FrontStaticEntitiesConfig.Rasterizer, effect: FrontStaticEntitiesConfig.Effects,
                transformMatrix: FrontStaticEntitiesConfig.TransformMatrix);
            foreach (var fe in FrontStaticEntities)
            {
                if (fe.Enable.IsVisible)
                    fe.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            //Desenha as camadas frontais.
            foreach (var fl in FrontLayers)
            {
                if (fl.Enable.IsVisible)
                    fl.Draw(gameTime, spriteBatch);
            }

            //Define novamente a view.
            Game.GraphicsDevice.Viewport = Viewport;

            base.Draw(gameTime, spriteBatch);
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

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//                

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Entities.Clear();
                Entities = null;                
                DrawableEntities.Clear();
                DrawableEntities = null;
                BackLayers.Clear();
                BackLayers = null;
                BackStaticEntities.Clear();
                BackStaticEntities = null;
                BackStaticEntitiesConfig = null;
                FrontLayers.Clear();
                FrontLayers = null;
                FrontStaticEntities.Clear();
                FrontStaticEntities = null;
                FrontStaticEntitiesConfig = null;
            }

            base.Dispose(disposing);
        }
    }
}