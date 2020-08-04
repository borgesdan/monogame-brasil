//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Estrutura que representa uma projeção de câmera no desenho 2D.
    /// </summary>
    public struct Camera : IEquatable<Camera>
    {        
        /// <summary>Obtém ou define a posição da câmera.</summary>
        public Vector2 Position;
        /// <summary>Obtém ou define a escala da câmera.</summary>
        public Vector2 Scale;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a posição no eixo X da câmera.</summary>
        public float X 
        {
            get => Position.X;
            set => Position = new Vector2(value, Position.Y);
        }

        /// <summary>Obtém ou define a posição no eixo Y da câmera.</summary>
        public float Y
        {
            get => Position.Y;
            set => Position = new Vector2(Position.X, value);
        }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        public Camera(Vector2 position, Vector2 scale)
        {            
            Position = position;
            Scale = scale;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Cria uma nova instância da estrutura Camera.</summary>
        public static Camera Create()
        {
            return new Camera(Vector2.Zero, Vector2.One);
        }    
        
        /// <summary>
        /// Obtém o tamanho total do campo de exibição da câmera.
        /// </summary>
        /// <param name="game">A instância atual da classe Game.</param>
        public Rectangle GetBounds(Game game)
        {
            return new Rectangle(Position.ToPoint(), game.Window.ClientBounds.Size);
        }

        /// <summary>Movimenta a câmera no sentido específicado.</summary>
        /// <param name="amount">O valor a ser movida a câmera.</param>
        public Camera Move(Vector2 amount)
        {
            Position += amount;
            return this;
        }

        /// <summary>
        /// Movimenta a câmera no sentido específicado.
        /// </summary>
        /// <param name="x">O valor do movimento no eixo X.</param>
        /// <param name="y">O valor do movimento no eixo Y.</param>
        public Camera Move(float x, float y)
        {
            return Move(new Vector2(x, y));
        }

        /// <summary>Define a escala da câmera nos valores X e Y.</summary>
        /// <param name="value">O valor do zoom</param>
        public Camera Zoom(float value)
        {
            Scale = new Vector2(value);
            return this;
        }

        /// <summary>
        /// Foca a câmera em um determinado objeto da tela.
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="bounds">Os limites do objeto.</param>
        public Camera Focus(Game game, Rectangle bounds)
        {
            X = bounds.Center.X - game.Window.ClientBounds.GetHalfW();
            Y = bounds.Center.Y - game.Window.ClientBounds.GetHalfH();

            return this;

            //X = bounds.X;
            //Y = bounds.Y;
        }

        /// <summary>Obtém a Matrix a ser usada no método SpriteBatch.Begin(transformMatrix).</summary>
        public Matrix GetTransform()
        {
            Matrix m = new Matrix();
            m += Matrix.CreateTranslation(-Position.X, -Position.Y, 0) * Matrix.CreateScale(Scale.X, Scale.Y, 0);
            return m;            
        }

        public override bool Equals(object obj)
        {
            return obj is Camera camera && Equals(camera);
        }

        public bool Equals(Camera other)
        {
            return Position.Equals(other.Position);
        }

        public override int GetHashCode()
        {
            return -425505606 + EqualityComparer<Vector2>.Default.GetHashCode(Position);
        }

        public static bool operator ==(Camera left, Camera right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Camera left, Camera right)
        {
            return !(left == right);
        }
    }
}