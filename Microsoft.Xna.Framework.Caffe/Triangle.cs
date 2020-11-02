using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Estrutura que representa um triângulo.
    /// </summary>
    public struct Triangle
    {
        /// <summary>
        /// As coordenadas A do triângulo.
        /// </summary>
        public Vector2 A;
        /// <summary>
        /// As coordenadas B do triângulo.
        /// </summary>
        public Vector2 B;
        /// <summary>
        /// As coordenadas C do triângulo.
        /// </summary>
        public Vector2 C;

        /// <summary>
        /// Obtém o comprimento de A para B.
        /// </summary>
        public float AB => Math.Abs(Vector2.Distance(A, B));

        /// <summary>
        /// Obtém o comprimento de B para C.
        /// </summary>
        public float BC => Math.Abs(Vector2.Distance(B, C));

        /// <summary>
        /// Obtém o comprimento de A para C.
        /// </summary>
        public float AC => Math.Abs(Vector2.Distance(A, C));

        /// <summary>
        /// Cria uma novo objeto de Triangle.
        /// </summary>
        /// <param name="a">Coordenadas A.</param>
        /// <param name="b">Coordenadas B.</param>
        /// <param name="c">Coordenadas C.</param>
        public Triangle(Point a, Point b, Point c)
        {
            A = a.ToVector2();
            B = b.ToVector2();
            C = c.ToVector2();
        }

        /// <summary>
        /// Cria uma novo objeto de Triangle.
        /// </summary>
        /// <param name="a">Coordenadas A.</param>
        /// <param name="b">Coordenadas B.</param>
        /// <param name="c">Coordenadas C.</param>
        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Obtém a área do triângulo
        /// </summary>
        public float GetArea()
        {
            return FindArea(A, B, C);
        }

        /// <summary>
        /// Obtém a área de um triângulo onde a, b e c são as coordenadas do triângulo.
        /// </summary>
        public static float FindArea(Point a, Point b, Point c)
        {
            return FindArea(a.ToVector2(), b.ToVector2(), c.ToVector2());
        }

        /// <summary>
        /// Obtém a área de um triângulo onde a, b e c são as coordenadas do triângulo.
        /// </summary>
        public static float FindArea(Vector2 a, Vector2 b, Vector2 c)
        {
            float ab = Vector2.Distance(a, b);
            float bc = Vector2.Distance(b, c);
            float ac = Vector2.Distance(c, a);

            //return Triangle.FindArea(Math.Abs(ab), Math.Abs(bc), Math.Abs(ac));
            return FindArea((int)ab, (int)bc, (int)ac);
        }

        /// <summary>
        /// Obtém a área de um triângulo onde a, b e c são os comprimentos dos lados.
        /// </summary>
        public static float FindArea(float a, float b,
                        float c)
        {
            //https://www.geeksforgeeks.org/c-program-find-area-triangle/

            // Length of sides must be positive 
            // and sum of any two sides 
            // must be smaller than third side. 
            if (a < 0 || b < 0 || c < 0)
            {
                //throw new Exception("Os comprimentos dos lados do triângulo devem ser positivo.");
                return 0;
            }

            if (a + b <= c || a + c <= b || b + c <= a)
            {
                //throw new Exception("A soma de dois lados não pode ser menor que o terceiro lado.");
                return 0;
            }

            float s = (a + b + c) / 2.0f;
            return (float)Math.Sqrt(s * (s - a) *
                                (s - b) * (s - c));
        }
    }
}