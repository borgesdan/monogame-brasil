using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen1 : Screen
    {
        AnimatedEntity entity;
        AnimatedEntity other;

        public Screen1(ScreenManager manager, string name):base(manager: manager, name, true)
        {

        }

        public override void Load()
        {
            entity = AnimatedEntity.CreateRectangle(Game, nameof(entity), new Point(100, 100), Color.DarkBlue, this);
            entity.Origin = new Vector2(entity.Transform.Width / 2, entity.Transform.Height / 2);
            entity.OnUpdate += Entity_OnUpdate;            

            BasicCollisionComponent collisionComponent = new BasicCollisionComponent();
            collisionComponent.OnCollision += CollisionComponent_OnCollision;
            entity.Components.Add(collisionComponent);

            other = AnimatedEntity.CreateRectangle(Game, nameof(other), new Point(200, 200), Color.DarkGreen, this);
            other.Transform.SetViewPosition(AlignType.Center);


            base.Load();
        }

        private void CollisionComponent_OnCollision(Entity2D source, GameTime gameTime, CollisionResult result, Entity2D collidedEntity)
        {
            if(result.Type == CollisionType.Rectangle)
            {
                entity.Transform.Move(result.RectangleResult.Subtract);
            }
        }

        private void Entity_OnUpdate(Entity2D source, GameTime gameTime)
        {
            var input = Manager.Input;

            if(input.Keyboard.IsDown(Keys.Right))
            {
                source.Transform.Move(new Vector2(3, 0));
            }
            if (input.Keyboard.IsDown(Keys.Left))
            {
                source.Transform.Move(new Vector2(-3, 0));
            }
            if (input.Keyboard.IsDown(Keys.Up))
            {
                source.Transform.Move(new Vector2(0, -3));
            }
            if (input.Keyboard.IsDown(Keys.Down))
            {
                source.Transform.Move(new Vector2(0, 3));
            }
            if (input.Keyboard.IsDown(Keys.A))
            {
                source.Transform.Rotation += MathHelper.ToRadians(2);
            }
            if (input.Keyboard.IsDown(Keys.S))
            {
                source.Transform.Rotation -= MathHelper.ToRadians(2);
            }
        }
    }
}