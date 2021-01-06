//Danilo Borges Santos, 2020

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um ator vazio de tamanho 0.
    /// </summary>
    public class EmptyActor : Actor
    {
        Sprite verticalLine = null;
        Sprite horizontalLine = null;
        Sprite center = null;

        /// <summary>
        /// Obtém ou define se deve exibir os eixos X e Y do ator.
        /// </summary>
        public bool ShowLocation { get; set; } = false;

        /// <summary>
        /// Inicializa uma nova instância de EmptyActor como cópia de outra instância.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public EmptyActor(EmptyActor source) : base(source)
        {
            verticalLine = source.verticalLine;
            horizontalLine = source.horizontalLine;
            center = source.center;
        }

        /// <summary>
        /// Inicializa uma nova instância de EmptyActor
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="name">O nome do ator.</param>
        public EmptyActor(Game game, string name) : base(game, name)
        {
            verticalLine = Sprite.GetRectangle(game, "vertical", new Point(1, 50), Color.Green);
            horizontalLine = Sprite.GetRectangle(game, "horizontal", new Point(50, 1), Color.Red);
            center = Sprite.GetRectangle(game, "center", new Point(5, 5), Color.Black);
        }
        
        public override Color[] GetData() => null;

        public override void UpdateBounds() 
        {            
            CalcBounds(0, 0, 0, 0);
        }

        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (ShowLocation)
            {
                horizontalLine.Transform.Position = this.Transform.Position;
                
                verticalLine.Transform.X = this.Transform.X;
                verticalLine.Transform.Y = this.Transform.Y - 50;

                center.Transform.Origin = new Vector2(center.Transform.Width / 2, center.Transform.Height / 2);
                center.Transform.Position = this.Transform.Position;


                horizontalLine.Draw(gameTime, spriteBatch);
                verticalLine.Draw(gameTime, spriteBatch);
                center.Draw(gameTime, spriteBatch);
            }
        }

        protected override void _Update(GameTime gameTime)
        {
            UpdateBounds();
        }
    }
}