// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics.Tile
{
    /// <summary>
    /// Representa o mapa de tiles.
    /// </summary>
    public class IsometricTileMap : IDisposable
    {
        // O mapa com a numeração dos tiles.
        private short[,] mapArray = null;        

        /// <summary>
        /// Obtém ou define a tabela de índices com seus respectivos Tiles.
        /// </summary>
        public Dictionary<short, IsometricTile> Table { get; set; } = new Dictionary<short, IsometricTile>();

        /// <summary>
        /// Inicializa uma nova instância de Map.
        /// </summary>
        /// <param name="array">O array com a numeração de tiles do mapa</param>
        public IsometricTileMap(short[,] array)
        {
            mapArray = array;
        }

        /// <summary>
        /// Inicializa uma nova instância de Map.
        /// </summary>
        /// <param name="array">O array com a numeração de tiles do mapa</param>
        /// <param name="table">A tabela de índices com seus respectivos Tiles.</param>
        public IsometricTileMap(short[,] array, Dictionary<short, IsometricTile> table)
        {
            mapArray = array;
            Table = table;
        }

        /// <summary>
        /// Obtém o mapa com a numeração dos tiles.
        /// </summary>
        public short[,] GetMap()
        {
            return (short[,])mapArray.Clone();
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
                this.mapArray = null;
                this.Table.Clear();
                this.Table = null;
            }

            disposed = true;
        }
    }
}