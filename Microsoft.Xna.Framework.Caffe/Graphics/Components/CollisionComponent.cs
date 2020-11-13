// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que verifica uma colisão de retângulos entre atores. 
    /// </summary>
    public class CollisionComponent : ActorComponent
    {
        /// <summary>Obtém ou define as entidades a serem utilizadas caso a propriedade Screen seja nula.</summary>
        List<Actor> Actors { get; set; } = new List<Actor>();
        /// <summary>Obtém ou define se o componente deve utilizar uma tela do tipo LayeredScreen para busca de atores para colisão.</summary>
        LayeredScreen Screen { get; set; } = null;

        /// <summary>
        /// Encapsula um método a ser chamado no fim do método Update deste component.
        /// <list type="number">
        /// <item>Actor é o ator que implementa esse componente.</item>
        /// <item>Actor é o outro ator que participa da colisão.</item>
        /// <item>GameTime são os valores de tempo do jogo.</item>
        /// <item>CollisionResult é o retorno da colisão.</item>
        /// </list>
        /// </summary>
        public event Action<Actor, Actor, GameTime, CollisionResult> OnCollision;

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de CollisionComponent
        /// </summary>
        /// <param name="actor">Define o ator o qual esse componente será associad.</param>
        /// <param name="screen">Define a tela para busca de entidades, pode ser null.</param>
        public CollisionComponent(Actor actor) : base(actor)
        {
            Name = nameof(CollisionComponent);
        }        

        /// <summary>
        /// Inicializa uma nova instância da classe CollisionComponent como uma cópia de outro CollisionComponent.
        /// </summary>
        /// <param name="destination">O ator a ser associado esse componente.</param>
        /// <param name="source">A origem para cópia.</param>
        public CollisionComponent(Actor destination, CollisionComponent source) : base(destination, source)
        {
            Screen = source.Screen;
            Actors = source.Actors;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//        

        /// <summary>
        /// Define a tela a ser utilizada para buscar os atores para verificação de colisão.
        /// </summary>        
        public void SetScreen(LayeredScreen screen)
        {
            Screen = screen;
            Actors.Clear();
        }

        /// <summary>
        /// Define os atores a serem utilizados para a verificação de colisão ao invés de utilizar uma tela.
        /// </summary>
        public void SetActors(params Actor[] entities)
        {
            Actors.Clear();
            Actors.AddRange(entities);

            Screen = null;
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        protected override void _Update(GameTime gameTime)
        {
            if (Screen != null)
            {
                //Busca todas as entidades vísiveis da tela.
                foreach (Actor other in Screen.DrawableActors)
                {
                    // Prossegue se a entidade atual é diferente da entidade da lista.
                    if (!Actor.Equals(other))
                    {
                        Check(other, gameTime);
                    }
                }
            }
            else
            {
                foreach (var other in Actors)
                {
                    // Prossegue se a entidade atual é diferente da entidade da lista.
                    if (!Actor.Equals(other))
                    {
                        Check(other, gameTime);
                    }
                }
            }

            base._Update(gameTime);
        }

        private void Check(Actor other, GameTime gameTime)
        {
            //Checa a colisão
            var result = Collision.ActorCollision(Actor, other);

            if (result.HasCollided)
            {
                //O que fazer sobre a colisão será definido pelo usuário.
                OnCollision?.Invoke(Actor, other, gameTime, result);
            }
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//

        private bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.Screen = null;
                this.Actors = null;
                this.OnCollision = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}