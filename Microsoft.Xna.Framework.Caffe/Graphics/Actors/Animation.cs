// Danilo Borges Santos, 2020.

using System.Collections.Generic;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Ator que representa uma animação de sprites.</summary>
    public class Animation : Actor
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//

        int _index = 0;
        int _frameIndex = 0;
        int time = 0;
        int elapsedGameTime = 0;   
        Vector2 drawOrigin = Vector2.Zero;  

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        
        /// <summary>Obtém ou define a lista de sprites.</summary>
        public List<Sprite> Sprites { get; set; } = new List<Sprite>();
        /// <summary>Obtém ou define em milisegundos o tempo de exibição de cada frame do sprite.</summary>
        public int Time 
        {
            get => time;
            set => time = Math.Abs(value);
        }
        /// <summary>Obtém o index atual do Sprite corrente.</summary>
        public int CurrentSpriteIndex 
        {
            get => _index;
            protected set
            {
                _index = value;
                OnChangeIndex?.Invoke(this);
            }
        }
        /// <summary>Obtém a posição do frame do Sprite corrente.</summary>
        public int CurrentFrameIndex 
        {
            get => _frameIndex;
            protected set
            {
                _frameIndex = value;
                OnChangeFrameIndex?.Invoke(this);
            }
        }
        
        /// <summary>Obtém o frame atual da animação.</summary>
        public SpriteFrame CurrentFrame
        { 
            get 
            {
                if (CurrentSprite != null)
                    return CurrentSprite[CurrentFrameIndex];
                else
                    return SpriteFrame.Empty;
            }
        }
        /// <summary>Obtém o Sprite corrente.</summary>
        public Sprite CurrentSprite { get; protected set; } = null;
        /// <summary>Obtém as caixas de colisão do frame corrente.</summary>
        public List<CollisionBox> CollisionBoxesList { get; private set; } = new List<CollisionBox>();
        /// <summary>Obtém as caixas de ataque do frame corrente.</summary>
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

                for (int i = 0; i <= CurrentSpriteIndex; i++)
                {
                    for(int f = 0; f <= Sprites[i].Boxes.Count - 1; f++)
                    {
                        if (i == CurrentSpriteIndex && f > CurrentFrameIndex)
                            break;

                        count++;
                    }
                }

                var m = Time * count;
                return new TimeSpan(0, 0, 0, 0, m);
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
        /// <param name="name">O nome da animação.</param>
        /// <param name="time">O tempo de cada quadro da animação.</param>
        /// <param name="sprite">O sprite a ser utilizado.</param>
        /// <param name="frameGroupName">O nome do grupo de frames a serem utilizados na animação.</param>
        public Animation(Game game, string name, int time, Sprite sprite, string frameGroupName) : this(game, name, time)
        {
            AddSprite(sprite, frameGroupName);
        }

        /// <summary>Inicializa uma nova instância da classe Animation como cópia de outro Animation.</summary>
        /// <param name="source">A animação a ser copiada.</param>
        public Animation(Animation source) : base(source)
        {
            elapsedGameTime = source.elapsedGameTime;
            CurrentSpriteIndex = source.CurrentSpriteIndex;
            CurrentFrameIndex = source.CurrentFrameIndex;            
            source.Sprites.ForEach(s => Sprites.Add(new Sprite(s)));

            int cs_index = source.Sprites.FindIndex(i => i == source.CurrentSprite);           
            CurrentSprite = Sprites[cs_index];

            Time = source.Time;
            //CurrentFrame = source.CurrentFrame;

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
        
        protected override void _Update(GameTime gameTime)
        {
            //Atualiza a animação.
            Animate(gameTime);                    

            //Atualiza o Transform
            Transform.Update();

            //Atualiza as caixas de colisão.
            SetBoxes();            
        }
        
        private void SetBoxes()
        {
            CollisionBoxesList.Clear();
            AttackBoxesList.Clear();

            if(CurrentSprite.Boxes[CurrentFrameIndex].CollisionBoxes != null)
                CollisionBoxesList = CurrentSprite.Boxes[CurrentFrameIndex].CollisionBoxes;

            if(CurrentSprite.Boxes[CurrentFrameIndex].AttackBoxes != null)
                AttackBoxesList = CurrentSprite.Boxes[CurrentFrameIndex].AttackBoxes;
        }        

        public override void UpdateBounds()
        {
            SpriteFrame frame = SpriteFrame.Empty;
            Point size = Point.Zero;

            if(CurrentFrame != null)
            {
                size = CurrentFrame.Bounds.Size;
                frame = CurrentFrame;
            }           

            CalcBounds(size.X, size.Y, frame.AlignX, frame.AlignY);
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
                    if (CurrentFrameIndex >= Sprites[CurrentSpriteIndex].Boxes.Count - 1)
                    {
                        //Se sim, é hora de pular de sprite ou voltar para o primeiro frame,
                        //Caso só tenhamos uma sprite

                        if (CurrentSpriteIndex >= Sprites.Count - 1)
                        {
                            CurrentSpriteIndex = 0;
                        }                            
                        else
                        {
                            CurrentSpriteIndex++;
                        }                            
                        
                        CurrentFrameIndex = 0;
                    }
                    else
                    {
                        CurrentFrameIndex++;
                    }                        

                    //Reseta o tempo.
                    elapsedGameTime = 0;
                    //Atualiza o sprite atual.
                    CurrentSprite = Sprites[CurrentSpriteIndex];                    
                }

                //CurrentFrame = CurrentSprite[CurrentFrameIndex];
            }

            if(IsFinished)
            {
                OnEndAnimation?.Invoke(this);
            }
        }        

        /// <summary>Avança um sprite da animação.</summary>
        public void ForwardIndex()
        {
            CurrentSpriteIndex++;

            if (CurrentSpriteIndex >= Sprites.Count - 1)
                CurrentSpriteIndex = 0;

            CurrentSprite = Sprites[CurrentSpriteIndex];
        }

        /// <summary>Avança um Frame do atual sprite da animação.</summary>
        public void ForwardFrameIndex()
        {
            CurrentFrameIndex++;

            if (CurrentFrameIndex >= Sprites[CurrentSpriteIndex].Boxes.Count - 1)
                CurrentFrameIndex = 0;
        }
        
        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (CurrentSprite != null)
            {
                spriteBatch.Draw(
                   texture: CurrentSprite.Texture,
                   position: Transform.Position,
                   sourceRectangle: CurrentFrame.Bounds,
                   color: Transform.Color,
                   rotation: Transform.Rotation,
                   origin: drawOrigin,
                   scale: Transform.Scale,
                   effects: Transform.SpriteEffects,
                   layerDepth: Transform.LayerDepth
                   );
            }           
            
        }

        /// <summary>Define as propriedades Index e FrameIndex com o valor 0.</summary>
        public void Reset()
        {
            CurrentSpriteIndex = 0;
            CurrentFrameIndex = 0;
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
                //CurrentFrame = CurrentSprite[CurrentFrameIndex];
                UpdateBounds();
            }
        }

        /// <summary>
        /// Adiciona um novo sprite à lista de sprites.
        /// </summary>
        /// <param name="source">O sprite a ser adicionado</param>
        /// <param name="boxesGroupName">O nome do grupo de frames a ser utilizado.</param>
        public void AddSprite(Sprite source, string boxesGroupName)
        {
            Sprite s = new Sprite(source);
            s.Boxes.Clear();

            var list = source.Boxes.GetByGroup(boxesGroupName);

            for (int i = 0; i < list.Count; i++)
                s.Boxes.Add(new BoxCollection(list[i]));

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