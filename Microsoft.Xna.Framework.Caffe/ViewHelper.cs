// Danilo Borges Santos, 2020.

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Classe de auxílio para verificação da tela de jogo.
    /// </summary>
    public static class ViewHelper
    {
        /// <summary>
        /// Verifica se os limites de um ator se encontram dentro do Viewport.
        /// </summary>
        /// <param name="viewport">A tela de visão desejada.</param>
        /// <param name="actorBounds">Os limites do ator.</param>
        public static bool CheckFieldOfView(Viewport viewport, Rectangle actorBounds)
        {
            if (actorBounds.Intersects(viewport.Bounds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifica se os limites de um ator se encontram dentro dos limites da câmera.
        /// </summary>    
        /// <param name="camera">A camera ativa.</param>
        /// <param name="actorBounds">Os limites do ator.</param>
        public static bool CheckFieldOfView(Camera camera, Rectangle actorBounds)
        {
            Rectangle total = Rectangle.Empty;
            total.X = (int)camera.ZoomOffset.X - (int)(camera.ZoomOffset.X / camera.Zoom);
            total.Y = (int)camera.ZoomOffset.Y - (int)(camera.ZoomOffset.Y / camera.Zoom);
            total.Width -= (int)camera.ZoomOffset.X - (int)(camera.ZoomOffset.X / camera.Zoom);
            total.Height -= (int)camera.ZoomOffset.Y - (int)(camera.ZoomOffset.Y / camera.Zoom);

            total.X += (int)camera.X;
            total.Y += (int)camera.Y;

            Rectangle cbounds = camera.Bounds;

            total.Width += cbounds.Width;
            total.Height += cbounds.Height;

            return CheckFieldOfView(new Viewport(total), actorBounds);
        }        

        /// <summary>Obtém a posição de um objeto ao definir o alinhamento relativo aos limites da viewport.</summary>   
        /// <param name="viewport">A tela de visão para cálculo.</param>
        /// <param name="size">O tamanho do ator.</param>
        /// <param name="align">O tipo de alinhamento.</param>
        public static Vector2 GetAlign(Viewport viewport, Vector2 size, AlignType align)
        {
            int viewWidth = viewport.Width;
            int viewHeight = viewport.Height;
            float actorWidth = size.X;
            float actorHeight = size.Y;

            Vector2 tempPosition = Vector2.Zero;

            switch (align)
            {
                case AlignType.Center:
                    tempPosition = new Vector2(viewWidth / 2 - actorWidth / 2, viewHeight / 2 - actorHeight / 2);
                    break;
                case AlignType.Left:
                    tempPosition = new Vector2(0, viewHeight / 2 - actorHeight / 2);
                    break;
                case AlignType.Right:
                    tempPosition = new Vector2(viewWidth - actorWidth, viewHeight / 2 - actorHeight / 2);
                    break;
                case AlignType.Bottom:
                    tempPosition = new Vector2(viewWidth / 2 - actorWidth / 2, viewHeight - actorHeight);
                    break;
                case AlignType.Top:
                    tempPosition = new Vector2(viewWidth / 2 - actorWidth / 2, 0);
                    break;
                case AlignType.LeftBottom:
                    tempPosition = new Vector2(0, viewHeight - actorHeight);
                    break;
                case AlignType.LeftTop:
                    tempPosition = new Vector2(0, 0);
                    break;
                case AlignType.RightBottom:
                    tempPosition = new Vector2(viewWidth - actorWidth, viewHeight - actorHeight);
                    break;
                case AlignType.RightTop:
                    tempPosition = new Vector2(viewWidth - actorWidth, 0);
                    break;
            }

            return tempPosition;
        }        
    }
}