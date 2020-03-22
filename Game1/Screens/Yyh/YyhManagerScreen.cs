// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Screens
{
    class YyhManagerScreen : Screen
    {
        //Utilizamos aqui um subgerenciador de telas.
        //A classe YyhManagerScreen servirá como um cônteiner e controladora de telas.
        SubScreenManager subManager;

        public YyhManagerScreen(ScreenManager manager, string name) : base(manager: manager, name: name, true)
        {            
        }

        public override void Load()
        {
            //Carregamos o subgerenciador.
            subManager = new SubScreenManager(this);

            //Criamos a tela passando o subgerenciador como parâmetro.
            YyhMenuScreen menuScreen = new YyhMenuScreen(subManager, nameof(menuScreen));
            //Podemos usar 'nameof()' com o nome da classe ou digitá-la. O nome é preferência do programador.
            YyhPressStartScreen pressStartScreen = new YyhPressStartScreen(subManager, "pressStartScreen");

            //Adicionamos a tela ao subgerenciador
            subManager.Add(pressStartScreen);
            subManager.Add(menuScreen);            

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            //Verificamos se o gerenciador tem uma tela ativa e mudamos a cor de fundo.
            if (subManager.Active != null)
                BackgroundColor = subManager.Active.BackgroundColor;

            //Atualizamos o subgerenciador.
            subManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Desenhamos o subgerenciador.
            subManager.Draw(gameTime, spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }
    }
}