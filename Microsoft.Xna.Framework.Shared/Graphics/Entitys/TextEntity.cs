// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma entidade que armazena e exibe textos.</summary>
    public class TextEntity : Entity2D
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        StringBuilder builder = null;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém um objeto SpriteFont a ser utilizado.</summary>
        public SpriteFont Font { get; set; } = null;
        
        /// <summary>Obtém ou define o texto a ser exibido atráves de um objeto StringBuilder.</summary>
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

        /// <summary>Inicializa uma nova instância da classe TextEntity.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        public TextEntity(Game game, string name) : base(game, name) { }

        /// <summary>Inicializa uma nova instância da classe TextEntity.</summary>
        /// <param name="screen">A tela em que a entidade será associada.</param>
        /// <param name="name">O nome da entidade.</param>
        public TextEntity(Screen screen, string name) : base(screen, name) { }

        /// <summary>Inicializa uma nova instância de TextEntity copiando uma outra entidade.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        public TextEntity(TextEntity source) : base(source)
        {
            Text = source.Text;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>Adicionar um objeto SpriteFont à entidade.</summary>
        /// <param name="font">A fonte a ser utilizada no desenho.</param>
        public void SetFont(SpriteFont font) => Font = font;

        /// <summary>Use para definir o caminho do arquivo SpriteFont e adicionar à entidade.</summary>
        /// <param name="path">O caminho da fonte da pasta Content.</param>
        public void SetFont(string path)
        {
            SetFont(Game.Content.Load<SpriteFont>(path));
        }

        /// <summary>Define o texto a ser exibido e a propriedade UseStringBuilder como True.</summary>
        /// <param name="stringBuilder">O texto a ser desenhado.</param>
        public void SetText(StringBuilder stringBuilder)
        {
            Text = stringBuilder;
        }

        /// <summary>Acrescenta um texto a string vigente.</summary>
        /// <param name="text">O texto a ser acrescentado.</param>
        public void AppendText(string text)
        {
            Text.Append(text);
        }        

        /// <summary>Atualiza a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;            

            UpdateBounds();

            base.Update(gameTime);
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            if (Text != null)
                spriteBatch.DrawString(Font, Text, Transform.Position, Transform.Color, Transform.Rotation, Origin, Transform.Scale, Transform.SpriteEffect, LayerDepth);

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>Atualizar os limites da entidade.</summary>
        public override void UpdateBounds()
        {
            Vector2 measure;

            if (Text == null || Text.Length == 0)
                measure = Vector2.Zero;
            else
                measure = Font.MeasureString(Text);

            //Atualiza o tamanho da entidade.
            float cbw = measure.X * Transform.Scale.X;
            float cbh = measure.Y * Transform.Scale.Y;
            Transform.Size = new Point((int)cbw, (int)cbh);

            //O tamanho da da entidade e sua posição.
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            int w = Transform.Width;
            int h = Transform.Height;

            int recX = (int)(x + Origin.X);
            int recY = (int)(y - Origin.Y);

            Bounds = new Rectangle(recX, recY, w, h);

            base.UpdateBounds();
        }

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

            base.Dispose(disposing);
        }
    }
}