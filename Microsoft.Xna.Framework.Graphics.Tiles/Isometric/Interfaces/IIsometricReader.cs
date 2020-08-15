// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um leitor de tiles isometricos.
    /// </summary>
    public interface IIsometricReader
    {
        /// <summary>
        /// Obtém se o método Read() leu todo seu conteúdo e chegou ao fim.
        /// </summary>
        bool IsRead { get; }

        /// <summary>
        /// Lê o array contido nos setores e ordena as posições dos tiles.
        /// </summary>
        void Read();
    }
}