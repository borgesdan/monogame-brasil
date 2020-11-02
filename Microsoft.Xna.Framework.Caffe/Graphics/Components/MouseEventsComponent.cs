// Danilo Borges Santos, 2020.

using System;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que verifica os eventos relacionados ao mouse no ator.
    /// </summary>
    public class MouseEventsComponent : ActorComponent
    {
        private MouseState old;
        private MouseState state;
        private bool mouseOn = false;

        //Double Clicl;
        private float dclickTime = 0;
        private short clicks = 0;

        /// <summary>Obtém ou define a tela ativa.</summary>
        public Screen Screen { get; set; }
        /// <summary>Obtém ou define o tempo para reconhecimento de um duplo clique em milisegundos.</summary>
        public float DoubleClickDelay { get; set; } = 200;

        /// <summary>Ocorre quando um botão do mouse é pressionado.</summary>
        public event Action<Actor, MouseState> Down;
        /// <summary>Ocorre quando um botão do mouse é liberado.</summary>
        public event Action<Actor, MouseState> Up;
        /// <summary>Ocorre quando o ponteiro do mouse estava fora e entrou nos limites do ator.</summary>
        public event Action<Actor, MouseState> Enter;
        /// <summary>Ocorre quando o ponteiro do mouse estava dentro e saiu nos limites do ator.</summary>
        public event Action<Actor, MouseState> Leave;
        /// <summary>Ocorre quando o ponteiro do mouse se encontra dentro dos limites do ator.</summary>
        public event Action<Actor, MouseState> On;
        /// <summary>Ocorre quando ocorre um click duplo sore o ator.</summary>
        public event Action<Actor, MouseState> DoubleClick;

        /// <summary>
        /// Inicializa uma nova instância de MouseEventsComponent.
        /// </summary>
        /// <param name="actor">O ator associado a esse componente.</param>
        public MouseEventsComponent(Actor actor) : base(actor)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância de MouseEventsComponent como cópia de outra instância.
        /// </summary>
        /// <param name="destination">O ator que o componente será associado.</param>
        /// <param name="source">O componente de origem.</param>
        public MouseEventsComponent(Actor destination, MouseEventsComponent source) : base(destination, source)
        {
            mouseOn = source.mouseOn;
            Down = source.Down;
            Up = source.Up;
            Enter = source.Enter;
            Leave = source.Leave;
            On = source.On;
        }        

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            dclickTime += gameTime.ElapsedGameTime.Milliseconds;

            if(dclickTime > DoubleClickDelay)
            {
                dclickTime = 0;
                clicks = 0;
            }

            Rectangle bounds = Actor.Bounds;
            Screen screen = Screen;
            old = state;
            state = Mouse.GetState();
            bool isVisible = true;

            //if (screen != null)
            //    isVisible = Util.CheckFieldOfView(screen, bounds);
            //else
            //    isVisible = Util.CheckFieldOfView(Actor.Game, new Camera(Actor.Game), bounds);

            if (isVisible)
            {
                //Se o ponteiro do mouse consta como dentro dos limites
                if (mouseOn)
                {
                    //mas verificando novamente ele consta fora
                    if (!bounds.Contains(state.Position))
                        Leave?.Invoke(Actor, state);
                }

                //se o ponteiro do mouse está dentro dos limites da entidade.
                if (bounds.Contains(state.Position))
                {
                    //se o ponteiro do mouse se encontrava fora
                    if (!mouseOn)
                        Enter?.Invoke(Actor, state);

                    mouseOn = true;
                    On?.Invoke(Actor, state);

                    //se um botão foi pressionado enquanto o controle está dentro
                    if (state.LeftButton == ButtonState.Pressed
                        || state.RightButton == ButtonState.Pressed
                        || state.MiddleButton == ButtonState.Pressed)
                    {
                        //old = state;
                        Down?.Invoke(Actor, state);  
                    }

                    //se um botão foi liberado
                    if (old.LeftButton == ButtonState.Pressed && state.LeftButton == ButtonState.Released
                        || old.RightButton == ButtonState.Pressed && state.RightButton == ButtonState.Released
                        || old.MiddleButton == ButtonState.Pressed && state.MiddleButton == ButtonState.Released)
                    {
                        Up?.Invoke(Actor, state);
                    }  
                    
                    //Verifica se houve clique duplo
                    if(old.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
                    {
                        clicks++;
                        dclickTime = 0;

                        if(clicks >= 2 && dclickTime < DoubleClickDelay)
                        {
                            DoubleClick?.Invoke(Actor, state);
                            clicks = 0;
                            dclickTime = 0;
                        }
                    }
                }
                else
                {
                    mouseOn = false;
                    //old = state;
                }                
            }

            base.Update(gameTime);
        }
    }
}