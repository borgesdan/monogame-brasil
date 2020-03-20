
namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que define a configuração do SpriteBatch na chamada do método Begin().
    /// </summary>
    public class SpriteBatchBeginConfig
    {
        public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState SamplerState { get; set; } = null;
        public DepthStencilState DepthStencilState { get; set; } = null;
        public RasterizerState RasterizerState { get; set; } = null;
        public Effect Effect { get; set; } = null;
        public Matrix? TransformMatrix { get; set; } = null;
    }
}