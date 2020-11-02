// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Classe de auxílio para cálculos de rotação.
    /// </summary>    
    public static class Rotation
    {
        /// <summary>
        /// Obtém a posição de um ponto rotacionado ao informar a origem e o grau de rotação.        
        /// </summary>
        /// <param name="point">A posição do ponto na tela.</param>
        /// <param name="origin">A origem nas coordenadas da tela.</param>
        /// <param name="degrees">O grau da rotação em radianos.</param>
        public static Point Get(Point point, Vector2 origin, double degrees)
        {
            //http://www.inf.pucrs.br/~pinho/CG/Aulas/Vis2d/Instanciamento/Instanciamento.htm

            //xf = (xo - xr) * cos(@) - (yo - yr) * sin(@) + xr
            //yf = (yo - yr) * cos(@) + (xo - xr) * sin(@) + yr

            //(xo, yo) = Ponto que você deseja rotacionar
            //(xr, yr) = Ponto em que você vai rotacionar o ponto acima(no seu caso o centro do retangulo)
            //(xf, yf) = O novo local do ponto rotacionado
            //@ = Angulo de rotação

            var resultX = (point.X - origin.X) * Math.Cos(degrees) - (point.Y - origin.Y) * Math.Sin(degrees) + origin.X;
            var resultY = (point.Y - origin.Y) * Math.Cos(degrees) + (point.X - origin.X) * Math.Sin(degrees) + origin.Y;

            return new Point((int)resultX, (int)resultY);
        }

        /// <summary>
        /// Obtém a posição de um retângulo rotacionado ao informar a origem e o grau de rotação.
        /// </summary>
        /// <param name="rectangle">O retângulo.</param>
        /// <param name="origin">A origem nas coordenadas da tela.</param>
        /// <param name="degrees">O grau da rotação em radianos.</param>
        public static RotatedRectangle Get(Rectangle rectangle, Vector2 origin, double degrees)
        {
            //Top-Left
            Point p1 = Get(new Point(rectangle.Left, rectangle.Top), origin, degrees);
            //Top-Right
            Point p2 = Get(new Point(rectangle.Right, rectangle.Top), origin, degrees);
            //Bottom-Right
            Point p3 = Get(new Point(rectangle.Right, rectangle.Bottom), origin, degrees);
            //Bottom-Left
            Point p4 = Get(new Point(rectangle.Left, rectangle.Bottom), origin, degrees);
            //Center
            Point p5 = Get(new Point(rectangle.Center.X, rectangle.Center.Y), origin, degrees);

            return new RotatedRectangle(p1, p2, p3, p4, p5);
        }
    }
}
