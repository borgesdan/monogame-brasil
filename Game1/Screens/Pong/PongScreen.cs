// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Game1.Screens
{
    class PongScreen : Screen
    {
        //A velocidade máxima que a bola pode ter no eixo X.
        readonly float MAX_VELOCITY_BALL_X = 20f;
        //A velocidade inicial da bola.
        readonly Vector2 START_VELOCITY_BALL = new Vector2(1.5F, 1.5F);

        //As entidades.
        AnimatedEntity ball;
        AnimatedEntity rightPlayer;
        AnimatedEntity leftPlayer;
        TextEntity text;

        //Sons
        //https://freesound.org/people/Anthousai/sounds/406277/
        SoundEffect hit;
        //https://freesound.org/people/Breviceps/sounds/450613/
        SoundEffect goal;

        /// <summary>
        /// Inicializa uma nova instância da classe PongScreen.
        /// </summary>
        /// <param name="manager">O gerenciador de telas.</param>
        public PongScreen(ScreenManager manager, string name) : base(manager: manager, name: name, loadScreen: true)
        {
            //A classe Screen dispõe de 4 construtores,
            //Mas aqui sobrecaregamos somente um,
            //Já setando loadScreen como True.
            
            //Definimos a cor de fundo.
            BackgroundColor = Color.DarkGreen;
        }

        //Aqui é onde iremos carregar os objetos da cena.
        public override void Load()
        {
            //Carregamento dos sons
            hit = Game.Content.Load<SoundEffect>("ball_hit");
            goal = Game.Content.Load<SoundEffect>("up");

            //Inicializamos a bola
            //Criamos uma entidade retangular.
            ball = AnimatedEntity.CreateRectangle(Game, "ball", new Point(10, 10), Color.Black);
            //Posicionamos no meio da tela.
            ball.Transform.SetViewPosition(AlignType.Center);
            //Iniciamos a velocidade da bola.
            ball.Transform.SetVelocity(START_VELOCITY_BALL);

            //Criamos um componente para verificar se a bola está fora dos limites da tela.
            OutOfBoundsComponent outBoundsBall = new OutOfBoundsComponent(Game.GraphicsDevice.Viewport.Bounds);
            //Definimos o que acontece caso a bola saia dos limites da tela.
            outBoundsBall.OnOutOfBounds += (Entity2D e, GameTime gt, Vector2 result) =>
            {
                //Lembrando que se a variável 'result' tiver o valor 0,0, então a bola permanece dentro dos limites da tela.
                //Se sair no eixo X.
                if (result.X != 0)
                {
                    //Houve um gol.
                    //Tocamos o som.
                    var g = goal.CreateInstance();
                    g.Play();

                    //Alinhamos a bola ao centro
                    e.Transform.SetViewPosition(AlignType.Center);

                    //Invertemos a posição dependendo do lado em que a bola saiu.
                    if(result.X > 0)
                        e.Transform.SetVelocity(-START_VELOCITY_BALL);
                    else
                        e.Transform.SetVelocity(START_VELOCITY_BALL);
                }
                //se sair no eixo Y.
                if (result.Y != 0)
                {
                    //Posiciona a bola nos limites da tela. "result.Y" é o quando a bola saiu dos limites da tela.
                    e.Transform.Y -= result.Y;
                    //Inverte a velocidade.
                    e.Transform.InvertVelocityY();
                }                
            };

            //Definimos a colisão da bola
            BasicCollisionComponent ballCollision = new BasicCollisionComponent();
            //Aqui definimos o que acontece ao colidir com as entidades da tela.
            ballCollision.OnCollision += (Entity2D source, GameTime gameTime, Rectangle intersection, Entity2D collidedEntity) =>
            {
                //Verificamos se a bola colidiu com o texto, se sim, não calculamos nada.
                if (collidedEntity.Name.Equals("text"))
                    return;

                //Tocamos o som de colisão.
                var h = hit.CreateInstance();
                h.Play();

                var t = source.Transform;
                //Vamos inverter a velocidade da bola.
                t.InvertVelocityX();

                //Se a velocidade em X é negativa
                if(t.Xv < 0)
                {
                    //e se a velocidade é maior que -MAX_VELOCITY_BALL, então acrescenta o valor.
                    if (t.Xv > -MAX_VELOCITY_BALL_X)
                        t.Xv += -0.5f;
                }
                //Se não se a velocidade é positiva
                else if(t.Xv > 0)
                {
                    //e se a velocidade é menor que MAX_VELOCITY_BALL, então acrescenta o valor.
                    if (t.Xv < MAX_VELOCITY_BALL_X)
                        t.Xv += 0.5f;
                }
            };

            //Adiciona os componentes da bola
            ball.Components.Add(outBoundsBall);
            ball.Components.Add(ballCollision);

            //Criamos o jogador do lado direito
            //Criamos um retângulo
            rightPlayer = AnimatedEntity.CreateRectangle(Game, "rightPlayer", new Point(20, 200), Color.Black);
            //Colocamos ele no lado direito da tela.
            rightPlayer.Transform.SetViewPosition(AlignType.Right);
            //Afastamos ele um pouco.
            rightPlayer.Transform.X -= 10;
            //Fazemos sua atualização.
            rightPlayer.OnUpdate += (Entity2D e, GameTime gt) =>
            {
                //Recebemos as entradas do usuário.
                var input = Manager.Input;

                //Se for teclado para cima ou para baixo, movimentamos o jogador
                if(input.Keyboard.IsDown(Keys.Up))
                {
                    e.Transform.Y -= 5;
                }
                else if(input.Keyboard.IsDown(Keys.Down))
                {
                    e.Transform.Y += 5;
                }
            };

            //Criamos outro componente para verificar se o jogador permanece nos limites da tela.
            OutOfBoundsComponent outBoundsPlayer = new OutOfBoundsComponent(Game.GraphicsDevice.Viewport.Bounds);
            outBoundsPlayer.OnOutOfBounds += (Entity2D e, GameTime gt, Vector2 result) =>
            {
                //Se saiu dos limites no eixo Y, colocamos ele de volta na tela.
                if (result.Y != 0)
                {
                    e.Transform.Y -= result.Y;
                }
            };

            //Adicionamos então o componente
            rightPlayer.Components.Add(outBoundsPlayer);

            //Criamos o jogador do lado esquerdo.
            //Aqui do mesmo modo que a entidade rightPlayer.
            leftPlayer = AnimatedEntity.CreateRectangle(Game, "leftPlayer", new Point(20, 200), Color.Black);
            leftPlayer.Transform.SetViewPosition(AlignType.Left);
            leftPlayer.Transform.X += 10;
            leftPlayer.OnUpdate += (Entity2D e, GameTime gt) =>
            {
                var input = Manager.Input;

                if (input.Keyboard.IsDown(Keys.W))
                {
                    e.Transform.Y -= 5;
                }
                else if (input.Keyboard.IsDown(Keys.S))
                {
                    e.Transform.Y += 5;
                }
            };
            //Adicionamos o mesmo componente do rightPlayer, mas criando uma nova instância dele.
            leftPlayer.Components.Add(new OutOfBoundsComponent(outBoundsPlayer));

            //O texto de exibição.
            text = new TextEntity(this, "text");
            text.SetFont("default");
            text.Text.Append("Teclas [W e S] ou [Up e Down] para movimentacao."); 

            //Adiciona as entidades a cena.
            Add(ball, rightPlayer, leftPlayer, text);

            //Chamamos o método da base.
            base.Load();
        }

        //Aqui iremos colocar o código para quando for necessário resetar a tela.
        public override void Reset()
        {
            //Alinhamos a bola e os componentes
            ball.Transform.SetViewPosition(AlignType.Center);
            ball.Transform.InvertVelocityX();
            rightPlayer.Transform.SetViewPosition(AlignType.Right);
            rightPlayer.Transform.X -= 10;
            leftPlayer.Transform.SetViewPosition(AlignType.Left);
            leftPlayer.Transform.X += 10;

            //Chamamos o método base.
            base.Reset();
        }
    }
}