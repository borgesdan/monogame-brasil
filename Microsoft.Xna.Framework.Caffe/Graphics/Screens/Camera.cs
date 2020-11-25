// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Estrutura que representa uma projeção de câmera no desenho 2D.
    /// </summary>
    public class Camera : IBoundsable
    {
        private Game game = null;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a posição no eixo X da câmera.</summary>
        public float X { get; set; } = 0;
        /// <summary>Obtém ou define a posição no eixo Y da câmera.</summary>
        public float Y { get; set; } = 0;
        /// <summary>Obtém ou define o zoom da câmera.</summary>
        public float Zoom { get; set; } = 1;
        /// <summary>Obtém ou define o deslocamento do zoom.</summary>
        public Vector2 ZoomOffset { get; set; }

        /// <summary>Obtém ou define a posição da câmera.</summary>
        public Vector2 Position { get => new Vector2(X, Y); set { X = value.X; Y = value.Y; } }
        /// <summary>Obtém o tamanho total do campo de exibição da câmera.</summary> 
        public Rectangle Bounds { get => new Rectangle(Position.ToPoint(), game.GraphicsDevice.Viewport.Bounds.Size); }        

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe Câmera.
        /// </summary>
        /// <param name="x">Posição no eixo X.</param>
        /// <param name="y">Posição no eixo Y.</param>
        /// <param name="zoom">O zoom da câmera (Padrão 1).</param>
        public Camera(Game game, float x, float y, float zoom)
        {
            X = x;
            Y = y;
            Zoom = zoom;
            this.game = game;

            Rectangle view = game.GraphicsDevice.Viewport.Bounds;
            ZoomOffset = new Vector2(view.Width * 0.5f, view.Height * 0.5f);
        }

        /// <summary>
        /// Inicializa uma nova instância da classe Camera.
        /// </summary>
        public Camera(Game game) : this(game, 0, 0, 1f) { }

        /// <summary>
        /// Inicializa uma nova instância da classe Camera como cópia de outra instância.
        /// </summary>
        /// <param name="camera">Instância a ser copiada.</param>
        public Camera(Camera camera)
        {
            X = camera.X;
            Y = camera.Y;
            Zoom = camera.Zoom;
            game = camera.game;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//                
        
        /// <summary>Movimenta a câmera no sentido especificado.</summary>
        /// <param name="amount">O valor a ser movida a câmera.</param>
        public void Move(Vector2 amount)
        {
            X += amount.X;
            Y += amount.Y;
        }

        /// <summary>
        /// Movimenta a câmera no sentido específicado.
        /// </summary>
        /// <param name="x">O valor do movimento no eixo X.</param>
        /// <param name="y">O valor do movimento no eixo Y.</param>
        public void Move(float x, float y)
        {
            Move(new Vector2(x, y));
        } 

        /// <summary>
        /// Aplica zoom na câmera.
        /// </summary>
        /// <param name="zoom">O incremento do zoom.</param>
        public void ZoomIn(float zoom)
        {
            Zoom += zoom;
        }
        
        /// <summary>
        /// Define a posição da câmera
        /// </summary>
        /// <param name="position">O vetor com a posição da câmera.</param>
        public void SetPosition(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
        }

        /// <summary>
        /// Foca a câmera em um determinado objeto da tela.
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="bounds">Os limites do objeto.</param>
        public void Focus(Rectangle bounds)
        {
            X = bounds.Center.X - game.GraphicsDevice.Viewport.Width / 2;
            Y = bounds.Center.Y - game.GraphicsDevice.Viewport.Height / 2;
        }

        /// <summary>Obtém a Matrix a ser usada no método SpriteBatch.Begin(transformMatrix).</summary>
        public Matrix GetTransform()
        {
            Matrix m = Matrix.CreateTranslation(-X - ZoomOffset.X, -Y - ZoomOffset.Y, 0)
                * Matrix.CreateScale(Zoom, Zoom, 1)
                * Matrix.CreateTranslation(ZoomOffset.X, ZoomOffset.Y, 0);

            return m;            
        }        
    }
}