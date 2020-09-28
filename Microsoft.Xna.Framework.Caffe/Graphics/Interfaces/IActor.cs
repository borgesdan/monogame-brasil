using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um ator do jogo: qualquer objeto que implemente essa interface e que disponibilize suas funcionalidades.
    /// </summary>
    /// <typeparam name="T">T é uma classe que implementa as interfaces IBoundsable, ITransformable<T>, IUpdateDrawable, IBoundsR, IDisposable</typeparam>
    public interface IActor<T> : IBoundsable, ITransformable<T>, IUpdateDrawable, IBoundsR, IDisposable where T : IBoundsable
    {
        public EnableGroup Enable { get; set; }
    }
}