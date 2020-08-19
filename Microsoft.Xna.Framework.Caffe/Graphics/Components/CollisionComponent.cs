// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que verifica uma colisão entre entidades. 
    /// </summary>
    public class CollisionComponent : EntityComponent
    {   
        /// <summary>Define através deste evento a ação necessária que deve ocorrer ao acontecer uma colisão.</summary>
        public event CollisionAction OnCollision;
        /// <summary>Obtém ou define se o componente deve utilizar a tela corrente (caso LayeredScreen) para busca das entidades para colisão.</summary>
        public bool UseCurrentScreen { get; set; } = true;
        /// <summary>Obtém ou define as entidades a serem utilizadas caso UseCurrentScreen seja falso.</summary>
        public List<Entity2D> Entities { get; set; } = new List<Entity2D>();

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de CollisionComponent
        /// </summary>
        public CollisionComponent()  : base()
        {
            Name = nameof(CollisionComponent);
        }

        /// <summary>
        /// Inicializa uma nova instância de CollisionComponent.
        /// </summary>
        /// <param name="collisionAction">Um método que define os mesmos parâmetros de um delegate CollisionAction.</param>
        public CollisionComponent(CollisionAction collisionAction) : this()
        {
            OnCollision = collisionAction;            
        }

        /// <summary>
        /// Inicializa uma nova instância da classe CollisionComponent como uma cópia de outro CollisionComponent.
        /// </summary>
        /// <param name="destination">A entidade a ser associada esse componente.</param>
        /// <param name="source">A origem para cópia.</param>
        public CollisionComponent(Entity2D destination, CollisionComponent source): base(destination, source)
        {
            OnCollision = source.OnCollision;
            UseCurrentScreen = source.UseCurrentScreen;
            Entities = source.Entities;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância de CollisionComponent quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">O objeto a ser copiado</param>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        public override T Clone<T>(T source, Entity2D destination)
        {
            if (source is CollisionComponent)
                return (T)Activator.CreateInstance(typeof(CollisionComponent), destination, source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            //Recebe a tela em que a entidade está associada.
            var screen = Entity.Screen;

            if(UseCurrentScreen)
            {
                if(screen != null && screen is LayeredScreen ls)
                {
                    //Busca todas as entidades vísiveis da tela.
                    foreach (var other in ls.DrawableEntities)
                    {
                        // Prossegue se a entidade atual é diferente da entidade da lista.
                        if (!Entity.Equals(other))
                        {
                            Check(other, gameTime);
                        }
                    }
                }                
            }
            else
            {
                foreach(var other in Entities)
                {
                    // Prossegue se a entidade atual é diferente da entidade da lista.
                    if (!Entity.Equals(other))
                    {
                        Check(other, gameTime);
                    }
                }
            }            

            base.Update(gameTime);
        }

        private void Check(Entity2D other, GameTime gameTime)
        {
            //Checa a colisão
            var result = Collision.EntityCollision(Entity, other);

            if (result.HasCollided)
            {
                //O que fazer sobre a colisão será definido pelo usuário.
                OnCollision?.Invoke(Entity, gameTime, result, other);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                OnCollision = null;
            }

            base.Dispose(disposing);
        }
    }
}