using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics.Tile
{
    /// <summary>
    /// Representa um leitor de mapas de tiles que os desenha na tela.
    /// </summary>
    public class IsometricTileMapReader : IUpdateDrawable
    {
        private short[,] array = null;
        private Screen _screen = null;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define o mapa de tiles.</summary>
        public IsometricTileMap Map { get; set; } = null;

        /// <summary>Obtém a lista de tiles ordenados pelo método Read(). Point representa a linha e a coluna onde se encontra o Tile.</summary>
        public Dictionary<Point, IsometricTile> Tiles { get; private set; } = new Dictionary<Point, IsometricTile>();

        /// <summary>Obtém se o método Read() leu todo seu conteúdo e chegou ao fim.</summary>
        public bool IsRead { get; private set; } = false;

        /// <summary>Obtém ou define a posição inicial para o cálculo de ordenação dos tiles.</summary>
        public Vector2 StartPosition { get; set; } = Vector2.Zero;       

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de MapReader.
        /// </summary>
        /// <param name="map">O mapa de tiles a ser lido.</param>
        public IsometricTileMapReader(Screen screen, IsometricTileMap map) 
        {
            _screen = screen;
            Map = map;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Lê o array contido no mapa e ordena as posições dos tiles.
        /// </summary>
        public void Read()
        {
            IsRead = false;

            array = Map.GetMap();
            Tiles.Clear();

            //dimensões do array
            int d0 = array.GetLength(0);
            int d1 = array.GetLength(1);

            for (int row = 0; row < d0; row++)
            {
                for (int col = 0; col < d1; col++)
                {
                    //O valor da posição no array
                    short index = array[row, col];
                    //Recebe o Tile da tabela                    

                    if(Map.Table.ContainsKey(index))
                    {
                        IsometricTile tile = new IsometricTile(Map.Table[index]);
                        //Atualiza todas as animações do tile
                        tile.UpdateBounds();

                        //largura e altura para cálculo
                        int w = tile.Animation.Bounds.Width;
                        int h = tile.Animation.Bounds.Height;
                        float sx = StartPosition.X;
                        float sy = StartPosition.Y;

                        //O cálculo se dá na animação do topo
                        //antes o valor de row era positivo
                        tile.Animation.X = ((w / 2) * -row) + ((w / 2) * col) + sx;
                        tile.Animation.Y = ((h / 2) * col) - ((h / 2) * -row) + sy;
                        
                        Tiles.Add(new Point(row, col), tile);
                    }                    
                }
            }

            IsRead = true;
        }

        /// <summary>
        /// Obtém um Tile informando sua posição no mapa.
        /// </summary>
        /// <param name="row">A linha desejada.</param>
        /// <param name="column">A coluna desejada.</param>
        public IsometricTile GetTile(int row, int column)
        {
            return Tiles[new Point(row, column)];
        }

        /// <summary>
        /// Atualiza os tiles.
        /// </summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public void Update(GameTime gameTime)
        {
            foreach(var t in Tiles.Values)
                t.Update(gameTime);
        }

        /// <summary>Desenha os tiles.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var t in Tiles)
            {   
                if(Util.CheckFieldOfView(_screen,t.Value.Animation.Bounds))
                    t.Value.Draw(gameTime, spriteBatch);
            }
        }
    }
}