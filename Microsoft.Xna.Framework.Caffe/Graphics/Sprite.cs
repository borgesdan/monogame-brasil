// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Armazena uma instância da classe Texture2D e seus frames.</summary>
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
        /// <summary>Obtém ou define a lista de caixas de colisão.</summary>
        public List<CollisionBox> CollisionBoxes { get; set; } = new List<CollisionBox>();
        /// <summary>Obtém ou define a lista de caixas de ataque.</summary>
        public List<AttackBox> AttackBoxes { get; set; } = new List<AttackBox>();

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//        

        /// <summary>
        /// Inicia uma nova instância da classe Sprite.
        /// </summary>
        /// <param name="game">Instância da classe Game.</param>
        /// <param name="sourceName">O caminho do arquivo de textura na pasta Content.</param>
        /// <param name="isSingleFrame">
        /// Defina True para informar que o Texture2D não é uma no estilo spritesheet para ser adicionado um SpriteFrame do tamanho da textura.
        /// </param>
        public Sprite(Game game, string sourceName, bool isSingleFrame) : this(game.Content.Load<Texture2D>(sourceName), isSingleFrame) { }

        /// <summary>
        /// Inicia uma nova instância da classe Sprite.
        /// </summary>
        /// <param name="texture">Um objeto da classe Texture2D.</param>        
        /// <param name="isSingleFrame">
        /// Defina True para informar que o Texture2D não é uma no estilo spritesheet para ser adicionado um SpriteFrame do tamanho da textura.
        /// </param>
        public Sprite(Texture2D texture, bool isSingleFrame)
        {
            Texture = texture;

            if (isSingleFrame)
            {
                SpriteFrame defaultFrame = SpriteFrame.Create(texture.Bounds, Vector2.Zero);
                Frames.Add(defaultFrame);
            }            
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Sprite como cópida de outra instância.
        /// </summary>
        /// <param name="source">A instância a ser copiada. A propriedade Texture será refereciada.</param>
        public Sprite(Sprite source)
        {            
            source.Frames.ForEach(f => Frames.Add(f));
            source.CollisionBoxes.ForEach(c => CollisionBoxes.Add(c));
            source.AttackBoxes.ForEach(a => AttackBoxes.Add(a));
            
            this.Texture = source.Texture;

            //Para uma cópia profunda de Texture2D é necessário esse código:
            //this.Texture = game.Content.Load<Texture2D>(source.Texture.Name);
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Retorna um SpriteFrame contido na propriedade Frames através de um index.</summary>
        /// <param name="index">Posição na lista a ser acessada.</param>        
        public SpriteFrame this[int index]
        {
            get => Frames[index];
            set => Frames[index] = value;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Adiciona objetos SpriteFrame na lista de Frames.
        /// </summary>
        /// <param name="frames">Frames a serem adicionados.</param>
        public void AddFrame(params SpriteFrame[] frames)
        {
            foreach(var f in frames) Frames.Add(f);
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
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="size">O tamanho do retângulo.</param>
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

                CollisionBoxes.Clear();
                CollisionBoxes = null;

                AttackBoxes.Clear();
                AttackBoxes = null;
            }                

            disposed = true;
        }
    }
}