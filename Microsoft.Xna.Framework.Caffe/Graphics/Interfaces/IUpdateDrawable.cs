namespace Microsoft.Xna.Framework.Graphics
{
    public interface IUpdateDrawable
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}