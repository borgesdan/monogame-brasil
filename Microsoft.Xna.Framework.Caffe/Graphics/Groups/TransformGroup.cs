// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Expõe acesso as transformações da entidade, como posição, velocidade, rotação, entre outros.</summary>
    public sealed class TransformGroup
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

                if (inCopy)
                    return;

                Entity.UpdateBounds();
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

                Entity.UpdateBounds();
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

                if (inCopy)
                    return;

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

                if (inCopy)
                    return;

                Entity.UpdateBounds();
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

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de TransformGroup.</summary>
        /// <param name="entity">A entidade a ser associada a este grupo.</param>
        public TransformGroup(Entity2D entity)
        {
            Entity = entity ?? throw new System.ArgumentNullException(nameof(entity));
        }

        /// <summary>
        /// Inicializa uma nova instância como cópia de outra instância de TransformGroup.
        /// </summary>
        /// <param name="destination">A entidade a ser associada a este grupo.</param>
        /// <param name="source">O TransformGroup a ser copiado.</param>
        public TransformGroup(Entity2D destination, TransformGroup source)
        {
            inCopy = true;

            this.Entity = destination;
            this.Size = source.Size;
            this.Color = source.Color;
            this.Entity = destination;            
            this.Rotation = source.Rotation;
            this.Scale = source.Scale;            
            this.SpriteEffect = source.SpriteEffect;
            this.Velocity = source.Velocity;
            this.Position = source.Position;

            inCopy = false;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Atualiza os cálculos de velocidade.
        /// </summary>
        public void Update()
        {
            if (Xv != 0)
                X += Velocity.X;
            if (Yv != 0)
                Y += Velocity.Y;

            Velocity += VResistance;
        }

        //Velocidade

        /// <summary>Interte a velocidade nos eixos X e Y.</summary>
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
        /// Define a resistência à velocidade
        /// </summary>
        /// <param name="resistance">O vetor com os valores da resistência.</param>
        public void SetVResistance(Vector2 resistance)
        {
            VResistance = resistance;
        }

        /// <summary>
        /// Define a resistência à velocidade
        /// </summary>
        /// <param name="resistance">O vetor com os valores da resistência.</param>
        /// <param name="velocity">O valor da velocidade caso precise redefiní-la.</param>
        public void SetVResistance(Vector2 resistance, Vector2 velocity)
        {
            VResistance = resistance;
            SetVelocity(velocity);
        }

        /// <summary>Define a velocidade.</summary>
        /// <param name="velocity">A velocidade no eixo X e Y.</param>
        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        //Posição

        /// <summary>Define a posição</summary>
        /// <param name="position">A posição no eixo X e Y.</param>
        public void SetPosition(Point position) 
        { 
            SetPosition(position.X, position.Y);
        }

        /// <summary>Define a posição</summary>
        /// <param name="position">A posição no eixo X e Y.</param>
        public void SetPosition(Vector2 position) => SetPosition(position.X, position.Y);

        /// <summary>Define a posição</summary>
        /// <param name="x">A posição no eixo X.</param>
        /// <param name="y">A posição no eixo Y.</param>
        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;

            Entity.UpdateBounds();
        }

        /// <summary>
        /// Incrementa a posição da entidade.
        /// </summary>
        /// <param name="x">Incremento no eixo X.</param>
        /// <param name="y">Incremento no eixo Y.</param>
        public void Move(float x, float y) => Move(new Vector2(x, y));

        /// <summary>
        /// Incrementa a posição da entidade.
        /// </summary>
        /// <param name="amount">A quantidade de posições a ser incrementada.</param>
        public void Move(Point amount) => Move(amount.ToVector2());

        /// <summary>
        /// Incrementa a posição da entidade.
        /// </summary>
        /// <param name="amount">A quantidade de posições a ser incrementada.</param>
        public void Move(Vector2 amount)
        {
            if(amount.X != 0)
                X += amount.X;
            if(amount.Y != 0)
                Y += amount.Y;

            Entity.UpdateBounds();
        }

        //Escala

        /// <summary>Define a escala.</summary>
        /// <param name="scale">A escala no eixo X e Y simultaneamente.</param>
        public void SetScale(float scale) => Scale = new Vector2(scale);

        /// <summary>Define a escala.</summary>
        /// <param name="x">A escala no eixo X.</param>
        /// <param name="y">A escala no eixo Y.</param>
        public void SetScale(float x, float y) => Scale = new Vector2(x, y);

        /// <summary>Define a escala</summary>
        /// <param name="velocity">A escala no eixo X e Y.</param>
        public void SetScale(Vector2 scale) 
        { 
            Scale = scale;
            Entity.UpdateBounds();
        }
        
        //Rotação

        /// <summary>Define a rotação em graus.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void SetRotationD(float degrees) => Rotation = MathHelper.ToRadians(degrees);

        /// <summary>Define a rotação em radianos.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void SetRotationR(float radians) => Rotation = radians;

        /// <summary>Incrementa a rotação em graus.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void RotateD(float degrees) => Rotation += MathHelper.ToRadians(degrees);

        /// <summary>Incremente a rotação em radianos.</summary>
        /// <param name="degrees">O grau da rotação</param>
        public void RotateR(float radians) => Rotation += radians;

        /// <summary>Define a posição da entidade relativa a Viewport.</summary>   
        /// <param name="alignType">O tipo de alinhamento da tela.</param>
        public void SetViewPosition(AlignType alignType)
        {
            Entity.UpdateBounds();            

            var view = Entity.Game.GraphicsDevice.Viewport;
            SetPosition(view.X, view.Y);

            Vector2 tempPosition = Util.AlignActor(view.Bounds, ScaledSize, alignType);

            tempPosition.X += Entity.Origin.X;
            tempPosition.Y += Entity.Origin.Y;

            _oldPosition = tempPosition;
            _position = tempPosition;

            Entity.UpdateBounds();
        }        
    }    
}
