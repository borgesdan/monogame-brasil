﻿// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma entidade com animações.</summary>
    public class AnimatedActor : Actor
    {
        //---------------------------------------//
        //-----         VARIÁVEIS           -----//
        //---------------------------------------//                
        private Vector2 percentage = Vector2.One;

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
        /// <summary>Obtém ou define a porcentagem de largura e altura do desenho. De 0f (0%) a 1f (100%).</summary>
        public Vector2 DrawPercentage
        {
            get => percentage;
            set
            {
                float x = MathHelper.Clamp(value.X, 0f, 1f);
                float y = MathHelper.Clamp(value.Y, 0f, 1f);

                percentage = new Vector2(x, y);
            }
        }
        /// <summary>Obtém ou define a porcentagem de largura do desenho. De 0f (0%) a 1f (100%).</summary>
        public float XDraw { get => DrawPercentage.X; set => DrawPercentage = new Vector2(value, YDraw); }
        /// <summary>Obtém ou define a porcentagem de altura do desenho. De 0f (0%) a 1f (100%).</summary>
        public float YDraw { get => DrawPercentage.Y; set => DrawPercentage = new Vector2(XDraw, value); }


        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de AnimatedActor como cópia de outro AnimatedEntity.</summary>
        /// <param name="source">o ator a ser copiado.</param>
        public AnimatedActor(AnimatedActor source) : base(source)
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

        /// <summary>Inicializa uma nova instância de AnimatedActor.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="name">O nome do ator.</param>
        public AnimatedActor(Game game, string name) : base(game, name)
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

        /// <summary>Atualiza o ator.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {   
            //Se a entidade não estiver disponível ou não existir uma animação ativa, então não se prossegue com a atualização.
            if (!Enable.IsEnabled || CurrentAnimation == null)
                return;

            if (!UpdateOffView && !CheckOffView())
                return;

            //Coloca OldPosition e Position com os mesmos valores.
            Transform.SetPosition(Transform.Position);

            //Seta as propriedades
            SetCurrentProperties();

            //Update da animação ativa.
            CurrentAnimation?.Update(gameTime);            

            base.Update(gameTime);
        }

        //Define as propridades da entidade para as propriedades da animação corrente.
        private void SetCurrentProperties()
        {
            CurrentAnimation.Transform.Set(Transform);
        }

        /// <summary>Desenha o ator.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Se a entidade não é visível
            //Se não existe uma animação ativa            
            if (!Enable.IsVisible || CurrentAnimation == null)
                return;            

            if(XRepeat == 0 && YRepeat == 0)
                CurrentAnimation?.Draw(gameTime, spriteBatch);
            else
            {
                Vector2 position = Transform.Position;

                for (int i = 0; i <= XRepeat; i++)
                {
                    Camera camera = new Camera(Game);

                    if (Screen != null)
                        camera = Screen.Camera;

                    var windowWidth = Game.Window.ClientBounds.Width;

                    CurrentAnimation.Transform.Position = position;

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

                        position.Y += CurrentAnimation.Transform.ScaledSize.Y;
                        CurrentAnimation.Transform.Position = position;

                        if (CurrentAnimation.Bounds.Bottom > camera.Y)
                        {
                            if (CurrentAnimation.Bounds.Y < camera.Y + windowHeight)
                                CurrentAnimation?.Draw(gameTime, spriteBatch);
                            else
                                break;
                        }                        
                    }

                    position.X += CurrentAnimation.Transform.ScaledSize.X;
                    position.Y = Transform.Y;
                }                
            }

            base._Draw(gameTime, spriteBatch);                    
        }        

        /// <summary>Adiciona uma nova animação ao ator.</summary>
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

        /// <summary>Adiciona uma lista de animações ao ator.</summary>
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

        /// <summary>Cria uma nova instância de AnimatedActor definida como um retângulo preenchido com uma cor definida.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        /// <param name="size">O tamanho do retângulo.</param>
        /// <param name="color">A cor do retângulo</param>
        public static AnimatedActor CreateRectangle(Game game, string name, Point size, Color color)
        {
            //Texture2D texture = Sprite.GetRectangle(game, name, new Point(size.X, size.Y), color).Texture;
            //Sprite sprite = new Sprite(game, name, texture, true);
            Sprite sprite = Sprite.GetRectangle(game, name, new Point(size.X, size.Y), color);
            Animation animation = new Animation(game, "default", 0);
            animation.AddSprites(sprite);

            AnimatedActor animatedEntity = new AnimatedActor(game, name);            
            animatedEntity.AddAnimation(animation);

            return animatedEntity;
        }

        /// <summary>
        /// Cria uma nova instância de AnimatedActor definida como um retângulo transparente mas com bordas visíveis.
        /// </summary>
        /// <param name="game">A instância da classe Game.</param>
        /// <param name="name">O nome da entidade.</param>
        /// <param name="size">O tamanho do retângulo</param>
        /// <param name="borderWidth">O tamanho da borda.</param>
        /// <param name="borderColor">A cor da borda.</param>
        public static AnimatedActor CreateRectangle2(Game game, string name, Point size, int borderWidth, Color borderColor)
        {
            //Texture2D texture = Sprite.GetRectangle2(game, name, new Point(size.X, size.Y), borderWidth, borderColor).Texture;
            //Sprite sprite = new Sprite(game, name, texture, true);
            Sprite sprite = Sprite.GetRectangle2(game, name, new Point(size.X, size.Y), borderWidth, borderColor);
            Animation animation = new Animation(game, "default", 0);
            animation.AddSprites(sprite);

            AnimatedActor animatedEntity = new AnimatedActor(game, name);            
            animatedEntity.AddAnimation(animation);

            return animatedEntity;
        }

        /// <summary>Atualiza os limites da entidade.</summary>
        public override void UpdateBounds()
        {
            //Atualiza o tamanho da entidade.

            //o tamanho do frame.
            int cbw = 0;
            int cbh = 0;

            if(CurrentAnimation != null)
            {
                //cbw = ActiveAnimation.Frame.Width * Transform.Scale.X;
                //cbh = ActiveAnimation.Frame.Height * Transform.Scale.Y;

                cbw = CurrentAnimation.CurrentFrame.Width;
                cbh = CurrentAnimation.CurrentFrame.Height;

                if (XRepeat > 0)
                    cbw *= XRepeat + 1;
                if (YRepeat > 0)
                    cbh *= YRepeat + 1;
            }

            Transform.Size = new Point(cbw, cbh);

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
                s_f_oc = CurrentAnimation.CurrentSprite.Boxes[CurrentAnimation.FrameIndex].SpriteFrame.Align;
            }
            else
            {
                s_f_oc = Vector2.Zero;
            }

            //A soma de todas as origens.
            var totalOrigin = ((Transform.Origin + s_f_oc) * Transform.Scale);

            int recX = (int)(x - totalOrigin.X);
            int recY = (int)(y - totalOrigin.Y);

            Bounds = new Rectangle(recX, recY, w, h);            
            
            //Adição dos boxes de colisão e ataque
            CollisionBoxes.Clear();
            AttackBoxes.Clear();

            foreach(CollisionBox cb in CurrentAnimation.CollisionBoxesList)
            {
                CollisionBox relative = cb.GetRelativePosition(CurrentAnimation.CurrentFrame.Bounds, Bounds, Transform.Scale, Transform.SpriteEffects); ;
                CollisionBoxes.Add(relative);
            }

            foreach (AttackBox ab in CurrentAnimation.AttackBoxesList)
            {
                AttackBox relative = ab.GetRelativePosition(CurrentAnimation.CurrentFrame.Bounds, Bounds, Transform.Scale, Transform.SpriteEffects);
                AttackBoxes.Add(relative);
            }

            //Criação do polígono (BoundsR).
            BoundsR = Util.CreateRotatedBounds(Transform, totalOrigin, Bounds);

            base.UpdateBounds();
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