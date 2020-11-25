// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Expõe acesso as transformações de um objeto, como posição, velocidade, rotação, entre outros.
    /// </summary>
    public sealed class TransformGroup
    {
        //Essa estrutura recebe os valores calculados das origens disponíveis em cada canto do limite do ator
        struct OriginValues
        {
            public Vector2 LeftTop;
            public Vector2 Left;
            public Vector2 LeftBottom;

            public Vector2 RightTop;
            public Vector2 Right;
            public Vector2 RightBottom;

            public Vector2 Top;
            public Vector2 Center;
            public Vector2 Bottom;

            public OriginValues(TransformGroup transform)
            {
                LeftTop = Vector2.Zero;
                Left = new Vector2(0, transform.Height / 2);
                LeftBottom = new Vector2(0, transform.Height);

                RightTop = new Vector2(transform.Width, 0);
                Right = new Vector2(transform.Width, transform.Height / 2);
                RightBottom = new Vector2(transform.Width, transform.Height);

                Top = new Vector2(transform.Width / 2, 0);
                Center = new Vector2(transform.Width / 2, transform.Height / 2);
                Bottom = new Vector2(transform.Width / 2, transform.Height);
            }
        }

        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        Vector2 _oldPosition = Vector2.Zero;
        Vector2 _position = Vector2.Zero;        

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//        

        /// <summary>Obtém a posição anterior a atualização da posição atual.</summary>
        public Vector2 OldPosition { get => _oldPosition; }

        /// <summary>Obtém ou define a posição atual.</summary>
        public Vector2 Position
        {
            get => _position;
            set
            {
                _oldPosition = _position;
                _position = value;
            }
        }
        /// <summary>Obtém ou define a velocidade.</summary>
        public Vector2 Velocity { get; set; }
        /// <summary>Obtém ou define o valor de resitência à velocidade definida.</summary>
        public Vector2 VResistance { get; set; }
        /// <summary>Obtém o tamanho da entidade.</summary>
        public Point Size { get; internal set; } = Point.Zero;
        /// <summary>Obtém ou define a rotação. Valor padrão 0.</summary>
        public float Rotation { get; set; } = 0;
        /// <summary>Obtém ou define a escala. Valor padrão Vector.One.</summary>
        public Vector2 Scale { get; set; } = Vector2.One;
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
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        /// <summary>Obtém ou define o posição no eixo X.</summary>
        public float X
        {
            get => Position.X;
            set
            {
                _oldPosition.X = _position.X;
                _position.X = value;
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
            }
        }
        /// <summary>Obtém ou define a escala em X.</summary>
        public float Xs { get => Scale.X; set => Scale = new Vector2(value, Ys); }
        /// <summary>Obtém ou define a escala em Y.</summary>
        public float Ys { get => Scale.Y; set => Scale = new Vector2(Xs, value); }
        /// <summary>Obtém ou define a velocidade no eixo X.</summary>
        public float Xv { get { return Velocity.X; } set { Velocity = new Vector2(value, Yv); } }
        /// <summary>Obtém ou define a velocidade no eixo Y.</summary>
        public float Yv { get { return Velocity.Y; } set { Velocity = new Vector2(Xv, value); } }
        /// <summary>Obtém a largura da entidade.</summary>
        public int Width { get { return Size.X; } }
        /// <summary>Obtém a altura da entidade.</summary>
        public int Height { get { return Size.Y; } }
        /// <summary>Obtém a largura escalada da entidade.</summary>
        public float ScaledWidth { get { return ScaledSize.X; } }
        /// <summary>Obtém a altura escalada da entidade.</summary>
        public float ScaledHeight { get { return ScaledSize.Y; } }
        /// <summary>Obtém ou define a origem para desenho de cada sprite.</summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;
        /// <summary>Obtém ou define a origem no eixo X.</summary>
        public float Xo { get => Origin.X; set => Origin = new Vector2(value, Yo); }
        /// <summary>Obtém ou define a origem no eixo Y.</summary>
        public float Yo { get => Origin.Y; set => Origin = new Vector2(Xo, value); }
        /// <summary>Obtém ou define a profundidade de desenho (se necessário) do objeto.</summary>
        public float LayerDepth { get; set; } = 0;        

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de TransformGroup.</summary>        
        public TransformGroup() { }

        /// <summary>
        /// Inicializa uma nova instância como cópia de outra instância de TransformGroup.
        /// </summary>
        /// <param name="source">O TransformGroup a ser copiado.</param>
        public TransformGroup(TransformGroup source)
        {
            Set(source);
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        public override string ToString()
        {
            return string.Concat("Pos: ", Position.ToString(), " Sca: ", Scale.ToString(), " Rot: ", Rotation.ToString(), " Ori: ", Origin, " Eff: ", SpriteEffects.ToString());
        }

        /// <summary>
        /// Obtém uma matriz com os valores das propriedades.
        /// </summary>
        public Matrix GetMatrix()
        {
            //return Matrix.CreateTranslation(new Vector3(-Origin.X, -Origin.Y, 0))
            //    * Matrix.CreateRotationZ(Rotation)
            //    * Matrix.CreateScale(new Vector3(Scale, 1))                 
            //    * Matrix.CreateTranslation(new Vector3(Position, 0));

            return Matrix.CreateTranslation(new Vector3(-Origin.X, -Origin.Y, 0))
                * Matrix.CreateScale(new Vector3(Scale, 1))
                * Matrix.CreateRotationZ(Rotation)                
                * Matrix.CreateTranslation(new Vector3(Position, 0));
        }

        /// <summary>
        /// Define as propriedades deste grupo como cópia de outro TransformGroup.
        /// </summary>        
        /// <param name="source">A instância para cópia das propriedades.</param>
        public void Set(TransformGroup source)
        {
            this.Size = source.Size;
            this.Color = source.Color;
            this.Rotation = source.Rotation;
            this.Scale = source.Scale;
            this.SpriteEffects = source.SpriteEffects;
            this.Velocity = source.Velocity;
            this.Position = source.Position;
            this.Origin = source.Origin;
            this.LayerDepth = source.LayerDepth;
        }

        /// <summary>
        /// Atualiza os cálculos de velocidade do TransformGroup.
        /// </summary>
        public void Update()
        {
            Velocity += VResistance;

            if (Xv != 0)
                X += Velocity.X;
            if (Yv != 0)
                Y += Velocity.Y;
        }

        //Velocidade

        /// <summary>Inverte a velocidade nos eixos X e Y.</summary>
        public Vector2 InvertVelocity()
        {
            InvertVelocityX();
            InvertVelocityY();

            return Velocity;
        }
        /// <summary>Inverte a velocidade no eixo X.</summary>
        public Vector2 InvertVelocityX()
        {
            Xv *= -1;
            return Velocity;
        }
        /// <summary>Inverte a velocidade no eixo Y.</summary>
        public Vector2 InvertVelocityY()
        {
            Yv *= -1;
            return Velocity;
        }

        /// <summary>Define a velocidade.</summary>
        /// <param name="velocity">A velocidade no eixo X e Y.</param>
        public void SetVelocity(float velocity)
        {
            SetVelocity(new Vector2(velocity));
        }

        /// <summary>Define a velocidade.</summary>
        /// <param name="x">A velocidade no eixo X.</param>
        /// <param name="y">A velocidade no eixo Y.</param>
        public void SetVelocity(float x, float y)
        {
            SetVelocity(new Vector2(x, y));
        }

        /// <summary>Define a velocidade.</summary>
        /// <param name="velocity">A velocidade no eixo X e Y.</param>
        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        /// <summary>
        /// Define a resistência à velocidade.
        /// </summary>
        /// <param name="x">Resistência no eixo X.</param>
        /// <param name="y">Resistência no eixo Y.</param>
        public void SetVResistance(float x, float y)
        {
            SetVResistance(new Vector2(x, y));
        }

        /// <summary>
        /// Define a resistência à velocidade.
        /// </summary>
        /// <param name="resistance">O vetor com os valores da resistência.</param>
        public void SetVResistance(Vector2 resistance)
        {
            VResistance = resistance;
        }

        /// <summary>
        /// Define a resistência à velocidade.
        /// </summary>
        /// <param name="resistance">O vetor com os valores da resistência.</param>
        /// <param name="velocity">O valor da velocidade caso precise redefiní-la.</param>
        public void SetVResistance(Vector2 resistance, Vector2 velocity)
        {
            VResistance = resistance;
            SetVelocity(velocity);
        }        

        /// <summary>
        /// Define a velocidade informando uma direção e a força da velocidade.
        /// </summary>
        /// <param name="direction">A direção desejada (pode ser acumulada com o operador |)</param>
        /// <param name="force">A força aplicada à velocidade (valor padrão 1)</param>
        public void SetVelocityDirection(Direction2D direction, float force)
        {
            force = Math.Abs(force);

            if (direction == (Direction2D.Up | Direction2D.Right))
                SetVelocity(1.5f * force, -0.75f * force);
            else if (direction == (Direction2D.Up | Direction2D.Left))
                SetVelocity(-1.5f * force, -0.75f * force);
            else if (direction == (Direction2D.Down | Direction2D.Right))
                SetVelocity(1.5f * force, 0.75f * force);
            else if (direction == (Direction2D.Down | Direction2D.Left))
                SetVelocity(-1.5f * force, 0.75f * force);
            else if (direction == Direction2D.Up)
                SetVelocity(0f, -1f * force);
            else if(direction == Direction2D.Down)
                SetVelocity(0f, 1f * force);
            else if (direction == Direction2D.Left)
                SetVelocity(-1f * force, 0);
            else if (direction == Direction2D.Right)
                SetVelocity(1f * force, 0);            
        }

        //Posição

        /// <summary>Define a posição.</summary>
        /// <param name="position">A posição no eixo X e Y.</param>
        public void SetPosition(Point position)
        {
            SetPosition(position.X, position.Y);
        }

        /// <summary>Define a posição.</summary>
        /// <param name="position">A posição no eixo X e Y.</param>
        public void SetPosition(Vector2 position) => SetPosition(position.X, position.Y);

        /// <summary>Define a posição e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="x">A posição no eixo X.</param>
        /// <param name="y">A posição no eixo Y.</param>
        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Define a posição do objeto relativa a um Viewport.</summary>   
        /// <param name="align">O tipo de alinhamento da tela.</param>
        public void SetPosition(AlignType align, Viewport viewport)
        {
            var view = viewport;
            SetPosition(view.X, view.Y);

            Vector2 tempPosition = Util.AlignObject(view.Bounds, ScaledSize, align);

            tempPosition.X += Origin.X;
            tempPosition.Y += Origin.Y;

            _oldPosition = tempPosition;
            _position = tempPosition;
        }

        /// <summary>
        /// Incrementa a posição do objeto.
        /// </summary>
        /// <param name="x">Incremento no eixo X.</param>
        /// <param name="y">Incremento no eixo Y.</param>
        public void Move(float x, float y) => Move(new Vector2(x, y));

        /// <summary>
        /// Incrementa a posição do objeto.
        /// </summary>
        /// <param name="amount">A quantidade de posições a ser incrementada.</param>
        public void Move(Point amount) => Move(amount.ToVector2());

        /// <summary>
        /// Incrementa a posição do objeto.
        /// </summary>
        /// <param name="amount">A quantidade de posições a ser incrementada.</param>
        public void Move(Vector2 amount)
        {
            if (amount.X != 0)
                X += amount.X;
            if (amount.Y != 0)
                Y += amount.Y;
        }

        //Escala

        /// <summary>Define a escala.</summary>
        /// <param name="scale">A escala no eixo X e Y simultaneamente.</param>
        public void SetScale(float scale) => Scale = new Vector2(scale);

        /// <summary>Define a escala.</summary>
        /// <param name="x">A escala no eixo X.</param>
        /// <param name="y">A escala no eixo Y.</param>
        public void SetScale(float x, float y) => Scale = new Vector2(x, y);

        /// <summary>Define a escala.</summary>
        /// <param name="velocity">A escala no eixo X e Y.</param>
        public void SetScale(Vector2 scale)
        {
            Scale = scale;
        }

        //Rotação

        /// <summary>Define a rotação em graus.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void SetRotationD(float degrees) => SetRotationR(MathHelper.ToRadians(degrees));

        /// <summary>Define a rotação em radianos.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void SetRotationR(float radians) 
        { 
            Rotation = radians;
        }

        /// <summary>Incrementa a rotação em graus.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void RotateD(float degrees) => RotateR(MathHelper.ToRadians(degrees));

        /// <summary>Incremente a rotação em radianos.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void RotateR(float radians) 
        { 
            Rotation += radians;
        }

        //Origem

        /// <summary>
        /// Define a origem do desenho e da rotação do objeto.
        /// </summary>
        /// <param name="x">O valor no eixo X.</param>
        /// <param name="y">O valor no eixo Y</param>
        public void SetOrigin(float x, float y) => SetOrigin(new Vector2(x, y));

        /// <summary>
        /// Define a origem do desenho e da rotação do objeto.
        /// </summary>
        /// <param name="origin">Os valores nos eixos X e Y.</param>
        public void SetOrigin(Vector2 origin)
        {
            Origin = origin;
        }

        /// <summary>
        /// Define a origem do desenho e da rotação do objeto.
        /// </summary>
        /// <param name="align">A posição da origem informando o alinhamento.</param>
        public void SetOrigin(AlignType align)
        {
            switch(align)
            {
                case AlignType.LeftTop:
                    Origin = GetOrigins().LeftTop;
                    break;
                case AlignType.Left:
                    Origin = GetOrigins().Left;
                    break;
                case AlignType.LeftBottom:
                    Origin = GetOrigins().LeftBottom;
                    break;
                case AlignType.RightTop:
                    Origin = GetOrigins().RightTop;
                    break;
                case AlignType.Right:
                    Origin = GetOrigins().Right;
                    break;
                case AlignType.RightBottom:
                    Origin = GetOrigins().RightBottom;
                    break;
                case AlignType.Top:
                    Origin = GetOrigins().Top;
                    break;
                case AlignType.Center:
                    Origin = GetOrigins().Center;
                    break;
                case AlignType.Bottom:
                    Origin = GetOrigins().Bottom;
                    break;
            }
        }
        
        private OriginValues GetOrigins()
        {
            return new OriginValues(this);
        }
    }
}
