// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

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
        /// <summary>Obtém ou define a lista de componentes.</summary>
        public List<EntityComponent> List { get; } = new List<EntityComponent>();

        /// <summary>
        /// Inicializa uma nova instância do ComponentGroup.
        /// </summary>
        /// <param name="entity">A entidade a ser associada a esse componente.</param>
        public ComponentGroup(Entity2D entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Inicializa uma nova instância como cópia de outro ComponentGroup.
        /// </summary>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        /// <param name="source">O ComponentGroup a ser copiado.</param>
        public ComponentGroup(Entity2D destination, ComponentGroup source)
        {
            this.Entity = destination;
            
            foreach(EntityComponent c in source.List)
            {
                var clone = c.Clone(c, destination);
                this.List.Add(clone);
            }
        }

        /// <summary>Obtém o primeiro componente encontrado do tipo informado.</summary>
        /// <typeparam name="T">O tipo do componente.</typeparam>
        public List<T> GetAll<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = List.FindAll(x => x.GetType().Equals(t_type));

            List<T> temp = new List<T>();

            foreach (var f in find)
                temp.Add((T)f);

            return temp;
        }

        /// <summary>Obtém o primeiro componente encontrado do tipo informado.</summary>
        /// <typeparam name="T">O tipo do componente.</typeparam>
        public T Get<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = List.Find(x => x.GetType().Equals(t_type));

            return (T)find;
        }

        /// <summary>
        /// Obtém o primeiro componente encontrado do tipo informado.
        /// </summary>
        /// <typeparam name="T">O tipo do componente.</typeparam>
        /// <param name="name">O nome do componente. Normalmente utilizando nameof(T) onde 'T' é o tipo dele.</param>
        public T GetByName<T>(string name) where T : EntityComponent
        {
            var find = List.Find(x => x.Name.Equals(name));

            if (find != null)
                return (T)find;
            else
                return null;
        }

        /// <summary>Adiciona um componente na lista de Componentes.</summary>
        /// <param name="component">O componente a ser adicionado.</param>
        public void Add(EntityComponent component)
        {
            component.Entity = Entity;
            List.Add(component);
        }

        /// <summary>Remove um componente na lista de Componentes.</summary>
        /// <typeparam name="T">O tipo do componente..</typeparam>
        public void Remove<T>() where T : EntityComponent
        {
            var t_type = typeof(T);
            var find = List.Find(x => x.GetType().Equals(t_type));

            if (find != null)
                List.Remove(find);
        }

        /// <summary>Remove um componente na lista de Componentes pelo seu nome.</summary>
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

        /// <summary>Desenha o componente.</summary>
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