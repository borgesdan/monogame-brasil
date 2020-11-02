// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Descreve um retângulo rotacionado.
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
        /// Cria uma novo objeto de RotatedRectangle.
        /// </summary>
        /// <param name="p1">O valor da coordenada Top-Left.</param>
        /// <param name="p2">O valor da coordenada Top-right.</param>
        /// <param name="p3">O valor da coordenada Bottom-Right.</param>
        /// <param name="p4">O valor da coordenada Bottom-Left.</param>
        /// <param name="center">O valor do centro do retângulo.</param>
        public RotatedRectangle(Point p1, Point p2, Point p3, Point p4, Point center)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            Center = center;
        }

        /// <summary>
        /// Cria um novo objeto de RotatedRectangle informando os argumentos para uma rotação de um Rectangle.
        /// </summary>
        /// <param name="rectangle">O retângulo a ser rotacionado.</param>
        /// <param name="origin">A origem nas coordenadas da tela.</param>
        /// <param name="degrees">O grau da rotação em radianos.</param>
        public RotatedRectangle(Rectangle rectangle, Vector2 origin, double degrees)
        {
            RotatedRectangle r = Rotation.Get(rectangle, origin, degrees);

            P1 = r.P1;
            P2 = r.P2;
            P3 = r.P3;
            P4 = r.P4;
            Center = r.Center;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//
        
        //public bool Intersects(RotatedRectangle rotatedRectangle)
        //{
        //    float area = Triangle.FindArea(rotatedRectangle.P1, rotatedRectangle.Center, rotatedRectangle.P2)
        //        + Triangle.FindArea(rotatedRectangle.P2, rotatedRectangle.Center, rotatedRectangle.P3)
        //        + Triangle.FindArea(rotatedRectangle.P3, rotatedRectangle.Center, rotatedRectangle.P4)
        //        + Triangle.FindArea(rotatedRectangle.P4, rotatedRectangle.Center, rotatedRectangle.P1);

        //    //float area = Triangle.FindArea(P1, P2, P3) + Triangle.FindArea(P3, P4, P1);

        //    float p1a = SumArea(P1, rotatedRectangle);
        //    float p2a = SumArea(P2, rotatedRectangle);
        //    float p3a = SumArea(P3, rotatedRectangle);
        //    float p4a = SumArea(P4, rotatedRectangle);

        //    //float p1a = SumArea(rotatedRectangle.P1);
        //    //float p2a = SumArea(rotatedRectangle.P2);
        //    //float p3a = SumArea(rotatedRectangle.P3);
        //    //float p4a = SumArea(rotatedRectangle.P4);

        //    if (p1a == area || p2a == area || p3a == area || p4a == area)
        //        return true;
        //    else
        //        return false;            
        //}

        //private float SumArea(Point p, RotatedRectangle rectangle)
        //{
        //    //float a = Triangle.FindArea(P1, p, P4);
        //    //float b = Triangle.FindArea(P4, p, P3);
        //    //float c = Triangle.FindArea(P3, p, P2);
        //    //float d = Triangle.FindArea(p, P2, P1);

        //    float a = Triangle.FindArea(rectangle.P1, p, rectangle.P4);
        //    float b = Triangle.FindArea(rectangle.P4, p, rectangle.P3);
        //    float c = Triangle.FindArea(rectangle.P3, p, rectangle.P2);
        //    float d = Triangle.FindArea(p, rectangle.P2, rectangle.P1);

        //    return a + b + c + d;
        //}

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
            return HashCode.Combine(P1, P2, P3, P4, Center);
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