using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Game2.Screens
{   

    class Screen8 : Screen
    {
        Sprite sprite_items;

        MarioEntity mario;
        AnimatedEntity floor;
        CoinEntity coin;

        float elapsedTime = 0;
        float coinTime = 2000;
        Random coinRandom = new Random();

        public Screen8(ScreenManager manager, string name) : base(manager: manager, name, true)
        {            
        }
        
        public override void Load()
        {
            sprite_items = new Sprite(Game, "items");

            //A largura do chão
            int floorWidth = Game.Window.ClientBounds.Width;

            //A entidade chão
            floor = AnimatedEntity.CreateRectangle(Game, "floor", new Point(floorWidth, 20), Color.Black);
            floor.Transform.SetViewPosition(AlignType.Center);

            //Mario
            mario = new MarioEntity(Game, "mario");
            mario.Transform.Y = floor.Transform.Y - mario.Bounds.Height;
            mario.Transform.X += 100;

            //Moeda
            coin = new CoinEntity(Game, "coin", sprite_items);
            coin.Enable = new EnableGroup(true, false);

            AddEntity(floor, mario);

            base.Load();
        }

        enum MarioState
        {
            Run,
            Jump
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if(elapsedTime > coinTime)
            {
                CoinEntity otherCoin = new CoinEntity(coin);
                otherCoin.Enable = new EnableGroup(true, true);

                var next = coinRandom.Next(2);

                if(next == 0)
                    otherCoin.Transform.Y = floor.Transform.Y - otherCoin.Bounds.Height;
                else
                    otherCoin.Transform.Y = 54;

                otherCoin.Transform.X = Game.Window.ClientBounds.Width;
                otherCoin.Transform.Xv = -2;
                otherCoin.OnUpdate += (Entity2D e, GameTime gt) =>
                {
                    if(e.Transform.X + e.Transform.Width < 0)
                    {
                        Entities.Remove(e);
                    }
                };

                AddEntity(otherCoin);

                elapsedTime = 0;
                coinTime -= 100;

                if (coinTime <= 100)
                    coinTime = 2000;
            }

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

        class MarioEntity : AnimatedEntity
        {
            MarioState marioState;

            SoundEffect jumpSound;
            SoundEffect coinSound;

            public MarioEntity(Game game, string name) : base(game, name)
            {
                jumpSound = Game.Content.Load<SoundEffect>("smw_jump");
                coinSound = Game.Content.Load<SoundEffect>("smw_coin");

                //O sprite do Mario
                Sprite sprite = new Sprite(Game, "mariosprite");

                //A lista de frames do sprite.
                List<SpriteFrame> run_frames = new List<SpriteFrame>();
                run_frames.Add(new SpriteFrame(412, 311, 36, 56));
                run_frames.Add(new SpriteFrame(492, 311, 36, 56));
                run_frames.Add(new SpriteFrame(572, 311, 36, 56));

                SpriteFrame jump_frame = new SpriteFrame(414, 227, 32, 62, new Vector2(0, 6)); //62 - 6 = 56 (Altura no Mario correndo);
                SpriteFrame fall_frame = new SpriteFrame(494, 229, 32, 58, new Vector2(0, 2));

                //A animação correndo
                Animation mario_run = new Animation(Game, 100, "mario_run");
                mario_run.AddSprite(sprite, run_frames.ToArray());

                //Animação pulando
                Animation mario_jump = new Animation(Game, 100, "mario_jump");
                mario_jump.AddSprite(sprite, jump_frame);

                //Animação caindo
                Animation mario_fall = new Animation(Game, 100, "mario_fall");
                mario_fall.AddSprite(sprite, fall_frame);

                //A entidade Mario. 
                AddAnimation(mario_run);
                AddAnimation(mario_jump);
                AddAnimation(mario_fall);
                OnUpdate += (Entity2D source, GameTime gameTime) =>
                {
                    var input = Screen.Manager.Input;

                    if (input.Keyboard.IsPress(Keys.Up))
                    {
                        if (marioState == MarioState.Run)
                        {
                            marioState = MarioState.Jump;
                            jumpSound.Play();
                            Change("mario_jump");
                            Transform.SetVelocity(0, -5f);
                        }
                    }

                    if (Transform.Y < 54)
                    {
                        Change("mario_fall");
                        Transform.SetVelocity(0, 4.5f);
                    }
                };

                BasicCollisionComponent collisionComponent = new BasicCollisionComponent();
                collisionComponent.OnCollision += (Entity2D source, GameTime gameTime, CollisionResult result, Entity2D collidedEntity) =>
                {
                    if (collidedEntity.Name == "floor")
                    {
                        Transform.SetVelocity(0, 0);
                        Transform.Move(result.RectangleResult.Subtract);
                        Change("mario_run");
                        marioState = MarioState.Run;
                    }
                    if(collidedEntity.Name == "coin")
                    {
                        coinSound.CreateInstance().Play();
                        Screen.Entities.Remove(collidedEntity);
                        collidedEntity = null;
                    }
                };

                Components.Add(collisionComponent);
                marioState = MarioState.Run;
            }
        }

        class CoinEntity : AnimatedEntity
        {
            public CoinEntity(CoinEntity source) : base(source)
            {

            }                

            public CoinEntity(Game game, string name, Sprite items) : base(game, name)
            {
                List<SpriteFrame> coin_frames = new List<SpriteFrame>();
                coin_frames.Add(new SpriteFrame(126, 354, 24, 32));
                coin_frames.Add(new SpriteFrame(156, 354, 16, 32, new Vector2(-4,0)));
                coin_frames.Add(new SpriteFrame(178, 354, 12, 32, new Vector2(-6,0)));
                coin_frames.Add(new SpriteFrame(196, 354, 16, 32, new Vector2(-4,0)));

                Animation coin_animation = new Animation(Game, 100, "coin");
                coin_animation.AddSprite(items, coin_frames.ToArray());                

                AddAnimation(coin_animation);
            }
        }
    }
}