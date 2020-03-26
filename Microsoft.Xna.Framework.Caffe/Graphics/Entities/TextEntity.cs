﻿// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

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

        StringBuilder builder = new StringBuilder();
        bool outOfView = false;

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
            Font = source.Font;
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

            //Calcula o BoundsR.
            Rectangle frame = new Rectangle(0, 0, Transform.Width, Transform.Height);
            var r = Rotation.GetRotation(frame, Origin, Transform.Rotation);

            BoundsR.Points.Clear();
            BoundsR.Points.Add(r.P1.ToVector2());
            BoundsR.Points.Add(r.P2.ToVector2());
            BoundsR.Points.Add(r.P3.ToVector2());
            BoundsR.Points.Add(r.P4.ToVector2());

            var xb = Transform.X - Origin.X;
            var yb = Transform.Y - Origin.Y;

            BoundsR.Offset(new Vector2(xb, yb));
            BoundsR.BuildEdges();

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