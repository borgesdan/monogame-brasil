// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um leitor de setores de tiles isometricos.
    /// </summary>
    public class IsoTileSectorReader : IUpdateDrawable, IIsoReader, IDisposable
    {
        short[,] map = null;
        List<short[]> total = new List<short[]>();
        IsoTileSector[,] array = null;
        Screen _screen = null;        
        Dictionary<Point, IsoTileSector> point_sector = new Dictionary<Point, IsoTileSector>();

        /// <summary>Obtém a lista de tiles ordenados pelo método Read(). A chave Point representa a linha e a coluna onde se encontra o Tile.</summary>
        public Dictionary<Point, IsoTile> Tiles { get; private set; } = new Dictionary<Point, IsoTile>();
        /// <summary>Obtém se o método Read() leu todo seu conteúdo e chegou ao fim.</summary>
        public bool IsRead { get; private set; } = false;
        /// <summary>Obtém ou define a posição inicial para o cálculo de ordenação dos tiles.</summary>
        public Vector2 StartPosition { get; set; } = Vector2.Zero;
        /// <summary>Obtém ou define a largura dos tiles para cálculos posteriores.</summary>
        public int TileWidth { get; set; }
        /// <summary>Obtém ou define a altura dos tiles para cálculos posteriores.</summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// Inicializa uma nova instância de SectorReader.
        /// </summary>
        /// <param name="screen">A tela a ser associada.</param>
        /// <param name="sectors">Os setores a serem lidos.</param>
        /// <param name="tileWidth">A largura dos tiles.</param>
        /// <param name="tileHeight">A altura dos tiles.</param>
        public IsoTileSectorReader(Screen screen, IsoTileSector[,] sectors, int tileWidth, int tileHeight)
        {
            _screen = screen;
            array = sectors;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        /// <summary>
        /// Inicializa uma nova instância de SectorReader.
        /// </summary>
        /// <param name="sectors">Os setores a serem lidos.</param>
        public IsoTileSectorReader(IsoTileSector[,] sectors)
        {
            array = sectors;
        }

        /// <summary>
        /// Lê o array contido nos setores e ordena as posições dos tiles.
        /// </summary>
        public void Read()
        {
            //dimensões do array
            int d0 = array.GetLength(0);
            int d1 = array.GetLength(1);

            total = new List<short[]>(d0 * IsoTileSector.Length);

            for (int t = 0; t < total.Capacity; t++)
                total.Add(null);

            //Confere a linha
            for (int row = 0; row < d0; row++)
            {
                //Confiro a coluna
                for (int col = 0; col < d1; col++)
                {
                    //busco o setor na linha e coluna selecionada
                    IsoTileSector s = array[row, col];
                    //recebo o mapa do setor
                    short[,] _map = s.GetMap();

                    int lr = _map.GetLength(0);
                    int lc = _map.GetLength(1);

                    //confiro a linha e a coluna do mapa
                    for (int sr = 0; sr < lr; sr++)
                    {
                        short[] numbers = new short[IsoTileSector.Length];

                        //faço a busca pelos números
                        for (int sc = 0; sc < lc; sc++)
                        {
                            numbers[sc] = _map[sr, sc];

                            point_sector.Add(new Point(sr + (row * lr), sc + (col * lc)), s);
                        }

                        //insiro a linha no array total
                        int ins = sr + (row * lr);

                        short[] index = total[ins];

                        if (index == null)
                            total[ins] = numbers;
                        else
                        {
                            List<short> n = new List<short>();
                            n.AddRange(index);

                            foreach (var j in numbers)
                                n.Add(j);

                            total[ins] = n.ToArray();
                        }
                    }
                }
            }

            map = new short[total.Count, total[01].GetLength(0)];

            for (int i = 0; i < total.Count; i++)
            {
                short[] row = total[i];

                for (int j = 0; j < row.GetLength(0); j++)
                {
                    map[i, j] = row[j];
                }
            }

            ReadFinalMap();
        }

        private void ReadFinalMap()
        {
            IsRead = false;
            Tiles.Clear();

            //dimensões do array
            int d0 = map.GetLength(0);
            int d1 = map.GetLength(1);

            for (int row = 0; row < d0; row++)
            {
                for (int col = 0; col < d1; col++)
                {
                    //O valor da posição no array
                    short index = map[row, col];
                    //Recebe o Tile da tabela
                    Dictionary<short, IsoTile> table = point_sector[new Point(row, col)].Table;

                    if (table.ContainsKey(index))
                    {
                        IsoTile tile = new IsoTile(table[index]);

                        //largura e altura para cálculo
                        //int w = tile.Animation.Bounds.Width;
                        //int h = tile.Animation.Bounds.Height;
                        int w = TileWidth;
                        int h = TileHeight;
                        float sx = StartPosition.X;
                        float sy = StartPosition.Y;

                        //O cálculo se dá na animação do topo
                        //com o valor de row positivo o mapa fica invertido
                        tile.Animation.X = ((w / 2) * -row) + ((w / 2) * col) + sx;
                        tile.Animation.Y = ((h / 2) * col) - ((h / 2) * -row) + sy;
                        tile.UpdateBounds();

                        tile.MapPoint = new Point(row, col);

                        Tiles.Add(new Point(row, col), tile);
                    }
                }
            }

            IsRead = true;
        }

        /// <summary>
        /// Obtém a coordenada de um ponto no mapa geral informando a linha e a coluna de um setor.
        /// </summary>
        /// <param name="sector">O setor informado.</param>
        /// <param name="row">A linha desejada.</param>
        /// <param name="column">A coluna desejada.</param>
        public Point GetPoint(Point sector, int row, int column)
        {
            int r = (IsoTileSector.Length * sector.X) + row;
            int c = (IsoTileSector.Length * sector.Y) + column;

            return new Point(r, c);
        }

        /// <summary>
        /// Obtém um Tile informando sua posição no mapa. Retorna null se não for encontrado.
        /// </summary>
        /// <param name="sector">O setor.</param>
        /// <param name="row">A linha desejada.</param>
        /// <param name="column">A coluna desejada.</param>
        public IsoTile GetTile(Point sector, int row, int column)
        {
            Point p = GetPoint(sector, row, column);
            return GetTile(p.X, p.Y);
        }

        /// <summary>
        /// Obtém um Tile informando sua posição no mapa. Retorna null se não for encontrado.
        /// </summary>
        /// <param name="row">A linha desejada.</param>
        /// <param name="column">A coluna desejada.</param>
        public IsoTile GetTile(int row, int column)
        {
            if (Tiles.ContainsKey(new Point(row, column)))
                return Tiles[new Point(row, column)];
            else
                return null;
        }

        /// <summary>
        /// Obtém o mapa.
        /// </summary>
        public short[,] GetMap()
        {
            return (short[,])map.Clone();
        }

        /// <inheritdoc />
        public void Update(GameTime gameTime)
        {
            foreach (var t in Tiles.Values)
                t.Update(gameTime);
        }

        /// <inheritdoc/>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var t in Tiles)
            {
                if(_screen != null)
                {
                    if (Util.CheckFieldOfView(_screen, t.Value.Animation.Bounds))
                    {
                        t.Value.Draw(gameTime, spriteBatch);
                    }
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

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.array = null;
                this.map = null;
                this.point_sector = null;
                this.total.Clear();
                this.total = null;
                this._screen = null;
                this.Tiles.Clear();
                this.Tiles = null;                
            }

            disposed = true;
        }
    }
}