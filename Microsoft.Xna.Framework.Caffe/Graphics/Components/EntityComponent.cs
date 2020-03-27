// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe base para um componente de entidade.</summary>
    public abstract class EntityComponent : IUpdateDrawable, IDisposable
    {
        private bool disposed = false;

        /// <summary>Obtém ou define a entidade de origem.</summary>
        public Entity2D Entity { get; set; } = null;
        /// <summary>
        /// Obtém ou define o nome do componente. Por padrão é definido pelo nome do componente, exemplo: OtherComponent.Name = nameof(OtherComponent).        
        /// </summary>
        public string Name { get; set; } = nameof(EntityComponent);

        /// <summary>
        /// Obtém ou define a disponibilidade do componente.
        /// </summary>
        public EnableGroup Enable { get; set; } = new EnableGroup(true, true);

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        protected EntityComponent() { }
        protected EntityComponent(EntityComponent source) 
        {
            Entity = source.Entity;
            Enable = source.Enable;
            Name = source.Name;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>Desenha o componente se necessário.</summary>
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