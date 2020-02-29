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
        public static bool Check(Entity2D entity, Entity2D other)
        {
            var b = entity.Bounds;
            var ob = other.Bounds;

            if (b.Intersects(ob))
                return true;
            else
                return false;
        }

        /// <summary>Verifica se os limites da entidade 1 está intersectando os limites da entidade 2 e retorna uma tupla com o valor da colisão e a intersecção resultante desta.</summary>
        /// <param name="entity">A primeira entidade.</param>
        /// <param name="other">A segunda entidade.</param>
        public static Tuple<bool, Rectangle> CheckResult(Entity2D entity, Entity2D other)
        {
            bool result = Check(entity, other);
            Rectangle rec = Rectangle.Empty;

            if (result)
                rec = Rectangle.Intersect(entity.Bounds, other.Bounds);

            return new Tuple<bool, Rectangle>(result, rec);
        }
    }
}