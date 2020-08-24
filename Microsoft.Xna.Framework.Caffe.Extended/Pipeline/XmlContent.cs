using System;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

//dotnet core
//<PackageReference Include="MonoGame.Framework.Content.Pipeline" Version="3.8.0.1641" />

namespace Microsoft.Xna.Framework.Content.Pipeline
{
    /// <summary>
    /// Classe que serializa ou desserializa um objeto no formato XML>
    /// </summary>
    public class XmlContent
    {
        /// <summary>
        /// Serializa um objeto no formato XmlContent.
        /// </summary>        
        /// /// <typeparam name="T">O tipo do parâmetro.</typeparam>
        /// <param name="path">O caminho com o nome do arquivo xml (extensão .xml).</param>
        /// <param name="obj">O objeto a ser serializado.</param>
        public static void Serialize<T>(string path, T obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                IntermediateSerializer.Serialize(writer, obj, null);
            }
        }

        /// <summary>
        /// Desserializa um XmlContent em um objeto.
        /// </summary>
        /// <typeparam name="T">O tipo do parâmetro</typeparam>
        /// <param name="path">O caminho do arquivo xml.</param>        
        public static T Deserialize<T>(string path)
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                T obj = IntermediateSerializer.Deserialize<T>(reader, null);
                return obj;
            }
        }
    }
}