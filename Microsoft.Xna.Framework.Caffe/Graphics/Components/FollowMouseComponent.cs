using System;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que implementa a funcionalidade da entidade seguir o ponteiro do mouse na tela.
    /// </summary>
    public class FollowMouseComponent : EntityComponent
    {        
        private MouseState state;

        /// <summary>
        /// Inicializa uma nova instância de FollowMouseComponent.
        /// </summary>
        public FollowMouseComponent()
        {
            state = Mouse.GetState();
        }

        /// <summary>
        /// Inicializa uma nova instância de FollowMouseComponent como cópia de outra instância.
        /// </summary>
        /// <param name="destination">A entidade a ser associada.</param>
        /// <param name="source">O componente a ser copiado.</param>
        public FollowMouseComponent(Entity2D destination, FollowMouseComponent source) : base(destination, source)
        {
            state = source.state;
        }

        /// <summary>
        /// Cria uma nova instância de FollowMouseComponent quando não é possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">O objeto a ser copiado.</param>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        public override T Clone<T>(T source, Entity2D destination)
        {
            if (source is FollowMouseComponent)
                return (T)Activator.CreateInstance(typeof(FollowMouseComponent), destination, source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            state = Mouse.GetState();            
            Entity.Transform.SetPosition(state.Position.ToVector2());

            base.Update(gameTime);
        }
    }
}