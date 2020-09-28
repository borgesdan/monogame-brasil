using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//https://stackoverflow.com/questions/22519730/how-do-i-flip-an-image-in-memory

namespace Caffe.Samples
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Sprite sprite;
        Sprite sprite2;
        InputManager input = new InputManager();
        Sprite led;
        Sprite pixel;

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

            sprite = new Sprite(this, "player");
            sprite.Boxes.Add(new SpriteFrame(18, 20, 76, 122), null, null);            
            sprite.UpdateBounds();

            sprite2 = new Sprite(sprite);
            //sprite2.Transform.SpriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            sprite2.UpdateBounds();
            sprite2.Transform.Origin = new Vector2(sprite2.Transform.Width / 2, sprite2.Transform.Height / 2);
            sprite2.Transform.SetPosition(AlignType.Center, GraphicsDevice.Viewport);
            sprite2.Transform.RotateD(45);

            led = Sprite.GetRectangle(this, new Point(50, 50), Color.White);
            led.Transform.SetPosition(AlignType.RightBottom, GraphicsDevice.Viewport);

            pixel = Sprite.GetRectangle(this, new Point(5, 5), Color.White);           
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();            

            input.Update(gameTime);            
            led.Update(gameTime);

            sprite2.Transform.Rotation += MathHelper.ToRadians(0.5f);

            sprite.Update(gameTime);
            sprite2.Update(gameTime);

            if (input.Keyboard.IsDown(Keys.Up))
            {
                sprite.Transform.Move(0, -4);

                //if (Collision.ActorCollision(sprite, sprite2).HasCollided)
                //{
                //    if (CheckCollision())
                //    {
                //        sprite.Transform.Move(0, 4);
                //    }
                //}    
                
                if(CheckCollision())
                {
                    sprite.Transform.Move(0, 4);
                }
            }
            if (input.Keyboard.IsDown(Keys.Down))
            {
                sprite.Transform.Move(0, 4);


                //if (Collision.ActorCollision(sprite, sprite2).HasCollided)
                //{
                //    if (CheckCollision())
                //    {
                //        sprite.Transform.Move(0, -4);
                //    }
                //}   

                if (CheckCollision())
                {
                    sprite.Transform.Move(0, -4);
                }
            }
            if (input.Keyboard.IsDown(Keys.Right))
            {
                sprite.Transform.Move(4, 0);

                //if (Collision.ActorCollision(sprite, sprite2).HasCollided)
                //{
                //    if (CheckCollision())
                //    {
                //        sprite.Transform.Move(-4, 0);
                //    }
                //}

                if (CheckCollision())
                {
                    sprite.Transform.Move(-4,0);
                }
            }
            if (input.Keyboard.IsDown(Keys.Left))
            {
                sprite.Transform.Move(-4, 0);

                //if (Collision.ActorCollision(sprite, sprite2).HasCollided)
                //{
                //    if (CheckCollision())
                //    {
                //        sprite.Transform.Move(4, 0);
                //    }
                //}

                if (CheckCollision())
                {
                    sprite.Transform.Move(4,0);
                }
            }
            
            if(input.Keyboard.IsPress(Keys.Space))
            {
                sprite2.Transform.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            
            base.Update(gameTime);
        }        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here     
            _spriteBatch.Begin();

            sprite.Draw(gameTime, _spriteBatch);
            sprite2.Draw(gameTime, _spriteBatch);
            led.Draw(gameTime, _spriteBatch);

            RotatedRectangle b = new RotatedRectangle(sprite2.Bounds, sprite2.Bounds.Location.ToVector2() + sprite2.Transform.Origin, sprite2.Transform.Rotation);

            pixel.Transform.Color = Color.Red;

            pixel.Transform.SetPosition(b.P1);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(b.P2);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(b.P3);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(b.P4);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(b.Center);
            pixel.Draw(gameTime, _spriteBatch);

            RotatedRectangle a = new RotatedRectangle(sprite.Bounds, sprite.Bounds.Location.ToVector2() + sprite.Transform.Origin, sprite.Transform.Rotation);

            pixel.Transform.Color = Color.Blue;

            pixel.Transform.SetPosition(a.P1);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(a.P2);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(a.P3);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(a.P4);
            pixel.Draw(gameTime, _spriteBatch);

            pixel.Transform.SetPosition(a.Center);
            pixel.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        bool CheckCollision()
        {
            //Color[] data1 = sprite.GetData();
            //Color[] data2 = sprite2.GetData();

            //return Collision.PerPixelCollision(sprite.Bounds, data1, sprite2.Bounds, data2);

            //RotatedRectangle a = new RotatedRectangle(new Rectangle(sprite.Transform.Position.ToPoint(), sprite.Transform.Size), sprite.Transform.Origin, sprite.Transform.Rotation);
            //RotatedRectangle b = new RotatedRectangle(new Rectangle(sprite2.Transform.Position.ToPoint(), sprite2.Transform.Size), sprite2.Transform.Origin, sprite2.Transform.Rotation);            

            RotatedRectangle a = new RotatedRectangle(sprite.Bounds, sprite.Bounds.Location.ToVector2() + sprite.Transform.Origin, sprite.Transform.Rotation);
            RotatedRectangle b = new RotatedRectangle(sprite2.Bounds, sprite2.Bounds.Location.ToVector2() + sprite2.Transform.Origin, sprite2.Transform.Rotation);

            if (a.Intersects(b))
            {
                led.Transform.Color = Color.Red;
                return true;
            }                
            else
            {
                led.Transform.Color = Color.White;
                return false;
            }                
        }
    }
}
