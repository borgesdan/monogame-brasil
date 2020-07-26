using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um polígono com vetores em coordenada 3D.
    /// </summary>
    public class Polygon3D
    {
        GraphicsDevice graphics = null;        
        VertexPositionColor[] vertices = null;
        Polygon _poly = new Polygon();

        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Obtém ou define o objeto Effect para desenho.
        /// </summary>
        public BasicEffect Effect { get; set; } 
        
        /// <summary>
        /// Obtém ou define o polígono a ser desenhado.
        /// </summary>
        public Polygon Poly 
        { 
            get => _poly; 
            set
            {
                _poly = value;

                if(_poly.Points.Count > 0)
                    Set(_poly);
            }
        }

        /// <summary>
        /// Inicializa uma nova instância de DebugPolygon.
        /// </summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="polygon">O polígono de referência</param>
        /// <param name="color">A cor do polígono.</param>
        public Polygon3D(Game game, Polygon polygon, Color color)
        {
            graphics = game.GraphicsDevice;
            Color = color;
            
            Poly = polygon;

            InitializeBasicEffect();
        }

        private void Set(Polygon polygon)
        {
            List<VertexPositionColor> vs = new List<VertexPositionColor>();

            polygon.Points.ForEach((Vector2 v) =>
            {
                vs.Add(new VertexPositionColor(new Vector3(v, 0), Color));
            }
            );

            vs.Add(vs[0]);
            vertices = vs.ToArray();
        }

        private void InitializeBasicEffect()
        {
            Effect = new BasicEffect(graphics);
            Effect.VertexColorEnabled = true;
            Effect.World = Matrix.CreateOrthographicOffCenter(0, graphics.Viewport.Width, graphics.Viewport.Height, 0, 0, 1);
        }

        /// <summary>
        /// Desenha o polígono.
        /// </summary>
        public void Draw()
        {
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);                
            }
        }
    }
}