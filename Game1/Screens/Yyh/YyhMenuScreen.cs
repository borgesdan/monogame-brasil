using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1.Screens
{
    class YyhMenuScreen : Screen
    {
        List<float> barPositions = new List<float>
        {
            -125,
            -90,
            -55,
            -20,
            17,
            53,
            88,
            123
        };

        int barIndex = 0;

        AnimatedEntity back;
        AnimatedEntity menu;
        AnimatedEntity bar;
        AnimatedEntity logo;
        TextEntity info;

        public YyhMenuScreen(SubScreenManager manager, string name) : base(subManager: manager, name: name, true)
        {
            BackgroundColor = Color.DarkGreen;
        }

        public override void Load()
        {
            back = new AnimatedEntity(this, nameof(back));
            Animation backAnm = new Animation(Game, 0, nameof(backAnm));
            backAnm.AddSprite("yyh/menu_back");
            back.Add(backAnm);            
            back.Transform.SetScale(0.6f, 0.6f);
            back.Transform.SetViewPosition(AlignType.Center);

            bar = new AnimatedEntity(this, nameof(bar));
            Animation barAnm = new Animation(Game, 200, nameof(barAnm));
            barAnm.AddSprite("yyh/menu_bar");
            barAnm.AddSprite("yyh/menu_bar");
            bar.Add(barAnm);
            bar.Transform.SetScale(0.6f, 0.6f);
            bar.Transform.SetViewPosition(AlignType.Center);
            bar.Transform.Y += barPositions[0];
            bar.OnUpdate += (Entity2D source, GameTime gameTime) =>
            {
                bar.Transform.SetViewPosition(AlignType.Center);
                bar.Transform.Y += barPositions[barIndex];

                var anm = (AnimatedEntity)source;

                anm.ActiveAnimation.OnEndAnimation += (Animation obj) =>
                {
                    if (anm.ActiveAnimation.Color == Color.White)
                        anm.ActiveAnimation.Color = Color.DarkSeaGreen;
                    else
                        anm.ActiveAnimation.Color = Color.White;
                };
            };

            menu = new AnimatedEntity(this, nameof(menu));
            Animation menuAnm = new Animation(Game, 0, nameof(menuAnm));
            menuAnm.AddSprite("yyh/menu");
            menu.Add(menuAnm);
            menu.Transform.SetScale(0.5f, 0.5f);
            menu.Transform.SetViewPosition(AlignType.Center);            

            logo = new AnimatedEntity(this, nameof(logo));
            Animation logoAnm = new Animation(Game, 20, nameof(logoAnm));
            logoAnm.AddSprite("yyh/logo_bottom");
            logo.Add(logoAnm);
            logo.Transform.SetScale(0.5f,0.5f);
            logo.Transform.SetViewPosition(AlignType.Bottom);
            logo.Transform.Y -= 10;

            info = new TextEntity(this, nameof(info));
            info.SetFont("default");
            info.Text.Append("Press A to change screen.");
            info.Transform.SetScale(1.5f, 1.5f);
            info.Transform.SetViewPosition(AlignType.LeftBottom);            

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            var input = SubManager.Input;

            if (input.Keyboard.IsPress(Keys.Down))
            {
                barIndex++;

                if (barIndex > barPositions.Count - 1)
                    barIndex = 0;
            }

            if (input.Keyboard.IsPress(Keys.Up))
            {
                barIndex--;

                if (barIndex < 0)
                    barIndex = barPositions.Count - 1;
            }           

            if (input.Keyboard.IsPress(Keys.A))
                SubManager.Change("pressStartScreen");

            base.Update(gameTime);
        }
    }
}