using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um componente que exibe informações do ator.
    /// </summary>
    public class DebugComponent : ActorComponent
    {        
        TextActor text = null;
        Sprite verticalLine = null;
        Sprite horizontalLine = null;

        /// <summary>
        /// Obtém ou define se as informações de transformações devem ser exibidas.
        /// </summary>
        public bool ShowTransform { get; set; } = true;
        /// <summary>
        /// Obtém ou define se os eixos da origem do ator devem ser exibidas.
        /// </summary>
        public bool ShowOrigin { get; set; } = true;
        /// <summary>
        /// Obtém ou define a cor da linha a ser desenhada.
        /// </summary>
        public Color LineColor { get; set; } = Color.White;
        /// <summary>
        /// Obtém ou define a cor do texto a ser desenhado.
        /// </summary>
        public Color TextColor { get; set; } = Color.White;        

        /// <summary>
        /// Inicializa uma nova instância de DebugComponent.
        /// </summary>
        /// <param name="actor">O ator a ser associado a esse componente.</param>
        /// <param name="font">O objeto SpriteFont a ser usado para desenho do texto.</param>
        public DebugComponent(Actor actor, SpriteFont font) : base(actor)
        {
            text = new TextActor(actor.Game, "", font);
            verticalLine = Sprite.GetRectangle(actor.Game, "", new Point(1, actor.Game.GraphicsDevice.Viewport.Height), Color.White);
            horizontalLine = Sprite.GetRectangle(actor.Game, "", new Point(actor.Game.GraphicsDevice.Viewport.Width, 1), Color.White);
            this.Priority = DrawPriority.Forward;
        }

        /// <summary>
        /// Inicializa uma nova instância de DebugComponent como cópia de outra instância.
        /// </summary>
        /// <param name="destination">O ator a ser associado esse componente</param>
        /// <param name="source">A instância de origem a ser copiada esse componente</param>
        public DebugComponent(Actor destination, DebugComponent source) : base(destination, source)
        {
            this.text = new TextActor(source.text);
            this.verticalLine = new Sprite(source.verticalLine);
            this.horizontalLine = new Sprite(source.horizontalLine);
            this.ShowOrigin = source.ShowOrigin;
            this.ShowTransform = source.ShowTransform;
            this.TextColor = source.TextColor;
            this.LineColor = source.LineColor;
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