// Danilo Borges Santos, 2020.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que expõe e acessa os componentes da um ator.
    /// </summary>
    public class ComponentGroup : IUpdateDrawable
    {
        /// <summary>Obtém ou define a lista de componentes.</summary>
        public List<ActorComponent> List { get; } = new List<ActorComponent>();

        /// <summary>
        /// Inicializa uma nova instância do ComponentGroup.
        /// </summary>
        public ComponentGroup() { }

        /// <summary>
        /// Inicializa uma nova instância como cópia de outro ComponentGroup.
        /// </summary>
        /// <param name="source">O ComponentGroup a ser copiado.</param>
        public ComponentGroup(ComponentGroup source)
        {            
            foreach(ActorComponent c in source.List)
            {
                var clone = Util.Clone(c, c);
                this.List.Add(clone);
            }
        }

        /// <summary>Obtém o primeiro componente encontrado do tipo informado.</summary>
        /// <typeparam name="T">O tipo do componente.</typeparam>
        public List<T> GetAll<T>() where T : ActorComponent
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
        public T Get<T>() where T : ActorComponent
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
        public T GetByName<T>(string name) where T : ActorComponent
        {
            var find = List.Find(x => x.Name.Equals(name));

            if (find != null)
                return (T)find;
            else
                return null;
        }

        //A ordem da adição dos componentes pode impactar no resultado desejado.
        //Por exemplo ao adicionar "entity.Components.Add(mouseEvents)" primeiro - sendo que mouseEvents é uma instância de MouseEventsComponent,
        //e depois "entity.Components.Add(followMouse)" - sendo que followMouse é uma instância de FollowMouseComponent;
        //Ao tentar verificar com mouseEvents se o ponteiro do mouse está sobre a entidade e depois move-la com followMouse
        //acontece um problema de não reconhecer bem o comando. O que não acontece se 'followMouse' for adicionado primeiro
        //na lista de componentes.

        /// <summary>Adiciona um componente na lista de Componentes.</summary>
        /// <param name="component">O componente a ser adicionado.</param>
        public void Add(ActorComponent component)
        {
            List.Add(component);
        }

        /// <summary>Remove um componente na lista de Componentes.</summary>
        /// <typeparam name="T">O tipo do componente..</typeparam>
        public void Remove<T>() where T : ActorComponent
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
            foreach (var component in List)
                component.Update(gameTime);
        }

        /// <summary>Desenha o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var component in List)
                component.Draw(gameTime, spriteBatch);
        }

        /// <summary>Desenha o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        /// <param name="priority">Define o componente que será desenhado atráves de sua prioridade.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, ActorComponent.DrawPriority priority)
        {
            foreach (var component in List)
            {
                if (component.Priority == priority)
                    component.Draw(gameTime, spriteBatch);
            }
        }
    }
}