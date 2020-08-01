//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe de auxílio.</summary>
    public static class Util
    {
        //---------------------------------------//
        //-----         EXTENSÃO            -----//
        //---------------------------------------//

        /// <summary>
        /// Obtém a metade da largura de um retângulo.
        /// </summary>
        public static int GetHalfW(this Rectangle rectangle)
        {
            if (rectangle.Width != 0)
                return rectangle.Width / 2;
            else
                return 0;
        }

        /// <summary>
        /// Obtém a metade da altura de um retângulo.
        /// </summary>
        public static int GetHalfH(this Rectangle rectangle)
        {
            if (rectangle.Height != 0)
                return rectangle.Height / 2;
            else
                return 0;
        }

        /// <summary>
        /// Obtém a metade do tamanho de um retângulo.
        /// </summary>
        public static Point GetHalf(this Rectangle rectangle)
        {
            return new Point(rectangle.GetHalfW(), rectangle.GetHalfH());
        }

        //---------------------------------------//
        //-----         UTILIDADE           -----//
        //---------------------------------------//

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

    //---------------------------------------//
    //-----         DELEGATES           -----//
    //---------------------------------------// 

    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos para o resultado de uma colisão entre entidades.
    /// </summary>
    /// <param name="source">A entidade que implementa este delegate</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="intersection">A área de intersecção entre as duas entidades.</param>
    /// <param name="collidedEntity">A entidade que recebeu a colisão.</param>
    public delegate void CollisionAction(Entity2D source, GameTime gameTime, CollisionResult result, Entity2D collidedEntity);

    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos como resultado de uma colisão entre boxes.
    /// </summary>
    /// <param name="source">A entidade que implementa este delegate</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="boxes">As caixas recorrentes da colisão.</param>
    /// <param name="result">O resultado da colisão entre os boxes.</param>
    /// <param name="collidedEntity">A entidade que recebeu a colisão.</param>
    public delegate void BoxCollisionAction<T1, T2>(Entity2D source, GameTime gameTime, Tuple<T1, T2> boxes, RectangleCollisionResult result, Entity2D collidedEntity) where T1 : struct where T2 : struct;

    /// <summary>
    /// Encapsula um metodo que tem os seguintes parâmetros definidos e que expõe o resultado final de uma ação.
    /// </summary>
    /// <typeparam name="T">O tipo do resultado.</typeparam>
    /// <param name="source">A entidade que implementa este delegate</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="result">O resultado exposto da ação a ser exposto.</param>
    public delegate void ResultAction<in T>(Entity2D source, GameTime gameTime, T result);

    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos para ser uma entidade atualizável.
    /// </summary>
    /// <typeparam name="T">Um tipo que implementa este delegate.</typeparam>
    /// <param name="source">Um tipo que implementa este delegate.</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    public delegate void UpdateAction<in T>(T source, GameTime gameTime);

    /// <summary>
    /// Encapsula um método que tem os seguintes parâmetros definidos para ser uma entidade desenhável.
    /// </summary>
    /// <typeparam name="T">Um tipo que implementa este delegate.</typeparam>
    /// <param name="source">Um tipo que implementa este delegate.</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="spriteBatch">Um objeto SpriteBatch para desenho do jogo.</param>
    public delegate void DrawAction<in T>(T source, GameTime gameTime, SpriteBatch spriteBatch);
}