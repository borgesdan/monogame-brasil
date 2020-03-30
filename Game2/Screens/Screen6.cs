using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Game2.Screens
{
    class Screen6 : Screen
    {
        public Screen6(ScreenManager manager, string name) : base(manager: manager, name, true)
        {
            BackgroundColor = Color.MediumVioletRed;
        }

        public override void Load()
        {
            Animation yoshi = new Animation(Game, 100, "yoshi");

            Sprite lowering = new Sprite(Game, "yoshi");
            lowering.AddFrame
                (
                new SpriteFrame(0, 0, 128, 112),
                new SpriteFrame(147, 0, 120, 112),
                new SpriteFrame(288, 16, 120, 96),
                new SpriteFrame(428, 20, 120, 92),
                new SpriteFrame(428, 20, 120, 92),
                new SpriteFrame(428, 20, 120, 92)
                );

            yoshi.AddSprite(lowering);

            AnimatedEntity entity1 = new AnimatedEntity(this, "entity1");
            entity1.AddAnimation(yoshi);            
            entity1.Transform.SetViewPosition(AlignType.Left);
            entity1.Transform.Move(100, 0);

            Animation yoshi2 = new Animation(Game, yoshi);            
            var frames = yoshi2[0].Frames.ToArray();
            frames[0].OriginCorrection = Vector2.Zero;
            frames[1].OriginCorrection = Vector2.Zero;
            frames[2].OriginCorrection = new Vector2(0, -16);
            frames[3].OriginCorrection = new Vector2(0, -20);
            frames[4].OriginCorrection = new Vector2(0, -20);
            frames[5].OriginCorrection = new Vector2(0, -20);

            yoshi2[0].Frames = frames.ToList();            

            AnimatedEntity entity2 = new AnimatedEntity(this, "entity2");
            entity2.AddAnimation(yoshi2);
            entity2.Transform.SetViewPosition(AlignType.Right);
            entity2.Transform.Move(-100, 0);

            AnimatedEntity line1x = AnimatedEntity.CreateRectangle(this, nameof(line1x), new Point(2, this.Viewport.Height), Color.White);
            line1x.Transform.X = entity1.Transform.X;

            AnimatedEntity line2x = AnimatedEntity.CreateRectangle(this, nameof(line2x), new Point(2, this.Viewport.Height), Color.White);
            line2x.Transform.X = entity2.Transform.X;

            AnimatedEntity liney = AnimatedEntity.CreateRectangle(this, nameof(liney), new Point(this.Viewport.Width, 2), Color.White);
            liney.Transform.Y = entity1.Transform.Y;

            AnimatedEntity liney2 = AnimatedEntity.CreateRectangle(this, nameof(liney2), new Point(this.Viewport.Width, 2), Color.White);
            liney2.Transform.Y = entity1.Transform.Y + 112;

            //O texto informativo.
            TextEntity txtEntity = new TextEntity(this, nameof(txtEntity));
            txtEntity.SetFont("default");
            txtEntity.Text.Append("6: Teste de correção da origem da animação no eixo Y.");
            txtEntity.Transform.Color = Color.White;

            base.Load();
        }

        //Sobrecarga do método Update da tela.
        public override void Update(GameTime gameTime)
        {
            var input = Manager.Input;

            //Mudar para próxima tela da lista
            if (input.Keyboard.IsPress(Keys.Space))
            {
                Manager.Next(true);

                //Poderia usar também o método Change() informando o nome da tela.
            }
            if (input.Keyboard.IsPress(Keys.Back))
            {
                Manager.Back(true);

                //Poderia usar também o método Change() informando o nome da tela.
            }


            base.Update(gameTime);
        }

        //Sobrecarga do método Reset
        public override void Reset()
        {
            base.Reset();
        }
    }
}