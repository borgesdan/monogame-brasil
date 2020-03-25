// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe estática que expõe métodos para verificação de colisão entre duas entidades.</summary>
    public static class Collision
    {
        /// <summary>Verifica se os limites da entidade 1 está intersectando os limites da entidade 2.</summary>
        /// <param name="entity">A primeira entidade.</param>
        /// <param name="other">A segunda entidade.</param>
        public static CollisionResult EntityCollision(Entity2D entity, Entity2D other)
        {
            //O resultado da colisão
            CollisionResult result = new CollisionResult(false, CollisionType.None, Rectangle.Empty, new PolygonCollisionResult());

            //Se alguma das entidades estão rotacionada, então a colisão tem que ser por polígono.
            if (entity.Transform.Rotation != 0 && other.Transform.Rotation != 0)
            {
                //Calcula a colisão por polígono
                var r = PolygonCollision(entity.BoundsR, other.BoundsR, entity.Transform.Velocity);
                //Adiciona o resultado.
                result = new CollisionResult(r.Intersect, CollisionType.Polygon, Rectangle.Empty, r);
            }
            //se todas as as entidades estão sem rotação, então pode-se calcular a colisão pela intersecção do retângulo.
            else
            {
                var b = entity.Bounds;
                var ob = other.Bounds;

                //Se houve a intersecção
                if (b.Intersects(ob))
                {
                    Rectangle intersect = Rectangle.Intersect(entity.Bounds, other.Bounds);
                    result = new CollisionResult(true, CollisionType.Rectangle, intersect, new PolygonCollisionResult());
                } 
            }

            return result;
        }

        /// <summary>
        /// Verifica se os limites do retângulo 1 está intersectando os limites da retângulo 2.
        /// </summary>
        /// <param name="bounds">O primeiro retângulo.</param>
        /// <param name="otherBounds">O segundo retângulo</param>
        public static bool BoundsCollision(Rectangle bounds, Rectangle otherBounds)
        {
            if (bounds.Intersects(otherBounds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifica se os limites do retângulo 1 está intersectando os limites da retângulo 2 retornando um.
        /// </summary>
        /// <param name="bounds">O primeiro retângulo.</param>
        /// <param name="otherBounds">O segundo retângulo</param>
        public static CollisionResult CollisionResult(Rectangle bounds, Rectangle otherBounds)
        {
            bool result = BoundsCollision(bounds, otherBounds);
            Rectangle rec = Rectangle.Empty;

            if (result)
                rec = Rectangle.Intersect(bounds, otherBounds);

            return new CollisionResult(true, CollisionType.Rectangle, rec, new PolygonCollisionResult());
        }        

        /// <summary>
        /// Verifica se um polígono colidiu com outro.
        /// </summary>
        /// <param name="polygonA">O primeiro polígono.</param>
        /// <param name="polygonB">O segundo polígono.</param>
        /// <param name="velocity">A sua velocidade.</param>
        // Check if polygon A is going to collide with polygon B for the given velocity
        public static PolygonCollisionResult PolygonCollision(Polygon polygonA, Polygon polygonB, Vector2 velocity)
        {
            //Código base disponível em:
            //https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection
            //Autor: Laurent Cozic

            PolygonCollisionResult result = new PolygonCollisionResult();
            result.Intersect = true;
            result.WillIntersect = true;

            int edgeCountA = polygonA.Edges.Count;
            int edgeCountB = polygonB.Edges.Count;
            float minIntervalDistance = float.PositiveInfinity;
            Vector2 translationAxis = new Vector2();
            Vector2 edge;

            // Loop through all the edges of both polygons
            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = polygonA.Edges[edgeIndex];
                }
                else
                {
                    edge = polygonB.Edges[edgeIndex - edgeCountA];
                }

                // ===== 1. Find if the polygons are currently intersecting =====

                // Find the axis perpendicular to the current edge
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting
                if (IntervalDistance(minA, maxA, minB, maxB) > 0) result.Intersect = false;

                // ===== 2. Now find if the polygons *will* intersect =====

                // Project the velocity on the current axis
                float velocityProjection = Vector2.Dot(axis, velocity);

                // Get the projection of polygon A during the movement
                if (velocityProjection < 0)
                {
                    minA += velocityProjection;
                }
                else
                {
                    maxA += velocityProjection;
                }

                // Do the same test as above for the new projection
                float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance > 0) result.WillIntersect = false;

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.Intersect && !result.WillIntersect) break;

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation vector
                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {
                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;

                    Vector2 d = polygonA.Center - polygonB.Center;
                    if (Vector2.Dot(d, translationAxis) < 0) translationAxis = -translationAxis;
                }
            }

            // The minimum translation vector can be used to push the polygons appart.
            // First moves the polygons by their velocity
            // then move polygonA by MinimumTranslationVector.
            if (result.WillIntersect) result.MinimumTranslationVector = translationAxis * minIntervalDistance;

            return result;
        }

        // Calculate the distance between [minA, maxA] and [minB, maxB]
        // The distance will be negative if the intervals overlap
        private static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            //Código base disponível em:
            //https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection
            //Autor: Laurent Cozic

            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        // Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        private static void ProjectPolygon(Vector2 axis, Polygon polygon, ref float min, ref float max)
        {
            //Código base disponível em:
            //https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection
            //Autor: Laurent Cozic

            // To project a point on an axis use the dot product            
            float d = Vector2.Dot(axis, polygon.Points[0]);
            min = d;
            max = d;
            for (int i = 0; i < polygon.Points.Count; i++)
            {
                d = Vector2.Dot(polygon.Points[i], axis);
                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }
    }
}