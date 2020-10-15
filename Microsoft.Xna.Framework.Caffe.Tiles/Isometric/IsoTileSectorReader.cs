// Danilo Borges Santos, 2020.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um leitor de setores de tiles isometricos.
    /// </summary>
    /// <typeparam name="T">T é uma estrutura (int, short, byte)</typeparam>
    public class IsoTileSectorReader<T> : IsoReader<T> where T : struct
    {
        List<T[]> total = new List<T[]>();
        IsoTileSector<T>[,] sectorsList = null;
        Dictionary<Point, IsoTileSector<T>> point_sector = new Dictionary<Point, IsoTileSector<T>>();
        
        /// <summary>Obtém ou define a posição inicial para o cálculo de ordenação dos tiles.</summary>
        public Vector2 StartPosition { get; set; } = Vector2.Zero;        
        /// <summary>Obtém ou define o valor que representa simultaneamente a quantidade de linhas e de colunas de todos os setores.</summary>
        public static int Length { get; set; } = 10;

        /// <summary>
        /// Inicializa uma nova instância de SectorReader.
        /// </summary>
        /// <param name="screen">A tela a ser associada.</param>
        /// <param name="sectors">Os setores a serem lidos.</param>
        /// <param name="tileWidth">A largura dos tiles.</param>
        /// <param name="tileHeight">A altura dos tiles.</param>
        /// <param name="length">Define o valor que representa simultaneamente a quantidade de linhas e de colunas de todos os setores.</param>
        public IsoTileSectorReader(Screen screen, IsoTileSector<T>[,] sectors, int tileWidth, int tileHeight, int length) : base(screen, tileWidth, tileHeight)
        {
            sectorsList = sectors;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Length = length;
        }        

        /// <summary>
        /// Lê o array contido nos setores e ordena as posições dos tiles.
        /// </summary>
        public override void Read()
        {
            //dimensões do array
            int d0 = sectorsList.GetLength(0);
            int d1 = sectorsList.GetLength(1);

            total = new List<T[]>(d0 * Length);

            for (int t = 0; t < total.Capacity; t++)
                total.Add(null);

            //Confere a linha
            for (int row = 0; row < d0; row++)
            {
                //Confiro a coluna
                for (int col = 0; col < d1; col++)
                {
                    //busco o setor na linha e coluna selecionada
                    IsoTileSector<T> s = sectorsList[row, col];
                    //recebo o mapa do setor
                    T[,] _map = s.GetMap();

                    int lr = _map.GetLength(0);
                    int lc = _map.GetLength(1);

                    //confiro a linha e a coluna do mapa
                    for (int sr = 0; sr < lr; sr++)
                    {
                        T[] numbers = new T[Length];

                        //faço a busca pelos números
                        for (int sc = 0; sc < lc; sc++)
                        {
                            numbers[sc] = _map[sr, sc];

                            point_sector.Add(new Point(sr + (row * lr), sc + (col * lc)), s);
                        }

                        //insiro a linha no array total
                        int ins = sr + (row * lr);

                        T[] index = total[ins];

                        if (index == null)
                            total[ins] = numbers;
                        else
                        {
                            List<T> n = new List<T>();
                            n.AddRange(index);

                            foreach (var j in numbers)
                                n.Add(j);

                            total[ins] = n.ToArray();
                        }
                    }
                }
            }

            TotalMap = new T[total.Count, total[01].GetLength(0)];

            for (int i = 0; i < total.Count; i++)
            {
                T[] row = total[i];

                for (int j = 0; j < row.GetLength(0); j++)
                {
                    TotalMap[i, j] = row[j];
                }
            }

            ReadFinalMap();
        }

        private void ReadFinalMap()
        {
            IsRead = false;
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
                    Dictionary<T, IsoTile> table = point_sector[new Point(row, col)].Table;

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
                        tile.Actor.Transform.X = ((w / 2) * -row) + ((w / 2) * col) + sx;
                        tile.Actor.Transform.Y = ((h / 2) * col) - ((h / 2) * -row) + sy;
                        tile.UpdateBounds();

                        tile.MapPoint = new Point(row, col);

                        Tiles.Add(new Point(row, col), tile);
                    }
                }
            }

            IsRead = true;
        }

        /// <summary>
        /// Obtém a linha e a coluna no mapa geral informando o setor.
        /// </summary>
        /// <param name="sector">O setor informado.</param>
        /// <param name="row">A linha desejada.</param>
        /// <param name="column">A coluna desejada.</param>
        public Point GetPoint(Point sector, int row, int column)
        {
            int r = (Length * sector.X) + row;
            int c = (Length * sector.Y) + column;

            return new Point(r, c);
        }

        /// <summary>
        /// Obtém a posição do tile informando o setor, a linha e a coluna desejada
        /// </summary>
        /// <param name="sector">O setor informado.</param>
        /// <param name="row">A linha desejada.</param>
        /// <param name="column">A coluna desejada.</param>
        public Vector2 GetPosition(Point sector, int row, int column)
        {
            Point point = GetPoint(sector, row, column);
            IsoTile tile = GetTile(point.X, point.Y);

            return tile.Actor.Transform.Position;
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
        /// Obtém o valor da posição informada.
        /// </summary>
        /// <param name="sector">O setor a ser buscado.</param>
        /// <param name="row">A linha desejada do setor.</param>
        /// <param name="column">A coluna desejada do setor.</param>
        public T GetValue(Point sector, int row, int column)
        {
            T[,] m = sectorsList[sector.X, sector.Y].GetMap();
            return m[row, column];
        }        

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            foreach (var t in Tiles.Values)
                t.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var t in Tiles)
            {
                if(Screen != null)
                {
                    t.Value.Draw(gameTime, spriteBatch);
                    //if (Util.CheckFieldOfView(_screen, t.Value.Actor.Bounds))
                    //{
                    //    t.Value.Draw(gameTime, spriteBatch);
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
                this.sectorsList = null;
                this.point_sector = null;
                this.total.Clear();
                this.total = null;              
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}