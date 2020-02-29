// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Descreve um retângulo 2D rotacionado.
    /// </summary>
    public struct RotatedRectangle : IEquatable<RotatedRectangle>
    {
        /// <summary>Obtém o valor da coordenada Top-Left rotacionado.</summary>
        public readonly Point Point1;
        /// <summary>Obtém o valor da coordenada Top-right rotacionado.</summary>
        public readonly Point Point2;
        /// <summary>Obtém o valor da coordenada Bottom-Right rotacionado.</summary>
        public readonly Point Point3;
        /// <summary>Obtém o valor da coordenada Bottom-Left rotacionado.</summary>
        public readonly Point Point4;

        /// <summary>
        /// Inicializa uma nova instância de RotatedRectangle com suas posições rotacionadas.
        /// </summary>
        /// <param name="p1">O valor da coordenada Top-Left.</param>
        /// <param name="p2">O valor da coordenada Top-right.</param>
        /// <param name="p3">O valor da coordenada Bottom-Right.</param>
        /// <param name="p4">O valor da coordenada Bottom-Left.</param>
        public RotatedRectangle(Point p1, Point p2, Point p3, Point p4)
        {
            Point1 = p1;
            Point2 = p2;
            Point3 = p3;
            Point4 = p4;
        }

        public override bool Equals(object obj)
        {
            return obj is RotatedRectangle rectangle && Equals(rectangle);
        }

        public bool Equals(RotatedRectangle other)
        {
            return Point1.Equals(other.Point1) &&
                   Point2.Equals(other.Point2) &&
                   Point3.Equals(other.Point3) &&
                   Point4.Equals(other.Point4);
        }

        public override int GetHashCode()
        {
            var hashCode = 1635792830;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Point1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Point2);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Point3);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Point4);
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