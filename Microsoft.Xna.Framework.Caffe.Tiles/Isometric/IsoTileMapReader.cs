// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um leitor de mapas de tiles que os desenha na tela.
    /// </summary>
    public class IsoTileMapReader : IUpdateDrawable, IIsoReader, IDisposable
    {
        private short[,] array = null;
        private Screen _screen = null;        

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define o mapa de tiles.</summary>
        public IsoTileMap Map { get; set; } = null;
        /// <summary>Obtém a lista de tiles ordenados pelo método Read(). Point representa a linha e a coluna onde se encontra o Tile.</summary>
        public Dictionary<Point, IsoTile> Tiles { get; private set; } = new Dictionary<Point, IsoTile>();
        /// <summary>Obtém se o método Read() leu todo seu conteúdo e chegou ao fim.</summary>
        public bool IsRead { get; private set; } = false;
        /// <summary>Obtém ou define a posição inicial para o cálculo de ordenação dos tiles.</summary>
        public Vector2 StartPosition { get; set; } = Vector2.Zero;
        /// <summary>Obtém ou define a largura dos tiles para cálculos posteriores.</summary>
        public int TileWidth { get; set; }
        /// <summary>Obtém ou define a altura dos tiles para cálculos posteriores.</summary>
        public int TileHeight { get; set; }

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
        public IsoTileMapReader(Screen screen, IsoTileMap map, int tileWidth, int tileHeight) 
        {
            _screen = screen;
            Map = map;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
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
        public IsoTile GetTile(int row, int column)
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
                this.Map = null;
                this.Tiles.Clear();
                this.Tiles = null;
                this._screen = null;
            }

            disposed = true;
        }
    }
}