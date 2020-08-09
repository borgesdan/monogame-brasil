// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe base para um componente de entidade.</summary>
    public abstract class EntityComponent : IUpdateDrawable, IDisposable
    {
        protected bool disposed = false;

        /// <summary>Obtém ou define a entidade de origem.</summary>
        public Entity2D Entity { get; set; } = null;
        /// <summary>
        /// Obtém ou define o nome do componente. Por padrão é definido como nameof(EntityComponent).        
        /// </summary>
        public string Name { get; set; } = nameof(EntityComponent);

        /// <summary>
        /// Obtém ou define a disponibilidade do componente.
        /// </summary>
        public EnableGroup Enable { get; set; } = EnableGroup.Available;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        protected EntityComponent() { }
        protected EntityComponent(Entity2D destination, EntityComponent source) 
        {
            Entity = destination;
            Enable = source.Enable;
            Name = source.Name;
        }

        /// <summary>
        /// Cria uma nova instância de EntityComponent quando não é possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">O objeto EntityComponent a ser copiado.</param>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        public abstract T Clone<T>(T source, Entity2D destination) where T : EntityComponent;

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>Desenha o componente, se necessário.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

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
                Entity = null;
                Name = null;
            }

            disposed = true;
        }
    }
}