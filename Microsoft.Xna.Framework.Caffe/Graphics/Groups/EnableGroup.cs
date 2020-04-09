// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe que agrupa propriedades para disponibilidade da entidade.</summary>
    public struct EnableGroup : IEquatable<EnableGroup>
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
        
        /// <summary>
        /// Retorna IsEnabled = false e IsVisible = true.
        /// </summary>
        public static EnableGroup Disabled
        {
            get => new EnableGroup(false, true);
        }

        /// <summary>
        /// Retorna IsEnabled = true e IsVisible = false.
        /// </summary>
        public static EnableGroup Hidden
        {
            get => new EnableGroup(true, false);
        }

        /// <summary>
        /// Retorna IsEnabled = false e IsVisible = false.
        /// </summary>
        public static EnableGroup Unavailable
        {
            get => new EnableGroup(false, false);
        }

        /// <summary>
        /// Retorna IsEnabled = true e IsVisible = true.
        /// </summary>
        public static EnableGroup Available
        {
            get => new EnableGroup(true, true);
        }

        public override bool Equals(object obj)
        {
            return obj is EnableGroup group && Equals(group);
        }

        public bool Equals(EnableGroup other)
        {
            return IsEnabled == other.IsEnabled &&
                   IsVisible == other.IsVisible;
        }

        public override int GetHashCode()
        {
            var hashCode = -567787825;
            hashCode = hashCode * -1521134295 + IsEnabled.GetHashCode();
            hashCode = hashCode * -1521134295 + IsVisible.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(EnableGroup left, EnableGroup right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EnableGroup left, EnableGroup right)
        {
            return !(left == right);
        }
    }
}