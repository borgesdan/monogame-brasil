// Danilo Borges Santos, 2020.

using System;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que verifica os eventos relacionados ao mouse na entidade.
    /// </summary>
    public class MouseEventsComponent : EntityComponent
    {
        private MouseState old;
        private bool mouseOn = false;

        /// <summary>Ocorre quando um botão do mouse é pressionado.</summary>
        public Action<Entity2D, MouseState> MouseDown;
        /// <summary>Ocorre quando um botão do mouse é liberado.</summary>
        public Action<Entity2D, MouseState> MouseUp;
        /// <summary>Ocorre quando o ponteiro do mouse estava fora e entrou nos limites da entidade.</summary>
        public Action<Entity2D, MouseState> MouseEnter;
        /// <summary>Ocorre quando o ponteiro do mouse estava dentro e saiu nos limites da entidade.</summary>
        public Action<Entity2D, MouseState> MouseLeave;
        /// <summary>Ocorre quando o ponteiro do mouse se encontra dentro doslimites da entidade.</summary>
        public Action<Entity2D, MouseState> MouseOn;

        /// <summary>
        /// Inicializa uma nova instância de MouseEventsComponent.
        /// </summary>
        public MouseEventsComponent() : base()
        {
        }

        /// <summary>
        /// Inicializa uma nova instância de MouseEventsComponent como cópia de outra instância.
        /// </summary>
        /// <param name="destination">A entidade em que o componente será associado.</param>
        /// <param name="source">O componente de origem.</param>
        public MouseEventsComponent(Entity2D destination, MouseEventsComponent source) : base(destination, source)
        {
            mouseOn = source.mouseOn;
            MouseDown = source.MouseDown;
            MouseUp = source.MouseUp;
            MouseEnter = source.MouseEnter;
            MouseLeave = source.MouseLeave;
            MouseOn = source.MouseOn;
        }

        /// <summary>
        /// Coia uma instância de MouseEventsComponent quando não é possível usar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo da instância.</typeparam>
        /// <param name="source">O componente a ser copiado.</param>
        /// <param name="destination">A entidade a ser associada.</param>
        public override T Clone<T>(T source, Entity2D destination)
        {
            if (source is MouseEventsComponent)
                return (T)Activator.CreateInstance(typeof(MouseEventsComponent), destination, source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle bounds = Entity.Bounds;
            Screen screen = Entity.Screen;
            var state = Mouse.GetState();
            bool isVisible;
            
            if (screen != null)
                isVisible = Util.CheckFieldOfView(screen, bounds);
            else
                isVisible = Util.CheckFieldOfView(Entity.Game, Camera.Create(), bounds);            

            if (isVisible)
            {
                //Se o ponteiro do mouse consta como dentro dos limites
                if (mouseOn)
                {
                    //mas verificando novamente ele consta fora
                    if (!bounds.Contains(state.Position))
                        MouseLeave?.Invoke(Entity, state);
                }

                //se o ponteiro do mouse está dentro dos limites da entidade.
                if (bounds.Contains(state.Position))
                {
                    //se o ponteiro do mouse se encontrava fora
                    if (!mouseOn)
                        MouseEnter?.Invoke(Entity, state);

                    mouseOn = true;
                    MouseOn?.Invoke(Entity, state);

                    //se um botão foi pressionado enquanto o controle está dentro
                    if (state.LeftButton == ButtonState.Pressed
                        || state.RightButton == ButtonState.Pressed
                        || state.MiddleButton == ButtonState.Pressed)
                    {
                        old = state;
                        MouseDown?.Invoke(Entity, state);
                    }

                    //se um botão foi liberado
                    if (old.LeftButton == ButtonState.Pressed && state.LeftButton == ButtonState.Released
                        || old.RightButton == ButtonState.Pressed && state.RightButton == ButtonState.Released
                        || old.MiddleButton == ButtonState.Pressed && state.MiddleButton == ButtonState.Released)
                    {
                        MouseUp?.Invoke(Entity, state);
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