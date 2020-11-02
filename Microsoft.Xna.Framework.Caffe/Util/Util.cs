// Danilo Borges Santos, 2020.

using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>Classe que expõe funções de auxílio.</summary>
    public static class Util
    {
        /// <summary>Obtém um tamanho multiplicado por uma escala.</summary>
        public static Vector2 GetScaledSize(Point size, Vector2 scale)
        {
            Vector2 sSize = new Vector2(size.X * scale.X, size.Y * scale.Y);
            return sSize;
        } 
        
        ///// <summary>
        ///// Calcula se os limites de um objeto se encontram no espaço de desenho da janela de jogo.
        ///// </summary>
        ///// <param name="game">A instância atual classe Game.</param>
        ///// <param name="camera">O objeto câmera a ser usado para os devidos cálculos.</param>
        ///// <param name="bounds">Os limites do objeto.</param>
        //public static bool CheckFieldOfView(Game game, Camera camera, Rectangle bounds)
        //{
        //    var x = camera.X;
        //    var y = camera.Y;
        //    var w = game.GraphicsDevice.Viewport.Width;
        //    var h = game.GraphicsDevice.Viewport.Height;

        //    if (camera.Zoom != 1)
        //    {
        //        if (camera.Zoom < 1)
        //        {
        //            w = (int)(w * (camera.Zoom * 100));
        //            h = (int)(h * (camera.Zoom * 100));
        //        }                 

        //        if (camera.Zoom > 1)
        //        {
        //            w = (int)(w * (camera.Zoom / 100));
        //            h = (int)(h * (camera.Zoom / 100));
        //        }                     
        //    }                

        //    Viewport visible_view = new Viewport((int)x, (int)y, w, h);

        //    if (visible_view.Bounds.Intersects(bounds))
        //        return true;
        //    else
        //        return false;
        //}

        /// <summary>
        /// Verifica se os limites de um ator se encontram dentro do Viewport.
        /// </summary>
        /// <param name="viewport">O viewport atual.</param>
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
        /// <param name="viewport">O viewport atual.</param>
        /// <param name="camera">A camera ativa.</param>
        /// <param name="actorBounds">Os limites do ator.</param>
        public static bool CheckFieldOfView(Camera camera, Rectangle actorBounds)
        {
            if (camera != null)
            {
                Rectangle total = Rectangle.Empty;
                total.X = (int)camera.ZoomOffset.X - (int)(camera.ZoomOffset.X / camera.Zoom);
                total.Y = (int)camera.ZoomOffset.Y - (int)(camera.ZoomOffset.Y / camera.Zoom);
                total.Width -= (int)camera.ZoomOffset.X - (int)(camera.ZoomOffset.X / camera.Zoom);
                total.Height -= (int)camera.ZoomOffset.Y - (int)(camera.ZoomOffset.Y / camera.Zoom);

                total.X += (int)camera.X;
                total.Y += (int)camera.Y;

                Rectangle cbounds = camera.GetBounds();

                total.Width += cbounds.Width;
                total.Height += cbounds.Height;

                return CheckFieldOfView(new Viewport(total), actorBounds);
            }
            else
                return false;
        }

        ///// <summary>
        ///// Calcula se os limites de um objeto se encontram no espaço de desenho da janela de jogo.
        ///// </summary>
        ///// <param name="screen">A tela a ser verificada.</param>
        ///// <param name="bounds">Os limites do objeto.</param>
        //public static bool CheckFieldOfView(Screen screen, Rectangle bounds)
        //{
        //    return CheckFieldOfView(screen.Game, screen.Camera, bounds);
        //}

        /// <summary>
        /// Calcula o BoundsR (os limites de um retângulo rotacionado) de um objeto.
        /// </summary>
        /// <param name="transform">O objeto Transform a ser usado para cálculo</param>
        /// <param name="totalOrigin">A origem para efeitos de cálculos.</param>
        /// <param name="bounds">Os limites do objeto.</param>
        public static Polygon CreateRotatedBounds<T>(TransformGroup<T> transform, Vector2 totalOrigin, Rectangle bounds) where T : IBoundsable
        {
            Polygon boundsR = new Polygon();
            
            //var r = Rotation.GetRotation(new Rectangle(transform.Scale.ToPoint(), transform.Size), totalOrigin, transform.Rotation);
            var r = Rotation.Get(new Rectangle(transform.Scale.ToPoint(), transform.ScaledSize.ToPoint()), totalOrigin, transform.Rotation);

            boundsR.Points.Clear();
            boundsR.Points.Add(r.P1.ToVector2());
            boundsR.Points.Add(r.P2.ToVector2());
            boundsR.Points.Add(r.P3.ToVector2());
            boundsR.Points.Add(r.P4.ToVector2());

            boundsR.Offset(bounds.Location.ToVector2());
            boundsR.BuildEdges();

            return boundsR;
        }

        /// <summary>Define a posição de um objeto relativo aos limites de um retângulo.</summary>   
        /// <param name="rectangle">O retângulo para alinhamento.</param>
        /// <param name="scaledSize">O tamanho do objeto escalado.</param>
        /// <param name="align">O tipo de alinhamento.</param>
        public static Vector2 AlignObject(Rectangle rectangle, Vector2 scaledSize, AlignType align)
        {
            int w = rectangle.Width;
            int h = rectangle.Height;

            float ew = scaledSize.X;
            float eh = scaledSize.Y;
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

        /// <summary>
        /// Cria um cópia de um objeto quando não for possível utilizar o seu construtor de cópia.
        /// </summary>
        /// <typeparam name="A">O tipo a ser informado.</typeparam>
        /// <param name="obj">O objeto de referência da cópia.</param>
        /// <param name="args">Os argumentos do construtor de cópia que não foi possível ser utilizado.</param>
        public static A Clone<A>(A obj, params object[] args)
        {
            return (A)Activator.CreateInstance(obj.GetType(), args);
        }
    }        
}