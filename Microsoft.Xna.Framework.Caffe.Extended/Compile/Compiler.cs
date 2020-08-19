// Danilo Borges Santos, 2020.

using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;

//.NET Core
//<PackageReference Include="System.CodeDom" Version="4.4.0" />

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Classe responsável por compilar código C# de um arquivo externo.
    /// </summary>
    public static class Compiler
    {        
        /// <summary>
        /// Obtém os resultados da tentativa de compilação.
        /// </summary>
        public static CompilerResults Results { get; private set; } = new CompilerResults(new TempFileCollection());

        /// <summary>
        /// Compila um código C# e retorna uma instância do tipo especificado. Retorna null caso ocorra algum erro.
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

/*  - EXEMPLO -
 * 
 *  Suponha que existe um arquivo C# escrito com um código que explicita uma classe
 *  que herda de AnimatedEntity que se chama ExempleEntity.
 *  
 *  Leia esse arquivo de código e traga para seu projeto da seguinte maneira.
 *  
 *  -- Código -- 
 *      AnimatedEntity entity = Compiler.CompileCSharpCode<AnimatedEntity>(@"file.cs", "ExempleEntity", new object[2] { Game, "name" }, usingsReferences);
 *      
 *      // O argumento 'usingReferences' são as referências necessárias para compilar o arquivo, por exemplo:
 *      //
 *      // string[] usings = new string[]
 *      // {
 *      //   { @"C:\Program Files (x86)\MonoGame\v3.0\Assemblies\Windows\MonoGame.Framework.dll" },
 *      // }
 *      
 *      //Invoca os métodos da classe base AnimatedEntity.
 *      entity.Update(gameTime);
 *      entity.Draw(gameTime); 
 *  
 *  -- Fim --
 */
