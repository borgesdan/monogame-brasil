// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Estrutura que representa uma projeção de câmera no desenho 2D.
    /// </summary>
    public class Camera
    {
        private Game game = null;

        /// <summary>Obtém ou define a posição no eixo X da câmera.</summary>
        public float X { get; set; }
        /// <summary>Obtém ou define a posição no eixo Y da câmera.</summary>
        public float Y { get; set; }
        /// <summary>Obtém ou define o zoom da câmera.</summary>
        public float Zoom { get; set; }

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a posição da câmera.</summary>
        public Vector2 Position { get => new Vector2(X, Y); set { X = value.X; Y = value.Y; } }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância da classe Câmera.
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
        }

        /// <summary>
        /// Cria uma nova instância da classe Câmera.
        /// </summary>
        public Camera(Game game) : this(game, 0, 0, 1f) { }

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
        
        /// <summary>
        /// Obtém o tamanho total do campo de exibição da câmera.
        /// </summary>        
        public Rectangle GetBounds()
        {
            return new Rectangle(Position.ToPoint(), game.Window.ClientBounds.Size);
        }

        /// <summary>Movimenta a câmera no sentido específicado.</summary>
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
            X = bounds.Center.X - game.Window.ClientBounds.GetHalfW();
            Y = bounds.Center.Y - game.Window.ClientBounds.GetHalfH();
        }

        /// <summary>Obtém a Matrix a ser usada no método SpriteBatch.Begin(transformMatrix).</summary>
        public Matrix GetTransform()
        {
            Matrix m = new Matrix();
            m += Matrix.CreateTranslation(-X, -Y, 0) * Matrix.CreateScale(Zoom, Zoom, 1);

            return m;            
        }        
    }

    /// <summary>
    /// Providência acesso rápido para manipular a câmera da tela selecionada.
    /// </summary>
    public static class SCamera
    {
        private static Screen _screen = null;

        /// <summary>
        /// Obtém a câmera ativa da tela.
        /// </summary>
        public static Camera GetCamera()
        {
            return _screen.Camera;
        }

        /// <summary>
        /// Providência acesso rápido para mover a câmera nos eixos X e Y.
        /// </summary>
        public static void Move(Vector2 movement)
        {
            Camera camera = _screen.Camera;
            camera.Move(movement);

            _screen.Camera = camera;
        }

        /// <summary>
        /// Providência acesso rápido para mover a câmera nos eixos X e Y.
        /// </summary>
        public static void Move(float x, float y) => Move(new Vector2(x, y));

        /// <summary>
        /// Define a posição da câmera.
        /// </summary>
        public static void SetPosition(Vector2 position)
        {
            Camera camera = _screen.Camera;

            camera.X = position.X;
            camera.Y = position.Y;

            _screen.Camera = camera;
        }

        /// <summary>
        /// Define a posição da câmera.
        /// </summary>
        public static void SetPosition(float x, float y) => SetPosition(new Vector2(x, y));

        public static void SetZoom(float zoom)
        {
            Camera camera = _screen.Camera;

            camera.Zoom = zoom;

            _screen.Camera = camera;
        }

        /// <summary>
        /// Define a tela com a câmera ativa
        /// </summary>        
        public static void SetScreen(Screen screen)
        {
            _screen = screen;
        }

        /// <summary>
        /// Foca a câmera em um determinado objeto da tela.
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="bounds">Os limites do objeto.</param>
        public static void Focus(Game game, Rectangle bounds)
        {
            Camera camera = _screen.Camera;

            camera.X = bounds.Center.X - game.Window.ClientBounds.GetHalfW();
            camera.Y = bounds.Center.Y - game.Window.ClientBounds.GetHalfH();

            _screen.Camera = camera;
        }
    }
}