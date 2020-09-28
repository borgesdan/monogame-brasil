// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Ator que armazena um Texture2D e seus frames.</summary>
    public class Sprite : Actor
    {
        //----------------------------------------//
        //-----         PROPRIEDADES         -----//
        //----------------------------------------//

        /// <summary>Obtém a textura para desenho.</summary>
        public Texture2D Texture { get; private set; } = null;
        /// <summary>Obtém ou define os SpriteFrames e suas listas de boxes.</summary>
        public BoxGroup Boxes { get; set; } = null;
        /// <summary>Obtém ou define o index atual da lista de Boxes.</summary>
        public int CurrentIndex { get; set; } = 0;

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//        

        /// <summary>
        /// Inicia uma nova instância da classe Sprite.
        /// </summary>
        /// <param name="game">Instância corrente da classe Game.</param>
        /// <param name="sourceName">O caminho do arquivo de textura na pasta Content.</param>
        /// <param name="isSingleFrame">
        /// Defina True para informar que o Texture2D não é um spritesheet. Será adicionado um SpriteFrame do tamanho da textura.
        /// </param>
        public Sprite(Game game, string sourceName, bool isSingleFrame = false) : this(game, game.Content.Load<Texture2D>(sourceName), isSingleFrame) { }

        /// <summary>
        /// Inicia uma nova instância da classe Sprite.
        /// </summary>
        /// <param name="game">Instância corrente da classe Game.</param>
        /// <param name="texture">Um objeto da classe Texture2D.</param>        
        /// <param name="isSingleFrame">
        /// Defina True para informar que o Texture2D não é um spritesheet. Será adicionado um SpriteFrame do tamanho da textura.
        /// </param>
        public Sprite(Game game, Texture2D texture, bool isSingleFrame = false) : base(game)
        {
            Texture = texture;
            Boxes = new BoxGroup(this);

            if (isSingleFrame)
            {
                SpriteFrame defaultFrame = SpriteFrame.Create(texture.Bounds, Vector2.Zero);
                Boxes.Add(defaultFrame, null, null);
            }            
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Sprite como cópia de outra instância.
        /// </summary>
        /// <param name="source">A instância a ser copiada. A propriedade Texture será refereciada.</param>
        public Sprite(Sprite source): base(source)
        {
            this.Boxes = new BoxGroup(source.Boxes);
            this.Texture = source.Texture;

            //Para uma cópia profunda de Texture2D é necessário esse código:
            //this.Texture = game.Content.Load<Texture2D>(source.Texture.Name);
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Obtém ou define um SpriteFrame contido na propriedade Boxes através de um index.</summary>
        /// <param name="index">Posição na lista a ser acessada.</param>        
        public SpriteFrame this[int index]
        {
            get => Boxes.GetSpriteFrame(index);
            set => Boxes.Values[index].SpriteFrame = value;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Atualiza o Sprite.
        /// </summary>
        /// <param name="gameTime">Obtém os tempos de jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            UpdateBounds();

            base.Update(gameTime);
        }

        /// <summary>
        /// Desenha o sprite.
        /// </summary>
        /// <param name="spriteBatch">A instância corrente do SpriteBatch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            spriteBatch.Draw(Texture, Transform.Position, this[CurrentIndex].Bounds, Transform.Color, Transform.Rotation, Transform.Origin, Transform.Scale, Transform.SpriteEffects, Transform.LayerDepth);

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Atualiza os limites do Sprite.
        /// </summary>
        public override void UpdateBounds() 
        {
            Point size = this[CurrentIndex].Bounds.Size;
            Transform.Size = size;

            //O tamanho da entidade e sua posição.
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            int w = (int)Transform.ScaledSize.X;
            int h = (int)Transform.ScaledSize.Y;

            //A origem do frame.
            Vector2 sa = this[CurrentIndex].Align;
            Vector2 finalOrigin = ((Transform.Origin + sa) * Transform.Scale);

            int recX = (int)(x - finalOrigin.X);
            int recY = (int)(y - finalOrigin.Y);

            Bounds = new Rectangle(recX, recY, w, h);
            BoundsR = Util.CreateBoundsR(Transform, finalOrigin, Bounds);

            base.UpdateBounds();
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

            Sprite s = new Sprite(game, texture, true);
            s.UpdateBounds();

            return s;
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

            Sprite s = new Sprite(game,texture, true);
            s.UpdateBounds();

            return s;
        }

        /// <summary>
        /// Obtém o conteúdo de cores da textura definida pelo SpriteFrame do valor do CurrentIndex.
        /// Caso não houve mudança no CurrentIndex e no SpriteEffects retornará o último array Color.
        /// </summary>
        public override Color[] GetData()
        {
            SpriteFrame frame = this[CurrentIndex];
            Color[] colors = new Color[frame.Width * frame.Height];

            Texture.GetData(0, frame.Bounds, colors, 0, colors.Length);

            return GetDataHelper(frame.Bounds, colors);
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
                Texture = null;
                Boxes = null;
                Transform = null;                
            }                

            disposed = true;

            base.Dispose(disposing);
        }
    }
}