// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada de exibição de uma tela.
    /// </summary>
    public abstract class ScreenLayer : IDisposable, IUpdateDrawable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//        
        protected Camera layerCamera = Camera.Create();

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//        
        /// <summary>Obtém a tela em que essa camada está associada.</summary>
        public Screen Screen { get; protected set; } = null;
        /// <summary>Obtém ou define o valor do efeito parallax. 1f = 100%.</summary>
        public float Parallax { get; set; } = 1f;
        /// <summary>Define o nome da camada.</summary>
        public string Name { get; set; } = "";
        /// <summary>Define a disponibilidade da camada.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);
        /// <summary>Obtém ou define a configuração do SpriteBatch na chamada do método Begin().</summary>
        public SpriteBatchBeginConfig SpriteBatchConfig { get; set; } = new SpriteBatchBeginConfig();

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//        

        /// <summary>
        /// Inicializa uma nova instância da classe ScreenLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        protected ScreenLayer(Screen screen)
        {   
            Screen = screen ?? throw new ArgumentNullException("screen", "É necessário informar uma tela para a camada ser associada");
        }

        /// <summary>
        /// Inicializa uma nova instância da classe ScreenLayer como cópia de outro Layer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        protected ScreenLayer(ScreenLayer source)
        {
            this.Screen = source.Screen;
            this.layerCamera = source.layerCamera;
            this.Parallax = source.Parallax;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//        

        /// <summary>Atualiza a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);         

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//
        private bool disposed = false;

        /// <summary>Libera os recursos dessa instância.</summary>
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
                Screen = null;
            }

            disposed = true;
        }
    }
}