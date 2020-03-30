// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que implementa a funcionalidade de uma colisão simples entre da tela. 
    /// </summary>
    class BasicCollisionComponent : EntityComponent
    {   
        /// <summary>Define através deste evento a ação necessária que deve ocorrer ao acontecer uma colisão.</summary>
        public event CollisionAction OnCollision;

        /// <summary>Obtém True se a entidade está colidindo.</summary>
        public bool IsColliding { get; private set; } = false;

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de BasicCollisionComponent
        /// </summary>
        public BasicCollisionComponent()  : base()
        {
            Name = nameof(BasicCollisionComponent);
        }

        /// <summary>
        /// Inicializa uma nova instância de BasicCollisionComponent.
        /// </summary>
        /// <param name="collisionAction">Um método que define os mesmos parâmetros de um delegate CollisionAction.</param>
        public BasicCollisionComponent(CollisionAction collisionAction) : this()
        {
            OnCollision = collisionAction;            
        }

        /// <summary>
        /// Inicializa uma nova instância da classe BasicCollisionComponent como uma cópia de outro BasicCollisionComponent.
        /// </summary>
        /// <param name="destination">A entidade a ser associada esse componente.</param>
        /// <param name="source">A origem para cópia.</param>
        public BasicCollisionComponent(Entity2D destination, BasicCollisionComponent source): base(destination, source)
        {
            OnCollision = source.OnCollision;
            IsColliding = source.IsColliding;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância de BasicCollisionComponent quando não é=for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">O objeto BasicCollisionComponent a ser copiado</param>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        public override T Clone<T>(T source, Entity2D destination)
        {
            if (source is BasicCollisionComponent)
                return (T)Activator.CreateInstance(typeof(BasicCollisionComponent), destination, source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            //Valor padrão.
            IsColliding = false;

            //Recebe a tela em que a entidade está associada.
            var screen = Entity.Screen;

            if (screen == null)
                return;
            
            //Busca todas as entidades vísiveis da tela.
            foreach(var other in screen.DrawableEntities)
            {
                // Prossegue se a entidade atual é diferente da entidade da lista.
                if (!Entity.Equals(other))
                {
                    //Checa a colisão
                    var result = Collision.EntityCollision(Entity, other);

                    if(result.HasCollided)
                    {
                        IsColliding = true;
                        //O que fazer sobre a colisão será definido pelo usuário.
                        OnCollision?.Invoke(Entity, gameTime, result, other);
                    }
                }
            }

            base.Update(gameTime);
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