//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um gerenciador de telas.
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
        /// <summary>Avança uma tela.</summary>        
        void Next(bool reset);
        /// <summary>Volta para a tela anterior.</summary>
        void Back(bool reset);
    }
}
