// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

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
        public Animation ActiveAnimation { get; private set; } = null;
        /// <summary>Obtém o nome da animação ativa.</summary>
        public string ActiveName { get => ActiveAnimation.Name; }        

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
            int index = source.Animations.FindIndex(a => a.Equals(source.ActiveAnimation));

            ActiveAnimation = Animations[index];
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
            get
            {
                return Find(name);
            }
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
            if (!Enable.IsEnabled || ActiveAnimation == null)
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

                        return;
                    }
                }
            }

            //Coloca OldPosition e Position com os mesmos valores.
            Transform.SetPosition(Transform.Position);

            //Seta as propriedades
            SetActiveProperties();

            //Update da animação ativa.
            ActiveAnimation?.Update(gameTime);            

            base.Update(gameTime);
        }

        private void SetActiveProperties()
        {
            ActiveAnimation.Color = Transform.Color;
            ActiveAnimation.SpriteEffect = Transform.SpriteEffect;
            ActiveAnimation.Rotation = Transform.Rotation;
            ActiveAnimation.Scale = Transform.Scale;
            ActiveAnimation.Position = Transform.Position;
            ActiveAnimation.LayerDepth = LayerDepth;
            ActiveAnimation.Origin = Origin;
            ActiveAnimation.DrawPercentage = DrawPercentage;
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Se a entidade não é visível
            //Se não existe uma animação ativa
            //Ou se a entidade se encontra fora dos limites da tela, não prossegue com a execução do método.            
            if (!Enable.IsVisible || ActiveAnimation == null || outOfView)
                return;            

            ActiveAnimation?.Draw(gameTime, spriteBatch);  
            
            base.Draw(gameTime, spriteBatch);
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
            Animation tempAnimation = Find(name);
            
            if(resetAnimation)
                ActiveAnimation?.Reset();

            ActiveAnimation = tempAnimation ?? throw new ArgumentException("Animação não encontrada com esse parâmetro", nameof(name));
            SetActiveProperties();

            UpdateBounds();
        }

        /// <summary>Encontra uma animação pelo seu nome.</summary>
        /// <param name="name">O nome da animação a ser encontrada.</param>
        public Animation Find(string name)
        {
            Animation a = Animations.Find(x => x.Name.Equals(name));
            return a;
        }

        /// <summary>Encontra todas as animações que contenham o nome especificado.</summary>
        /// <param name="name">O nome a ser pesquisado.</param>
        public List<Animation> FindAll(string name)
        {
            var anms = Animations.FindAll(x => x.Name.Contains(name));
            return anms;
        }

        /// <summary>Cria uma nova instância de AnimatedEntity definida como um retângulo.</summary>
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

        /// <summary>Atualiza os limites da entidade.</summary>
        public override void UpdateBounds()
        {
            //Atualiza o tamanho da entidade.

            //o tamanho do frame.
            float cbw = 0;
            float cbh = 0;

            if(ActiveAnimation != null)
            {
                cbw = ActiveAnimation.Frame.Width * Transform.Scale.X;
                cbh = ActiveAnimation.Frame.Height * Transform.Scale.Y;
            }
            
            Transform.Size = new Point((int)cbw, (int)cbh);

            //O tamanho da entidade e sua posição.
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            int w = Transform.Width;
            int h = Transform.Height;

            //A origem do frame.
            Vector2 s_f_oc;

            if (ActiveAnimation != null && ActiveAnimation.CurrentSprite != null)
            {
                s_f_oc = ActiveAnimation.CurrentSprite.Frames[ActiveAnimation.FrameIndex].OriginCorrection;
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
                ActiveAnimation = null;
            }

            base.Dispose(disposing);
        }
    }
}