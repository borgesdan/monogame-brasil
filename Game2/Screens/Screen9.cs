using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen9 : Screen
    {
        public Screen9(ScreenManager manager, string name) : base(manager: manager, name, true)
        {
        }

        public override void Load()
        {
            //O sprite do Mario
            Sprite sprite = new Sprite(Game, "mariosprite");

            //A lista de frames do sprite.
            List<SpriteFrame> run_frames = new List<SpriteFrame>();
            run_frames.Add(new SpriteFrame(412, 311, 36, 56));
            run_frames.Add(new SpriteFrame(492, 311, 36, 56));
            run_frames.Add(new SpriteFrame(572, 311, 36, 56));

            Animation animation = new Animation(Game, 100, "");
            animation.AddSprite(sprite, run_frames.ToArray());

            ScreenLayer layer = new ScreenLayer(this);
            layer.AddAnimation(animation);
            layer.InfinityX = true;            
            layer.InfinityY = true;

            AddBackLayer(layer);

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            var input = Manager.Input;

            if (input.Keyboard.IsDown(Keys.Right))
            {
                camera.Move(5, 0);
            }
            if (input.Keyboard.IsDown(Keys.Left))
            {
                camera.Move(-5, 0);
            }
            if (input.Keyboard.IsDown(Keys.Up))
            {
                camera.Move(0, -5);
            }
            if (input.Keyboard.IsDown(Keys.Down))
            {
                camera.Move(0, 5);
            }

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
    }
}