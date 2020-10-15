// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um leitor de mapas de tiles que os desenha na tela.
    /// </summary>
    /// <typeparam name="T">T é uma estrutura (int, short, byte)</typeparam>
    public class IsoTileMapReader<T> : IsoReader<T> where T : struct
    {
        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define o mapa de tiles.</summary>
        public IsoTileMap<T> Map { get; set; } = null;        
        /// <summary>Obtém ou define a posição inicial para o cálculo de ordenação dos tiles.</summary>
        public Vector2 StartPosition { get; set; } = Vector2.Zero;        

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de MapReader.
        /// </summary>
        /// <param name="screen">A tela a ser associada a esse leitor (pode ser null).</param>
        /// <param name="map">O mapa de tiles a ser lido.</param>
        /// <param name="tileWidth">A largura dos tiles.</param>
        /// <param name="tileHeight">A altura dos tiles.</param>
        public IsoTileMapReader(Screen screen, IsoTileMap<T> map, int tileWidth, int tileHeight) : base(screen, tileWidth, tileHeight)
        {
            Map = map;            
        }        

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Lê o array contido no mapa e ordena as posições dos tiles.
        /// </summary>
        public override void Read()
        {
            IsRead = false;

            TotalMap = Map.GetMap();
            Tiles.Clear();

            //dimensões do array
            int d0 = TotalMap.GetLength(0);
            int d1 = TotalMap.GetLength(1);

            for (int row = 0; row < d0; row++)
            {
                for (int col = 0; col < d1; col++)
                {
                    //O valor da posição no array
                    T index = TotalMap[row, col];
                    
                    //Recebe o Tile da tabela
                    if(Map.Table.ContainsKey(index))
                    {
                        IsoTile tile = new IsoTile(Map.Table[index]);
                        //Atualiza todas as animações do tile
                        //tile.UpdateBounds();
                        //largura e altura para cálculo
                        //Usa as configuraçõs do tamanho do tile pela animação
                        //int w = tile.Animation.Bounds.Width;
                        //int h = tile.Animation.Bounds.Height;

                        //Usa as configurações de tamanho geral
                        int w = TileWidth;
                        int h = TileHeight;
                        float sx = StartPosition.X;
                        float sy = StartPosition.Y;

                        //O cálculo se dá na animação do topo
                        //antes o valor de row era positivo
                        tile.Actor.Transform.X = ((w / 2) * -row) + ((w / 2) * col) + sx;
                        tile.Actor.Transform.Y = ((h / 2) * col) - ((h / 2) * -row) + sy;
                        
                        Tiles.Add(new Point(row, col), tile);
                    }                    
                }
            }

            IsRead = true;
        }              

        /// <summary>
        /// Atualiza os tiles.
        /// </summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            foreach(var t in Tiles.Values)
                t.Update(gameTime);
        }

        /// <summary>Desenha os tiles.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var t in Tiles)
            {   
                if(Screen != null)
                {
                    t.Value.Draw(gameTime, spriteBatch);
                    //if (Util.CheckFieldOfView(Screen, t.Value.Actor.Bounds))
                    //{

                    //}                        
                }
                else
                {
                    t.Value.Draw(gameTime, spriteBatch);
                }
            }
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
                this.Map = null;
            }

            disposed = true;
            base.Dispose(disposing);
        }
    }
}