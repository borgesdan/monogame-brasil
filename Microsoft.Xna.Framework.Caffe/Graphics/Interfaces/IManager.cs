using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Interface para implementação de um gerenciador de telas.
    /// </summary>
    public interface IManager : IDisposable, IUpdateDrawable
    {
        /// <summary>
        /// Obtém a instância corrente da classe Game.
        /// </summary>
        Game Game { get; }
        /// <summary>
        /// Obtém a tela ativa.
        /// </summary>
        Screen Active { get; }

        /// <summary>
        /// Obtém a tela segundo seu nome.
        /// </summary>
        Screen this[string name] { get; }

        /// <summary>Reseta a tela.</summary>
        void Reset(string name);
        /// <summary>Adiciona uma tela</summary>
        void Add(params Screen[] screens);
        /// <summary>Remove uma tela</summary>
        void Remove(string name);
        /// <summary>Troca a tela ativa.</summary>
        void Change(string name, bool reset);
    }
}
