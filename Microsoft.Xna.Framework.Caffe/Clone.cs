// Danilo Borges Santos, 2020.

using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>Classe de auxílio para clonagem de um objeto</summary>
    public static class Clone
    {   
        /// <summary>
        /// Cria um cópia de um objeto quando pertence a uma classe herdada e não é possível utilizar seu construtor de cópia.
        /// </summary>
        /// <typeparam name="A">O tipo da classe do objeto.</typeparam>
        /// <param name="obj">O objeto de referência para cópia.</param>
        /// <param name="args">Os argumentos do construtor de cópia que não foi possível ser utilizado.</param>
        public static A Get<A>(A obj, params object[] args)
        {
            return (A)Activator.CreateInstance(obj.GetType(), args);
        }
    }        
}