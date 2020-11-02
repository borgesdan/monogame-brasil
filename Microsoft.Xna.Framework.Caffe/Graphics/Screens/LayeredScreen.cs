// Danilo Borges Santos, 2020.

using System.Collections.Generic;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe que representa uma tela de jogo com suas entidades e camadas.</summary>
    public class LayeredScreen : Screen
    {
        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//        
        /// <summary>Obtém ou define a lista de atores que serão desenhadas atrás de DrawableEntities e que não serão afetadas pela câmera e nem pela Viewport.</summary>
        public List<Actor> BackStaticActors { get; set; } = new List<Actor>();
        /// <summary>Obtém ou define a lista de atores que serão desenhadas a frente de DrawableEntities e que não serão afetadas pela câmera e nem pela Viewport.</summary>
        public List<Actor> FrontStaticActors { get; set; } = new List<Actor>();
        /// <summary>Obtém a lista de atores que serão desenhadas.</summary>
        public List<Actor> DrawableActors { get; private set; } = new List<Actor>();
        
        /// <summary>Obtém ou define a lista de camadas traseiras.</summary>
        public List<ScreenLayer> BackLayers { get; set; } = new List<ScreenLayer>();
        /// <summary>Obtém ou define a lista de camadas frontais.</summary>
        public List<ScreenLayer> FrontLayers { get; set; } = new List<ScreenLayer>();
        
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para os atores traseiros que não são afetados pela Camera da tela.</summary>
        public SpriteBatchBeginConfig BackStaticConfig { get; set; } = new SpriteBatchBeginConfig();
        /// <summary>Obtém ou define as configurações do SpriteBatch.Begin para os atores frontais que não são afetados pela Camera da tela.</summary>
        public SpriteBatchBeginConfig FrontStaticConfig { get; set; } = new SpriteBatchBeginConfig();
                
        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//       


        /// <summary>Inicializa uma nova instância da classe Screen.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="manager">O gerenciador de telas atual.</param>
        /// <param name="name">Nome da tela.</param>
        /// <param name="loadScene">True se a tela será carregada.</param>
        public LayeredScreen(Game game, ScreenManager manager, string name, bool loadScreen) :base(game, manager, name, loadScreen) { }

        /// <summary>
        /// Inicializa uma nova instância da classe LayredScreen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public LayeredScreen(LayeredScreen source) : base(source)
        {
            foreach (var bse in source.BackStaticActors)
                this.BackStaticActors.Add(bse);

            foreach (var bl in source.BackLayers)
                this.BackLayers.Add(bl);

            foreach (var e in source.Actors)
                this.Actors.Add(e);

            foreach (var de in source.DrawableActors)
            {
                var index = source.DrawableActors.IndexOf(de);
                this.DrawableActors.Add(this.Actors[index]);
            }

            foreach (var fl in source.FrontLayers)
                this.FrontLayers.Add(fl);

            foreach (var fse in source.FrontStaticActors)
                this.FrontStaticActors.Add(fse);            

            this.BackStaticConfig = source.BackStaticConfig;
            this.FrontStaticConfig = source.FrontStaticConfig;
            this.DrawableConfig = source.DrawableConfig;
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

            DrawableActors.Clear();

            //Atualiza as entidades.
            UpdateEntities(gameTime);

            base.Update(gameTime);
        }

        //Atualiza as entidades.
        private void UpdateEntities(GameTime gameTime)
        {
            foreach (var a in Actors)
            {
                //Se a entidade é visível em tela.
                if (Util.CheckFieldOfView(Camera, a.Bounds))
                {
                    //Adiciona-a a lista de entidades desenháveis.
                    DrawableActors.Add(a);
                }
            }
            
            for (int e = 0; e < BackLayers.Count; e++)
                BackLayers[e].Update(gameTime);

            for (int e = 0; e < FrontLayers.Count; e++)
                FrontLayers[e].Update(gameTime);

            for (int e = 0; e < BackStaticActors.Count; e++)
                BackStaticActors[e].Update(gameTime);

            for (int e = 0; e < FrontStaticActors.Count; e++)
                FrontStaticActors[e].Update(gameTime);
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
                bl.Draw(gameTime, spriteBatch);

            //Desenha as entidades não afetadas pela câmera.
            spriteBatch.Begin(sortMode: BackStaticConfig.SortMode, blendState: BackStaticConfig.BlendState, samplerState: BackStaticConfig.Sampler,
                depthStencilState: BackStaticConfig.DepthStencil, rasterizerState: BackStaticConfig.Rasterizer, effect: BackStaticConfig.Effect,
                transformMatrix: BackStaticConfig.TransformMatrix);
            foreach (var bse in BackStaticActors)
            {
                bse.Draw(gameTime, spriteBatch);

            }
            spriteBatch.End();
           
            //Inicia o spritebatch com a câmera.
            spriteBatch.Begin(sortMode: DrawableConfig.SortMode, blendState: DrawableConfig.BlendState, samplerState: DrawableConfig.Sampler,
                depthStencilState: DrawableConfig.DepthStencil, rasterizerState: DrawableConfig.Rasterizer, effect: DrawableConfig.Effect,
                transformMatrix: Camera.GetTransform());
            //Desenhas a entidades e chama o evento.
            foreach (var de in DrawableActors)
            {
                de.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();            

            //Desenhas as entidades não afetadas pela câmera [Frente].
            spriteBatch.Begin(sortMode: FrontStaticConfig.SortMode, blendState: FrontStaticConfig.BlendState, samplerState: FrontStaticConfig.Sampler,
                depthStencilState: FrontStaticConfig.DepthStencil, rasterizerState: FrontStaticConfig.Rasterizer, effect: FrontStaticConfig.Effect,
                transformMatrix: FrontStaticConfig.TransformMatrix);
            foreach (var fe in FrontStaticActors)
            {
                fe.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            //Desenha as camadas frontais.
            foreach (var fl in FrontLayers)
            {
                fl.Draw(gameTime, spriteBatch);
            }           

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>Adiciona entidades a cena.</summary>
        /// <param name="actors">Lista de entidades a serem adicionada.</param>
        public void AddActor(params Actor[] actors)
        {
            foreach(var e in actors)
            {
                e.Screen = this;
                Actors.Add(e);
            }
        }

        /// <summary>Adiciona camadas traseiras a cena.</summary>
        /// <param name="entities">Lista de camadas a serem adicionada.</param>
        public void AddBackLayer(params ScreenLayer[] backlayers)
        {
            foreach (var e in backlayers)
            {
                BackLayers.Add(e);
            }
        }

        /// <summary>Adiciona camadas frontais a cena.</summary>
        /// <param name="entities">Lista de camadas a serem adicionada.</param>
        public void AddFrontLayer(params ScreenLayer[] frontLayers)
        {
            foreach (var e in frontLayers)
            {
                FrontLayers.Add(e);
            }
        }

        /// <summary>Adiciona entidades a cena que não são afetadas pela câmera.</summary>
        /// <param name="entities">Lista de entidades a serem adicionada.</param>
        public void AddBackStatic(params Actor[] backActors)
        {
            foreach (var e in backActors)
            {
                e.Screen = this;
                BackStaticActors.Add(e);
            }
        }

        /// <summary>Adiciona entidades a cena que não são afetadas pela câmera.</summary>
        /// <param name="entities">Lista de entidades a serem adicionada.</param>
        public void AddFrontStatic(params Actor[] frontActors)
        {
            foreach (var e in frontActors)
            {
                e.Screen = this;
                FrontStaticActors.Add(e);
            }
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------// 
        private bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {           
                DrawableActors.Clear();
                DrawableActors = null;
                BackLayers.Clear();
                BackLayers = null;
                BackStaticActors.Clear();
                BackStaticActors = null;
                BackStaticConfig = null;
                FrontLayers.Clear();
                FrontLayers = null;
                FrontStaticActors.Clear();
                FrontStaticActors = null;
                FrontStaticConfig = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}