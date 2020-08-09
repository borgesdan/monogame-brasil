// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que define a configuração do SpriteBatch na chamada do método Begin().
    /// </summary>
    public class SpriteBatchBeginConfig
    {
        public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState Blend { get; set; } = null;
        public SamplerState Sampler { get; set; } = null;
        public DepthStencilState DepthStencil { get; set; } = null;
        public RasterizerState Rasterizer { get; set; } = null;
        public Effect Effects { get; set; } = null;
        public Matrix? TransformMatrix { get; set; } = null;

        /// <summary>
        /// Inicializa uma nova instância de SpriteBatchBeginConfig.
        /// </summary>
        public SpriteBatchBeginConfig()
        {
        }

        /// <summary>
        /// Inicializa uma nova instância de SpriteBatchBeginConfig como cópia de outro SpriteBatchBeginConfig.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public SpriteBatchBeginConfig(SpriteBatchBeginConfig source)
        {            
            this.SortMode = source.SortMode;
            this.Blend = source.Blend;
            this.Sampler = source.Sampler;
            this.DepthStencil = source.DepthStencil;
            this.Rasterizer = source.Rasterizer;
            this.Effects = source.Effects;
            this.TransformMatrix = source.TransformMatrix;
        }
    }
}