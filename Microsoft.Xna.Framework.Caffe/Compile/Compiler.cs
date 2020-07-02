using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Xna.Framework.Compile
{
    /// <summary>
    /// Classe responsável por compilar código C#.
    /// </summary>
    public static class Compiler
    {        
        /// <summary>
        /// Obtém os resultados da tentativa de compilação.
        /// </summary>
        public static CompilerResults Results { get; private set; } = null;

        /// <summary>
        /// Compila o código C# e retorna uma instância do tipo especificado. Retorna null caso ocorra algum erro.
        /// </summary>
        /// <typeparam name="T">O tipo da instância.</typeparam>
        /// <param name="sourceFile">O caminho do arquivo C#.</param>
        /// <param name="className">O nome da classe principal do arquivo.</param>
        /// <param name="args">Os argumentos do construtor para a criação da instância.</param>
        /// <param name="references">As referências using para serem adicionadas.</param>
        public static T CompileCSharpCode<T>(string sourceFile, string className, object[] args, params string[] references) where T : class
        {
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                CompilerParameters cp = new CompilerParameters();

                //Carrega as referências do código
                cp.ReferencedAssemblies.Add("System.dll");
                
                foreach (string r in references)
                    cp.ReferencedAssemblies.Add(r);

                cp.GenerateExecutable = false;
                cp.GenerateInMemory = true;

                //Compila o código
                CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
                Results = cr;

                if (cr.Errors.Count > 0)
                {                   
                    return null;
                }

                Assembly asm = cr.CompiledAssembly;
                Type type = asm.GetType(className);

                return (T)asm.CreateInstance(type.FullName, true, BindingFlags.Default, null, args, CultureInfo.CurrentCulture, null);
            }            
        }
    }
}