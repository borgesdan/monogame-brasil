using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public class DebugComponent : ActorComponent
    {        
        TextActor text = null;
        Sprite verticalLine = null;
        Sprite horizontalLine = null;

        public bool ShowTransform { get; set; } = true;
        public bool ShowOrigin { get; set; } = true;
        public Color LineColor { get; set; } = Color.White;
        public Color TextColor { get; set; } = Color.White;        

        public DebugComponent(Actor actor, SpriteFont font) : base(actor)
        {
            text = new TextActor(actor.Game, "", font);
            verticalLine = Sprite.GetRectangle(actor.Game, "", new Point(1, actor.Game.GraphicsDevice.Viewport.Height), Color.White);
            horizontalLine = Sprite.GetRectangle(actor.Game, "", new Point(actor.Game.GraphicsDevice.Viewport.Width, 1), Color.White);
            this.Priority = DrawPriority.Forward;
        }

        public DebugComponent(Actor destination, ActorComponent source) : base(destination, source)
        {
        }

        protected override void _Update(GameTime gameTime)
        {            
            var transform = Actor.Transform;

            text.Text.Clear();
            text.Text.AppendLine(Actor.Name);
            text.Text.Append("X: " + transform.Position.X);
            text.Text.AppendLine(" Y: " + transform.Position.X);
            text.Text.Append("SX: " + transform.Scale.X);
            text.Text.AppendLine("SY: " + transform.Scale.Y);
            text.Text.AppendLine("R: " + transform.Rotation);
            text.Text.Append("W: " + transform.ScaledSize.X);
            text.Text.Append("H: " + transform.ScaledSize.Y);

            text.Transform.Origin = new Vector2(text.Bounds.Width / 2, text.Bounds.Height / 2);
            text.Transform.SetPosition(Actor.Bounds.Center);
            //text.Transform.Origin = new Vector2(0, text.Bounds.Height);
            //text.Transform.SetPosition(new Vector2(transform.X + transform.Origin.X, transform.Y + transform.Origin.Y));
            //text.Transform.SetPosition(Actor.Bounds.Right, Actor.Bounds.Top);

            horizontalLine.Transform.SetPosition(0, Actor.Bounds.Y + transform.Origin.Y);
            verticalLine.Transform.SetPosition(Actor.Bounds.X + transform.Origin.X, 0);

            horizontalLine.Transform.Color = LineColor;
            verticalLine.Transform.Color = LineColor;
            text.Transform.Color = TextColor;
        }

        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(ShowOrigin)
            {
                verticalLine.Draw(gameTime, spriteBatch);
                horizontalLine.Draw(gameTime, spriteBatch);
            }
            if(ShowTransform)
            {
                text.Draw(gameTime, spriteBatch);
            }            
        }
    }
}