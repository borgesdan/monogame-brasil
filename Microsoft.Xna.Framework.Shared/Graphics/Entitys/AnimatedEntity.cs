// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

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

        /// <summary>Inicializa uma nova instância de AnimatedEntity copiando uma outra entidade.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        public AnimatedEntity(AnimatedEntity source) : base(source)
        {
            Animations = source.Animations;
            ActiveAnimation = source.ActiveAnimation;
        }

        /// <summary>Inicializa uma nova instância AnimatedEntity.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        public AnimatedEntity(Game game, string name) : base(game, name)
        {
        }

        /// <summary>Inicializa uma nova instância AnimatedEntity.</summary>
        /// <param name="screen">A tela que a entidade será associada.</param>
        /// <param name="name">O nome da entidade</param>
        public AnimatedEntity(Screen screen, string name) : base(screen, name) 
        {
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//
        
        /// <summary>Obtém uma animação através do seu nome.</summary>
        /// <param name="name">O nome da animação.</param>
        /// <returns>Retorna a primeira animação encontrada com esse nome.</returns>
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

        /// <summary>Atualiza a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {   
            //Se a entidade não estiver disponível ou não existir uma animação ativa, então não se prossegue com a atualização.
            if (!Enable.IsEnabled || ActiveAnimation == null)
                return;

            //Define que a entidade está dentro doslimites de desenho da tela
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
            ActiveAnimation.Color = Transform.Color;
            ActiveAnimation.SpriteEffect = Transform.SpriteEffect;
            ActiveAnimation.Rotation = Transform.Rotation;
            ActiveAnimation.Scale = Transform.Scale;
            ActiveAnimation.Position = Transform.Position;
            ActiveAnimation.LayerDepth = LayerDepth;
            ActiveAnimation.Origin = Origin;
            ActiveAnimation.DrawPercentage = DrawPercentage;

            //Update da animação ativa.
            ActiveAnimation?.Update(gameTime);            

            base.Update(gameTime);
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
        /// <param name="newAnimation">Um instância da classe Animation.</param>
        public void Add(Animation newAnimation)
        {
            Animations.Add(newAnimation);            

            if(Animations.Count == 1)
            {
                string name = Animations[0].Name;
                Change(name);                
                UpdateBounds();
            }
        }

        /// <summary>Adiciona uma lista de animações à entidade.</summary>
        /// <param name="animations">Uma lista de animações.</param>
        public void Add(params Animation[] animations)
        {
            foreach(var a in animations)
            {
                Add(a);
            }
        }

        /// <summary>Troca a animação ativa.</summary>
        /// <param name="name">Nome da próxima animação. A animação atual será resetada.</param>
        public void Change(string name) => Change(name, true);

        /// <summary>Change the current animation</summary>
        /// <param name="name">Nome da animação.</param>
        /// <param name="resetAnimation">True se a animação atual será resetada.</param>
        public void Change(string name, bool resetAnimation)
        {
            Animation tempAnimation = Find(name);
            
            if(resetAnimation)
                ActiveAnimation?.ResetIndex();

            ActiveAnimation = tempAnimation ?? throw new ArgumentException("Animação não encontrada com esse parâmetro", nameof(name));

            UpdateBounds();
        }

        /// <summary>Encontra uma animação pelo seu nome.</summary>
        /// <param name="name">O nome da animação a ser encontrada.</param>
        /// <returns>Retorna a primeira animação encontrada a partir do nome ou null se não existir.</returns>
        public Animation Find(string name)
        {
            Animation a = Animations.Find(x => x.Name.Equals(name));
            return a;
        }

        /// <summary>Encontra todas as animações que contenham esse nome.</summary>
        /// <param name="name">O nome a ser pesquisado.</param>
        /// <returns>Retorna uma lista de animações ou null se não for encontrada nenhuma.</returns>
        public List<Animation> FindAll(string name)
        {
            var anms = Animations.FindAll(x => x.Name.Contains(name));
            return anms;
        }

        /// <summary>Cria uma nova entidade definida como um retângulo.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        /// <param name="size">O tamanho do retângulo.</param>
        /// <param name="color">A cor do retângulo</param>
        /// <returns>Retorna uma entidade com uma animação com um sprite retangular.</returns>
        public static AnimatedEntity CreateRectangle(Game game, string name, Point size, Color color) => GetRectangle(game, name, size, color, null);

        /// <summary>Cria uma nova entidade definida como um retângulo.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        /// <param name="size">O tamanho do retângulo.</param>
        /// <param name="color">A cor do retângulo</param>
        /// <param name="screen">A tela em que a entidade será associada.</param>
        /// <returns>Retorna uma entidade com uma animação com um sprite retangular.</returns>
        public static AnimatedEntity GetRectangle(Game game, string name, Point size, Color color, Screen screen)
        {
            Texture2D texture = Sprite.GetRectangle(game, new Point(size.X, size.Y), color).Texture;
            Sprite sprite = new Sprite(texture, true);
            Animation animation = new Animation(game, 0, "default");
            animation.AddSprite(sprite);

            AnimatedEntity animatedEntity = new AnimatedEntity(game, name);
            animatedEntity.Add(animation);

            screen?.Add(animatedEntity);

            return animatedEntity;
        }        

        /// <summary>Atualiza os limites da entidade.</summary>
        public override void UpdateBounds()
        {
            //Atualiza o tamanho da entidade.

            float cbw, cbh;

            if(ActiveAnimation != null)
            {
                cbw = ActiveAnimation.Frame.Width * Transform.Scale.X;
                cbh = ActiveAnimation.Frame.Height * Transform.Scale.Y;
            }
            else
            {
                cbw = 0;
                cbh = 0;
            }
            
            Transform.Size = new Point((int)cbw, (int)cbh);

            //O tamanho da da entidade e sua posição.
            int x = (int)Transform.X;
            int y = (int)Transform.Y;
            int w = Transform.Width;
            int h = Transform.Height;

            Vector2 s_f_oc;

            if (ActiveAnimation != null && ActiveAnimation.CurrentSprite != null)
            {
                s_f_oc = ActiveAnimation.CurrentSprite.Frames[ActiveAnimation.FrameIndex].OriginCorrection;
            }
            else
            {
                s_f_oc = Vector2.Zero;
            }            

            int recX = (int)(x + (Origin.X + s_f_oc.X));
            int recY = (int)(y - (Origin.Y + s_f_oc.Y));

            Bounds = new Rectangle(recX, recY, w, h);            

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