// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Descreve um retângulo 2D rotacionado.
    /// </summary>
    public struct RotatedRectangle : IEquatable<RotatedRectangle>
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        /// <summary>Obtém o valor da coordenada Top-Left rotacionado.</summary>
        public readonly Point P1;
        /// <summary>Obtém o valor da coordenada Top-right rotacionado.</summary>
        public readonly Point P2;
        /// <summary>Obtém o valor da coordenada Bottom-Right rotacionado.</summary>
        public readonly Point P3;
        /// <summary>Obtém o valor da coordenada Bottom-Left rotacionado.</summary>
        public readonly Point P4;
        /// <summary>Obtém o valor do centro do retângulo rotacionado.</summary>
        public readonly Point Center;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância de RotatedRectangle com suas posições rotacionadas.
        /// </summary>
        /// <param name="p1">O valor da coordenada Top-Left.</param>
        /// <param name="p2">O valor da coordenada Top-right.</param>
        /// <param name="p3">O valor da coordenada Bottom-Right.</param>
        /// <param name="p4">O valor da coordenada Bottom-Left.</param>
        /// <param name="center">O valor do centor do retângulo.</param>
        public RotatedRectangle(Point p1, Point p2, Point p3, Point p4, Point center)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            Center = center;
        }

        /// <summary>
        /// Cria uma nova instância de RotatedRectangle com suas posições rotacionadas.
        /// </summary>
        /// <param name="rectangle">O retângulo.</param>
        /// <param name="origin">A origem da rotação.</param>
        /// <param name="degrees">O grau da rotação em radianos.</param>
        public RotatedRectangle(Rectangle rectangle, Vector2 origin, double degrees) : this(Rotation.GetRotation(rectangle, origin, degrees)) { }        

        /// <summary>
        /// Cria uma nova instância de RotatedRectangle como cópia de outro RotatedRectangle.
        /// </summary>
        /// <param name="source">O retângulo para cópia.</param>
        public RotatedRectangle(RotatedRectangle source)
        {
            P1 = source.P1;
            P2 = source.P2;
            P3 = source.P3;
            P4 = source.P4;
            Center = source.Center;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        public override bool Equals(object obj)
        {
            return obj is RotatedRectangle rectangle && Equals(rectangle);
        }

        public bool Equals(RotatedRectangle other)
        {
            return P1.Equals(other.P1) &&
                   P2.Equals(other.P2) &&
                   P3.Equals(other.P3) &&
                   P4.Equals(other.P4) &&
                   Center.Equals(other.Center);
        }

        public override int GetHashCode()
        {
            var hashCode = 1635792830;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(P1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(P2);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(P3);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(P4);
            return hashCode;
        }

        public static bool operator ==(RotatedRectangle left, RotatedRectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RotatedRectangle left, RotatedRectangle right)
        {
            return !(left == right);
        }
    }
}