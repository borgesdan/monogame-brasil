// Danilo Borges Santos, 2020.

using System.Collections.Generic;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma animação de sprites.</summary>
    public class Animation : IDisposable, IUpdateDrawable, IBoundsable
    {
        #region CAMPOS

        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        int _index = 0;
        int _findex = 0;
        int time = 0;
        int elapsedGameTime = 0;        
        bool useDestinationBounds = false;
        bool disposed = false;
        Rectangle destinationBounds = Rectangle.Empty;
        Vector2 origin = Vector2.Zero;
        Vector2 drawPercentage = Vector2.One;        

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém a instância da classe Game.</summary>
        public Game Game { get; protected set; } = null;
        ///<summary>Obtém ou define se a animaçãp é desenhável e atualizável.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);
        /// <summary>Obtém ou define a lista de sprites.</summary>
        public List<Sprite> Sprites { get; set; } = new List<Sprite>();
        /// <summary>Obtém ou define o tempo de exibição de cada frame do sprite.</summary>
        public int Time 
        {
            get => time;
            set
            {
                time = MathHelper.Clamp(value, 0, int.MaxValue);
            }
        }
        /// <summary>Obtém a posição atual da animação</summary>
        public int Index 
        {
            get => _index;
            protected set
            {
                _index = value;
                OnChangeIndex?.Invoke(this);
            }
        }
        /// <summary>Obtém a posição do frame da sprite corrente.</summary>
        public int FrameIndex 
        {
            get => _findex;
            protected set
            {
                _findex = value;
                OnChangeFrameIndex?.Invoke(this);
            }
        }
        /// <summary>Obtém ou define a posição na tela da animação</summary>
        public Vector2 Position { get; set; } = Vector2.Zero;
        /// <summary>Obtém ou define a posição no eixo X.</summary>
        public float X { get => Position.X; set => Position = new Vector2(value, Y); }
        /// <summary>Obtém ou define a posição no eixo Y.</summary>
        public float Y { get => Position.Y; set => Position = new Vector2(X, value); }
        /// <summary>Obtém ou define a origem para desenho de cada sprite.</summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;
        /// <summary>Obtém ou define a origem no eixo X.</summary>
        public float Xo { get => Origin.X; set => Origin = new Vector2(value, Yo); }
        /// <summary>Obtém ou define a origem no eixo Y.</summary>
        public float Yo { get => Origin.Y; set => Origin = new Vector2(Xo, value); }
        /// <summary>Obtém o atual frame da animação.</summary>
        public Rectangle Frame { get; protected set; }
        /// <summary>Obtém os limites da animação. Sua largura e altura (com escala) e sua posição.</summary>
        public virtual Rectangle Bounds 
        {
            get
            {
                Rectangle b = new Rectangle((int)Position.X, (int)Position.Y, (int)ScaledSize.X, (int)ScaledSize.Y);
                return b;
            }
        }
        /// <summary>Obtém o tamanho atual da animação</summary>
        public Point Size { get; protected set; } = Point.Zero;
        /// <summary>Obtém a largura da animação</summary>
        public int Width { get => Size.X; }
        /// <summary>Obtém a altura da animação</summary>
        public int Height { get => Size.Y; }
        /// <summary>Obtém o tamanho da Textura em relação a escala atual.</summary>
        public Vector2 ScaledSize { get => Util.GetScaledSize(Size, Scale); }
        /// <summary>Obtém ou define a rotação da sprite corrente ao ser desenhada.</summary>
        public float Rotation { get; set; } = 0.0f;
        /// <summary>Obtém ou define a escala da sprite quando ela for desenhada.</summary>
        public Vector2 Scale { get; set; } = Vector2.One;
        /// <summary>Obtém ou define a escala em X.</summary>
        public float Xs { get => Scale.X; set => Scale = new Vector2(value, Ys); }
        /// <summary>Obtém ou define a escala em Y.</summary>
        public float Ys { get => Scale.Y; set => Scale = new Vector2(Xs, value); }
        /// <summary>Obtém ou define a cor da sprite corrente ao ser desenhada.</summary>
        public Color Color { get; set; } = Color.White;
        /// <summary>Obtém ou define o LayerDepth do método Draw.</summary>
        public float LayerDepth { get; set; } = 0;
        /// <summary>Obtém ou define os efeitos da sprite corrente ao ser desenhada.</summary>
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        /// <summary>Obtém o Sprite atual que está sendo trabalhado.</summary>
        public Sprite CurrentSprite { get; protected set; } = null;
        /// <summary>Obtém as caixas de colisão do atual frame.</summary>
        public List<CollisionBox> CollisionBoxesList { get; private set; } = new List<CollisionBox>();
        /// <summary>Obtém as caixas de ataque do atual frame.</summary>
        public List<AttackBox> AttackBoxesList { get; private set; } = new List<AttackBox>();
        /// <summary>Obtém ou define o nome da animação.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Retorna True se a animação chegou ao fim.</summary>
        public bool IsFinished
        {
            get
            {
                if (ElapsedTime == TotalTime)
                    return true;
                else
                    return false;                
            }
        }
        /// <summary>Obtém o tempo total da animação.</summary>
        public TimeSpan TotalTime
        {
            get
            {
                int frames = 0;

                foreach(var sf in Sprites)
                {
                    frames += sf.Frames.Count;
                }
                
                int m = Time * frames;
                return new TimeSpan(0, 0, 0, 0, m);
            }
        }
        /// <summary>Obtém o tempo que já se passou da animação.</summary>
        public TimeSpan ElapsedTime
        {
            get
            {
                int count = 0;

                for (int i = 0; i <= Index; i++)
                {
                    for(int f = 0; f <= Sprites[i].Frames.Count - 1; f++)
                    {
                        if (i == Index && f > FrameIndex)
                            break;

                        count++;
                    }
                }

                var m = Time * count;
                return new TimeSpan(0, 0, 0, 0, m);
            }
        }
        
        /// <summary>
        /// Obtém ou define a porcentagem de largura e altura do desenho. De 0f (0%) a 1f (100%).
        /// </summary>
        public Vector2 DrawPercentage 
        {
            get => drawPercentage;
            set
            {
                float x = MathHelper.Clamp(value.X, 0f, 1f);
                float y = MathHelper.Clamp(value.Y, 0f, 1f);

                drawPercentage = new Vector2(x, y);
            }
        }

        //---------------------------------------//
        //-----         EVENTOS             -----//
        //---------------------------------------//

        /// <summary>Evento chamado no fim do método Update.</summary>
        public event UpdateAction<Animation> OnUpdate;        
        /// <summary>Evento chamado no fim do método Draw.</summary>
        public event DrawAction<Animation> OnDraw;
        /// <summary>Evento chamado quando a animação chega ao fim.</summary>
        public event Action<Animation> OnEndAnimation;
        /// <summary>Evento chamado quando o valor do Index é mudado.</summary>
        public event Action<Animation> OnChangeIndex;
        /// <summary>Evento chamado quando o valor do FrameIndex é mudado.</summary>
        public event Action<Animation> OnChangeFrameIndex;

        #endregion

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância da classe Animation.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="time">O tempo de cada quadro da animação.</param>
        /// <param name="name">O nome da animação.</param>
        public Animation(Game game, int time, string name)
        {
            Game = game;
            Time = time;
            Name = name;
        }

        /// <summary>Inicializa uma nova instância da classe Animation como cópia de outro Animation.</summary>
        /// <param name="source">A animação a ser copiada.</param>
        public Animation(Animation source)
        {
            elapsedGameTime = source.elapsedGameTime;            
            Game = source.Game;
            Index = source.Index;
            FrameIndex = source.FrameIndex;
            Position = source.Position;
            Origin = source.Origin;
            
            source.Sprites.ForEach(s => Sprites.Add(new Sprite(s)));
            
            Rotation = source.Rotation;
            Scale = source.Scale;
            Color = source.Color;
            LayerDepth = source.LayerDepth;
            SpriteEffect = source.SpriteEffect;
            Enable = source.Enable;

            int cs_index = source.Sprites.FindIndex(i => i == source.CurrentSprite);
           
            CurrentSprite = Sprites[cs_index];

            Time = source.Time;
            Frame = source.Frame;
            Name = source.Name;
            DrawPercentage = source.DrawPercentage;

            source.CollisionBoxesList.ForEach(cb => CollisionBoxesList.Add(cb));
            source.AttackBoxesList.ForEach(ab => AttackBoxesList.Add(ab));
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Obtém ou define um Sprite contido na propriedade Sprites através de um index.</summary>
        /// <param name="index">Posição na lista a ser acessada.</param>  
        public Sprite this[int index]
        {
            get => Sprites[index];        
            set => Sprites[index] = value;            
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <inheritdoc />
        public virtual void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            //Atualiza a animação.
            Animate(gameTime);                       

            //Atualiza o tamanho da animação.
            UpdateBounds();

            //Atualiza os valores para o desenho final.
            UpdateOrigin();

            //Atualiza as caixas de colisão.
            SetBoxes();

            //Verifica se é necessário usar 'destinationBounds' ao invés de 'Bounds' no método Draw.
            if (DrawPercentage == Vector2.One)
                useDestinationBounds = false;
            else
            {
                useDestinationBounds = true;

                int x = Frame.X;
                int y = Frame.Y;
                float w = Frame.Width * DrawPercentage.X;
                float h = Frame.Height * DrawPercentage.Y;

                destinationBounds = new Rectangle(x, y, (int)w, (int)h);
            }

            //Chama OnUpdate
            OnUpdate?.Invoke(this, gameTime);
        }
        
        private void SetBoxes()
        {
            CollisionBoxesList.Clear();
            AttackBoxesList.Clear();

            foreach (CollisionBox cb in CurrentSprite.CollisionBoxes)
            {
                if (cb.Index == FrameIndex)
                    CollisionBoxesList.Add(cb);
            }

            foreach (AttackBox ab in CurrentSprite.AttackBoxes)
            {
                if (ab.Index == FrameIndex)
                    AttackBoxesList.Add(ab);
            }
        }

        /// <summary>
        /// Atualiza os limites da animação.
        /// </summary>
        public virtual void UpdateBounds()
        {
            Rectangle currentFrame = Rectangle.Empty;

            if (CurrentSprite != null)
                currentFrame = CurrentSprite[FrameIndex].Bounds;                

            Size = currentFrame.Size;
            Frame = currentFrame;
        }

        private void UpdateOrigin()
        {
            Vector2 f_o = CurrentSprite.Frames[FrameIndex].OriginCorrection;
            
            origin = Origin + f_o;
        }

        private void Animate(GameTime gameTime)
        {
            //Verifica se existem Sprites.
            if (Sprites.Count > 0)
            {
                if (CurrentSprite == null)
                    CurrentSprite = Sprites[0];

                //Não continua se o tempo igual a zero.
                if (Time == 0)
                    return;

                //Tempo que já se passou desde a última troca de sprite.
                elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedGameTime > Time)
                {
                    //Verifica se o index do frame atual, é maior que a quantidade de frames do sprite ativo.
                    if (FrameIndex >= Sprites[Index].Frames.Count - 1)
                    {
                        //Se sim, é hora de pular de sprite ou voltar para o primeiro frame,
                        //Caso só tenhamos uma sprite

                        if (Index >= Sprites.Count - 1)
                        {
                            Index = 0;
                        }                            
                        else
                        {
                            Index++;
                        }                            
                        
                        FrameIndex = 0;
                    }
                    else
                    {
                        FrameIndex++;
                    }                        

                    //Reseta o tempo.
                    elapsedGameTime = 0;
                    //Atualiza o sprite atual.
                    CurrentSprite = Sprites[Index];
                }
            }

            if(IsFinished)
            {
                OnEndAnimation?.Invoke(this);
            }
        }

        /// <summary>
        /// Importa os valores das propriedades da entidade e as define na animação.
        /// </summary>
        /// <param name="entity"></param>
        public void SetProperties(Entity2D entity)
        {
            Color = entity.Transform.Color;
            SpriteEffect = entity.Transform.SpriteEffect;
            Rotation = entity.Transform.Rotation;
            Scale = entity.Transform.Scale;
            Position = entity.Transform.Position;
            LayerDepth = entity.LayerDepth;
            Origin = entity.Origin;
            DrawPercentage = entity.DrawPercentage;
        }

        /// <summary>Avança um sprite da animação.</summary>
        public void ForwardIndex()
        {
            Index++;

            if (Index >= Sprites.Count - 1)
                Index = 0;

            CurrentSprite = Sprites[Index];
            UpdateBounds();
        }

        /// <summary>Avança um Frame do atual sprite da animação.</summary>
        public void ForwardFrameIndex()
        {
            FrameIndex++;

            if (FrameIndex >= Sprites[Index].Frames.Count - 1)
                FrameIndex = 0;

            UpdateBounds();
        }

        /// <inheritdoc />
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible ||        //Se a animação está em modo visível
                CurrentSprite == null)    //Se existe um sprite ativo.
                return;            

            Rectangle _bounds = Frame;

            if (useDestinationBounds)
                _bounds = destinationBounds;
            
            spriteBatch.Draw(
                   texture: CurrentSprite.Texture,
                   position: Position,
                   sourceRectangle: _bounds,
                   color: Color,
                   rotation: Rotation,
                   origin: origin,
                   scale: Scale,
                   effects: SpriteEffect,
                   layerDepth: LayerDepth
                   );

            //chama OnDraw
            OnDraw?.Invoke(this, gameTime, spriteBatch);            
        }

        /// <summary>Define as propriedades Index e FrameIndex com o valor 0.</summary>
        public void Reset()
        {
            Index = 0;
            FrameIndex = 0;
            elapsedGameTime = 0;
        }

        /// <summary>Adiciona uma quantidade desejada de objetos a lista de Sprites.</summary>
        /// <param name="source">Lista contendo os caminhos das texturas na pasta Content.</param>
        public void AddSprites(params string[] sources)
        {
            List<Sprite> tmpSprites = new List<Sprite>();

            foreach(string s in sources)
            {
                Sprite temp = new Sprite(Game.Content.Load<Texture2D>(s), true);
                tmpSprites.Add(temp);
            }

            AddSprites(tmpSprites.ToArray());
        }        

        /// <summary>Adiciona sprites a animação.</summary>
        /// <param name="sprites">Os sprites a serem adicionados.</param>
        public void AddSprites(params Sprite[] sprites)
        {
            if (sprites != null)
                Sprites.AddRange(sprites);

            if (CurrentSprite == null)
            {
                CurrentSprite = Sprites[0];
                Frame = CurrentSprite[FrameIndex].Bounds;
            }
        }

        /// <summary>
        /// Adiciona um sprite e os frames desejados da ação.
        /// </summary>
        /// <param name="source">O sprite a ser adicionado. A referêcia será copiada em uma nova instância</param>
        /// <param name="frames">A lista de frames no sprite.</param>
        public void AddSprite(Sprite source, params SpriteFrame[] frames)
        {
            Sprite sprite = new Sprite(source);
            sprite.Frames.Clear();
            
            foreach(var f in frames) 
            {
                sprite.AddFrame(f);
            }

            AddSprites(sprite);
        }

        /// <summary>
        /// Adiciona um sprite e os frames desejados da ação.
        /// </summary>
        /// <param name="source">O sprite a ser adicionado através de um arquivo na pasta Content.</param>
        /// <param name="frames">A lista de frames no sprite.</param>
        public void AddSprite(string source, params SpriteFrame[] frames)
        {
            Sprite sprite = new Sprite(Game, source);

            foreach (var f in frames)
            {
                sprite.AddFrame(f);
            }

            AddSprites(sprite);
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//
        
        /// <inheritdoc />
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
                Game = null;
                Sprites.Clear();
                Sprites = null;
                CurrentSprite = null;
                Name = null;

                OnChangeFrameIndex = null;
                OnChangeIndex = null;
                OnDraw = null;
                OnEndAnimation = null;
                OnUpdate = null;
            }

            disposed = true;
        }
    }
}