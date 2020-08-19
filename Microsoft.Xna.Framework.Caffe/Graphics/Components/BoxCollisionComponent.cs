// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que verifica uma colisão de boxes entre entidades. 
    /// </summary>
    public class BoxCollisionComponent : EntityComponent
    {
        /// <summary>Obtém ou define se o componente deve utilizar a tela corrente (caso LayeredScreen) para busca das entidades para colisão.</summary>
        public bool UseCurrentScreen { get; set; } = true;
        /// <summary>Obtém ou define as entidades a serem utilizadas caso UseCurrentScreen seja falso.</summary>
        public List<Entity2D> Entities { get; set; } = new List<Entity2D>();

        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre CollisionBoxes.</summary>
        public BoxCollisionAction<CollisionBox, CollisionBox> CxCCollision;
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre um CollisionBox e um AttackBox.</summary>
        public BoxCollisionAction<CollisionBox, AttackBox> CxACollision;        
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre AttackBoxes.</summary>
        public BoxCollisionAction<AttackBox, AttackBox> AxACollision;
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre um CollisionBox e um retângulo.</summary>
        public BoxCollisionAction<CollisionBox, Rectangle> CxBCollision;
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre um AttackBox e um retângulo.</summary>
        public BoxCollisionAction<AttackBox, Rectangle> AxBCollision;

        /// <summary>
        /// Inicializa uma nova instância de BoxCollisionComponent.
        /// </summary>
        public BoxCollisionComponent() : base()
        {
            Name = nameof(BoxCollisionComponent);
        }

        /// <summary>
        /// Inicializa uma nova instância da classe BoxCollisionComponent como uma cópia de outro boxCollisionComponent.
        /// </summary>
        /// <param name="destination">A entidade a ser associada esse componente.</param>
        /// <param name="source">A origem para cópia.</param>
        public BoxCollisionComponent(Entity2D destination, BoxCollisionComponent source) : base(destination, source)
        {
            AxACollision = source.AxACollision;
            CxACollision = source.CxACollision;
            CxCCollision = source.CxCCollision;
        }

        /// <summary>
        /// Cria uma nova instância de BoxCollisionComponent quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">O objeto a ser copiado</param>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        public override T Clone<T>(T source, Entity2D destination)
        {
            if (source is BoxCollisionComponent)
                return (T)Activator.CreateInstance(typeof(BoxCollisionComponent), destination, source);
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
                if (screen != null && screen is LayeredScreen ls)
                {
                    //Busca todas as entidades vísiveis da tela.
                    foreach (var other in ls.DrawableEntities)
                    {
                        // Prossegue se a entidade atual é diferente da entidade da lista.
                        if (!Entity.Equals(other))
                        {
                            //Checa a colisão
                            var e = (AnimatedEntity)Entity;
                            var o = (AnimatedEntity)other;

                            Check(e, o, gameTime);
                        }
                    }
                }                
            }
            else
            {
                foreach (var other in Entities)
                {
                    // Prossegue se a entidade atual é diferente da entidade da lista.
                    if (!Entity.Equals(other))
                    {
                        //Checa a colisão
                        var e = (AnimatedEntity)Entity;
                        var o = (AnimatedEntity)other;

                        Check(e, o, gameTime);
                    }
                }
            }

            base.Update(gameTime);
        }

        private void Check(AnimatedEntity e, AnimatedEntity o, GameTime gameTime)
        {
            RectangleCollisionResult result = new RectangleCollisionResult();

            //Procura todos os CollisionBox da entidade
            for (int i = 0; i < e.CollisionBoxes.Count; i++)
            {
                CollisionBox ecb = e.CollisionBoxes[i];

                for (int j = 0; j < o.CollisionBoxes.Count; j++)
                {
                    CollisionBox ocb = o.CollisionBoxes[j];

                    if (Collision.BoundsCollision(ecb.Bounds, ocb.Bounds))
                    {
                        result.Intersection = Rectangle.Intersect(ecb.Bounds, ocb.Bounds);
                        result.Subtract = Collision.IntersectionSubtract(ecb.Bounds, ocb.Bounds);

                        CxCCollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, CollisionBox>(ecb, ocb), result, o);
                    }
                }

                for (int j = 0; j < o.AttackBoxes.Count; j++)
                {
                    AttackBox oab = o.AttackBoxes[j];

                    if (Collision.BoundsCollision(ecb.Bounds, oab.Bounds))
                    {
                        result.Intersection = Rectangle.Intersect(ecb.Bounds, oab.Bounds);
                        result.Subtract = Collision.IntersectionSubtract(ecb.Bounds, oab.Bounds);

                        CxACollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, AttackBox>(ecb, oab), result, o);
                    }
                }

                if (Collision.BoundsCollision(e.CollisionBoxes[i].Bounds, o.Bounds))
                {
                    result.Intersection = Rectangle.Intersect(ecb.Bounds, o.Bounds);
                    result.Subtract = Collision.IntersectionSubtract(ecb.Bounds, o.Bounds);

                    CxBCollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, Rectangle>(ecb, o.Bounds), result, o);
                }
            }

            //Procura todos os AttackBox da entidade
            for (int i = 0; i < e.AttackBoxes.Count; i++)
            {
                AttackBox eab = e.AttackBoxes[i];

                for (int j = 0; j < o.CollisionBoxes.Count; j++)
                {
                    CollisionBox ocb = o.CollisionBoxes[j];

                    if (Collision.BoundsCollision(eab.Bounds, ocb.Bounds))
                    {
                        result.Intersection = Rectangle.Intersect(eab.Bounds, ocb.Bounds);
                        result.Subtract = Collision.IntersectionSubtract(eab.Bounds, ocb.Bounds);

                        CxACollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, AttackBox>(ocb, eab), result, o);
                    }
                }

                for (int j = 0; j < o.AttackBoxes.Count; j++)
                {
                    AttackBox oab = o.AttackBoxes[j];

                    if (Collision.BoundsCollision(eab.Bounds, oab.Bounds))
                    {
                        result.Intersection = Rectangle.Intersect(eab.Bounds, oab.Bounds);
                        result.Subtract = Collision.IntersectionSubtract(eab.Bounds, oab.Bounds);

                        AxACollision?.Invoke(Entity, gameTime, new Tuple<AttackBox, AttackBox>(eab, oab), result, o);
                    }
                }

                if (Collision.BoundsCollision(e.AttackBoxes[i].Bounds, o.Bounds))
                {
                    result.Intersection = Rectangle.Intersect(eab.Bounds, o.Bounds);
                    result.Subtract = Collision.IntersectionSubtract(eab.Bounds, o.Bounds);

                    AxBCollision?.Invoke(Entity, gameTime, new Tuple<AttackBox, Rectangle>(eab, o.Bounds), result, o);
                }
            }
        }
    }
}