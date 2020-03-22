using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Game1.Screens
{
    //Tela de 'press start'
    class YyhPressStartScreen : Screen
    {
        //Tempo para contagem do trovão.
        float elapsedTime = 0;

        //Som de trovão
        //https://freesound.org/people/OneSoundToRuleThemAll/sounds/238797/
        SoundEffect thunder;

        //Entidades
        AnimatedEntity logo;
        AnimatedEntity pushStart;
        TextEntity info;

        public YyhPressStartScreen(SubScreenManager manager, string name) : base(subManager: manager, name: name, true)
        {
            //Carregamos a tela recebendo como parâmetro um subgerenciador de telas.

            //Definimos a cor de fundo.
            BackgroundColor = Color.Black;
        }

        public override void Load()
        {
            //Carregamos o som.
            thunder = Game.Content.Load<SoundEffect>("yyh/thunder");

            //Carregamos as entidades.

            //O logo.
            logo = new AnimatedEntity(this, "logo");
            //Criamos a animação do logo. O tempo é 0 porque é uma imagem estática.
            Animation logoAnm = new Animation(Game, 0, "logoAnm");            
            //Carregamos a textura da animação.
            logoAnm.AddSprite("yyh/logo");
            //Adicionamos a animação ao logo
            logo.Add(logoAnm);
            
            //Posicionamos o logo.
            logo.Transform.SetScale(0.5F, 0.5F);
            logo.Transform.SetViewPosition(AlignType.Top);
            logo.Transform.Y += 20;

            //O processo é o mesmo para as entidades restantes.

            pushStart = new AnimatedEntity(this, "pressStart");
            Animation pushAnm = new Animation(Game, 800, "pushAnm");
            pushAnm.AddSprite("yyh/push_start");
            pushAnm.AddSprite("yyh/push_start");
            pushStart.Add(pushAnm);            
            
            pushStart.Transform.SetScale(0.6f, 0.6F);
            pushStart.Transform.SetViewPosition(AlignType.Center);
            pushStart.Transform.Y += 50;
            pushStart.OnUpdate += (Entity2D source, GameTime gameTime) =>
            {
                //Definimos o evento OnUpdate para trocar a cor do press start sempre no fim da animação.

                var anm = (AnimatedEntity)source;

                anm.ActiveAnimation.OnEndAnimation += (Animation obj) =>
                {
                    if (anm.ActiveAnimation.Color == Color.White)
                        anm.ActiveAnimation.Color = Color.DarkBlue;
                    else
                        anm.ActiveAnimation.Color = Color.White;
                };
            };

            info = new TextEntity(this, nameof(info));
            info.SetFont("default");
            info.Text.Append("Press A to change screen.");
            info.Transform.SetScale(1.5f, 1.5f);
            info.Transform.SetViewPosition(AlignType.LeftBottom);

            Add(logo, pushStart, info);

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            //Código para exibir trocar a cor de fundo após 10 segundos e simular um efeito de relâmpago.
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if(elapsedTime >= 10000F)
            {
                thunder.Play();
                BackgroundColor = Color.White;
                elapsedTime = 0;
            }
            else
            {
                BackgroundColor = Color.Black;
            }

            //Código para trocar de tela.
            var input = SubManager.Input;

            if (input.Keyboard.IsPress(Keys.A))
                SubManager.Change("menuScreen");

            base.Update(gameTime);            
        }        

        public override void Reset()
        {
            base.Reset();
        }
    }
}