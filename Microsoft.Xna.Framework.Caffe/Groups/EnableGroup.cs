// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework
{
    /// <summary>Classe que agrupa propriedades para disponibilidade de um objeto.</summary>
    public class EnableGroup
    {
        /// <summary>Obtém ou define se o objeto está ativo.</summary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>Obtém ou define se o objeto é visível.</summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>Inicia uma nova instância da classe EnableGroup com suas preferências</summary>
        public EnableGroup() : this(true, true) { }

        /// <summary>Inicia uma nova instância da classe EnableGroup com suas preferências</summary>
        /// <param name="enabled">True se a entidade está ativa.</param>
        /// <param name="visible">True se a entidade é visível.</param>
        public EnableGroup(bool enabled, bool visible)
        {
            IsEnabled = enabled;
            IsVisible = visible;
        }
    }
}