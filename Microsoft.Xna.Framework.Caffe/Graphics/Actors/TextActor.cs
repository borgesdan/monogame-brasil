// Danilo Borges Santos, 2020.

using System.Text;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa um ator que armazena e exibe textos.</summary>
    public class TextActor : Actor
    {
        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a instância de SpriteFont a ser utilizada.</summary>
        public SpriteFont Font { get; set; } = null;

        /// <summary>Obtém ou define o texto a ser exibido atráves de uma instância da classe StringBuilder.</summary>
        public StringBuilder Text { get; set; } = new StringBuilder();

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
        
        protected override void _Update(GameTime gameTime)
        {
            UpdateBounds();
        }
        
        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Text != null)
                spriteBatch.DrawString(Font, Text, Transform.Position, Transform.Color, Transform.Rotation, Transform.Origin, Transform.Scale, Transform.SpriteEffects, Transform.LayerDepth);
        }        

        public override void UpdateBounds()
        {
            Vector2 measure = Vector2.Zero;

            if (Text != null && Text.Length > 0)
                measure = Font.MeasureString(Text);
                        
            CalcBounds((int)measure.X, (int)measure.Y, 0, 0);
        }

        public override Color[] GetData()
        {
            return null;
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