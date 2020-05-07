using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics.Tile
{
    /// <summary>
    /// Representa o mapa de tiles.
    /// </summary>
    public class IsometricTileMap
    {
        // O mapa com a numeração dos tiles.
        private readonly short[,] mapArray = null;        

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
        /// Obtém o mapa com a numeração dos tiles.
        /// </summary>
        public short[,] GetMap()
        {
            return (short[,])mapArray.Clone();
        }
    }
}