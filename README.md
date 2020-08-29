# MonoGame Caffe

Conjunto de classes e estruturas em C# para auxiliar no desenvolvimento de jogos 2D com a biblioteca MonoGame.

## Exemplos

O projeto trabalha com o conceito de entidades (Entity2D), telas (Screen), gerenciador de telas (ScreenManager), e gerenciamento de entradas do usuário (InputManager)

### ScreenManager e 'loading screen'

Você pode fazer, através do gerenciador de telas (ScreenManager), uma tela de carregamento paralela. Para isso criamos uma instância dela e chamamos o método.

```
ScreenManager manager = new ScreenManger(game);
manager.LoadAsyc(string name, bool callWhenIsFinished);
```

onde os parâmetros 'name' e 'callWhenIsFinished' no método LoadAsync são respectivamente: o nome da tela a ser carredada de modo assíncrono e se a tela será chamada automaticamente ao fim do carregamento.

ScreenManager também expõe outros métodos para gerenciamento de telas e você pode usá-lo na sua classe principal, normalmente 'Game1', ao chamar o método Update e Draw nos respectivos lugares.

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

Hierarquicamente, temos o gerenciador de telas (ScreenManager), que recebe as telas (Screen) e dentro destas temos as entidade (Entity2D).

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
	public override void Load() { LoadState = ScreenLoadState.Loading; }
}

```

### Gerenciador de Entradas (InputManager)

Temos também o InputManager, que expõe os acessos ao GamePad, Keyboard e Mouse de maneira mais rápida.

```
InputManager input = new InputManager(game);
if(input.Keyboard.IsPress(Keys.Right))
	entity.Transform.Move(-5,0) //andar para esquerda...
	
```

Entre outras funcionalidades.

## Construído com:

* [Visual Studio 2019](https://visualstudio.microsoft.com/pt-br/) - IDE
* [MonoGame](http://www.monogame.net/) - O framework base.

## Versão

* Versão:............... 1.0 (Conillon)
* Última atualização:... (13/08/2020)

## Autor

* **Danilo Borges Santos** - [borgesdan](https://github.com/borgesdan)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details