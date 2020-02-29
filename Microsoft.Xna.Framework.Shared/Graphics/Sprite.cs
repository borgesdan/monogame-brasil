// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe que serve como contêiner para uma textura e seus frames.</summary>
    public class Sprite : IDisposable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//
        private bool disposed = false;

        //----------------------------------------//
        //-----         PROPRIEDADES         -----//
        //----------------------------------------//

        /// <summary>Obtém a textura para desenho.</summary>
        public Texture2D Texture { get; private set; } = null;

        /// <summary>Obtém ou define a lista de frames da textura.</summary>
        public List<SpriteFrame> Frames { get; set; } = new List<SpriteFrame>();        

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//

        /// <summary>Inicia uma nova instância da classe Sprite.</summary>
        /// <param name="game">Instância atual da classe Game.</param>
        /// <param name="sourceName">O caminho do arquivo de textura na pasta Content.</param>
        public Sprite(Game game, string sourceName) : this(game.Content.Load<Texture2D>(sourceName)) { }

        /// <summary>Inicia uma nova instância da classe Sprite.</summary>
        /// <param name="texture">Um objeto da classe Texture2D.</param>
        public Sprite(Texture2D texture) : this(texture, false) { }

        /// <summary>Inicia uma nova instância da classe Sprite.</summary>
        /// <param name="game">Instância atual da classe Game.</param>
        /// <param name="sourceName">O caminho do arquivo de textura na pasta Content.</param>
        /// <param name="addSingleFrame">Defina True para adicionar um frame do tamanho da textura na lista de Frames.</param>
        public Sprite(Game game, string sourceName, bool addSingleFrame) : this(game.Content.Load<Texture2D>(sourceName), addSingleFrame) { }

        /// <summary>Inicia uma nova instância da classe Sprite.</summary>
        /// <param name="texture">Um objeto da classe Texture2D.</param>        
        /// <param name="addSingleFrame">Defina True para adicionar um frame do tamanho da textura na lista de Frames.</param>
        public Sprite(Texture2D texture, bool addSingleFrame)
        {
            Texture = texture ?? throw new ArgumentNullException(nameof(texture));

            if (addSingleFrame)
            {
                SpriteFrame defaultFrame = new SpriteFrame(texture.Bounds);
                Frames.Add(defaultFrame);
            }
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Sprite como cópida de outra instância.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public Sprite(Sprite source)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            this.Frames = source.Frames;
            this.Texture = source.Texture;
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Retorna um SpriteFrame contido na propriedade TextureFrames através de um index.</summary>
        /// <param name="index">Posição na lista a ser acessada.</param>        
        public SpriteFrame this[int index]
        {
            get
            {
                try
                {
                    return Frames[index];
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(ex.Message);
                }
            }
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Adiciona retângulos que representam partes (recortes) em uma folha de sprite (sprite sheet), se a textura é deste tipo.        
        /// </summary>
        /// <param name="x">Posição em X referente a textura.</param>
        /// <param name="y">Posição em Y referente a textura.</param>
        /// <param name="w">Largura referente a textura.</param>
        /// <param name="h">Altura referente a textura.</param>
        /// </param>
        public void AddFrame(int x, int y, int w, int h) => AddFrame(new SpriteFrame(x, y, w, h));        

        /// <summary>
        /// Adiciona retângulos que representam partes (recortes) em uma folha de sprite (sprite sheet), se a textura é deste tipo.        
        /// </summary>
        /// <param name="frame">Um objeto da classe SpriteFrame.</param>
        public void AddFrame(SpriteFrame frame)
        {
            Frames.Add(frame);
        }

        /// <summary>
        /// Cria um instância da classe Sprite, com uma textura retangular.
        /// </summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="size">O tamanho do retângulo</param>
        /// <param name="color">A cor definida.</param>
        public static Sprite GetRectangle(Game game, Point size, Color color)
        {
            if(game == null)
                throw new ArgumentNullException(nameof(game));

            if(size.X <= 0 || size.Y <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            Color[] data;
            Texture2D texture;

            //Inicializa a textura com o tamanho definido no retângulo.
            texture = new Texture2D(game.GraphicsDevice, size.X, size.Y);
            //Inicializa o array de cores, sendo a quantidade a multiplicação da altura e largura do retângulo.
            data = new Color[texture.Width * texture.Height];

            //Cada cor do array é setada com a cor definida do argumento.
            for (int i = 0; i < data.Length; ++i)
                data[i] = color;

            //Seta o array de cores a textura
            texture.SetData(data);

            return new Sprite(texture, true);
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
                Texture?.Dispose();
                Texture = null;

                Frames.Clear();
                Frames = null;
            }                

            disposed = true;
        }
    }
}