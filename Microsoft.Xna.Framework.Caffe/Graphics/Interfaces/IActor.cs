using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um ator do jogo: qualquer objeto que implemente essa interface e que disponibilize suas funcionalidades.
    /// </summary>    
    public interface IActor : IBoundsable, ITransformable, IUpdateDrawable, IRotatedBounds, IDisposable
    {
        public EnableGroup Enable { get; set; }
    }
}