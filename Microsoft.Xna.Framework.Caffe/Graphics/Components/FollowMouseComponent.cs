// Danilo Borges Santos, 2020.

using System;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que implementa a funcionalidade do ator seguir o ponteiro do mouse na tela.
    /// </summary>
    public class FollowMouseComponent : ActorComponent
    {
        private MouseState old;
        private MouseState state;

        /// <summary>Obtém ou define se a entidade deve seguir o mouse.</summary>
        public bool Follow { get; set; } = false;

        /// <summary>
        /// Inicializa uma nova instância de FollowMouseComponent.
        /// </summary>
        /// <param name="actor">O ator associado a esse componente.</param>
        public FollowMouseComponent(Actor actor) : base(actor)
        {
            state = Mouse.GetState();
            old = state;
        }

        /// <summary>
        /// Inicializa uma nova instância de FollowMouseComponent como cópia de outra instância.
        /// </summary>
        /// <param name="destination">O ator a ser associado.</param>
        /// <param name="source">O componente a ser copiado.</param>
        public FollowMouseComponent(Actor destination, FollowMouseComponent source) : base(destination, source)
        {
            old = source.old;
            state = source.state;
        }
        
        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            old = state;
            state = Mouse.GetState();

            var pos = state.Position - old.Position;

            if (Follow)
                Actor.Transform.Move(pos);

            base.Update(gameTime);
        }
    }
}