// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma entidade que armazena e exibe textos.</summary>
    public class TextEntity : Entity2D
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        StringBuilder builder = new StringBuilder();
        bool outOfView = false;

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

        /// <summary>Inicializa uma nova instância da classe TextEntity.</summary>
        /// <param name="game">A instância ativa da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        public TextEntity(Game game, string name) : base(game, name) { }        

        /// <summary>Inicializa uma nova instância da classe TextEntity.</summary>
        /// <param name="screen">A tela em que a entidade será associada.</param>
        /// <param name="name">O nome da entidade.</param>
        public TextEntity(Screen screen, string name) : base(screen, name) { }        

        /// <summary>Inicializa uma nova instância de TextEntity como cópia de outro TextEntity.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        public TextEntity(TextEntity source) : base(source)
        {
            var glyphs = source.Font.Glyphs;
            List<Rectangle> gs = new List<Rectangle>();
            List<Rectangle> cs = new List<Rectangle>();
            List<Vector3> ks = new List<Vector3>();
            List<char> chars = new List<char>();
            foreach (var g in glyphs)
            {
                chars.Add(g.Character);
                gs.Add(g.BoundsInTexture);
                cs.Add(g.Cropping);
                ks.Add(new Vector3(g.LeftSideBearing, g.Width, g.RightSideBearing));
            }

            SpriteFont font = new SpriteFont(source.Font.Texture, gs, cs, chars, source.Font.LineSpacing, source.Font.Spacing, ks, source.Font.DefaultCharacter);
            
            Font = font;
            Text = source.Text;
        }

        /// <summary>
        /// Cria uma nova instância de TextEntity quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">A entidade a ser copiada.</param>
        public override T Clone<T>(T source)
        {
            if (source is TextEntity)
                return (T)Activator.CreateInstance(typeof(TextEntity), source);
            else
                throw new InvalidCastException();
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>Adiciona um objeto SpriteFont à entidade.</summary>
        /// <param name="font">A fonte a ser utilizada no desenho.</param>
        public void SetFont(SpriteFont font) => Font = font;

        /// <summary>Define o caminho do arquivo SpriteFont e adiciona à entidade.</summary>
        /// <param name="path">O caminho da fonte na pasta Content.</param>
        public void SetFont(string path)
        {
            SetFont(Game.Content.Load<SpriteFont>(path));
        }          

        /// <summary>Atualiza a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            //Se UpdateOutOfView é false, então é necessário saber se a entidade está dentro dos limites de desenho da tela.
            if (!UpdateOutOfView)
            {
                if (Screen != null)
                {
                    if (!Util.CheckFieldOfView(Screen, Bounds))
                    {
                        //Se o resultado for false, definimos 'outOfView' como true para verificação no método Draw.
                        outOfView = true;

                        return;
                    }
                }
            }

            UpdateBounds();

            base.Update(gameTime);
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible || outOfView)
                return;

            if (Text != null)
                spriteBatch.DrawString(Font, Text, Transform.Position, Transform.Color, Transform.Rotation, Origin, Transform.Scale, Transform.SpriteEffect, LayerDepth);

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>Atualiza os limites da entidade.</summary>
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

            var totalOrigin = Origin * Transform.Scale;

            int recX = (int)(x - totalOrigin.X);
            int recY = (int)(y - totalOrigin.Y);

            Bounds = new Rectangle(recX, recY, w, h);

            //Calcula o BoundsR. 
            Util.CreateBoundsR(this, totalOrigin, Bounds);

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