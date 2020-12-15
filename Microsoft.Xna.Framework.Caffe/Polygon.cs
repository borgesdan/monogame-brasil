// Danilo Borges Santos, 2020.

// Polygon class by
//
// Copyright (c) 2006 Laurent Cozic
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// Code: https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection

using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Representa um polígono.
    /// </summary>
    public class Polygon
    {
        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//        

        /// <summary>
        /// Obtém ou define os pontos do polígono.
        /// </summary>
        public List<Vector2> Points { get; set; } = new List<Vector2>();
        /// <summary>
        /// Obtém as bordas do polígono.
        /// </summary>
        public List<Vector2> Edges { get; private set; } = new List<Vector2>();

        /// <summary>
        /// Obtém o centro do polígono.
        /// </summary>
        public Vector2 Center 
        {
            get 
            {
                float totalX = 0;
                float totalY = 0;
                for (int i = 0; i < Points.Count; i++)
                {
                    totalX += Points[i].X;
                    totalY += Points[i].Y;
                }

                return new Vector2(totalX / Points.Count, totalY / Points.Count);
            }
        }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de Polygon.
        /// </summary>
        public Polygon() { }

        /// <summary>
        /// Inicializa uma nova instância de Polygon como cópia de outro Polygon.
        /// </summary>
        public Polygon(Polygon source)
        {
            source.Points.ForEach(p => Points.Add(p));
            BuildEdges();
        }

        /// <summary>
        /// Inicializa uma nova instância de Polygon.
        /// </summary>
        /// <param name="rectangle">Define os pontos do polígono através de um retângulo.</param>
        public Polygon(Rectangle rectangle)
        {
            Set(rectangle);
        }

        /// <summary>
        /// Inicializa uma nova instância de Polygon.
        /// </summary>
        /// <param name="points">Define os pontos do polígono.</param>
        public Polygon(params Vector2[] points)
        {
            Set(points);
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Verifica e calcula as bordas.
        /// </summary>
        public void BuildEdges()
        {
            Vector2 p1;
            Vector2 p2;
            Edges.Clear();
            for (int i = 0; i < Points.Count; i++)
            {
                p1 = Points[i];
                if (i + 1 >= Points.Count)
                {
                    p2 = Points[0];
                }
                else
                {
                    p2 = Points[i + 1];
                }
                Edges.Add(p2 - p1);
            }            
        }

        /// <summary>
        /// Aplica o deslocamento das posições dos pontos do polígono.
        /// </summary>
        /// <param name="point">O valor no eixo X e Y.</param>
        public void Offset(Point point)
        {
            Offset(point.X, point.Y);
        }

        /// <summary>
        /// Aplica o deslocamento das posições dos pontos do polígono.
        /// </summary>
        /// <param name="vector">O valor no eixo X e Y.</param>
        public void Offset(Vector2 vector)
        {
            Offset(vector.X, vector.Y);
        }

        /// <summary>
        /// Aplica o deslocamento das posições dos pontos do polígono.
        /// </summary>
        /// <param name="x">O valor no eixo X.</param>
        /// <param name="y">O valor no eixo Y.</param>
        public void Offset(float x, float y)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Vector2 p = Points[i];
                Points[i] = new Vector2(p.X + x, p.Y + y);
            }
        }

        /// <summary>
        /// Define os pontos do polígono.
        /// </summary>
        /// <param name="rectangle">Define os pontos através de um retângulo.</param>
        public void Set(Rectangle rectangle)
        {
            Points.Clear();

            Points.Add(new Vector2(rectangle.Left, rectangle.Top));
            Points.Add(new Vector2(rectangle.Right, rectangle.Top));
            Points.Add(new Vector2(rectangle.Right, rectangle.Bottom));
            Points.Add(new Vector2(rectangle.Left, rectangle.Bottom));

            BuildEdges();
        }

        /// <summary>
        /// Define os pontos do polígono.
        /// </summary>
        /// <param name="points">Define os pontos através de uma lista de pontos.</param>
        public void Set(params Vector2[] points)
        {
            Points.Clear();

            foreach (var p in points)
            {
                Points.Add(p);
            }

            BuildEdges();
        }

        /// <summary>
        /// Define os pontos do polígono.
        /// </summary>
        /// <param name="points">Define os pontos através de uma lista de pontos.</param>
        public void Set(params Point[] points)
        {
            Points.Clear();

            foreach (var p in points)
            {
                Points.Add(p.ToVector2());
            }

            BuildEdges();
        }

        /// <summary>
        /// Define os pontos do polígono.
        /// </summary>
        /// <param name="polygon">Define os pontos atráves de uma cópia de um polígono.</param>
        public void Set(Polygon polygon)
        {
            Points.Clear();
            polygon.Points.ForEach(p => Points.Add(p));
            BuildEdges();
        }

        /// <summary>
        /// Define os pontos do polígono.
        /// </summary>
        /// <param name="rectangle">Define os pontos através de um retângulo rotacionado.</param>
        public void Set(RotatedRectangle rotatedRectangle)
        {            
            Set(rotatedRectangle.P1, rotatedRectangle.P2, rotatedRectangle.P3, rotatedRectangle.P4);
        }
    }
}