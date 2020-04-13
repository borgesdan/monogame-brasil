//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada que não sofre modificação em sua posição e tamanho,
    /// e não é afetada pela posição da câmera da tela associada.
    /// </summary>
    public class StaticLayer : Layer
    {
        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe StaticLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        /// <param name="animation">A animação a ser exibida na camada.</param>
        public StaticLayer(Screen screen, Animation animation) : base(screen, animation)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe StaticLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>        
        public StaticLayer(Screen screen) : base(screen)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe StaticLayer como cópia de outro StaticLayer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public StaticLayer(StaticLayer source) : base(source)
        {            
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância da camada quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">A entidade a ser copiada.</param>
        public override T Clone<T>(T source)
        {
            if (source is StaticLayer)
                return (T)Activator.CreateInstance(typeof(StaticLayer), source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            base.Update(gameTime);
        }

        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            //Resetamos a view pois a visão é estática.
            //SetDefaultView();

            GraphicsDevice device = Screen.Game.GraphicsDevice;
            Viewport oldView = device.Viewport;

            device.Viewport = View;

            //Desenhamos na tela.
            spriteBatch.Begin();
            Animation.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            //Recuperamos a viewport originária da tela.
            device.Viewport = oldView;

            base.Draw(gameTime, spriteBatch);            
        }
    }
}