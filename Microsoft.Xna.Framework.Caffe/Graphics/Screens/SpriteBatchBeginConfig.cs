// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que define a configuração do SpriteBatch na chamada do método Begin().
    /// </summary>
    public class SpriteBatchBeginConfig
    {
        public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState Sampler { get; set; } = null;
        public DepthStencilState DepthStencil { get; set; } = null;
        public RasterizerState Rasterizer { get; set; } = null;
        public Effect Effect { get; set; } = null;
        public Matrix? TransformMatrix { get; set; } = null;

        /// <summary>
        /// Inicializa uma nova instância de SpriteBatchBeginConfig.
        /// </summary>
        public SpriteBatchBeginConfig()
        {
        }

        public SpriteBatchBeginConfig(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix? transformMatrix)
        {
            SortMode = sortMode;
            BlendState = blendState;
            Sampler = samplerState;
            DepthStencil = depthStencilState;
            Rasterizer = rasterizerState;
            Effect = effect;
            TransformMatrix = transformMatrix;
        }

        /// <summary>
        /// Inicializa uma nova instância de SpriteBatchBeginConfig como cópia de outro SpriteBatchBeginConfig.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public SpriteBatchBeginConfig(SpriteBatchBeginConfig source)
        {            
            this.SortMode = source.SortMode;
            this.BlendState = source.BlendState;
            this.Sampler = source.Sampler;
            this.DepthStencil = source.DepthStencil;
            this.Rasterizer = source.Rasterizer;
            this.Effect = source.Effect;
            this.TransformMatrix = source.TransformMatrix;
        }
    }
}