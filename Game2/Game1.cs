using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        AnimatedEntity player;
        AnimatedEntity player2;
        InputManager input;

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

            input = new InputManager();

            player = new AnimatedEntity(this, "player");
            Animation stand = new Animation(this, 100, "stand");
            
            Sprite sprite = new Sprite(this, "player");
            sprite.Boxes.Add(new SpriteFrame(18, 20, 76, 122), null, null);

            stand.AddSprites(sprite);

            player.AddAnimation(stand);

            player2 = new AnimatedEntity(player);            
            player2.Transform.SetPosition(AlignType.Center, GraphicsDevice.Viewport);

            Microsoft.Xna.Framework.Graphics.Components.Componentes.BoundsComponent<Sprite> componente = new Microsoft.Xna.Framework.Graphics.Components.Componentes.BoundsComponent<Sprite>();
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            input.Update(gameTime);

            if (input.Keyboard.IsDown(Keys.Left))
            {
                player.Transform.Move(-4, 0);
                Check(-4, 0);
                
            }                
            else if (input.Keyboard.IsDown(Keys.Right))
            {
                player.Transform.Move(4, 0);
                Check(4, 0);
            }                
            else if (input.Keyboard.IsDown(Keys.Up))
            {
                player.Transform.Move(0, -4);
                Check(0, -4);
            }
            else if (input.Keyboard.IsDown(Keys.Down))
            {
                player.Transform.Move(0, 4);
                Check(0, 4);
            }

            player.Update(gameTime);
            player2.Update(gameTime);            

            base.Update(gameTime);
        }

        public void Check(float subValueX, float subValueY)
        {
            bool result = Collision.PerPixelCollision(player.Bounds, player.CurrentAnimation.CurrentSprite, player.CurrentAnimation.SpriteIndex,
                player2.Bounds, player2.CurrentAnimation.CurrentSprite, player2.CurrentAnimation.SpriteIndex);
            
            if(result)
                player.Transform.Move(-subValueX, -subValueY);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            player.Draw(gameTime, _spriteBatch);
            player2.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
