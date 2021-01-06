// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa um ator com um grupo de animações.</summary>
    public class AnimatedActor : Actor
    {
        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define a lista de animações</summary>
        public List<Animation> Animations { get; set; } = new List<Animation>();
        /// <summary>Obtém a animação ativa.</summary>
        public Animation CurrentAnimation { get; private set; } = null;
        /// <summary>Obtém o nome da animação ativa.</summary>
        public string CurrentName { get => CurrentAnimation.Name; }
        /// <summary>Obtém as caixas de colisão do atual frame da animação corrente.</summary>
        public List<CollisionBox> CollisionBoxes { get; private set; } = new List<CollisionBox>();
        /// <summary>Obtém as caixas de ataque do atual frame da animação corrente.</summary>
        public List<AttackBox> AttackBoxes { get; private set; } = new List<AttackBox>();


        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de AnimatedActor como cópia de outro AnimatedActor.</summary>
        /// <param name="source">o ator a ser copiado.</param>
        public AnimatedActor(AnimatedActor source) : base(source)
        {            
            source.Animations.ForEach(a => this.Animations.Add(new Animation(a)));
            //Busca do index da animação ativa.
            int index = source.Animations.FindIndex(a => a.Equals(source.CurrentAnimation));
            CurrentAnimation = Animations[index];

            source.CollisionBoxes.ForEach(cb => CollisionBoxes.Add(cb));
            source.AttackBoxes.ForEach(ab => AttackBoxes.Add(ab));
        }

        /// <summary>Inicializa uma nova instância de AnimatedActor.</summary>
        /// <param name="game">A instância corrente da classe Game.</param>
        /// <param name="name">O nome do ator.</param>
        public AnimatedActor(Game game, string name) : base(game, name) { }

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
        
        protected override void _Update(GameTime gameTime)
        {               
            if (CurrentAnimation != null)
            {
                //Coloca OldPosition e Position com os mesmos valores.
                //Transform.SetPosition(Transform.Position);

                //Seta as propriedades
                SetCurrentProperties();

                //Update da animação ativa.
                CurrentAnimation.Update(gameTime);
            }            
        }

        //Define as propridades da entidade para as propriedades da animação corrente.
        private void SetCurrentProperties()
        {
            CurrentAnimation.Transform.Set(Transform);
            CurrentAnimation.Screen = Screen;
        }
        
        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {    
            if (CurrentAnimation != null)
            {
                CurrentAnimation.Draw(gameTime, spriteBatch);
            }
        }        

        /// <summary>Adiciona uma nova animação ao ator.</summary>
        /// <param name="animation">Um instância da classe Animation.</param>
        public void AddAnimation(Animation animation)
        {
            Animations.Add(animation);            

            if(Animations.Count == 1)
            {
                ChangeAnimation(Animations[0], true); 
            }
        }

        /// <summary>Adiciona uma lista de animações ao ator.</summary>
        /// <param name="animations">A lista de animações.</param>
        public void AddAnimations(params Animation[] animations)
        {
            if(animations != null)
            {
                foreach (var a in animations)
                {
                    AddAnimation(a);
                }
            }            
        }

        /// <summary>Troca a animação ativa.</summary>
        /// <param name="animation">A instância da animação desejada.</param>
        /// <param name="resetAnimation">Defina True se deseja que a animação corrente será resetada.</param>
        public void ChangeAnimation(Animation animation, bool resetAnimation)
        {
            if(Animations.Contains(animation))
                ChangeAnimation(animation.Name, resetAnimation);
        }

        /// <summary>Troca a animação ativa.</summary>
        /// <param name="index">O index da animação na lista de animações.</param>
        /// <param name="resetAnimation">Defina True se deseja que a animação corrente será resetada.</param>
        public void ChangeAnimation(int index, bool resetAnimation)
        {            
            ChangeAnimation(Animations[index].Name, resetAnimation);
        }        

        /// <summary>Troca a animação ativa.</summary>
        /// <param name="name">Nome da próxima animação.</param>
        /// <param name="resetAnimation">Defina True se deseja que a animação corrente será resetada.</param>
        public void ChangeAnimation(string name, bool resetAnimation)
        {
            Animation tempAnimation = GetAnimation(name);
            
            if(resetAnimation)
                CurrentAnimation?.Reset();

            CurrentAnimation = tempAnimation ?? throw new ArgumentException("Animação não encontrada com esse parâmetro", nameof(name));
            SetCurrentProperties();

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

        public override void UpdateBounds()
        {
            Point size = Point.Zero;
            SpriteFrame frame = SpriteFrame.Empty;

            if (CurrentAnimation != null)
            {
                size = new Point(CurrentAnimation.CurrentFrame.Width, CurrentAnimation.CurrentFrame.Height);
                frame = CurrentAnimation.CurrentFrame;
            }

            CalcBounds(size.X, size.Y, frame.AlignX, frame.AlignY);
        }

        /// <summary>
        /// Obtém o conteúdo de cores da animação atual.        
        /// </summary>
        public override Color[] GetData()
        {
            return CurrentAnimation.GetData();
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
                Animations.Clear();
                Animations = null;
                CurrentAnimation = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}