//Código base disponível em:
//https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection
//Autor: Laurent Cozic

using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    public class Polygon
    {
        public List<Vector2> Points { get; set; } = new List<Vector2>();
        public List<Vector2> Edges { get; set; } = new List<Vector2>();

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

        public Polygon()
        {
        }

        public Polygon(params Vector2[] points)
        {
            foreach(var p in points)
            {
                Points.Add(p);
            }

            BuildEdges();
        }

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

        public void Offset(Vector2 vector)
        {
            Offset(vector.X, vector.Y);            
        }

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