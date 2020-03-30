// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

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

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
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

        private Camera(Vector2 position, Vector2 zoom)
        {            
            Position = position;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//
        /// <summary>Cria uma nova instância da estrutura Camera.</summary>
        public static Camera Create()
        {
            return new Camera(Vector2.Zero, Vector2.One);
        }        

        /// <summary>Movimenta a câmera no sentido específicado.</summary>
        /// <param name="amount">O valor a ser movida a câmera.</param>
        public void Move(Vector2 amount)
        {
            Position += amount;
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

        /// <summary>Obtém a Matrix a ser usada no método SpriteBatch.Begin(transformMatrix).</summary>
        public Matrix GetTransform()
        {
            Matrix m = new Matrix();
            m += Matrix.CreateTranslation(Position.X, Position.Y, 0);
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