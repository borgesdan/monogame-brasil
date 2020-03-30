using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada de exibição de uma tela.
    /// </summary>
    public class ScreenLayer
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        private Camera layerCamera = Camera.Create();
        private Camera oldCamera = Camera.Create();
        private Camera camera = Camera.Create();    
        private int top, left, right, bottom = 0;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define as animações a serem exibidas na camada.</summary>
        public List<Animation> Actors { get; set; } = new List<Animation>();
        /// <summary>Obtém a tela em que essa camada está associada.</summary>
        public Screen Screen { get; }
        /// <summary>Obtém ou define o Viewport de desenho da camada.</summary>
        public Viewport View { get; set; } = new Viewport(); 

        /// <summary>Obtém ou define o limite de rolagem da tela para cima.</summary>
        public int Top { get => top; set => top = MathHelper.Clamp(value, 0, int.MaxValue); }
        /// <summary>Obtém ou define o limite de rolagem da tela para baixo.</summary>
        public int Bottom { get => bottom; set => bottom = MathHelper.Clamp(value, 0, int.MaxValue); }
        /// <summary>Obtém ou define o limite de rolagem da tela para direita.</summary>
        public int Right { get => right; set => right = MathHelper.Clamp(value, 0, int.MaxValue); }
        /// <summary>Obtém ou define o limite de rolagem da tela para esquerda.</summary>
        public int Left { get => left; set => left = MathHelper.Clamp(value, 0, int.MaxValue); }
        
        /// <summary>Obtém a área total da camada.</summary>
        public Rectangle Area
        {
            get => new Rectangle(View.X - Left, View.Y - Top, View.Width + Right, View.Height + Bottom);
        }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe ScreenLayer.
        /// </summary>
        /// <param name="screen">A tela em que a camada será associada.</param>
        public ScreenLayer(Screen screen)
        {            
            Screen = screen;
            View = screen.Game.GraphicsDevice.Viewport;
        }

        public ScreenLayer(ScreenLayer source)
        {
            this.Screen = source.Screen;

            foreach (var a in source.Actors)
            {
                this.Actors.Add(new Animation(Screen.Game, a));
            }

            this.Bottom = source.Bottom;
            this.camera = source.camera;
            this.layerCamera = source.layerCamera;
            this.Left = source.Left;
            this.oldCamera = source.oldCamera;
            this.Right = source.Right;
            this.Top = source.Top;
            this.View = source.View;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Atualiza a tela.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public void Update(GameTime gameTime)
        {
            oldCamera = camera;
            camera = Screen.Camera;

            var diff = camera.Position - oldCamera.Position;
            var c = layerCamera;

            if(diff.X < 0)
            {
                c.Move(diff.X, 0);
                Rectangle cBounds = new Rectangle(c.Position.ToPoint(), Screen.Game.Window.ClientBounds.Size);

                if (cBounds.Left < Area.Left)
                    c.X = Area.X;
            }
            else if(diff.X > 0)
            {
                c.Move(diff.X, 0);
                Rectangle cBounds = new Rectangle(c.Position.ToPoint(), Screen.Game.Window.ClientBounds.Size);

                if (cBounds.Right > Area.Right)
                {
                    var diffR = cBounds.Right - Area.Right;
                    c.X -= diffR;
                }   
            }

            if(diff.Y < 0)
            {
                c.Move(0, diff.Y);
                Rectangle cBounds = new Rectangle(c.Position.ToPoint(), Screen.Game.Window.ClientBounds.Size);

                if (cBounds.Top < Area.Top)
                    c.Y = Area.Y;
            }
            else if(diff.Y > 0)
            {
                c.Move(0, diff.Y);
                Rectangle cBounds = new Rectangle(c.Position.ToPoint(), Screen.Game.Window.ClientBounds.Size);

                if (cBounds.Bottom > Area.Bottom)
                {
                    var diffB = cBounds.Bottom - Area.Bottom;
                    c.X -= diffB;
                }
            }

            layerCamera = c;

            Actors.ForEach(a => a.Update(gameTime));
        }

        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Screen.Game.GraphicsDevice.Viewport = View;

            spriteBatch.Begin(transformMatrix: layerCamera.GetTransform());

            Actors.ForEach(a => a.Draw(gameTime, spriteBatch));

            spriteBatch.End();
        }
    }
}