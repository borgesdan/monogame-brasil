//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe que guarda uma textura e seus frames.</summary>
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

        /// <summary>
        /// Inicia uma nova instância da classe Sprite informando o caminho da textura 
        /// e se um frame do tamanho da textura será adicionado a propriedade Frames.
        /// </summary>
        /// <param name="game">Instância da classe Game.</param>
        /// <param name="sourceName">O caminho do arquivo de textura na pasta Content.</param>
        /// <param name="addSingleFrame">Defina True para adicionar um frame do tamanho da textura na lista de Frames.</param>
        public Sprite(Game game, string sourceName, bool addSingleFrame) : this(game.Content.Load<Texture2D>(sourceName), addSingleFrame) { }

        /// <summary>
        /// Inicia uma nova instância da classe Sprite informando a textura 
        /// e se um frame do tamanho da textura será adicionado a propriedade Frames.
        /// </summary>
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
            this.Frames = new List<SpriteFrame>(source.Frames);
            //this.Texture = game.Content.Load<Texture2D>(source.Texture.Name);
            this.Texture = source.Texture;
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Retorna um SpriteFrame contido na propriedade TextureFrames através de um index.</summary>
        /// <param name="index">Posição na lista a ser acessada.</param>        
        public SpriteFrame this[int index]
        {
            get => Frames[index];
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Adiciona retângulos que representam partes (recortes) em uma textura.
        /// </summary>
        /// <param name="frames">Frames a serem adicionados.</param>
        public void AddFrame(params SpriteFrame[] frames)
        {
            foreach(var f in frames)
            {
                Frames.Add(f);
            }            
        }

        /// <summary>
        /// Desenha o sprite na tela ou parte dele através de um index.
        /// </summary>
        /// <param name="spriteBatch">A instância do spriteBatch para desenho.</param>
        /// <param name="position">A posição para desenho na tela.</param>
        /// <param name="frameIndex">O index do frame a ser exibido. Defina 0 para exibir o primeiro frame.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, int frameIndex)
        {
            var frame = this[frameIndex].Bounds;

            spriteBatch.Draw(Texture, position, frame, Color.White);
        }

        /// <summary>
        /// Cria um nova instância da classe Sprite com uma textura retangular preenchida com a cor definida.
        /// </summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="size">O tamanho do retângulo</param>
        /// <param name="color">A cor definida.</param>
        public static Sprite GetRectangle(Game game, Point size, Color color)
        {            
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

        /// <summary>
        /// Cria uma nova instância da classe Sprite com uma textura retangular transparente mas com bordas coloridas.
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="size">O tamanho do retângulo.</param>
        /// <param name="borderWidth">O tamanho da borda.</param>
        /// <param name="borderColor">A cor da borda.</param>
        public static Sprite GetRectangle2(Game game, Point size, int borderWidth, Color borderColor)
        {
            //https://stackoverflow.com/questions/13893959/how-to-draw-the-border-of-a-square/13894276            

            Color[] data;
            Texture2D texture;

            texture = new Texture2D(game.GraphicsDevice, size.X, size.Y);
            data = new Color[texture.Width * texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    bool colored = false;
                    for (int i = 0; i <= borderWidth; i++)
                    {
                        if (x == i || y == i || x == texture.Width - 1 - i || y == texture.Height - 1 - i)
                        {
                            data[x + y * texture.Width] = borderColor;
                            colored = true;
                            break;
                        }
                    }

                    if (colored == false)
                        data[x + y * texture.Width] = Color.Transparent;
                }
            }

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
                Texture = null;

                Frames.Clear();
                Frames = null;
            }                

            disposed = true;
        }
    }
}