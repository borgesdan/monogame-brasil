using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Enumera os tipos de colisão.
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// Não houve colisão ou não é possível estabelecer o tipo dela.
        /// </summary>
        None,
        /// <summary>
        /// Colisão entre retângulos nas rotacionados.
        /// </summary>
        Rectangle,
        /// <summary>
        /// Colisão entre polígonos (caso algum dos retângulos esteja rotacionado).
        /// </summary>
        Polygon
    }

    /// <summary>
    /// Estrutura que guarda o resultado de uma colisão.
    /// </summary>
    public struct CollisionResult : IEquatable<CollisionResult>
    {
        /// <summary>
        /// Obtém True caso houve uma colisão.
        /// </summary>
        public readonly bool HasCollided;
        /// <summary>
        /// O tipo da colisão (Retângulo ou polígonos (retângulo rotacionado)).
        /// </summary>
        public CollisionType Type;        
        /// <summary>
        /// Obtém a intersecção da colisão entre retângulos.
        /// </summary>
        public readonly Rectangle Intersection;
        /// <summary>
        /// Obtém o resultado de uma colisão entre polígonos (caso os retângulos estejam rotacionados).
        /// </summary>
        public readonly PolygonCollisionResult PolygonResult;

        /// <summary>
        /// Cria uma nova instância de CollisionResult.
        /// </summary>
        /// <param name="result">True caso houve uma colisão.</param>
        /// <param name="type">True caso a colisão foi entre retângulos não rotacionados.</param>
        /// <param name="intersection">A intersecção da colisão entre retângulos não rotacionados.</param>
        /// <param name="polyResult">O resultado de uma colisão entre polígonos (caso os retângulos estejam rotacionados).</param>
        public CollisionResult(bool result, CollisionType type, Rectangle intersection, PolygonCollisionResult polyResult)
        {
            HasCollided = result;
            Type = type;
            Intersection = intersection;
            PolygonResult = polyResult;
        }

        public override bool Equals(object obj)
        {
            return obj is CollisionResult result && Equals(result);
        }

        public bool Equals(CollisionResult other)
        {
            return HasCollided == other.HasCollided &&
                   Type == other.Type &&
                   Intersection.Equals(other.Intersection) &&
                   PolygonResult.Equals(other.PolygonResult);
        }

        public override int GetHashCode()
        {
            var hashCode = -1257791954;
            hashCode = hashCode * -1521134295 + HasCollided.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Rectangle>.Default.GetHashCode(Intersection);
            hashCode = hashCode * -1521134295 + EqualityComparer<PolygonCollisionResult>.Default.GetHashCode(PolygonResult);
            return hashCode;
        }

        public static bool operator ==(CollisionResult left, CollisionResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CollisionResult left, CollisionResult right)
        {
            return !(left == right);
        }
    }
}