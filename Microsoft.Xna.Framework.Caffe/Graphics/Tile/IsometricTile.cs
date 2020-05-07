namespace Microsoft.Xna.Framework.Graphics.Tile
{
    /// <summary>
    /// Representa um tile isométrico.
    /// </summary>
    public class IsometricTile : IUpdateDrawable
    {
        public static int TILE_WIDTH { get; set; } = 170;
        public static int TILE_HEIGHT { get; set; } = 85;

        /// <summary>Obtém ou define a animação do topo do Tile.</summary>
        public Animation Animation { get; set; } = null;
        /// <summary>Obtém ou define onde o tile se encontra no mapa.</summary>
        public Point MapPoint { get; set; } = Point.Zero;
        /// <summary>Obtém ou define o valor do Tile.</summary>
        public short Value { get; set; } = 0;

        /// <summary>
        /// Inicializa uma nova instância de Tile.
        /// </summary>
        public IsometricTile() { }

        /// <summary>
        /// Inicializa uma nova instância de Tile.
        /// </summary>
        /// <param name="animation">Define a animação do topo do Tile</param>
        public IsometricTile(Animation animation)
        {
            Animation = animation;
        }

        /// <summary>
        /// Inicializa uma nova instância de Tile como cópia de outra instância de Tile.
        /// </summary>
        /// <param name="source">A instância de origem.</param>
        public IsometricTile(IsometricTile source)
        {
            if(source.Animation != null)
                Animation = new Animation(source.Animation);
            
            Value = source.Value;
        }

        /// <summary>
        /// Atualiza o tamanho da animação.
        /// </summary>
        public void UpdateBounds()
        {
            Animation?.UpdateBounds();
        }

        /// <summary>
        /// Atualiza o tile.
        /// </summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public void Update(GameTime gameTime)
        {
            Animation?.Update(gameTime);
        }
        
        /// <summary>Desenha o tile.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Animation?.Draw(gameTime, spriteBatch);
        }
    }
}