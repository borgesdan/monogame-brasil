using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa uma área de jogo com tamanho predefinido, para cálculos e demilitações.
    /// </summary>
    public class Area : Actor
    {
        int a_width = 0;
        int a_height = 0;
        Sprite rectangle = null;

        /// <summary>
        /// Obtém ou define se a área será visível.
        /// </summary>
        public bool ShowArea { get; set; } = false;        

        /// <summary>
        /// Inicializa uma nova instância de Area como cópia de outra instância.
        /// </summary>
        /// <param name="source">A instânca da classe Area a ser copiada.</param>
        public Area(Area source) : base(source)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Area.
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        /// <param name="width">A largura da área.</param>
        /// <param name="height">A altura da área</param>
        public Area(Game game, string name, int width, int height) : base(game, name)
        {
            this.a_width = width;
            this.a_height = height;
            this.Transform.Size = new Point(width, height);
            this.rectangle = Sprite.GetRectangle(game, "", new Point(width, height), Color.White);
        }

        public override Color[] GetData()
        {
            return null;
        }

        public override void UpdateBounds()
        {
            CalcBounds(a_width, a_height, 0, 0);
        }

        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(ShowArea)
            {
                Color color = Color.Lerp(Transform.Color, Color.Transparent, 0.8f);
                rectangle.Transform.Set(Transform);
                rectangle.Transform.Color = color;

                rectangle.Draw(gameTime, spriteBatch);
            }
        }

        protected override void _Update(GameTime gameTime)
        {
            UpdateBounds();
        }
    }
}
