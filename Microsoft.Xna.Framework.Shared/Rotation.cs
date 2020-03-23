// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Classe de auxílio para cálculos de rotação.
    /// </summary>
    public static class Rotation
    {
        /// <summary>
        /// Obtém a posição de um ponto ao informar a origem e o grau de rotação.
        /// </summary>
        /// <param name="point">A posição do ponto na tela.</param>
        /// <param name="origin">A origem da rotação.</param>
        /// <param name="degreesInRadians">O grau da rotação em radianos.</param>
        /// <returns>A posição rotacionada no eixo X e Y.</returns>
        public static Point GetRotation(Point point, Vector2 origin, double degreesInRadians)
        {
            //http://www.inf.pucrs.br/~pinho/CG/Aulas/Vis2d/Instanciamento/Instanciamento.htm

            //xf = (xo - xr) * cos(@) - (yo - yr) * sin(@) + xr
            //yf = (yo - yr) * cos(@) + (xo - xr) * sin(@) + yr

            //(xo, yo) = Ponto que você deseja rotacionar
            //(xr, yr) = Ponto em que você vai rotacionar o ponto acima(no seu caso o centro do retangulo)
            //(xf, yf) = O novo local do ponto rotacionado
            //@ = Angulo de rotação

            var resultX = (point.X - origin.X) * Math.Cos(degreesInRadians) - (point.Y - origin.Y) * Math.Sin(degreesInRadians) + origin.X;
            var resultY = (point.Y - origin.Y) * Math.Cos(degreesInRadians) + (point.X - origin.X) * Math.Sin(degreesInRadians) + origin.Y;

            return new Point((int)resultX, (int)resultY);
        }

        /// <summary>
        /// Obtém a posição de um retângulo ao informar a origem e o grau de rotação.
        /// </summary>
        /// <param name="rectangle">O retângulo.</param>
        /// <param name="origin">A origem da rotação.</param>
        /// <param name="degreesInRadians">O grau da rotação em radianos.</param>
        /// <returns>Retorna a posição dos cantos do retângulo após a rotação.</returns>
        public static RotatedRectangle GetRotation(Rectangle rectangle, Vector2 origin, double degreesInRadians)
        {
            //Top-Left
            Point p1 = GetRotation(rectangle.Location, origin, degreesInRadians);
            //Top-Right
            Point p2 = GetRotation(new Point(rectangle.Right, rectangle.Top), origin, degreesInRadians);
            //Bottom-Right
            Point p3 = GetRotation(new Point(rectangle.Right, rectangle.Bottom), origin, degreesInRadians);
            //Bottom-Left
            Point p4 = GetRotation(new Point(rectangle.Left, rectangle.Bottom), origin, degreesInRadians);
            //Center
            Point p5 = GetRotation(new Point(rectangle.Width / 2, rectangle.Height / 2), origin, degreesInRadians);

            return new RotatedRectangle(p1, p2, p3, p4);
        }
    }
}
