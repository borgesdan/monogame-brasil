// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que implementa a funcionalidade de uma colisão simples entre retângulos com outras entidades da tela. 
    /// </summary>
    class BasicCollisionComponent : EntityComponent
    {   
        /// <summary>Define através deste evento a ação necessária que deve ocorrer ao acontecer uma colisão.</summary>
        public event CollisionAction OnCollision;

        /// <summary>Obtém True se a entidade está colidindo.</summary>
        public bool IsColliding { get; private set; } = false;

        /// <summary>
        /// Inicializa uma nova instância de BasicCollisionComponent, 
        /// definindo posteriormente a ação necessária para uma ocorrência de colisão no evento
        /// OnCollision desta classe.
        /// </summary>
        public BasicCollisionComponent()  : base()
        {
            Name = nameof(BasicCollisionComponent);
        }

        /// <summary>
        /// Inicializa uma nova instância de BasicCollisionComponent informando um método
        /// como ação necessária quando ocorrer uma colisão.
        /// </summary>
        /// <param name="collisionAction">Um método que define os mesmos parâmetros de um delegate CollisionAction.</param>
        public BasicCollisionComponent(CollisionAction collisionAction) : this()
        {
            OnCollision = collisionAction;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe BasicCollisionComponent como uma cópia.
        /// </summary>
        /// <param name="source">A origem para cópia.</param>
        public BasicCollisionComponent(BasicCollisionComponent source): base(source)
        {
            OnCollision = source.OnCollision;
            IsColliding = source.IsColliding;
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
            foreach(var other in screen.DrawableEntitys)
            {
                // Prossegue se a entidade atual é diferente da entidade da lista.
                if (!Entity.Equals(other))
                {
                    //Checa a colisão
                    var result = Collision.CheckResult(Entity, other);
                    var bvalue = result.Item1;
                    var intersection = result.Item2;

                    if(bvalue)
                    {
                        IsColliding = true;
                        //O que fazer sobre a colisão será definido pelo usuário.
                        OnCollision?.Invoke(Entity, gameTime, intersection, other);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                OnCollision = null;
            }

            base.Dispose(disposing);
        }
    }
}