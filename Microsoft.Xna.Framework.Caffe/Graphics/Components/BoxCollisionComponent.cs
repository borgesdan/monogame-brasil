using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que verifica uma colisão de boxes entre entidades. 
    /// </summary>
    public class BoxCollisionComponent : EntityComponent
    {
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre CollisionBoxes.</summary>
        public BoxCollisionAction<CollisionBox, CollisionBox> CxCCollision;
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre um CollisionBox e um AttackBox.</summary>
        public BoxCollisionAction<CollisionBox, AttackBox> CxACollision;        
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre AttackBoxes.</summary>
        public BoxCollisionAction<AttackBox, AttackBox> AxACollision;
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre um CollisionBox e um retângulo.</summary>
        public BoxCollisionAction<CollisionBox, Rectangle> CxBCollision;
        /// <summary>Encapsula um método a ser chamado como resultado de uma colisão entre um CollisionBox e um retângulo.</summary>
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

            if (screen == null)
                return;

            //Busca todas as entidades vísiveis da tela.
            foreach (var other in screen.DrawableEntities)
            {
                // Prossegue se a entidade atual é diferente da entidade da lista.
                if (!Entity.Equals(other))
                {
                    //Checa a colisão
                    var e = (AnimatedEntity)Entity;
                    var o = (AnimatedEntity)other;
                    RectangleCollisionResult result = new RectangleCollisionResult();

                    //Procura todos os CollisionBox da entidade
                    for(int i = 0; i < e.CollisionBoxes.Count; i++)
                    {
                        CollisionBox ecb = e.CollisionBoxes[i];

                        for(int j = 0; j < o.CollisionBoxes.Count; j++)
                        {
                            CollisionBox ocb = o.CollisionBoxes[j];

                            if(Collision.BoundsCollision(ecb.Bounds, ocb.Bounds))
                            {
                                result.Intersection = Rectangle.Intersect(ecb.Bounds, ocb.Bounds);
                                result.Subtract = Collision.IntersectionSubtract(ecb.Bounds, ocb.Bounds);

                                CxCCollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, CollisionBox>(ecb, ocb), result, other);
                            }
                        }

                        for (int j = 0; j < o.AttackBoxes.Count; j++)
                        {
                            AttackBox oab = o.AttackBoxes[j];

                            if (Collision.BoundsCollision(ecb.Bounds, oab.Bounds))
                            {
                                result.Intersection = Rectangle.Intersect(ecb.Bounds, oab.Bounds);
                                result.Subtract = Collision.IntersectionSubtract(ecb.Bounds, oab.Bounds);

                                CxACollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, AttackBox>(ecb, oab), result, other);
                            }   
                        }

                        if (Collision.BoundsCollision(e.CollisionBoxes[i].Bounds, other.Bounds))
                        {
                            result.Intersection = Rectangle.Intersect(ecb.Bounds, other.Bounds);
                            result.Subtract = Collision.IntersectionSubtract(ecb.Bounds, other.Bounds);

                            CxBCollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, Rectangle>(ecb, other.Bounds), result, other);
                        }
                    }

                    //Procura todos os AttackBox da entidade
                    for (int i = 0; i < e.AttackBoxes.Count; i++)
                    {
                        AttackBox eab = e.AttackBoxes[i];

                        for (int j = 0; j < o.AttackBoxes.Count; j++)
                        {
                            AttackBox oab = o.AttackBoxes[j];

                            result.Intersection = Rectangle.Intersect(eab.Bounds, oab.Bounds);
                            result.Subtract = Collision.IntersectionSubtract(eab.Bounds, oab.Bounds);

                            AxACollision?.Invoke(Entity, gameTime, new Tuple<AttackBox, AttackBox>(eab, oab), result, other);
                        }                        

                        if (Collision.BoundsCollision(e.AttackBoxes[i].Bounds, other.Bounds))
                        {
                            result.Intersection = Rectangle.Intersect(eab.Bounds, other.Bounds);
                            result.Subtract = Collision.IntersectionSubtract(eab.Bounds, other.Bounds);

                            AxBCollision?.Invoke(Entity, gameTime, new Tuple<AttackBox, Rectangle>(eab, other.Bounds), result, other);
                        }
                    }

                    //Uso do for ao invés do foreach pois a coleção era modificada ao usar o UpdateBounds no evento.

                    //foreach (CollisionBox cb in e.CollisionBoxes)
                    //{
                    //    foreach(CollisionBox ocb in o.CollisionBoxes)
                    //    {
                    //        if(Collision.BoundsCollision(cb.Bounds, ocb.Bounds))
                    //        {
                    //            result.Intersection = Rectangle.Intersect(cb.Bounds, ocb.Bounds);
                    //            result.Subtract = Collision.IntersectionSubtract(cb.Bounds, ocb.Bounds);

                    //            CxCCollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, CollisionBox>(cb, ocb), result, other);
                    //        }
                    //    }

                    //    foreach (AttackBox oab in o.AttackBoxes)
                    //    {
                    //        if (Collision.BoundsCollision(cb.Bounds, oab.Bounds))
                    //        {
                    //            result.Intersection = Rectangle.Intersect(cb.Bounds, oab.Bounds);
                    //            result.Subtract = Collision.IntersectionSubtract(cb.Bounds, oab.Bounds);

                    //            CxACollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, AttackBox>(cb, oab), result, other);
                    //        }
                    //    }

                    //    if(Collision.BoundsCollision(cb.Bounds, other.Bounds))
                    //    {
                    //        result.Intersection = Rectangle.Intersect(cb.Bounds, other.Bounds);
                    //        result.Subtract = Collision.IntersectionSubtract(cb.Bounds, other.Bounds);

                    //        CxBCollision?.Invoke(Entity, gameTime, new Tuple<CollisionBox, Rectangle>(cb, other.Bounds), result, other);
                    //    }
                    //}

                    ////Procura todos os AttackBox da entidade
                    //foreach (AttackBox ab in e.AttackBoxes)
                    //{
                    //    foreach(AttackBox oab in o.AttackBoxes)
                    //    {                            
                    //        result.Intersection = Rectangle.Intersect(ab.Bounds, oab.Bounds);
                    //        result.Subtract = Collision.IntersectionSubtract(ab.Bounds, oab.Bounds);

                    //        AxACollision?.Invoke(Entity, gameTime, new Tuple<AttackBox, AttackBox>(ab, oab), result, other);
                    //    }

                    //    if (Collision.BoundsCollision(ab.Bounds, other.Bounds))
                    //    {                            
                    //        result.Intersection = Rectangle.Intersect(ab.Bounds, other.Bounds);
                    //        result.Subtract = Collision.IntersectionSubtract(ab.Bounds, other.Bounds);

                    //        AxBCollision?.Invoke(Entity, gameTime, new Tuple<AttackBox, Rectangle>(ab, other.Bounds), result, other);
                    //    }
                    //}
                }
            }

            base.Update(gameTime);
        }
    }
}