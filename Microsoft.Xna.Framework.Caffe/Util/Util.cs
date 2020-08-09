// Danilo Borges Santos, 2020.

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    /// <summary>Classe de auxílio.</summary>
    public static class Util
    {
        /// <summary>Obtém o tamanho de de um objeto Point multiplicado por uma escala.</summary>
        /// <param name="size">O tamanho da entidade.</param>
        /// <param name="scale">A escala da entidade.</param>
        public static Vector2 GetScaledSize(Point size, Vector2 scale)
        {
            Vector2 sSize = new Vector2(size.X * scale.X, size.Y * scale.Y);
            return sSize;
        } 
        
        /// <summary>
        /// Calcula se os limites de uma entidade em uma viewport se encontra no espaço de desenho da janela de jogo.
        /// </summary>
        /// <param name="game">A instância atual classe Game.</param>
        /// <param name="viewport">A viewport em que se encontra a entidade.</param>
        /// <param name="bounds">Os limites da entidade.</param>
        public static bool CheckFieldOfView(Game game, Camera camera, Rectangle bounds)
        {
            var x = camera.X;
            var y = camera.Y;
            var w = game.Window.ClientBounds.Width;
            var h = game.Window.ClientBounds.Height;

            if (camera.Scale != Vector2.One)
            {
                if (camera.Scale.X < 1)
                {
                    w = (int)(w * (camera.Scale.X * 100));
                }                    
                if (camera.Scale.Y < 1)
                {
                    h = (int)(h * (camera.Scale.Y * 100));
                }                    

                if (camera.Scale.X > 1)
                {
                    w = (int)(w * (camera.Scale.X / 100));
                }                    
                if (camera.Scale.Y > 1)
                {
                    h = (int)(h * (camera.Scale.Y / 100));
                }
                    
            }                

            Viewport visible_view = new Viewport((int)x, (int)y, w, h);

            if (visible_view.Bounds.Intersects(bounds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Calcula se os limites de uma entidade se encontra no espaço de desenho da janela de jogo.
        /// </summary>
        /// <param name="screen">A tela a ser verificada.</param>
        /// <param name="bounds">Os limites da entidade.</param>
        public static bool CheckFieldOfView(Screen screen, Rectangle bounds)
        {
            return CheckFieldOfView(screen.Game, screen.Camera, bounds);
        }

        /// <summary>
        /// Calcula o BoundsR no UpdateBounds na classe Entity.
        /// </summary>
        public static void CreateBoundsR(Entity2D e, Vector2 totalOrigin, Rectangle bounds)
        {
            var transform = e.Transform;
            var boundsR = e.BoundsR;
            
            //var r = Rotation.GetRotation(new Rectangle(transform.Scale.ToPoint(), transform.Size), totalOrigin, transform.Rotation);
            var r = Rotation.GetRotation(new Rectangle(transform.Scale.ToPoint(), transform.ScaledSize.ToPoint()), totalOrigin, transform.Rotation);

            boundsR.Points.Clear();
            boundsR.Points.Add(r.P1.ToVector2());
            boundsR.Points.Add(r.P2.ToVector2());
            boundsR.Points.Add(r.P3.ToVector2());
            boundsR.Points.Add(r.P4.ToVector2());

            boundsR.Offset(bounds.Location.ToVector2());
            boundsR.BuildEdges();
        }

        /// <summary>Define a posição do ator relativa a um retângulo.</summary>   
        /// <param name="view">O retângulo para alinhamento.</param>
        /// <param name="actorScaledSize">O tamanho total do ator.</param>
        /// <param name="align">O tipo de alinhamento da tela.</param>
        public static Vector2 AlignActor(Rectangle rectangle, Vector2 actorScaledSize, AlignType align)
        {
            int w = rectangle.Width;
            int h = rectangle.Height;

            float ew = actorScaledSize.X;
            float eh = actorScaledSize.Y;
            Vector2 tempPosition = Vector2.Zero;

            switch (align)
            {
                case AlignType.Center:
                    tempPosition = new Vector2(w / 2 - ew / 2, h / 2 - eh / 2);
                    break;
                case AlignType.Left:
                    tempPosition = new Vector2(0, h / 2 - eh / 2);
                    break;
                case AlignType.Right:
                    tempPosition = new Vector2(w - ew, h / 2 - eh / 2);
                    break;
                case AlignType.Bottom:
                    tempPosition = new Vector2(w / 2 - ew / 2, h - eh);
                    break;
                case AlignType.Top:
                    tempPosition = new Vector2(w / 2 - ew / 2, 0);
                    break;
                case AlignType.LeftBottom:
                    tempPosition = new Vector2(0, h - eh);
                    break;
                case AlignType.LeftTop:
                    tempPosition = new Vector2(0, 0);
                    break;
                case AlignType.RightBottom:
                    tempPosition = new Vector2(w - ew, h - eh);
                    break;
                case AlignType.RightTop:
                    tempPosition = new Vector2(w - ew, 0);
                    break;
            }

            return tempPosition;
        } 
    }        
}