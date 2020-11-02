// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Representa um componente que executa tarefas pré-definidas em objetos que herdam a classe Actor.
    /// </summary>
    public abstract class ActorComponent : IUpdateDrawable, IDisposable
    {
        /// <summary>
        /// Define se o componente será desenhado atrás ou na frente do ator.
        /// </summary>
        public enum DrawPriority : byte
        {
            Back, Forward
        }

        /// <summary>
        /// Obtém ou define se o componente será desenhado atrás ou na frente do ator.
        /// </summary>
        public DrawPriority Priority { get; protected set; } = DrawPriority.Back;
        /// <summary>
        /// Obtém o ator o qual esse componente é associado.
        /// </summary>
        public Actor Actor { get; protected set; } = default;
        /// <summary>
        /// Obtém ou define o nome específico do componente.
        /// </summary>
        public string Name { get; set; } = nameof(ActorComponent);
        /// <summary>
        /// Obtém ou define a disponibilidade do componente.
        /// </summary>
        public EnableGroup Enable { get; set; } = new EnableGroup();

        protected ActorComponent(Actor actor) 
        {
            Actor = actor;
            Actor.Components.Add(this);
        }

        protected ActorComponent(Actor destination, ActorComponent source)
        {
            Actor = destination;
            Enable = new EnableGroup(source.Enable.IsEnabled, source.Enable.IsVisible);
            Name = source.Name;
        }        

        /// <summary>
        /// Desenha o componente.
        /// </summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">A instância atual de um SpriteBatch.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//
        bool disposed = false;

        /// <summary>Libera os recursos dessa instância.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.Name = null;
                this.Actor = default;
            }

            disposed = true;
        }
    }
}