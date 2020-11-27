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
        /// <summary>Obtém ou define a lista de camadas traseiras.</summary>
        public List<ScreenLayer> BackLayers { get; set; } = new List<ScreenLayer>();
        /// <summary>Obtém ou define a lista de camadas frontais.</summary>
        public List<ScreenLayer> FrontLayers { get; set; } = new List<ScreenLayer>();
                
        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//    

        /// <summary>Inicializa uma nova instância da classe Screen.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="manager">O gerenciador de telas atual.</param>
        /// <param name="name">Nome da tela.</param>
        /// <param name="loadScene">True se a tela será carregada.</param>
        public LayeredScreen(Game game, ScreenManager manager, string name, bool loadScreen) : base(game, manager, name, loadScreen) 
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe LayredScreen copiando uma outra tela. 
        /// </summary>
        /// <param name="source">A tela a ser copiada.</param>
        public LayeredScreen(LayeredScreen source) : base(source)
        {
            foreach (var bl in source.BackLayers)
                this.BackLayers.Add(bl);

            foreach (var e in source.Actors)
                this.Actors.Add(e);

            foreach (var de in source.VisibleActors)
            {
                var index = source.VisibleActors.IndexOf(de);
                this.VisibleActors.Add(this.Actors[index]);
            }

            foreach (var fl in source.FrontLayers)
                this.FrontLayers.Add(fl);
            
            this.DrawConfig = source.DrawConfig;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Atualiza a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void _Update(GameTime gameTime)
        {
            VisibleActors.Clear();

            //Atualiza as entidades.
            UpdateLayers(gameTime);

            base._Update(gameTime);
        }

        //Atualiza as entidades.
        private void UpdateLayers(GameTime gameTime)
        {   
            for (int e = 0; e < BackLayers.Count; e++)
                BackLayers[e].Update(gameTime);

            for (int e = 0; e < FrontLayers.Count; e++)
                FrontLayers[e].Update(gameTime);            
        }

        /// <summary>Desenha a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Desenha as camadas traseiras.           
            for (int e = 0; e < BackLayers.Count; e++)
                BackLayers[e].Draw(gameTime, spriteBatch);

            base._Draw(gameTime, spriteBatch);

            //Desenha as camadas frontais.
            for (int e = 0; e < FrontLayers.Count; e++)
                FrontLayers[e].Draw(gameTime, spriteBatch);
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
                BackLayers.Add(e);
        }

        /// <summary>Adiciona camadas frontais a cena.</summary>
        /// <param name="entities">Lista de camadas a serem adicionada.</param>
        public void AddFrontLayer(params ScreenLayer[] frontLayers)
        {
            foreach (var e in frontLayers)
                FrontLayers.Add(e);
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
                BackLayers.Clear();
                BackLayers = null;
                FrontLayers.Clear();
                FrontLayers = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}