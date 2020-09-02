// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa o mapa de tiles.
    /// </summary>
    public class IsoTileMap<T> : IDisposable where T : struct
    {
        // O mapa com a numeração dos tiles.
        private T[,] mapArray = null;        

        /// <summary>
        /// Obtém ou define a tabela de índices com seus respectivos Tiles.
        /// </summary>
        public Dictionary<T, IsoTile> Table { get; set; } = new Dictionary<T, IsoTile>();

        /// <summary>
        /// Inicializa uma nova instância de IsoTileMap.
        /// </summary>
        /// <param name="array">O array com a numeração de tiles do mapa</param>
        public IsoTileMap(T[,] array)
        {
            mapArray = array;
        }

        /// <summary>
        /// Inicializa uma nova instância de IsoTileMap.
        /// </summary>
        /// <param name="array">O array com a numeração de tiles do mapa</param>
        /// <param name="table">A tabela de índices com seus respectivos Tiles.</param>
        public IsoTileMap(T[,] array, Dictionary<T, IsoTile> table)
        {
            mapArray = array;
            Table = table;
        }

        /// <summary>
        /// Obtém o mapa com a numeração dos tiles.
        /// </summary>
        public T[,] GetMap()
        {
            return (T[,])mapArray.Clone();
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