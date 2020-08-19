// Danilo Borges Santos, 2020.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        TextEntity textEntity;
        AnimatedEntity rectangleEntity;
        AnimatedEntity backEntity;

        float textScale = 0.005f;
        Random randomColor = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //TEXT
            textEntity = new TextEntity(this, "helloText");
            textEntity.SetFont("default");
            textEntity.TextBuilder.Append("Hello World!");
            //Já que adicionamos um texto é necessário atualizar o tamanho da entidade 
            textEntity.UpdateBounds();
            //Define a origem para desenho no centro do texto
            textEntity.Origin = new Vector2(textEntity.Transform.Width / 2, textEntity.Transform.Height / 2);
            textEntity.Transform.SetViewPosition(AlignType.Center);

            //FILLED RECTANGLE
            rectangleEntity = AnimatedEntity.CreateRectangle(this, "filledRectangle", new Point(200, 200), Color.DarkBlue);
            rectangleEntity.Origin = new Vector2(rectangleEntity.Transform.Width / 2, rectangleEntity.Transform.Height / 2);
            rectangleEntity.Transform.SetViewPosition(AlignType.Center);

            //Componente para trocar a cor do retângulo
            ColorComponent colorComponent = new ColorComponent();
            colorComponent.Delay = 25;
            colorComponent.FinalColor = Color.Green;  
            //Sempre que a cor desejada for alcançada trocaremos por outra através deste evento OnChangeColor.
            colorComponent.OnChangeColor += (GameTime gameTime) =>
            {
                int r = randomColor.Next(0, 256);
                int g = randomColor.Next(0, 256);
                int b = randomColor.Next(0, 256);

                colorComponent.FinalColor = new Color(r, g, b);
            };

            rectangleEntity.Components.Add(colorComponent);

            //BACK RECTANGLE
            backEntity = AnimatedEntity.CreateRectangle2(this, "backRectangle", new Point(400, 400), 2, Color.Black);
            backEntity.Origin = new Vector2(backEntity.Transform.Width / 2, backEntity.Transform.Height / 2);
            backEntity.Transform.SetViewPosition(AlignType.Center);            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Rotaciona o retâgulo de fundo
            backEntity.Transform.Rotation -= 0.01f;

            //Rotaciona o texto
            textEntity.Transform.Rotation += 0.005f;
            //Altera o tamanho
            textEntity.Transform.Scale += new Vector2(textScale);
            
            //Sempre que chegar no limite do tamanho inverte o valor da escala
            if (textEntity.Transform.Scale.X >= 2 || textEntity.Transform.Scale.X <= 0.5f)
                textScale *= -1;

            //Atualiza as entidades
            backEntity.Update(gameTime);
            rectangleEntity.Update(gameTime);
            textEntity.Update(gameTime);            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            backEntity.Draw(gameTime, _spriteBatch);
            rectangleEntity.Draw(gameTime, _spriteBatch);
            textEntity.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
