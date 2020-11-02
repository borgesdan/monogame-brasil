// Danilo Borges Santos, 2020.

using System.Collections.Generic;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Ator que representa uma animação de sprites.</summary>
    public class Animation : Actor
    {
        #region CAMPOS E PROPRIEDADES

        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        int _index = 0;
        int _findex = 0;
        int time = 0;
        int elapsedGameTime = 0;        
        bool useDestinationBounds = false;
        Rectangle destinationBounds = Rectangle.Empty;
        Vector2 drawOrigin = Vector2.Zero;
        Vector2 drawPercentage = Vector2.One;        

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        
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
        /// <summary>Obtém a posição atual do Sprite a ser utilizado.</summary>
        public int SpriteIndex 
        {
            get => _index;
            protected set
            {
                _index = value;
                OnChangeIndex?.Invoke(this);
            }
        }
        /// <summary>Obtém a posição do frame do Sprite atual.</summary>
        public int FrameIndex 
        {
            get => _findex;
            protected set
            {
                _findex = value;
                OnChangeFrameIndex?.Invoke(this);
            }
        }
        
        /// <summary>Obtém o frame atual da animação.</summary>
        public SpriteFrame CurrentFrame { get; protected set; }
        /// <summary>Obtém o Sprite atual que está sendo trabalhado.</summary>
        public Sprite CurrentSprite { get; protected set; } = null;
        /// <summary>Obtém as caixas de colisão do atual frame.</summary>
        public List<CollisionBox> CollisionBoxesList { get; private set; } = new List<CollisionBox>();
        /// <summary>Obtém as caixas de ataque do atual frame.</summary>
        public List<AttackBox> AttackBoxesList { get; private set; } = new List<AttackBox>();        

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
                    frames += sf.Boxes.Count;
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

                for (int i = 0; i <= SpriteIndex; i++)
                {
                    for(int f = 0; f <= Sprites[i].Boxes.Count - 1; f++)
                    {
                        if (i == SpriteIndex && f > FrameIndex)
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
        /// <param name="name">O nome do ator.</param>
        /// <param name="time">O tempo de cada quadro da animação.</param>
        public Animation(Game game, string name, int time) : base(game, name)
        {
            Time = time;
        }

        /// <summary>Inicializa uma nova instância da classe Animation.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="time">O tempo de cada quadro da animação.</param>
        /// <param name="name">O nome da animação.</param>
        /// <param name="sprite">O sprite a ser utilizado.</param>
        /// <param name="frameGroupName">O nome do grupo de frames a serem utilizados na animação.</param>
        public Animation(Game game, int time, string name, Sprite sprite, string frameGroupName) : base(game, name)
        {
            Time = time;

            AddSprite(sprite, frameGroupName);
        }

        /// <summary>Inicializa uma nova instância da classe Animation como cópia de outro Animation.</summary>
        /// <param name="source">A animação a ser copiada.</param>
        public Animation(Animation source) : base(source)
        {
            elapsedGameTime = source.elapsedGameTime;
            SpriteIndex = source.SpriteIndex;
            FrameIndex = source.FrameIndex;            
            source.Sprites.ForEach(s => Sprites.Add(new Sprite(s)));

            int cs_index = source.Sprites.FindIndex(i => i == source.CurrentSprite);           
            CurrentSprite = Sprites[cs_index];

            Time = source.Time;
            CurrentFrame = source.CurrentFrame;
            DrawPercentage = source.DrawPercentage;

            source.CollisionBoxesList.ForEach(cb => CollisionBoxesList.Add(cb));
            source.AttackBoxesList.ForEach(ab => AttackBoxesList.Add(ab));
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Obtém ou define um Sprite contido na lista Sprites através de um index.</summary>
        /// <param name="index">Posição na lista a ser acessada.</param>  
        public Sprite this[int index]
        {
            get => Sprites[index];        
            set => Sprites[index] = value;            
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//        

        /// <summary>
        /// Atualiza a animação.
        /// </summary>
        /// <param name="gameTime">Recebe os valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            if (!UpdateOffView)
                return;

            //Atualiza a animação.
            Animate(gameTime);                       

            //Atualiza o tamanho da animação.
            UpdateBounds();            

            //Atualiza o Transform
            Transform.Update();

            //Atualiza as caixas de colisão.
            SetBoxes();

            //Verifica se é necessário usar 'destinationBounds' ao invés de 'Bounds' no método Draw.
            if (DrawPercentage == Vector2.One)
                useDestinationBounds = false;
            else
            {
                useDestinationBounds = true;

                int x = CurrentFrame.X;
                int y = CurrentFrame.Y;
                float w = CurrentFrame.Width * DrawPercentage.X;
                float h = CurrentFrame.Height * DrawPercentage.Y;

                destinationBounds = new Rectangle(x, y, (int)w, (int)h);
            }

            base.Update(gameTime);
        }
        
        private void SetBoxes()
        {
            CollisionBoxesList.Clear();
            AttackBoxesList.Clear();

            CollisionBoxesList = CurrentSprite.Boxes[FrameIndex].CollisionBoxes;
            AttackBoxesList = CurrentSprite.Boxes[FrameIndex].AttackBoxes;
        }

        /// <summary>
        /// Atualiza os limites da animação.
        /// </summary>
        public override void UpdateBounds()
        {
            CurrentFrame = CurrentSprite != null ? CurrentSprite[FrameIndex] : SpriteFrame.Create(Rectangle.Empty, Vector2.Zero);

            Point size = CurrentFrame.Bounds.Size;
            Transform.Size = size;

            //O tamanho da entidade e sua posição.
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            int w = (int)Transform.ScaledSize.X;
            int h = (int)Transform.ScaledSize.Y;

            //A origem do frame.
            Vector2 sa = CurrentSprite[FrameIndex].Align;
            drawOrigin = ((Transform.Origin + sa) * Transform.Scale);

            int recX = (int)(x - drawOrigin.X);
            int recY = (int)(y - drawOrigin.Y);

            Bounds = new Rectangle(recX, recY, w, h);
            BoundsR = Util.CreateRotatedBounds(Transform, drawOrigin, Bounds);            
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
                    if (FrameIndex >= Sprites[SpriteIndex].Boxes.Count - 1)
                    {
                        //Se sim, é hora de pular de sprite ou voltar para o primeiro frame,
                        //Caso só tenhamos uma sprite

                        if (SpriteIndex >= Sprites.Count - 1)
                        {
                            SpriteIndex = 0;
                        }                            
                        else
                        {
                            SpriteIndex++;
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
                    CurrentSprite = Sprites[SpriteIndex];
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
            SpriteIndex++;

            if (SpriteIndex >= Sprites.Count - 1)
                SpriteIndex = 0;

            CurrentSprite = Sprites[SpriteIndex];
            UpdateBounds();
        }

        /// <summary>Avança um Frame do atual sprite da animação.</summary>
        public void ForwardFrameIndex()
        {
            FrameIndex++;

            if (FrameIndex >= Sprites[SpriteIndex].Boxes.Count - 1)
                FrameIndex = 0;

            UpdateBounds();
        }

        /// <inheritdoc />
        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible ||        //Se a animação está em modo visível
                CurrentSprite == null)    //Se existe um sprite ativo.
                return;

            Rectangle _bounds = CurrentFrame.Bounds;

            if (useDestinationBounds)
                _bounds = destinationBounds;
            
            spriteBatch.Draw(
                   texture: CurrentSprite.Texture,
                   position: Transform.Position,
                   sourceRectangle: _bounds,
                   color: Transform.Color,
                   rotation: Transform.Rotation,
                   origin: drawOrigin,
                   scale: Transform.Scale,
                   effects: Transform.SpriteEffects,
                   layerDepth: Transform.LayerDepth
                   );

            //chama OnDraw
            base._Draw(gameTime, spriteBatch);
        }

        /// <summary>Define as propriedades Index e FrameIndex com o valor 0.</summary>
        public void Reset()
        {
            SpriteIndex = 0;
            FrameIndex = 0;
            elapsedGameTime = 0;
        }

        /// <summary>Adiciona sprites a animação.</summary>
        /// <param name="source">Lista contendo os caminhos das texturas na pasta Content.</param>
        public void AddSprites(params string[] sources)
        {
            List<Sprite> tmpSprites = new List<Sprite>();

            foreach(string s in sources)
            {
                Sprite temp = new Sprite(Game, "", Game.Content.Load<Texture2D>(s), true);
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
                CurrentFrame = CurrentSprite[FrameIndex];
            }
        }

        public void AddSprite(Sprite source, string boxesGroupName)
        {
            Sprite s = new Sprite(source);
            s.Boxes.Clear();

            var list = source.Boxes.GetByGroup(boxesGroupName);

            for (int i = 0; i < list.Count; i++)
            {
                s.Boxes.Add(new BoxCollection(list[i]));

                //BoxCollection boxes = list[i];
                //SpriteFrame sf = boxes.SpriteFrame;
                //var clist = new List<CollisionBox>();
                //var alist = new List<AttackBox>();
                //string group = "";

                //source.Boxes.Values[i].CollisionBoxes.ForEach(cb => clist.Add(cb));
                //source.Boxes.Values[i].AttackBoxes.ForEach(ab => alist.Add(ab));
                //group = source.Boxes.Values[i].Group;

                //s.Boxes.Add(new BoxCollection(group, sf, clist, alist));
            }

            AddSprites(s);
        }

        /// <summary>
        /// Obtém o conteúdo de cores da textura ativa definida pelo SpriteFrame ativo.
        /// Caso não houve mudança no SpriteFrame e no SpriteEffects retornará o último array Color.
        /// </summary>
        public override Color[] GetData()
        {
            SpriteFrame frame = CurrentFrame;

            Color[] colors = new Color[frame.Width * frame.Height];
            CurrentSprite.Texture.GetData(0, frame.Bounds, colors, 0, colors.Length);

            return GetData(frame.Bounds, colors, Transform.SpriteEffects);
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//
        bool disposed = false;
        
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Sprites.Clear();
                Sprites = null;
                CurrentSprite = null;                

                OnChangeFrameIndex = null;
                OnChangeIndex = null;
                OnEndAnimation = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}