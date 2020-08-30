SOBRE

Este projeto é um exemplo de como fazer um Hello World utilizando algumas classes e estruturas contidas no MonoGame Caffe.

EXPLICAÇÃO

1) As variáveis:

TextEntity textEntity;
AnimatedEntity rectangleEntity;
AnimatedEntity backEntity;

fornecem acesso ao desenho de textos e sprites. 

2) Neste contexto não utilizamos um sprite externo, mas criamos um retângulo através do método estático AnimatedEntity.CreateRectangle().

3) O objeto textEntity recebe uma string a ser adicionada na propriedade TextBuilder, que é um objeto da classe StringBuilder.

4) Ao adicionar o texto tornou-se necessário atualizar seu tamanho através do método textEntity.UpdateBounds();

5) ColorComponent é um componente que pode ser adicionado a uma entidade para obter uma mudança de cor dinamicamente.

6) É necessário atualizar e desenhar as entidades nos métodos Update e Draw da classe Game.


