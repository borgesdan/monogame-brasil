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

        AnimatedEntity entity;
        DebugPolygon polygon;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            entity = AnimatedEntity.CreateRectangle(this, "", new Point(100, 100), Color.AliceBlue);
            //entity = AnimatedEntity.CreateRectangle2(this, "", new Point(100, 100), 1, Color.Black);
            entity.Transform.SetViewPosition(AlignType.Center);
            entity.UpdateOutOfView = true;
            
            FollowMouseComponent followMouse = new FollowMouseComponent();
            
            MouseEventsComponent mouseEvents = new MouseEventsComponent();
            mouseEvents.MouseDown += new System.Action<Entity2D, MouseState>
                (
                (Entity2D e, MouseState ms) => 
                {
                    if(ms.LeftButton == ButtonState.Pressed)
                    {
                        var fm = e.Components.Get<FollowMouseComponent>();
                        fm.Follow = true;
                    }                    
                }
                );
            mouseEvents.MouseUp += new System.Action<Entity2D, MouseState>
                (
                (Entity2D e, MouseState ms) =>
                {
                    var fm = e.Components.Get<FollowMouseComponent>();
                    fm.Follow = false;
                }
                );

            entity.Components.Add(followMouse);
            entity.Components.Add(mouseEvents);            

            polygon = new DebugPolygon(GraphicsDevice, entity.BoundsR, Color.Red);
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
            entity.Update(gameTime);

            if(Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                var c = entity.Components.Get<FollowMouseComponent>();

                c.Follow = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                var c = entity.Components.Get<FollowMouseComponent>();

                c.Follow = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            polygon.Set(entity.BoundsR.Points);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            entity.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            polygon.Draw();

            base.Draw(gameTime);
        }
    }
}
