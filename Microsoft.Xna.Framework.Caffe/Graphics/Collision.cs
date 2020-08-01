//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

// PolygonCollision, IntervalDistance, ProjectPolygon by
//
// Copyright (c) 2006 Laurent Cozic
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// Code: https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Expõe métodos para verificação de colisão entre duas entidades.</summary>
    public static class Collision
    {
        /// <summary>Verifica se os limites da entidade 1 está intersectando os limites da entidade 2.</summary>
        /// <param name="entity">A primeira entidade.</param>
        /// <param name="other">A segunda entidade.</param>
        public static CollisionResult EntityCollision(Entity2D entity, Entity2D other)
        {
            entity.UpdateBounds();
            other.UpdateBounds();

            CollisionResult result = new CollisionResult();
            result.Type = CollisionType.None;
            result.HasCollided = false;

            //Se as entidades não estão rotacionadas, então fazemos o cálculo simples de intersecção.
            if(entity.Transform.Rotation == 0 && other.Transform.Rotation == 0)
            {
                //Verifica se  há colisão.
                if (BoundsCollision(entity.Bounds, other.Bounds))
                {
                    //Cria o resultado da colisão entre retângulos.
                    RectangleCollisionResult rcr = new RectangleCollisionResult();
                    rcr.Intersection = Rectangle.Intersect(entity.Bounds, other.Bounds);

                    //O vetor de subtração a ser completado e adicionado.
                    Vector2 sub = Vector2.Zero;

                    var eb = entity.Bounds;
                    var ob = other.Bounds;

                    //Lógica de colisão entre retângulos

                    //Se na intersecção entre os retângulos
                    //A altura é maior que a largura da intersecção,
                    //Então significa que foi uma colisão lateral.
                    if(rcr.Intersection.Height > rcr.Intersection.Width)
                    {
                        //Verificamos o limite.
                        //Se a ponta direita é maior que a ponta esquerda do outro retângulo
                        //e essa ponta está dentro do outro retângulo.
                        //Então encontramos o valor de subtração.
                        //A lógica serve para o restante.
                        if (eb.Right > ob.Left && eb.Right < ob.Right)
                        {
                            sub.X -= eb.Right - ob.Left;
                        }
                        else if(eb.Left < ob.Right && eb.Left > ob.Left)
                        {
                            sub.X -= eb.Left - ob.Right;
                        }
                    }
                    //O contrário é uma colisão vertical.
                    if (rcr.Intersection.Width > rcr.Intersection.Height)
                    {
                        if (eb.Bottom > ob.Top && eb.Bottom < ob.Bottom)
                        {
                            sub.Y -= eb.Bottom - ob.Top;
                        }
                        else if(eb.Top < ob.Bottom && eb.Top > ob.Top)
                        {
                            sub.Y -= eb.Top - ob.Bottom;
                        }
                    }

                    rcr.Subtract = sub;

                    result.HasCollided = true;
                    result.Type = CollisionType.Rectangle;
                    result.RectangleResult = rcr;
                }       
            }
            //se as entidades estão rotacionadas.
            else
            {
                PolygonCollisionResult pcr = PolygonCollision(entity.BoundsR, other.BoundsR, entity.Transform.Velocity);
                
                if(pcr.Intersect)
                {
                    result.HasCollided = true;
                    result.Type = CollisionType.Polygon;
                    result.PolygonResult = pcr;
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
        /// Retorna a subtração da intersecção entre dois retângulos
        /// </summary>
        public static Vector2 IntersectionSubtract(Rectangle one, Rectangle two)
        {
            Rectangle rcr = Rectangle.Intersect(one, two);
            Vector2 sub = Vector2.Zero;

            //Lógica de colisão entre retângulos

            //Se na intersecção entre os retângulos
            //A altura é maior que a largura da intersecção,
            //Então significa que foi uma colisão lateral.
            if (rcr.Height > rcr.Width)
            {
                //Verificamos o limite.
                //Se a ponta direita é maior que a ponta esquerda do outro retângulo
                //e essa ponta está dentro do outro retângulo.
                //Então encontramos o valor de subtração.
                //A lógica serve para o restante.
                if (one.Right > two.Left && one.Right < two.Right)
                {
                    sub.X -= one.Right - two.Left;
                }
                else if (one.Left < two.Right && one.Left > two.Left)
                {
                    sub.X -= one.Left - two.Right;
                }
            }
            //O contrário é uma colisão vertical.
            if (rcr.Width > rcr.Height)
            {
                if (one.Bottom > two.Top && one.Bottom < two.Bottom)
                {
                    sub.Y -= one.Bottom - two.Top;
                }
                else if (one.Top < two.Bottom && one.Top > two.Top)
                {
                    sub.Y -= one.Top - two.Bottom;
                }
            }

            return sub;
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
            if (result.WillIntersect) result.Subtract = translationAxis * minIntervalDistance;

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