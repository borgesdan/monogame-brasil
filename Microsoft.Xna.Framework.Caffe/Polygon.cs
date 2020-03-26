//Código base disponível em:
//https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection
//Autor: Laurent Cozic

using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Representa um polígono.
    /// </summary>
    public class Polygon
    {
        /// <summary>
        /// Obtém ou define os pontos do polígono.
        /// </summary>
        public List<Vector2> Points { get; set; } = new List<Vector2>();
        /// <summary>
        /// Obtém ou define as bordas do polígono.
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

        /// <summary>
        /// Inicializa uma nova instância de Polygon.
        /// </summary>
        public Polygon()
        {
        }

        /// <summary>
        /// Inicializa uma nova instância de Polygon.
        /// </summary>
        /// <param name="points">Define os pontos do polígono.</param>
        public Polygon(params Vector2[] points)
        {
            foreach(var p in points)
            {
                Points.Add(p);
            }

            BuildEdges();
        }

        /// <summary>
        /// Verifica e Calcula as bordas.
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
    }
}