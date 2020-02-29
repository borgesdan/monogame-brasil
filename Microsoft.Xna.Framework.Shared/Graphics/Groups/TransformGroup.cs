// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe que expõe acesso as definições de valores da entidade, como posição, velocidade, rotação, entre outros.</summary>
    public sealed class TransformGroup
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        Vector2 _oldPosition = Vector2.Zero;
        Vector2 _position = Vector2.Zero;
        Vector2 _scale = Vector2.One;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém a entidade a qual este grupo está atrelado.</summary>
        public Entity2D Entity { get; set; } = null;

        /// <summary>Obtém a posição anterior da atualização da posição atual.</summary>
        public Vector2 OldPosition { get => _oldPosition; }

        /// <summary>Obtém ou define a posição atual.</summary>
        public Vector2 Position 
        {
            get => _position;
            set
            {
                _oldPosition = _position;
                _position = value;

                Entity.UpdateBounds();
            }
        }
        /// <summary>Obtém ou define a velocidade.</summary>
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        /// <summary>Obtém o tamanho da entidade.</summary>
        public Point Size { get; internal set; } = Point.Zero;
        /// <summary>Obtém ou define a rotação. Valor padrão 0.</summary>
        public float Rotation { get; set; } = 0;
        /// <summary>Obtém ou define a escala. Valor padrão Vector.Zero.</summary>
        public Vector2 Scale 
        { 
            get => _scale;
            set
            {
                _scale = value;

                Entity.UpdateBounds();
            }
        }
        /// <summary>Obtém o valor da escala * tamanho.</summary>
        public Vector2 ScaledSize
        {
            get
            {
                Vector2 sSize = new Vector2(Size.X * Scale.X, Size.Y * Scale.Y);
                return sSize;
            }
        }
        /// <summary>Obtém ou define a cor.</summary>
        public Color Color { get; set; } = Color.White;
        /// <summary>Obtém ou define as configurações de SpriteEffect.</summary>
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        /// <summary>Obtém ou define o posição no eixo X.</summary>
        public float X 
        { 
            get => Position.X;  
            set 
            {
                _oldPosition.X = _position.X;
                _position.X = value;

                Entity.UpdateBounds();
            }
        }
        /// <summary>Obtém ou define a posição no eixo Y.</summary>
        public float Y
        {
            get => Position.Y;
            set
            {
                _oldPosition.Y = _position.Y;
                _position.Y = value;

                Entity.UpdateBounds();
            }
        }
        /// <summary>Obtém ou define a velocidade no eixo X.</summary>
        public float Xv { get { return Velocity.X; } set { Velocity = new Vector2(value, Velocity.Y); } }
        /// <summary>Obtém ou define a velocidade no eixo Y.</summary>
        public float Yv { get { return Velocity.Y; } set { Velocity = new Vector2(Velocity.X, value); } }
        /// <summary>Obtém a largura da entidade.</summary>
        public int Width { get { return Size.X; } }
        /// <summary>Obtém a altura da entidade.</summary>
        public int Height { get { return Size.Y; } }
        /// <summary>Obtém a direção da entidade.</summary>
        public Vector2 Direction 
        { 
            get
            {
                Vector2 diference = Position - OldPosition;

                if (diference != Vector2.Zero)
                    diference.Normalize();

                return diference;
            }
        }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de TransformGroup.</summary>
        /// <param name="entity">A entidade a ser atrelada este grupo.</param>
        public TransformGroup(Entity2D entity)
        {
            Entity = entity ?? throw new System.ArgumentNullException(nameof(entity));
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>Interter a velocidade atual.</summary>
        public Vector2 InvertVelocity()
        {
            InvertVelocityX();
            InvertVelocityY();
            return Velocity;
        }
        /// <summary>Inverter a velocidade no eixo X.</summary>
        /// <returns>Retorna a velocidade com o valor invertido.</returns>
        public Vector2 InvertVelocityX() 
        {
            Xv *= -1;
            return Velocity;
        }
        /// <summary>Inverter a velocidade no eixo Y.</summary>
        /// <returns>Retorna a velocidade com o valor invertido.</returns>
        public Vector2 InvertVelocityY() 
        { 
            Yv *= -1;
            return Velocity;
        }

        /// <summary>Definir a posição</summary>
        /// <param name="position">A posição no eixo X e Y.</param>
        public void SetPosition(Vector2 position) => Position = position;
        /// <summary>Definir a posição</summary>
        /// <param name="x">A posição no eixo X.</param>
        /// <param name="y">A posição no eixo Y.</param>
        public void SetPosition(float x, float y) => Position = new Vector2(x, y);

        /// <summary>
        /// Incrementa a posição da entidade.
        /// </summary>
        /// <param name="amount">A quantidade de posições a ser incrementada.</param>
        public void IncreasePosition(Vector2 amount) => Position += amount;

        /// <summary>Definir a velocidade</summary>
        /// <param name="velocity">A velocidade no eixo X e Y.</param>
        public void SetVelocity(Vector2 velocity) => Velocity = velocity;
        /// <summary>Definir a velocidade.</summary>
        /// <param name="x">A velocidade no eixo X.</param>
        /// <param name="y">A velocidade no eixo Y.</param>
        public void SetVelocity(float x, float y) => Velocity = new Vector2(x, y);

        /// <summary>Definir a escala.</summary>
        /// <param name="x">A escala no eixo X. [Padrão x = 1; Duplicar x = 2;]</param>
        /// <param name="y">A escala no eixo Y. [Padrão y = 1] Duplicar y = 2;]</param>
        public void SetScale(float x, float y) => Scale = new Vector2(x, y);

        /// <summary>Definir a escala</summary>
        /// <param name="velocity">A escala no eixo X e Y. [Padrão x e y = 1; Duplicar x e y = 2]</param>
        public void SetScale(Vector2 scale) => Scale = scale;

        /// <summary>Definir a posição da entidade relativa a Viewport.</summary>   
        /// <param name="alignType">O tipo de alinhamento da tela.</param>
        public void SetScreenPosition(AlignType alignType)
        {
            Entity.UpdateBounds();

            var view = Entity.Game.GraphicsDevice.Viewport;
            int w = view.Width;
            int h = view.Height;
            int ew = Width;
            int eh = Height;            
            float bx = Entity.Bounds.X;
            float by = Entity.Bounds.Y;
            Vector2 tempPosition = Vector2.Zero;

            switch (alignType)
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

            tempPosition.X += bx;
            tempPosition.Y -= by;

            _oldPosition = tempPosition;
            _position = tempPosition;

            Entity.UpdateBounds();
        }        
    }

    /// <summary>Enumeração que representa o tipo de alinhamento na tela da sprite.</summary>
    public enum AlignType : byte
    {
        Center = 1,
        Left,
        Right,
        Top,
        Bottom,
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom
    }
}
