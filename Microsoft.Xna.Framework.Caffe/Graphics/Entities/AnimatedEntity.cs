//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma entidade com animações.</summary>
    public class AnimatedEntity : Entity2D
    {
        private bool outOfView = false;

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a lista de animações</summary>
        public List<Animation> Animations { get; set; } = new List<Animation>();
        /// <summary>Obtém a animação ativa.</summary>
        public Animation CurrentAnimation { get; private set; } = null;
        /// <summary>Obtém o nome da animação ativa.</summary>
        public string CurrentName { get => CurrentAnimation.Name; }
        /// <summary>Obtém ou define o número de vezes em que esta entidade será desenhada na tela no eixo X. Esta propriedade afeta no cálculo do tamanho no método UpdateBounds().</summary>
        public int XRepeat { get; set; } = 0;
        /// <summary>Obtém ou define o número de vezes em que esta entidade será desenhada na tela no eixo Y. Esta propriedade afeta no cálculo do tamanho no método UpdateBounds().</summary>
        public int YRepeat { get; set; } = 0;
        /// <summary>Obtém as caixas de colisão do atual frame.</summary>
        public List<CollisionBox> CollisionBoxes { get; private set; } = new List<CollisionBox>();
        /// <summary>Obtém as caixas de ataque do atual frame.</summary>
        public List<AttackBox> AttackBoxes { get; private set; } = new List<AttackBox>();


        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de AnimatedEntity como cópia de outro AnimatedEntity.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        public AnimatedEntity(AnimatedEntity source) : base(source)
        {
            //Cópia das animações.
            source.Animations.ForEach(a => this.Animations.Add(new Animation(a)));
            //Busca do index da animação ativa.
            int index = source.Animations.FindIndex(a => a.Equals(source.CurrentAnimation));

            CurrentAnimation = Animations[index];

            XRepeat = source.XRepeat;
            YRepeat = source.YRepeat;

            source.CollisionBoxes.ForEach(cb => CollisionBoxes.Add(cb));
            source.AttackBoxes.ForEach(ab => AttackBoxes.Add(ab));
        }

        /// <summary>Inicializa uma nova instância de AnimatedEntity.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        public AnimatedEntity(Game game, string name) : base(game, name)
        {
        }              

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//

        /// <summary>Obtém uma animação através do seu nome.</summary>
        /// <param name="name">O nome da animação.</param>
        public Animation this[string name]
        {
            get => GetAnimation(name);
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//        

        /// <summary>
        /// Cria uma nova instância de AnimatedEntity quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">A entidade a ser copiada.</param>
        public override T Clone<T>(T source)
        {
            if (source is AnimatedEntity)
                return (T)Activator.CreateInstance(typeof(AnimatedEntity), source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {   
            //Se a entidade não estiver disponível ou não existir uma animação ativa, então não se prossegue com a atualização.
            if (!Enable.IsEnabled || CurrentAnimation == null)
                return;

            //Define que a entidade está dentro dos limites de desenho da tela
            outOfView = false;

            //Se UpdateOutOfView é false, então é necessário saber se a entidade está dentro dos limites de desenho da tela.
            if (!UpdateOutOfView)
            {
                if(Screen != null)
                {
                    if (!Util.CheckFieldOfView(Screen, Bounds))
                    {
                        //Se o resultado for false, definimos 'outOfView' como true para verificação no método Draw.
                        outOfView = true;
                        //return;
                    }
                }
            }

            //Coloca OldPosition e Position com os mesmos valores.
            Transform.SetPosition(Transform.Position);

            //Seta as propriedades
            SetActiveProperties();

            //Update da animação ativa.
            CurrentAnimation?.Update(gameTime);            

            base.Update(gameTime);
        }

        private void SetActiveProperties()
        {
            CurrentAnimation.SetProperties(this);
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Se a entidade não é visível
            //Se não existe uma animação ativa
            //Ou se a entidade se encontra fora dos limites da tela, não prossegue com a execução do método.            
            if (!Enable.IsVisible || CurrentAnimation == null || outOfView)
                return;            

            if(XRepeat == 0 && YRepeat == 0)
                CurrentAnimation?.Draw(gameTime, spriteBatch);
            else
            {
                Vector2 position = Transform.Position;

                for (int i = 0; i <= XRepeat; i++)
                {
                    Camera camera = Camera.Create();

                    if (Screen != null)
                        camera = Screen.Camera;

                    var windowWidth = Game.Window.ClientBounds.Width;

                    CurrentAnimation.Position = position;

                    if (CurrentAnimation.Bounds.Right > camera.X)
                    {
                        if (CurrentAnimation.Bounds.X < camera.X + windowWidth)
                            CurrentAnimation?.Draw(gameTime, spriteBatch);
                        else
                            break;
                    }

                    for (int j = 0; j < YRepeat; j++)
                    {
                        var windowHeight = Game.Window.ClientBounds.Height;

                        position.Y += CurrentAnimation.ScaledSize.Y;
                        CurrentAnimation.Position = position;

                        if (CurrentAnimation.Bounds.Bottom > camera.Y)
                        {
                            if (CurrentAnimation.Bounds.Y < camera.Y + windowHeight)
                                CurrentAnimation?.Draw(gameTime, spriteBatch);
                            else
                                break;
                        }                        
                    }

                    position.X += CurrentAnimation.ScaledSize.X;
                    position.Y = Transform.Y;
                }                
            }

            base.Draw(gameTime, spriteBatch);

            if (DEBUG.IsEnabled && Screen != null)
            {
                if(DEBUG.ShowCollisionBox)
                {
                    foreach(CollisionBox cb in CollisionBoxes)
                    {
                        Screen.DebugPolygons.Add(new Tuple<Polygon, Color>(new Polygon(cb.Bounds), DEBUG.CollisionBoxColor));
                    }
                }

                if (DEBUG.ShowAttackBox)
                {
                    foreach (AttackBox ab in AttackBoxes)
                    {
                        Screen.DebugPolygons.Add(new Tuple<Polygon, Color>(new Polygon(ab.Bounds), DEBUG.AttackBoxColor));
                    }
                }
            }            
        }        

        /// <summary>Adiciona uma nova animação à entidade.</summary>
        /// <param name="animation">Um instância da classe Animation.</param>
        public void AddAnimation(Animation animation)
        {
            Animations.Add(animation);            

            if(Animations.Count == 1)
            {
                string name = Animations[0].Name;
                ChangeAnimation(name);                
                UpdateBounds();
            }
        }

        /// <summary>Adiciona uma lista de animações à entidade.</summary>
        /// <param name="animations">Uma lista de animações.</param>
        public void AddAnimations(params Animation[] animations)
        {
            foreach(var a in animations)
            {
                AddAnimation(a);
            }
        }

        /// <summary>Troca a animação ativa.</summary>
        /// <param name="name">Nome da próxima animação. A animação atual será resetada.</param>
        public void ChangeAnimation(string name) => ChangeAnimation(name, true);

        /// <summary>Troca a animação ativa.</summary>
        /// <param name="name">Nome da próxima animação.</param>
        /// <param name="resetAnimation">True se a animação atual será resetada.</param>
        public void ChangeAnimation(string name, bool resetAnimation)
        {
            Animation tempAnimation = GetAnimation(name);
            
            if(resetAnimation)
                CurrentAnimation?.Reset();

            CurrentAnimation = tempAnimation ?? throw new ArgumentException("Animação não encontrada com esse parâmetro", nameof(name));
            SetActiveProperties();

            UpdateBounds();
        }

        /// <summary>Encontra uma animação pelo seu nome.</summary>
        /// <param name="name">O nome da animação a ser encontrada.</param>
        public Animation GetAnimation(string name)
        {
            Animation a = Animations.Find(x => x.Name.Equals(name));
            return a;
        }

        /// <summary>Encontra todas as animações que contenham o nome especificado.</summary>
        /// <param name="name">O nome a ser pesquisado.</param>
        public List<Animation> GetAllAnimations(string name)
        {
            var anms = Animations.FindAll(x => x.Name.Contains(name));
            return anms;
        }

        /// <summary>Cria uma nova instância de AnimatedEntity definida como um retângulo preenchido com uma cor definida.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        /// <param name="size">O tamanho do retângulo.</param>
        /// <param name="color">A cor do retângulo</param>
        public static AnimatedEntity CreateRectangle(Game game, string name, Point size, Color color)
        {
            Texture2D texture = Sprite.GetRectangle(game, new Point(size.X, size.Y), color).Texture;
            Sprite sprite = new Sprite(texture, true);
            Animation animation = new Animation(game, 0, "default");
            animation.AddSprites(sprite);

            AnimatedEntity animatedEntity = new AnimatedEntity(game, name);
            animatedEntity.AddAnimation(animation);

            return animatedEntity;
        }

        /// <summary>
        /// Cria uma nova instância de AnimatedEntity definida como um retângulo transparente mas com bordas visíveis.
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        /// <param name="size">O tamanho do retângulo</param>
        /// <param name="borderWidth">O tamanho da borda.</param>
        /// <param name="borderColor">A cor da borda.</param>
        public static AnimatedEntity CreateRectangle2(Game game, string name, Point size, int borderWidth, Color borderColor)
        {
            Texture2D texture = Sprite.GetRectangle2(game, new Point(size.X, size.Y), borderWidth, borderColor).Texture;
            Sprite sprite = new Sprite(texture, true);
            Animation animation = new Animation(game, 0, "default");
            animation.AddSprites(sprite);

            AnimatedEntity animatedEntity = new AnimatedEntity(game, name);
            animatedEntity.AddAnimation(animation);

            return animatedEntity;
        }

        /// <summary>Atualiza os limites da entidade.</summary>
        public override void UpdateBounds()
        {
            //Atualiza o tamanho da entidade.

            //o tamanho do frame.
            float cbw = 0;
            float cbh = 0;

            if(CurrentAnimation != null)
            {
                //cbw = ActiveAnimation.Frame.Width * Transform.Scale.X;
                //cbh = ActiveAnimation.Frame.Height * Transform.Scale.Y;

                cbw = CurrentAnimation.Frame.Width;
                cbh = CurrentAnimation.Frame.Height;

                if (XRepeat > 0)
                    cbw *= XRepeat + 1;
                if (YRepeat > 0)
                    cbh *= YRepeat + 1;
            }

            Transform.Size = new Point((int)cbw, (int)cbh);

            //O tamanho da entidade e sua posição.
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            //int w = Transform.Width;
            //int h = Transform.Height;
            int w = (int)Transform.ScaledSize.X;
            int h = (int)Transform.ScaledSize.Y;

            //A origem do frame.
            Vector2 s_f_oc;

            if (CurrentAnimation != null && CurrentAnimation.CurrentSprite != null)
            {
                s_f_oc = CurrentAnimation.CurrentSprite.Frames[CurrentAnimation.FrameIndex].OriginCorrection;
            }
            else
            {
                s_f_oc = Vector2.Zero;
            }

            //A soma de todas as origens.
            var totalOrigin = ((Origin + s_f_oc) * Transform.Scale);

            int recX = (int)(x - totalOrigin.X);
            int recY = (int)(y - totalOrigin.Y);

            Bounds = new Rectangle(recX, recY, w, h);
            
            //Adição dos boxes de colisão e ataque
            CollisionBoxes.Clear();
            AttackBoxes.Clear();

            foreach(CollisionBox cb in CurrentAnimation.CollisionBoxesList)
            {
                CollisionBox relative = cb.GetRelativePosition(CurrentAnimation.Frame, Bounds, Transform.Scale, Transform.SpriteEffect);
                CollisionBoxes.Add(relative);
            }

            foreach (AttackBox ab in CurrentAnimation.AttackBoxesList)
            {
                AttackBox relative = ab.GetRelativePosition(CurrentAnimation.Frame, Bounds, Transform.Scale, Transform.SpriteEffect);
                AttackBoxes.Add(relative);
            }

            //Criação do polígono (BoundsR).
            Util.CreateBoundsR(this, totalOrigin, Bounds);

            base.UpdateBounds();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Animations.Clear();
                Animations = null;
                CurrentAnimation = null;
            }

            base.Dispose(disposing);
        }
    }
}