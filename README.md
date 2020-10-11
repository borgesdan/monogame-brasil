![MonoGame.Caffe Logo](Logos/banner_800.png)

# MonoGame Caffe

Conjunto de classes, estruturas e ferramentas para facilitar o desenvolvimento de jogos 2D com o [MonoGame](http://www.monogame.net/).


## Versão

**2.0 (Capuccino) 01/10/2020**

* Implementação de atores (Actor);
* As classes Sprite, Animation e Entity2D herdam da classe Actor;
* Sprite e Animation tem suporte a transformações (implementam a propriedade TransformGroup);
* Componentes e telas dão suporte a atores, não somente as Entidades;
* Colisão por pixel;
* Facilidade em receber um array Color[] da textura observando a propriedade SpriteEffects;
* Camadas de telas dão suporte a atores, cada uma com sua variação;
* Um SpriteFrame pode conter vários CollisionBox e AttackBox e pertecem a classe BoxCollection;
* Implementação da estrutura Triangle;
* IsometricTile agora implementa um ator em vez de animação;
* Implementação do GhostComponent que facilita exibir imagens repetidas de um ator;

* Retirada da classe SubScreenManager;
* Retirada do método SetViewPosition, o método SetPosition implementa a mesma função;

**1.0 (Conillon) 11/04/2020**


## Autor

* **Danilo Borges Santos** - [borgesdan](https://github.com/borgesdan)

## Licença

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details