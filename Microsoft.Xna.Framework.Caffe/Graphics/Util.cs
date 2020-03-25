// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe de auxílio.</summary>
    public static class Util
    {
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
        /// <returns>Retorna true se a entidade se encontra no espaço de desenho da janela de jogo.</returns>
        public static bool CheckFieldOfView(Game game, Camera camera, Viewport viewport, Rectangle bounds)
        {
            var x = camera.GetTransform().Translation.X;
            var y = camera.GetTransform().Translation.Y;
            var w = game.Window.ClientBounds.Width;
            var h = game.Window.ClientBounds.Height;

            Viewport visible_view = new Viewport((int)x, (int)y, w, h);
            Rectangle intersection = Rectangle.Intersect(visible_view.Bounds, viewport.Bounds);

            if (intersection.Intersects(bounds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Calcula se os limites de uma entidade em uma viewport se encontra no espaço de desenho da janela de jogo.
        /// </summary>
        /// <param name="screen">A tela a ser verificada.</param>
        /// <param name="bounds">Os limites da entidade.</param>
        /// <returns>Retorna true se a entidade se encontra no espaço de desenho da janela de jogo.</returns>
        public static bool CheckFieldOfView(Screen screen, Rectangle bounds)
        {
            return CheckFieldOfView(screen.Game, screen.Camera, screen.MainViewport, bounds);
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
    /// Encapsula um metodo que tem os seguintes parâmetros definidos e que expõe o resultado final de uma ação.
    /// </summary>
    /// <typeparam name="T">O tipo do resultado.</typeparam>
    /// <param name="source">A entidade que implementa este delegate</param>
    /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
    /// <param name="result">O resultado exposto da ação a ser exposto.</param>
    public delegate void ResultAction<T>(Entity2D source, GameTime gameTime, T result);

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