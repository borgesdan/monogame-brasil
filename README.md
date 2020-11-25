![MonoGame.Caffe Logo](Logos/banner_800.png)

# MonoGame Caffe

Conjunto de classes, estruturas e ferramentas para facilitar o desenvolvimento de jogos 2D com o [MonoGame](http://www.monogame.net/).


## Versão

**2.0.1 (Capuccino) 25/11/2020**

* Limpeza de código;
* Retirada de métodos e propriedades desnecessários;
* Transformação de classes genéricas desnecessárias em classes comuns;
* Implementação de recursos para agilizar o desenvolvimento.

**2.0 (Capuccino) 01/10/2020**

* Implementação de atores (Actor);
* As classes Sprite, Animation, TextActor (anterior TextEntity) e AnimatedActor (anterior AnimatedEntity) herdam da classe Actor;
* Colisão por pixel;
* Facilidade em receber um array Color[] da textura observando a propriedade SpriteEffects;
* Camadas de telas dão suporte a atores, cada uma com sua variação;
* Um SpriteFrame pode conter vários CollisionBox e AttackBox e pertecem a classe BoxCollection;
* Implementação da estrutura Triangle;
* IsometricTile agora implementa um ator em vez de animação;
* Implementação do GhostComponent que facilita exibir imagens repetidas de um ator;
* MouseEventComponent implementa a propriedade de verificação de duplo clique em um ator;

* Retirada das entidades (Entity2D);
* Retirada da classe SubScreenManager;
* Retirada do método SetViewPosition, o método SetPosition implementa a mesma função;

* Entre outras funcionalidades.

**1.0 (Conillon) 11/04/2020**


## Autor

* **Danilo Borges Santos** - [borgesdan](https://github.com/borgesdan)

## Licença

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details