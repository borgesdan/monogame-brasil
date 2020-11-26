// Danilo Borges Santos, 2020.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada que não sofre modificação da câmera corrente da tela.
    /// </summary>
    public class StaticLayer : ScreenLayer
    {
        /// <summary>
        /// Obtém ou define a lista de atores da camada.
        /// </summary>
        public List<Actor> Actors = new List<Actor>();

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe StaticLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        /// <param name="actors">O atores a serem exibidos na camada.</param>
        public StaticLayer(Screen screen, params Actor[] actors) : base(screen)
        {
            if (actors != null)
            {
                foreach (var a in actors)
                {                   
                    Actors.Add(a);
                }
            }
        }

            /// <summary>
            /// Inicializa uma nova instância da classe StaticLayer como cópia de outro StaticLayer.
            /// </summary>
            /// <param name="source">A instância a ser copiada.</param>
            public StaticLayer(StaticLayer source) : base(source)
        {
            source.Actors.ForEach(a => this.Actors.Add(a));
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//        

        ///<inheritdoc/>
        public override void _Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            for (int i = 0; i < Actors.Count; i++)
            {
                if(Actors[i].Enable.IsEnabled)
                    Actors[i].Update(gameTime);
            }

            base._Update(gameTime);
        }

        ///<inheritdoc/>
        public override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;            

            //Desenhamos na tela.
            spriteBatch.Begin(SpriteBatchConfig.SortMode, SpriteBatchConfig.BlendState, SpriteBatchConfig.Sampler, SpriteBatchConfig.DepthStencil, SpriteBatchConfig.Rasterizer, SpriteBatchConfig.Effect, null);

            for (int i = 0; i < Actors.Count; i++)
            {
                if (Actors[i].Enable.IsEnabled)
                    Actors[i].Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base._Draw(gameTime, spriteBatch);
        }
    }
}