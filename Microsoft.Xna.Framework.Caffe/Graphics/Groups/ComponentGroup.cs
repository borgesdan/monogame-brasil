// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que expõe e acessa os componentes da entidade.
    /// </summary>
    public class ComponentGroup : IUpdateDrawable
    {
        /// <summary>Obtém a entidade associada.</summary>
        public Entity2D Entity { get; private set; } = null;
        /// <summary>Obtém a lista de componentes.</summary>
        public List<EntityComponent> List { get; } = new List<EntityComponent>();

        /// <summary>
        /// Inicializa uma nova instância do ComponentGroup
        /// </summary>
        /// <param name="entity"></param>
        public ComponentGroup(Entity2D entity)
        {
            Entity = entity;
        }

        /// <summary>Obtém o primeiro componente encontrado seguindo o tipo informado.</summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        /// <returns>Retorna uma lista com todos os componentes encontrados.</returns>
        public List<T> GetAll<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = List.FindAll(x => x.GetType().Equals(t_type));

            List<T> temp = new List<T>();

            foreach (var f in find)
                temp.Add((T)f);

            return temp;
        }

        /// <summary>Obtém o primeiro componente encontrado seguindo o tipo informado.</summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        /// <returns>Retorna o primeiro componente encontrado na lista de Components.</returns>
        public T Get<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = List.Find(x => x.GetType().Equals(t_type));

            return (T)find;
        }

        /// <summary>
        /// Obtém o primeiro componente encontrado seguindo o tipo informado.
        /// </summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        /// <param name="internalName">O nome interno do componente. Normalmente utilizando nameof(T) onde 'T' é o tipo dele.</param>
        /// <returns>Retorna um componente através dos parâmetros solicitados.</returns>
        public T GetByName<T>(string internalName) where T : EntityComponent
        {
            var find = List.Find(x => x.Name.Equals(internalName));

            if (find != null)
                return (T)find;
            else
                return null;
        }

        /// <summary>Adiciona um componente na lista de Componentes.</summary>
        /// <param name="component">Uma instância da classe EntityComponent.</param>
        public void Add(EntityComponent component)
        {
            component.Entity = Entity;
            List.Add(component);
        }

        /// <summary>Remove um componente na lista de Componentes.</summary>
        /// <typeparam name="T">O tipo do componente herdado de um EntityComponent.</typeparam>
        public void Remove<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = List.Find(x => x.GetType().Equals(t_type));

            if (find != null)
                List.Remove(find);
        }

        /// <summary>Remove um componente na lista de Componentes.</summary>
        /// <param name="internalName">O nome interno do componente. Normalmente utilizando nameof(T) onde 'T' é o tipo dele.</param>
        public void RemoveByName(string internalName)
        {
            var find = List.Find(x => x.Name.Equals(internalName));

            if (find != null)
                List.Remove(find);
        }

        /// <summary>Atualiza a o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public void Update(GameTime gameTime)
        {
            //Atualiza os componentes.
            foreach (var cmp in List)
            {
                if (cmp.Entity == null)
                    cmp.Entity = Entity;

                if(cmp.Enable.IsEnabled)
                {
                    cmp.Update(gameTime);
                }                
            }
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var cmp in List)
            {
                if (cmp.Entity == null)
                    cmp.Entity = Entity;

                if (cmp.Enable.IsVisible)
                {
                    cmp.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}