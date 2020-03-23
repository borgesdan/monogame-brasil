//Essa classe só é usada como auxílio.
//Será apagada no futuro.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe de criação de um retângulo para visualização em modo debug.</summary>
    class DebugRectangle
    {
        GraphicsDevice graphics;
        BasicEffect basicEffect;
        Rectangle rectangle;
        Color color;
        
        public Color Color 
        {
            get => color;
            set
            {
                color = value;
                UpdateColorVertices();
            }
        }
        public VertexPositionColor[] Vertices { get; set; }
        public Rectangle Rectangle 
        {
            get => rectangle;
            set
            {
                var old = rectangle;
                rectangle = value;

                if (rectangle.Equals(old))
                    return;

                var points = CheckRectangle();
                SetVertices(points);
            }
        }
        
        public DebugRectangle(GraphicsDevice graphicsDevice, Rectangle rec, Color clr)
        {
            graphics = graphicsDevice;
            rectangle = rec;
            color = clr;

            List<Vector3> points = CheckRectangle();
            SetVertices(points);
            InitializeBasicEffect();
        }

        List<Vector3> CheckRectangle()
        {            
            Vector3 p1 = new Vector3(Rectangle.X, Rectangle.Y, 0);
            Vector3 p2 = new Vector3(p1.X + Rectangle.Width, p1.Y, 0); 
            Vector3 p3 = new Vector3(p2.X, p1.Y + Rectangle.Height, 0);
            Vector3 p4 = new Vector3(p1.X, p1.Y + Rectangle.Height, 0);
            Vector3 p5 = new Vector3(Rectangle.X, Rectangle.Y, 0);

            List<Vector3> points = new List<Vector3>();
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);
            points.Add(p4);
            points.Add(p5);

            return points;
        }

        void SetVertices(List<Vector3> points)
        {
            Vertices = new[]
            {
                new VertexPositionColor(points[0], Color),
                new VertexPositionColor(points[1], Color),
                new VertexPositionColor(points[2], Color),
                new VertexPositionColor(points[3], Color),
                new VertexPositionColor(points[4], Color)
            };
        }

        void UpdateColorVertices()
        {
            for (int x = 0; x < Vertices.Length; x++)
                Vertices[x].Color = Color;
        }

        void InitializeBasicEffect()
        {
            basicEffect = new BasicEffect(graphics);
            basicEffect.VertexColorEnabled = true;
            basicEffect.World = Matrix.CreateOrthographicOffCenter(0, graphics.Viewport.Width, graphics.Viewport.Height, 0, 0, 1);            
        }

        public void Draw()
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives(PrimitiveType.LineStrip, Vertices, 0, 4);
            }
        }
    }
}