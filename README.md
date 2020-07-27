# MonoGame.Caffe

Conjunto de classes para auxiliar no desenvolvimento de jogos 2D como a biblioteca C# MonoGame.

## Início

A solução tem um projeto do tipo Shared nomeado Microsoft.Xna.Framework.Caffe para uso em seus projeto ao referenciá-lo.

Como destaques podemos citar:

1º) ScreenManager (gerenciador de telas): recebe instâncias da classe Screen (telas do jogo). Você pode trocar de telas ao invocar o método ScreenManager.Change(string: name) onde o argumento 'name' é o nome da tela. Também é possível carregar uma tela em paralelo utilizando o conceito de "load screen".

2º)InputManager: que expõe os GamePad's, o teclado e o mouse de maneira acessíveis para facilitar a verifcação de entradas do usuário.

3º)Entity2D: são entidades a serem usadas na tela do jogo.

Entre outras funcionalidades.

### Pré-requisitos

Por ser um projeto C# do tipo Shared, os requisitos necessários é seu projeto suportar a linguagem e sua compatibilidade com as instruções da biblioteca. Basicamente, referencie o projeto 'Microsoft.Xna.Framework.Caffe' ao seu e faça os ajustes necessários.


## Exemplos

O projeto trabalha com o conceito de entidades (Entity2D), telas (Screen), gerenciador de telas (ScreenManager), e gerenciamento de entradas do usuário (InputManager)

### ScreenManager e 'loading screen'

Você pode fazer, através do gerenciador de telas (ScreenManager), uma tela de carregamento. Para isso criamos uma instância dela e chamamos o método.

```
ScreenManager manager = new ScreenManger(game);
manager.LoadAsyc(string name, bool callWhenIsFinished);
```

onde os argumentos 'name' e 'callWhenIsFinished' no método LoadAsync são respectivamente: o nome da tela a ser carredada de modo assíncrono e se a tela será chamada automaticamente ao fim do carregamento.

ScreenManager também expõe outros métodos para gerenciamento de telas e você pode usá-lo na sua classe principal, normalmente 'Game1', chamando seu método Update e Draw nos respectivos lugares.

```
public class Game1...

ScreenManager manager;

//in load
manager = new ScreenManager(this);

//in update
manager.Update(gameTime);

//in draw

spriteBatch.Begin();
manager.Draw(gameTime, spriteBatch);
spriteBatch.End();

```

### Telas e Entidades

Hierarquicamente, temos o gerenciador de telas (ScreenManager), que recebe as telas (Screen) e dentro das telas temos as entidade (Entity2D).

```
ScreenManager manager = new ScreenManager(game)

Screen screen = new Screen(manager, "nova tela", true);
AnimatedEntity entity = new AnimatedEntity(screen, "entidade");

screen.Add(entity);

manager.Add(screen);

```

Recomenda-se criar uma classe herdada de Screen e criar sua própria tela a partir dela, para assim, sobrecarregar os métodos virtuais disponíveis:

```
public class MinhaTela : Screen
{
	pubblic override void Load() { LoadState = ScreenLoadState.Loading; }
}

```

### Gerenciador de Entradas (InputManager)

Temos também o InputManager, que expõe os acessos ao GamePad, Keyboard e Mouse de maneira mais rápida.

```
InputManager input = new InputManager(game);
if(input.Keyboard.IsPress(Keys.Right))
//andar para frente...

```

Entre outras funcionalidades.

## Construído com:

* [Visual Studio 2019](https://visualstudio.microsoft.com/pt-br/) - IDE
* [MonoGame](http://www.monogame.net/) - O framework base.

## Versão

Versão 1.0 (Conillon) (29/03/2020)

Versão 0.5 (Alho-Poró)

O projeto Monogame.Caffe está em desenvolvimento. Todavia, a partir daqui já é possível construir projetos. Sinta-se a vontade para isso, e principalmente para envios de sugestões.

## Autor

* **Danilo Borges Santos** - [borgesdan](https://github.com/borgesdan)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
