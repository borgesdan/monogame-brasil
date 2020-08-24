SOBRE: Microsoft.Xna.Framework.Caffe.Extendend

A biblioteca expõe classes distintas para diversas funcionalidades que não são necessárias por padrão para o funcionamento do Monogame Caffe.
Como cada uma pode utilizar namespaces distintos apague a que não for conveniente para o momento.
Assim utilize a classe necessária para uma situação específica. 
Observe abaixo as observações sobre as classes.

DEPENDÊNCIAS:

# Pipeline/XmlContent.cs utiliza o namespace "MonoGame.Framework.Content.Pipeline" que não se encontra por default nos arquivos de projetos do Monogame
	> caso use o .Net Core adicione nas referências de projeto a linha
		<PackageReference Include="MonoGame.Framework.Content.Pipeline" Version="3.8.0.1641" />
	> caso use o .Net Framework:
		> Vá até C:\Program Files (x86)\MSBuild\MonoGame\v3.0\Tools\
		> Adicione nas referências de projeto o arquivo MonoGame.Framework.Content.Pipeline.dll
	> Para tutoriais sobre o IntermediateSerializer visite http://shawnhargreaves.com/blogindex.html

# Compile/Compiler.cs utiliza o namespace "System.Codedom.Compiler" que não se encontra por default no namespace System
	> caso use o .Net Core adicione nas referências de projeto a linha
		<PackageReference Include="System.CodeDom" Version="4.4.0" />
	> caso use o .Net Framework adicione o System.Codedom nas referências de projeto