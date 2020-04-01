// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe que agrupa propriedades para disponibilidade da entidade.</summary>
    public struct EnableGroup
    {
        /// <summary>Obtém ou define se a entidade está ativa.</summary>
        public bool IsEnabled;
        /// <summary>Obtém ou define se a entidade é visível.</summary>
        public bool IsVisible;

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