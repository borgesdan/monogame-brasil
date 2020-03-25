using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Polygon poly;
        //Polygon poly2;
        AnimatedEntity shadow;
        AnimatedEntity ent;

        AnimatedEntity ent2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //poly = new Polygon
            //    (
            //    this,
            //    new Vector2(200,100),
            //    new Vector2(250, 50),
            //    new Vector2(300, 50),
            //    new Vector2(350, 100),
            //    new Vector2(300, 150),
            //    new Vector2(250, 150)
            //    );

            //poly2 = new Polygon
            //    (
            //    this,
            //    new Vector2(400, 300),
            //    new Vector2(450, 250),
            //    new Vector2(550, 300),
            //    new Vector2(450, 350)
            //    );

            shadow = AnimatedEntity.CreateRectangle(this, "ent", new Point(30, 30), Color.Green);
            shadow.Transform.SetPosition(100, 100);            
            shadow.Transform.Color = new Color(200, 200, 200, 200);

            ent = AnimatedEntity.CreateRectangle(this, "ent", new Point(100, 100), Color.Black);
            ent.Transform.SetPosition(100, 100);
            //ent.Transform.Rotation = MathHelper.ToRadians(2);
            ent.Origin = new Vector2(ent.Bounds.Width / 2, ent.Bounds.Height / 2);
            ent.Transform.Color = new Color(200, 200, 200, 200);

            ent2 = AnimatedEntity.CreateRectangle(this, "ent2", new Point(100, 100), Color.DarkBlue);
            ent2.Transform.SetPosition(300, 200);
            //ent2.Transform.Rotation = MathHelper.ToRadians(80);            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Vector2 v = new Vector2();

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                v.X = 2;
                //ent.Transform.Rotation += 0.005f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                v.X = -2;
                //ent.Transform.Rotation -= 0.005f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                v.Y = -2;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                v.Y = 2;
            }

            var r = Collision.PolygonCollision(ent.BoundsR, ent2.BoundsR, v);
            Vector2 playerTranslation = v;

            if (r.Intersect)
            {
                playerTranslation = v + r.MinimumTranslationVector;
            }

            var p = ent.Transform.Position;
            p.X += playerTranslation.X;
            p.Y += playerTranslation.Y; 

            ent.Transform.Position = p;

            shadow.Update(gameTime);
            ent.Update(gameTime);
            ent2.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //poly.Draw();
            //poly2.Draw();

            spriteBatch.Begin();
            shadow.Draw(gameTime, spriteBatch);
            ent.Draw(gameTime, spriteBatch);
            ent2.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
