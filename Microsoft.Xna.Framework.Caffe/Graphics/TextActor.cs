// Danilo Borges Santos, 2020.

using System.Text;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa um ator que armazena e exibe textos.</summary>
    public class TextActor : Actor
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        StringBuilder builder = new StringBuilder();

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a instância de SpriteFont a ser utilizada.</summary>
        public SpriteFont Font { get; set; } = null;
        
        /// <summary>Obtém ou define o texto a ser exibido atráves de uma instância da classe StringBuilder.</summary>
        public StringBuilder Text
        {
            get => builder;
            set
            {
                builder = value;                
                UpdateBounds();
            }
                
        }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância da classe TextActor.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="name">O nome do ator.</param>
        public TextActor(Game game, string name) : base(game, name) { }

        /// <summary>Inicializa uma nova instância da classe TextActor.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="name">O nome do ator.</param>
        /// <param name="font">A fonte para desenho do texto.</param>
        public TextActor(Game game, string name, SpriteFont font) : base(game, name) 
        {
            Font = font;
        }

        /// <summary>Inicializa uma nova instância de TextActor como cópia de outro TextActor.</summary>
        /// <param name="source">O ator a ser copiado.</param>
        public TextActor(TextActor source) : base(source)
        {
            //Para uma cópia profunda
            //Font = GetDeeepCopy(source.Font);
            
            Font = source.Font;
            Text = new StringBuilder(source.Text.ToString());
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Obtém uma nova instância de um SpriteFont através de um cópia profunda.
        /// </summary>
        /// <param name="source">A origem para cópia.</param>
        public SpriteFont GetDeepCopy(SpriteFont source)
        {
            var glyphs = source.Glyphs;
            List<Rectangle> glyphBounds = new List<Rectangle>();
            List<Rectangle> cropping = new List<Rectangle>();
            List<Vector3> kerning = new List<Vector3>();
            List<char> chars = new List<char>();
            
            foreach (var g in glyphs)
            {
                chars.Add(g.Character);
                glyphBounds.Add(g.BoundsInTexture);
                cropping.Add(g.Cropping);
                kerning.Add(new Vector3(g.LeftSideBearing, g.Width, g.RightSideBearing));
            }

            SpriteFont font = new SpriteFont(source.Texture, glyphBounds, cropping, chars, source.LineSpacing, source.Spacing, kerning, source.DefaultCharacter);
            return font;
        }

        /// <summary>Adiciona um objeto SpriteFont ao ator.</summary>
        /// <param name="font">A fonte a ser utilizada no desenho.</param>
        public void SetFont(SpriteFont font) => Font = font;

        /// <summary>Define o caminho do arquivo SpriteFont e adiciona ao ator.</summary>
        /// <param name="path">O caminho da fonte na pasta Content.</param>
        public void SetFont(string path)
        {
            SetFont(Game.Content.Load<SpriteFont>(path));            
        } 

        /// <summary>Atualiza o ator.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            if (!UpdateOffView && !CheckOffView())
                return;

            UpdateBounds();

            base.Update(gameTime);
        }

        /// <summary>Desenha o ator.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            if (Text != null)
                spriteBatch.DrawString(Font, Text, Transform.Position, Transform.Color, Transform.Rotation, Transform.Origin, Transform.Scale, Transform.SpriteEffects, Transform.LayerDepth);

            base._Draw(gameTime, spriteBatch);
        }

        /// <summary>Atualiza os limites do ator.</summary>
        public override void UpdateBounds()
        {
            Vector2 measure;

            if (Text == null || Text.Length == 0)
                measure = Vector2.Zero;
            else
                measure = Font.MeasureString(Text);

            //Atualiza o tamanho da entidade.
            //float cbw = measure.X * Transform.Scale.X;
            //float cbh = measure.Y * Transform.Scale.Y;

            float cbw = measure.X;
            float cbh = measure.Y;

            Transform.Size = new Point((int)cbw, (int)cbh);

            //O tamanho da da entidade e sua posição.
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            //int w = Transform.Width;
            //int h = Transform.Height;
            int w = (int)Transform.ScaledSize.X;
            int h = (int)Transform.ScaledSize.Y;

            var totalOrigin = Transform.Origin * Transform.Scale;

            int recX = (int)(x - totalOrigin.X);
            int recY = (int)(y - totalOrigin.Y);

            Bounds = new Rectangle(recX, recY, w, h);

            //Calcula o BoundsR. 
            Util.CreateRotatedBounds(Transform, totalOrigin, Bounds);

            base.UpdateBounds();
        }

        /// <summary>GetData em TextActor retornará um array vazio.</summary>
        public override Color[] GetData()
        {
            return new Color[0];
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//
        bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Font = null;                    
                Text.Clear();
                Text = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}