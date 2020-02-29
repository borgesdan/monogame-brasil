# monogame-brasil
Conjunto simpes de classes para auxiliar no desenvolvimento de jogos 2D como a biblioteca C# MonoGame.

A solução MonoGameDevelop tem um projeto do tipo Shared nomeado Microsoft.Xna.Framework.Shared passível de uso em seus projeto ao referenciá-lo.

Como destaques podemos citar:

1º) ScreenManager (gerenciador de telas): que recebe instâncias de Screen (telas do jogo). Você pode então trocar de telas com um ScreenManager.Change(string: name) onde 'name' é o nome da tela. Também é possível carregar uma tela em paralelo, utilizando o conceito de uma tela em "Loading". Para isso é necessário ter uma tela ativa no momento, e usar o método ScreenManager.LoadAsyc(string name, bool callWhenIsFinished), onde 'name' é o nome da tela a ser carregada assícronamente, e se 'callWhenIsFinished' receber o valor 'true', a tela carregada será chaamada ao término da execução da Task.

2º)InputManager: que expõe os GamePad's, o teclado e o mouse de maneira acessíveis para facilitar a verifcação de entradas do usuário.

3º)Entity2D: são entidades a serem usadas na tela do jogo. No momento contamos com AnimatedEntity e TextEntity.

Entre outros recursos.

O projeto está em desenvolvimento.
Contato: danilo.bsto[@]gmail.com

Desenvolvido por: Danilo Borges Santos
Brasil, 2020.
