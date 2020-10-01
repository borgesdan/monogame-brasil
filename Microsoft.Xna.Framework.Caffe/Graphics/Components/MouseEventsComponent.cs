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
        private bool mouseOn = false;

        /// <summary>Obtém ou define a tela ativa.</summary>
        public Screen Screen { get; set; }

        /// <summary>Ocorre quando um botão do mouse é pressionado.</summary>
        public Action<Actor, MouseState> MouseDown;
        /// <summary>Ocorre quando um botão do mouse é liberado.</summary>
        public Action<Actor, MouseState> MouseUp;
        /// <summary>Ocorre quando o ponteiro do mouse estava fora e entrou nos limites do ator.</summary>
        public Action<Actor, MouseState> MouseEnter;
        /// <summary>Ocorre quando o ponteiro do mouse estava dentro e saiu nos limites do ator.</summary>
        public Action<Actor, MouseState> MouseLeave;
        /// <summary>Ocorre quando o ponteiro do mouse se encontra dentro dos limites do ator.</summary>
        public Action<Actor, MouseState> MouseOn;

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
        /// <param name="destination">A entidade em que o componente será associado.</param>
        /// <param name="source">O componente de origem.</param>
        public MouseEventsComponent(Actor destination, MouseEventsComponent source) : base(destination, source)
        {
            mouseOn = source.mouseOn;
            MouseDown = source.MouseDown;
            MouseUp = source.MouseUp;
            MouseEnter = source.MouseEnter;
            MouseLeave = source.MouseLeave;
            MouseOn = source.MouseOn;
        }        

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle bounds = Actor.Bounds;
            Screen screen = Screen;
            var state = Mouse.GetState();
            bool isVisible;

            if (screen != null)
                isVisible = Util.CheckFieldOfView(screen, bounds);
            else
                isVisible = Util.CheckFieldOfView(Actor.Game, Camera.Create(), bounds);

            if (isVisible)
            {
                //Se o ponteiro do mouse consta como dentro dos limites
                if (mouseOn)
                {
                    //mas verificando novamente ele consta fora
                    if (!bounds.Contains(state.Position))
                        MouseLeave?.Invoke(Actor, state);
                }

                //se o ponteiro do mouse está dentro dos limites da entidade.
                if (bounds.Contains(state.Position))
                {
                    //se o ponteiro do mouse se encontrava fora
                    if (!mouseOn)
                        MouseEnter?.Invoke(Actor, state);

                    mouseOn = true;
                    MouseOn?.Invoke(Actor, state);

                    //se um botão foi pressionado enquanto o controle está dentro
                    if (state.LeftButton == ButtonState.Pressed
                        || state.RightButton == ButtonState.Pressed
                        || state.MiddleButton == ButtonState.Pressed)
                    {
                        old = state;
                        MouseDown?.Invoke(Actor, state);
                    }

                    //se um botão foi liberado
                    if (old.LeftButton == ButtonState.Pressed && state.LeftButton == ButtonState.Released
                        || old.RightButton == ButtonState.Pressed && state.RightButton == ButtonState.Released
                        || old.MiddleButton == ButtonState.Pressed && state.MiddleButton == ButtonState.Released)
                    {
                        MouseUp?.Invoke(Actor, state);
                    }

                    old = state;
                }
                else
                {
                    mouseOn = false;
                    old = state;
                }
            }

            base.Update(gameTime);
        }
    }
}