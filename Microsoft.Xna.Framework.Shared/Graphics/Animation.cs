// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System.Collections.Generic;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma animação de sprites.</summary>
    public class Animation : IDisposable, IUpdateDrawable
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        protected Game game = null;        
        protected int elapsedGameTime = 0;
        protected Vector2 origin = Vector2.Zero;                
        protected bool useDestinationBounds = false;
        protected Rectangle destinationBounds = Rectangle.Empty;
        protected bool disposed = false;

        private int time = 0;        
        private Vector2 drawPercentage = Vector2.One;
        private int _index = 0;
        private int _findex = 0;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        ///<summary>Obtém ou define se a animaçãp é desenhável ou atualizável.</summary>
        public EnableGroup Enable { get; set; } = new EnableGroup();
        /// <summary>Obtém ou define a lista de sprites.</summary>
        public List<Sprite> Sprites { get; set; } = new List<Sprite>();
        /// <summary>Obtém ou define o tempo de exibição de cada sprite (se inidividuais) ou de cada retângulo (se uma folha de sprites)</summary>
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
        /// <summary>Obtém ou define a origem para desenho de cada sprite.</summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;
        /// <summary>Obtém o retângulo atual da animação</summary>
        public Rectangle Bounds { get; protected set; } = Rectangle.Empty;
        /// <summary>Obtém o tamanho atual da animação</summary>
        public Point Size { get; protected set; } = Point.Zero;
        /// <summary>Obtém o tamanho da Textura em relação a escala atual.</summary>
        public Vector2 ScaledSize { get => Util.GetScaledSize(Size, Scale); }
        /// <summary>Obtém ou define a rotação da sprite corrente ao ser desenhada.</summary>
        public float Rotation { get; set; } = 0.0f;
        /// <summary>Obtém ou define a escala da sprite quando ela for desenhada.</summary>
        public Vector2 Scale { get; set; } = Vector2.One;
        /// <summary>Obtém ou define a cor da sprite corrente ao ser desenhada.</summary>
        public Color Color { get; set; } = Color.White;
        /// <summary>Obtém ou define o LayerDepth do método Draw.</summary>
        public float LayerDepth { get; set; } = 0;
        /// <summary>Obtém ou define os efeitos da sprite corrente ao ser desenhada.</summary>
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        /// <summary>Obtém o Sprite atual que está sendo trabalhado.</summary>
        public Sprite CurrentSprite { get; protected set; } = null;
        /// <summary>Obtém ou define o nome da animação.</summary>
        public string Name { get; set; } = string.Empty;          

        /// <summary>Retorna o valor True se a animação chegou ao fim.</summary>
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
        /// Obtém ou define a porcentagem de largura e altura do desenho. De 0f a 1f.
        /// <para>
        /// O valor 1f em X e Y representa 100% do desenho.
        /// </para>
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

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância da classe Animation.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="time">O tempo de cada quadro da animação.</param>
        public Animation(Game game, int time, string name)
        {
            this.game = game;
            Time = time;
            Name = name;
        }

        /// <summary>Inicializa uma nova instância da classe Animation utilizando uma cópia profunda de uma animação como origem.</summary>
        /// <param name="source">A animação a ser copiada.</param>
        public Animation(Animation source)
        {
            elapsedGameTime = source.elapsedGameTime;
            game = source.game;
            Index = source.Index;
            FrameIndex = source.FrameIndex;
            Position = source.Position;
            Origin = source.Origin;
            Sprites = source.Sprites;            
            Rotation = source.Rotation;
            Scale = source.Scale;
            Color = source.Color;
            LayerDepth = source.LayerDepth;
            SpriteEffect = source.SpriteEffect;
            Enable = source.Enable;
            CurrentSprite = source.CurrentSprite;
            Time = source.Time;
            Bounds = source.Bounds;
            Name = source.Name;
            DrawPercentage = source.DrawPercentage;
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Retorna um Sprite contido na propriedade Sprites através de um index.</summary>
        /// <param name="index">Posição na lista a ser acessada.</param>  
        public Sprite this[int index]
        {
            get
            {
                try
                {
                    return Sprites[index];
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(ex.Message);
                }
            }
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Atualiza a animação.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
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

            //Chama OnUpdate
            OnUpdate?.Invoke(this, gameTime);

            //Verifica se é necessário usar 'destinationBounds' ao invés de 'Bounds' no método Draw.
            if (DrawPercentage == Vector2.One)
                useDestinationBounds = false;
            else
            {
                useDestinationBounds = true;

                int x = Bounds.X;
                int y = Bounds.Y;
                float w = Bounds.Width * DrawPercentage.X;
                float h = Bounds.Height * DrawPercentage.Y;

                destinationBounds = new Rectangle(x, y, (int)w, (int)h);
            }                
        }
        
        protected virtual void UpdateBounds()
        {
            Rectangle currentFrame = CurrentSprite[FrameIndex].Bounds;
            Size = currentFrame.Size;
            Bounds = currentFrame;
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

                //Não continua se o tempo for menor que zero.
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

        /// <summary>Método de desenho.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible || CurrentSprite == null)
                return;

            Rectangle _bounds = Bounds;

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

        /// <summary>Desenha a animação no formato 'tilemap'. Ou pode setar o valor da propriedade DrawMode como TileMap e chamar o método Draw.</summary>
        protected void DrawTileMap(GameTime gameTime, SpriteBatch spriteBatch, byte[,] map)
        {
            int dx = map.GetLength(0);
            int dy = map.GetLength(1);

            for (int line = 0; line < dx; line++)
            {
                for (int column = 0; column < dy; column++)
                {
                    if (map[line, column].Equals(1))
                    {
                        Vector2 position = Position;
                        position.X += ScaledSize.X * column;
                        position.Y += ScaledSize.Y * line;

                        if (useDestinationBounds)
                        {
                            spriteBatch.Draw(
                                texture: CurrentSprite.Texture,
                                position: position,
                                sourceRectangle: destinationBounds,
                                color: Color,
                                rotation: Rotation,
                                origin: origin,
                                scale: Scale,
                                effects: SpriteEffect,
                                layerDepth: LayerDepth
                                );
                        }
                        else
                        {
                            spriteBatch.Draw(
                                   texture: CurrentSprite.Texture,
                                   position: position,
                                   sourceRectangle: Bounds,
                                   color: Color,
                                   rotation: Rotation,
                                   origin: origin + CurrentSprite.Frames[FrameIndex].OriginCorrection,
                                   scale: Scale,
                                   effects: SpriteEffect,
                                   layerDepth: LayerDepth
                                   );
                        }                        
                    }
                }
            }
        }

        /// <summary>Define as propriedades Index e FrameIndex com o valor 0.</summary>
        public void ResetIndex()
        {
            Index = 0;
            FrameIndex = 0;
        }

        /// <summary>Adiciona uma quantidade desejada de objetos a lista de Sprites.</summary>
        /// <param name="source">Lista contendo os caminhos das texturas na pasta Content.</param>
        public void AddSprite(params string[] sources)
        {
            List<Sprite> tmpSprites = new List<Sprite>();

            foreach(string s in sources)
            {
                Sprite temp = new Sprite(game.Content.Load<Texture2D>(s));
                tmpSprites.Add(temp);
            }

            AddSprite(tmpSprites.ToArray());
        }

        /// <summary>Adiciona objetos da classe Sprite a lista.</summary>
        /// <param name="source">Caminho da textura na pasta Content.</param>
        /// <param name="frames">Lista de retângulos (Frames) relativos a textura.</param>
        public void AddSprite(string source, params SpriteFrame[] frames)
        {   
            Sprite tempSprite = new Sprite(game, source);

            foreach (SpriteFrame sf in frames)
                tempSprite.AddFrame(sf);

            AddSprite(tempSprite);
        }

        /// <summary>Adiciona objetos da classe Sprite a lista.</summary>
        /// <param name="sprites">Lista com objetos da classe Sprite.</param>
        public void AddSprite(params Sprite[] sprites)
        {
            if (sprites != null)
                Sprites.AddRange(sprites);

            if (CurrentSprite == null)
            {
                CurrentSprite = Sprites[0];
                Bounds = CurrentSprite[FrameIndex].Bounds;
            }
        }

        //A ser chamado em uma classe derivada quando necessário.
        protected void InvokeOnDraw(Animation animation, GameTime gameTime, SpriteBatch spriteBatch)
        {
            OnDraw?.Invoke(animation, gameTime, spriteBatch);
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//

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
                game = null;
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