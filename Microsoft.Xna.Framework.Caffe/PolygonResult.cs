//Código base disponível em:
//https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection
//Autor: Laurent Cozic

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Estrutura que guarda os resultados de uma colisão de polígonos.
    /// </summary>
    public struct PolygonCollisionResult : IEquatable<PolygonCollisionResult>
    {
        /// <summary>Obtém o valor True caso o polígono esteja previsto a colidir baseado em sua velocidade.</summary>
        public bool WillIntersect;
        /// <summary>Obtém o valor True caso o polígono esteja intersectando outro polígono (Colidiu).</summary>
        public bool Intersect;
        /// <summary>Obtém o quanto o polígono adentrou na colisão no eixo X e Y.</summary>
        public Vector2 MinimumTranslationVector;

        /// <summary>
        /// Cria uma nova instância de PolygonCollisionResult.
        /// </summary>
        /// <param name="willIntersect">Define se o polígono está previsto a colidir baseado em sua velocidade.</param>
        /// <param name="intersect">Define se o polígono estpa intersectando outro polígono (Colidiu).</param>
        /// <param name="minimumTranslationVector">Define o quanto o polígono adentrou na colisão no eixo X e Y.</param>
        public PolygonCollisionResult(bool willIntersect, bool intersect, Vector2 minimumTranslationVector)
        {
            WillIntersect = willIntersect;
            Intersect = intersect;
            MinimumTranslationVector = minimumTranslationVector;
        }

        public override bool Equals(object obj)
        {
            return obj is PolygonCollisionResult result && Equals(result);
        }

        public bool Equals(PolygonCollisionResult other)
        {
            return WillIntersect == other.WillIntersect &&
                   Intersect == other.Intersect &&
                   MinimumTranslationVector.Equals(other.MinimumTranslationVector);
        }

        public override int GetHashCode()
        {
            var hashCode = -2017691047;
            hashCode = hashCode * -1521134295 + WillIntersect.GetHashCode();
            hashCode = hashCode * -1521134295 + Intersect.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(MinimumTranslationVector);
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
}