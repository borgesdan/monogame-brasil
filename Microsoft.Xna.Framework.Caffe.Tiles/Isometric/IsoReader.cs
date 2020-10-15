using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Represenha um leitor de tiles isométricos.
    /// </summary>
    /// <typeparam name="T">T é uma estrutura (int, short, byte)</typeparam>
    public abstract class IsoReader<T> : IUpdateDrawable, IIsoReader, IDisposable where T : struct
    {
        protected T[,] TotalMap = null;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        protected Screen Screen { get; set; } = null;
        /// <summary>Obtém se o método Read() leu todo seu conteúdo e chegou ao fim.</summary>
        public bool IsRead { get; protected set; } = false;
        /// <summary>Obtém a lista de tiles ordenados pelo método Read(). Point representa a linha e a coluna onde se encontra o Tile.</summary>
        public Dictionary<Point, IsoTile> Tiles { get; protected set; } = new Dictionary<Point, IsoTile>();
        /// <summary>Obtém ou define a largura dos tiles para cálculos posteriores.</summary>
        public int TileWidth { get; set; }
        /// <summary>Obtém ou define a altura dos tiles para cálculos posteriores.</summary>
        public int TileHeight { get; set; }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        protected IsoReader(Screen screen, int tileWidth, int tileHeight)
        {
            Screen = screen;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public virtual void Read()
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }        

        public virtual void Update(GameTime gameTime)        
        {            
        }        

        /// <summary>
        /// Obtém o valor da posição informada no mapa total.
        /// </summary>
        /// <param name="row">A linha desejada.</param>
        /// <param name="column">A coluna desejada.</param>
        public T GetValue(int row, int column)
        {
            return TotalMap[row, column];
        }

        /// <summary>
        /// Obtém um Tile informando sua posição no mapa total. Retorna null se não for encontrado.
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
        /// Obtém a posição do tile informando a linha e a coluna no mapa total.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Vector2 GetPosition(int row, int column)
        {
            IsoTile tile = GetTile(row, column);
            return tile.Actor.Transform.Position;
        }

        /// <summary>
        /// Obtém o mapa total.
        /// </summary>
        public short[,] GetMap()
        {
            return (short[,])TotalMap.Clone();
        }

        /// <summary>
        /// Obtém o número de elementos do mapa total informando a dimensão.
        /// </summary>
        /// <param name="dimension">Linha = 0 ou coluna = 1</param>        
        public int GetLength(int dimension)
        {
            return TotalMap.GetLength(dimension);
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//

        private bool disposed = false;

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
                TotalMap = null;
                Tiles.Clear();
                Tiles = null;
            }

            disposed = true;
        }
    }
}