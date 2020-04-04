// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que representa uma camada de exibição de uma tela.
    /// </summary>
    public class ScreenLayer : IDisposable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        //Representam a máxima posição onde a câmera pode ir
        readonly int max_x_viewport = -2000000000;
        readonly int max_y_viewport = -2000000000;

        private Camera layerCamera = Camera.Create();
        private Vector2 oldPosition = Vector2.Zero;
        private Vector2 position = Vector2.Zero;  
        private int top, left, right, bottom = 0;

        private float infPositionStartX = 0;
        private float infPositionStartY = 0;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém a animação a ser exibida na camada.</summary>
        public Animation Animation { get; private set; } = null;
        /// <summary>Obtém a tela em que essa camada está associada.</summary>
        public Screen Screen { get; }
        /// <summary>Obtém ou define o Viewport de desenho da camada.</summary>
        public Viewport View { get; set; } = new Viewport();
        
        /// <summary>Obtém ou define o valor do efeito parallax. 1f = 100%.</summary>
        public float Parallax { get; set; } = 1f;
        /// <summary>Obtém ou define se a animação será repetida ao infinito no eixo X.</summary>
        public bool InfinityX { get; set; } = false;
        /// <summary>Obtém ou define se a animação será repetida ao infinito no eixo Y.</summary>
        public bool InfinityY { get; set; } = false;

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

        /// <summary>
        /// Inicializa uma nova instância da classe ScreenLayer como cópia de outro ScreenLayer.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public ScreenLayer(ScreenLayer source)
        {
            this.Screen = source.Screen;
            this.Animation = new Animation(source.Animation);
            this.Bottom = source.Bottom;
            this.position = source.position;
            this.layerCamera = source.layerCamera;
            this.Left = source.Left;
            this.oldPosition = source.oldPosition;
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
            //Se houver repetição no eixo X.
            if (InfinityX)
            {
                //Só recebe o valor da posição da câmera no eixo X.
                var pos = layerCamera.Position;
                pos.X = Screen.Camera.Position.X * Parallax;
                layerCamera.Position = pos;

                //Ajuda a view.
                Viewport view = View;
                view.Width = Screen.Game.Window.ClientBounds.Width;
                view.X = 0;                
                View = view;
                
                //Recebe os valores para o cálculo.
                Point v1 = new Point(max_x_viewport, 0);
                Point v2 = layerCamera.Position.ToPoint();
                int aw = Animation.Bounds.Width;

                //Cálculos

                //Distância entre a o eixo X da view e o eixo X da câmera, R = (v1.X - v2.X)
                int r = v1.X - v2.X;
                //Quantidade de animações dentro dessa distância, Q = |R| / aw
                int q = Math.Abs(r) / aw;
                //Posição para início do desenho, Ps = (aw * q) - aw
                int ps = Math.Abs(v1.X) - (aw * q);

                infPositionStartX = -ps;
            }
            if(InfinityY)
            {
                //layerCamera.Position = Screen.Camera.Position * Parallax;
                var pos = layerCamera.Position;
                pos.Y = Screen.Camera.Position.Y * Parallax;
                layerCamera.Position = pos;

                Viewport view = View;                
                view.Height = Screen.Game.Window.ClientBounds.Height;
                view.Y = 0;
                View = view;

                //Point v1 = view.Bounds.Location;
                Point v1 = new Point(0, max_y_viewport);
                Point v2 = layerCamera.Position.ToPoint();
                int ah = Animation.Bounds.Height;

                //Distância entre a o eixo X da view e o eixo X da câmera, R = (v1.Y - v2.Y)
                int r = v1.Y - v2.Y;
                //Quantidade de animações dentro dessa distância, Q = |R| / ah
                int q = Math.Abs(r) / ah;
                //Posição para início do desenho Ps = (aw * q) - aw
                int ps = Math.Abs(v1.Y) - (ah * q);

                infPositionStartY = -ps;
            }
            //Se não, calcula normalmente
            else
            {
                CalcCamera();
            }            

            Animation.Update(gameTime);
        }

        //Calcula a posição da câmera.
        private void CalcCamera()
        {
            //Cálculos para a verificação do limite da viewport.
            //A câmera não pode se mover mais do que o tamanho da camada.

            oldPosition = position;
            position = Screen.Camera.Position;

            var diff = position - oldPosition;
            var c = layerCamera;
            Rectangle cBounds = new Rectangle(0, 0, Screen.Game.Window.ClientBounds.Width, Screen.Game.Window.ClientBounds.Height);

            if (diff.X != 0)
            {
                c.Move(diff.X * Parallax, 0);
                cBounds.X = (int)c.X;

                if (cBounds.Left < Area.Left || cBounds.Right > Area.Right)
                    c.X -= diff.X * Parallax;
            }
            if (diff.Y != 0)
            {
                c.Move(0, diff.Y * Parallax);
                cBounds.Y = (int)c.Y;

                if (cBounds.Top < Area.Top || cBounds.Bottom > Area.Bottom)
                    c.Y -= diff.Y * Parallax;
            }

            layerCamera = c;
        }

        /// <summary>
        /// Adiciona uma animação ao layer.
        /// </summary>
        /// <param name="animation">A animação a ser adicionada.</param>
        public void AddAnimation(Animation animation)
        {
            Animation = animation;
            animation.UpdateBounds();
            SetSize(animation.ScaledSize.ToPoint());
        }

        /// <summary>
        /// Define o tamanho da Viewport da camada.
        /// </summary>
        /// <param name="size">O tamanho da viewport.</param>
        public void SetSize(Point size)
        {
            var v = View;
            v.Width = size.X;
            v.Height = size.Y;

            View = v;
        }

        /// <summary>
        /// Aumenta ou diminui o tamanho da Viewport da camada.
        /// </summary>
        /// <param name="amount">O montante a ser adicionado ou subtraido do tamanho da viewport</param>
        public void FixSize(Point amount)
        {
            var v = View;
            v.Width += amount.X;
            v.Height += amount.Y;

            View = v;
        }

        /// <summary>
        /// Define a posição da Viewport da camada.
        /// </summary>
        /// <param name="position">A posição a ser definida.</param>
        public void SetPosition(Point position)
        {
            var v = View;
            v.X = position.X;
            v.Y = position.Y;

            View = v;
        }

        /// <summary>Desenha a camada.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Screen.Game.GraphicsDevice.Viewport = View;

            spriteBatch.Begin(transformMatrix: layerCamera.GetTransform());
            
            //O desenho caso seja para o infinito.
            if(InfinityX || InfinityY)
            {
                //Recebe os valores para o infinito em X
                int sw = View.Width;
                int aw = Animation.Bounds.Width;
                int rw = sw / aw;
                var posx = infPositionStartX;

                //Em Y.
                int sh = View.Height;
                int ah = Animation.Bounds.Height;
                var posy = infPositionStartY;

                float total = (posy + ah) + sh;
                    
                //Uma correção para não ter animação duplicada.
                if(!InfinityY)
                    total = posy + sh;

                while (posy < total)
                {                    
                    for (int ix = 0; ix <= rw; ix++)
                    {
                        Animation.Position = new Vector2(posx, posy);
                        Animation.Draw(gameTime, spriteBatch);
                        posx += aw;

                        //Uma correção para não ter animação duplicada.
                        if (!InfinityX)
                            ix++;
                    }

                    posx = infPositionStartX;
                    posy += ah;
                }
            }
            else
            {
                Animation.Draw(gameTime, spriteBatch);
            }           

            spriteBatch.End();
        }

        private bool disposed = false;
        
        /// <summary>Libera os recursos dessa instância.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Animation = null;
            }

            disposed = true;
        }
    }
}