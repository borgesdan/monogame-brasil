namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Implementa a funcionalidade de transformação.
    /// </summary>
    /// <typeparam name="T">T é uma classe que implementa a interface IBoundsable</typeparam>
    public interface ITransformable<T> where T: IBoundsable
    {
        public TransformGroup<T> Transform { get; }
    }
}