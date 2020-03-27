// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;
using System.Collections.Generic;

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

    //---------------------------------------//
    //-----         RETÂNGULO           -----//
    //---------------------------------------//

    /// <summary>
    /// Estrutura que guarda o resultado de uma colisão entre retângulos não rotacionados.
    /// </summary>
    public struct RectangleCollisionResult : IEquatable<RectangleCollisionResult>
    {
        /// <summary>
        /// Obtém a intersecção da colisão entre retângulos.
        /// </summary>
        public Rectangle Intersection;

        /// <summary>
        /// Obtém o quanto é necessário para voltar a posição antes da colisão.
        /// </summary>
        public Vector2 Subtract;

        /// <summary>
        /// Cria uma nova instância de RectangleCollisionResult.
        /// </summary>
        /// <param name="intersection">A intersecção da colisão entre retângulos.</param>
        /// <param name="distance">O quanto um retângulo intersectou o outro.</param>
        public RectangleCollisionResult(Rectangle intersection, Vector2 distance)
        {
            Intersection = intersection;
            Subtract = distance;
        }

        public override bool Equals(object obj)
        {
            return obj is RectangleCollisionResult result && Equals(result);
        }

        public bool Equals(RectangleCollisionResult other)
        {
            return Intersection.Equals(other.Intersection) &&
                   Subtract.Equals(other.Subtract);
        }

        public override int GetHashCode()
        {
            var hashCode = -1513100886;
            hashCode = hashCode * -1521134295 + EqualityComparer<Rectangle>.Default.GetHashCode(Intersection);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Subtract);
            return hashCode;
        }

        public static bool operator ==(RectangleCollisionResult left, RectangleCollisionResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleCollisionResult left, RectangleCollisionResult right)
        {
            return !(left == right);
        }
    }

    //---------------------------------------//
    //-----         POLÍGONO            -----//
    //---------------------------------------//

    /// <summary>
    /// Estrutura que guarda os resultados de uma colisão de polígonos.
    /// </summary>
    public struct PolygonCollisionResult : IEquatable<PolygonCollisionResult>
    {
        /// <summary>Obtém o valor True caso o polígono esteja previsto a colidir baseado em sua velocidade.</summary>
        public bool WillIntersect;
        /// <summary>Obtém o valor True caso o polígono esteja intersectando outro polígono (Colidiu).</summary>
        public bool Intersect;
        /// <summary>Obtém o quanto é necessário para voltar a posição antes da colisão.</summary>
        public Vector2 Subtract; //MinimumTranslationVector

        /// <summary>
        /// Cria uma nova instância de PolygonCollisionResult.
        /// </summary>
        /// <param name="willIntersect">Define se o polígono está previsto a colidir baseado em sua velocidade.</param>
        /// <param name="intersect">Define se o polígono estpa intersectando outro polígono (Colidiu).</param>
        /// <param name="distance">Define o quanto o polígono adentrou na colisão no eixo X e Y.</param>
        public PolygonCollisionResult(bool willIntersect, bool intersect, Vector2 distance)
        {
            WillIntersect = willIntersect;
            Intersect = intersect;
            Subtract = distance;
        }

        public override bool Equals(object obj)
        {
            return obj is PolygonCollisionResult result && Equals(result);
        }

        public bool Equals(PolygonCollisionResult other)
        {
            return WillIntersect == other.WillIntersect &&
                   Intersect == other.Intersect &&
                   Subtract.Equals(other.Subtract);
        }

        public override int GetHashCode()
        {
            var hashCode = -2017691047;
            hashCode = hashCode * -1521134295 + WillIntersect.GetHashCode();
            hashCode = hashCode * -1521134295 + Intersect.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Subtract);
            return hashCode;
        }

        public static bool operator ==(PolygonCollisionResult left, PolygonCollisionResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PolygonCollisionResult left, PolygonCollisionResult right)
        {
            return !(left == right);
        }
    }

    //---------------------------------------//
    //-----         COLISÃO           -----//
    //---------------------------------------//

    /// <summary>
    /// Estrutura que guarda o resultado de uma colisão.
    /// </summary>
    public struct CollisionResult : IEquatable<CollisionResult>
    {
        /// <summary>
        /// Obtém True caso houve uma colisão.
        /// </summary>
        public bool HasCollided;
        /// <summary>
        /// O tipo da colisão (Retângulo ou polígonos (retângulo rotacionado)).
        /// </summary>
        public CollisionType Type;

        /// <summary>
        /// Obtém o resultado da colisão entre dois retângulos não rotacionados.
        /// </summary>
        public RectangleCollisionResult RectangleResult;

        /// <summary>
        /// Obtém o resultado de uma colisão entre polígonos (caso os retângulos estejam rotacionados).
        /// </summary>
        public PolygonCollisionResult PolygonResult;

        /// <summary>
        /// Cria uma nova instância de CollisionResult.
        /// </summary>
        /// <param name="result">True caso houve uma colisão.</param>
        /// <param name="type">True caso a colisão foi entre retângulos não rotacionados.</param>
        /// <param name="intersection">A intersecção da colisão entre retângulos não rotacionados.</param>
        /// <param name="polyResult">O resultado de uma colisão entre polígonos (caso os retângulos estejam rotacionados).</param>
        public CollisionResult(bool result, CollisionType type, RectangleCollisionResult rectangleResult, PolygonCollisionResult polyResult)
        {
            HasCollided = result;
            Type = type;
            RectangleResult = rectangleResult;
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
                   RectangleResult.Equals(other.RectangleResult) &&
                   PolygonResult.Equals(other.PolygonResult);
        }

        public override int GetHashCode()
        {
            var hashCode = -926503291;
            hashCode = hashCode * -1521134295 + HasCollided.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<RectangleCollisionResult>.Default.GetHashCode(RectangleResult);
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