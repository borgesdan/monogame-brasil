// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Expõe acesso as transformações de um objeto, como posição, velocidade, rotação, entre outros.
    /// </summary>
    /// <typeparam name="T">T é uma classe que implementa a interface IBoundsable.</typeparam>
    public sealed class TransformGroup<T> where T : IBoundsable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        Vector2 _oldPosition = Vector2.Zero;
        Vector2 _position = Vector2.Zero;
        Vector2 _scale = Vector2.One;
        float _rotation = 0f;

        //Só usada no construtor de cópia.
        readonly bool inCopy = false;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém o define o objeto a qual este grupo está atrelado.</summary>
        public T Owner { get; set; } = default;

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

                if (inCopy)
                    return;
            }
        }
        /// <summary>Obtém ou define a velocidade.</summary>
        public Vector2 Velocity { get; set; }
        /// <summary>Obtém ou define o valor de resitência à velocidade definida.</summary>
        public Vector2 VResistance { get; set; }
        /// <summary>Obtém o tamanho da entidade.</summary>
        public Point Size { get; internal set; } = Point.Zero;
        /// <summary>Obtém ou define a rotação. Valor padrão 0.</summary>
        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;

                if (inCopy)
                    return;
            }
        }
        /// <summary>Obtém ou define a escala. Valor padrão Vector.Zero.</summary>
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;

                if (inCopy)
                    return;
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
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        /// <summary>Obtém ou define o posição no eixo X.</summary>
        public float X
        {
            get => Position.X;
            set
            {
                _oldPosition.X = _position.X;
                _position.X = value;

                if (inCopy)
                    return;
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

                if (inCopy)
                    return;
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
        /// <param name="owner">O objeto a ser associada a este grupo.</param>
        public TransformGroup(T owner)
        {
            Owner = owner ?? throw new System.ArgumentNullException(nameof(owner));
        }

        /// <summary>
        /// Inicializa uma nova instância como cópia de outra instância de TransformGroup.
        /// </summary>
        /// <param name="destination">A entidade a ser associada a este grupo.</param>
        /// <param name="source">O TransformGroup a ser copiado.</param>
        public TransformGroup(T destination, TransformGroup<T> source)
        {
            inCopy = true;

            this.Owner = destination;
            this.Size = source.Size;
            this.Color = source.Color;
            this.Rotation = source.Rotation;
            this.Scale = source.Scale;
            this.SpriteEffects = source.SpriteEffects;
            this.Velocity = source.Velocity;
            this.Position = source.Position;
            this.Origin = source.Origin;
            this.LayerDepth = source.LayerDepth;

            inCopy = false;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        public void Set<T2>(TransformGroup<T2> source) where T2 : IBoundsable
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
        /// Atualiza os cálculos necessários do TransformGroup e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        public void Update()
        {
            if (Xv != 0)
                X += Velocity.X;
            if (Yv != 0)
                Y += Velocity.Y;

            Velocity += VResistance;

            Owner.UpdateBounds();
        }

        //Velocidade

        /// <summary>Inverte a velocidade nos eixos X e Y e invoca o método UpdateBounds do objeto associado.</summary>
        public Vector2 InvertVelocity()
        {
            InvertVelocityX();
            InvertVelocityY();

            return Velocity;
        }
        /// <summary>Inverte a velocidade no eixo X e invoca o método UpdateBounds do objeto associado.</summary>
        public Vector2 InvertVelocityX()
        {
            Xv *= -1;
            Owner.UpdateBounds();

            return Velocity;
        }
        /// <summary>Inverte a velocidade no eixo Y e invoca o método UpdateBounds do objeto associado.</summary>
        public Vector2 InvertVelocityY()
        {
            Yv *= -1;
            Owner.UpdateBounds();

            return Velocity;
        }

        /// <summary>Define a velocidade e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="velocity">A velocidade no eixo X e Y.</param>
        public void SetVelocity(float velocity)
        {
            SetVelocity(new Vector2(velocity));
        }

        /// <summary>Define a velocidade e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="x">A velocidade no eixo X.</param>
        /// <param name="y">A velocidade no eixo Y.</param>
        public void SetVelocity(float x, float y)
        {
            SetVelocity(new Vector2(x, y));
        }

        /// <summary>
        /// Define a resistência à velocidade e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="x">Resistência no eixo X.</param>
        /// <param name="y">Resistência no eixo Y.</param>
        public void SetVResistance(float x, float y)
        {
            SetVResistance(new Vector2(x, y));
        }

        /// <summary>
        /// Define a resistência à velocidade e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="resistance">O vetor com os valores da resistência.</param>
        public void SetVResistance(Vector2 resistance)
        {
            VResistance = resistance;
            Owner.UpdateBounds();
        }

        /// <summary>
        /// Define a resistência à velocidade e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="resistance">O vetor com os valores da resistência.</param>
        /// <param name="velocity">O valor da velocidade caso precise redefiní-la.</param>
        public void SetVResistance(Vector2 resistance, Vector2 velocity)
        {
            VResistance = resistance;
            SetVelocity(velocity);

            Owner.UpdateBounds();
        }

        /// <summary>Define a velocidade e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="velocity">A velocidade no eixo X e Y.</param>
        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
            Owner.UpdateBounds();
        }

        //Posição

        /// <summary>Define a posição e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="position">A posição no eixo X e Y.</param>
        public void SetPosition(Point position)
        {
            SetPosition(position.X, position.Y);
        }

        /// <summary>Define a posição e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="position">A posição no eixo X e Y.</param>
        public void SetPosition(Vector2 position) => SetPosition(position.X, position.Y);

        /// <summary>Define a posição e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="x">A posição no eixo X.</param>
        /// <param name="y">A posição no eixo Y.</param>
        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;

            Owner.UpdateBounds();
        }

        /// <summary>Define a posição do objeto relativa a um Viewport e invoca o método UpdateBounds do objeto associado.</summary>   
        /// <param name="align">O tipo de alinhamento da tela.</param>
        public void SetPosition(AlignType align, Viewport viewport)
        {
            Owner.UpdateBounds();

            var view = viewport;
            SetPosition(view.X, view.Y);

            Vector2 tempPosition = Util.AlignObject(view.Bounds, ScaledSize, align);

            tempPosition.X += Origin.X;
            tempPosition.Y += Origin.Y;

            _oldPosition = tempPosition;
            _position = tempPosition;

            Owner.UpdateBounds();
        }

        /// <summary>
        /// Incrementa a posição do objeto e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="x">Incremento no eixo X.</param>
        /// <param name="y">Incremento no eixo Y.</param>
        public void Move(float x, float y) => Move(new Vector2(x, y));

        /// <summary>
        /// Incrementa a posição do objeto e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="amount">A quantidade de posições a ser incrementada.</param>
        public void Move(Point amount) => Move(amount.ToVector2());

        /// <summary>
        /// Incrementa a posição do objeto e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="amount">A quantidade de posições a ser incrementada.</param>
        public void Move(Vector2 amount)
        {
            if (amount.X != 0)
                X += amount.X;
            if (amount.Y != 0)
                Y += amount.Y;

            Owner.UpdateBounds();
        }

        //Escala

        /// <summary>Define a escala e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="scale">A escala no eixo X e Y simultaneamente.</param>
        public void SetScale(float scale) => Scale = new Vector2(scale);

        /// <summary>Define a escala e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="x">A escala no eixo X.</param>
        /// <param name="y">A escala no eixo Y.</param>
        public void SetScale(float x, float y) => Scale = new Vector2(x, y);

        /// <summary>Define a escala e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="velocity">A escala no eixo X e Y.</param>
        public void SetScale(Vector2 scale)
        {
            Scale = scale;
            Owner.UpdateBounds();
        }

        //Rotação

        /// <summary>Define a rotação em graus e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void SetRotationD(float degrees) => SetRotationR(MathHelper.ToRadians(degrees));

        /// <summary>Define a rotação em radianos e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void SetRotationR(float radians) 
        { 
            Rotation = radians;
            Owner.UpdateBounds();
        }

        /// <summary>Incrementa a rotação em graus e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void RotateD(float degrees) => RotateR(MathHelper.ToRadians(degrees));

        /// <summary>Incremente a rotação em radianos e invoca o método UpdateBounds do objeto associado.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void RotateR(float radians) 
        { 
            Rotation += radians;
            Owner.UpdateBounds();
        }

        //Origem

        /// <summary>
        /// Define a origem do desenho e da rotação do objeto e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="x">O valor no eixo X.</param>
        /// <param name="y">O valor no eixo Y</param>
        public void SetOrigin(float x, float y) => SetOrigin(new Vector2(x, y));

        /// <summary>
        /// Define a origem do desenho e da rotação do objeto e invoca o método UpdateBounds do objeto associado.
        /// </summary>
        /// <param name="origin">Os valores nos eixos X e Y.</param>
        public void SetOrigin(Vector2 origin)
        {
            Origin = origin;
            Owner.UpdateBounds();
        }
    }
}
