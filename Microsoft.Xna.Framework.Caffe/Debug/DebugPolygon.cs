using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    public class DebugPolygon
    {
        GraphicsDevice graphics;
        BasicEffect basicEffect;
        Color color;
        VertexPositionColor[] vertices = null;

        public BasicEffect Effect { get => basicEffect; }        

        public DebugPolygon(GraphicsDevice graphicsDevice, Polygon poly, Color clr)
        {
            graphics = graphicsDevice;
            color = clr;

            List<VertexPositionColor> vs = new List<VertexPositionColor>();

            poly.Points.ForEach((Vector2 v) => 
            {
                vs.Add(new VertexPositionColor(new Vector3(v, 0), clr));
            }
            );

            vs.Add(vs[0]);
            vertices = vs.ToArray();

            InitializeBasicEffect();
        }

        public void Set(List<Vector2> points)
        {
            List<VertexPositionColor> vs = new List<VertexPositionColor>();

            points.ForEach((Vector2 v) =>
            {
                vs.Add(new VertexPositionColor(new Vector3(v, 0), color));
            }
            );

            vs.Add(vs[0]);
            vertices = vs.ToArray();
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
                graphics.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
            }
        }
    }
}