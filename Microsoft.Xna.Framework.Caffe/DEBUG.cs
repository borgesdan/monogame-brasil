// Danilo Borges Santos, 2020.

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Representa um modo de visualização do desenvolvedor.
    /// </summary>
    public static class DEBUG
    {
        private static Polygon3D poly = null;
        public static List<Tuple<Polygon, Color>> Polygons { get; set; } = new List<Tuple<Polygon, Color>>();        
        
        public static Color BoundsColor { get; set; } = Color.DarkBlue;        
        public static Color CollisionBoxColor { get; set; } = Color.Green;
        public static Color AttackBoxColor { get; set; } = Color.Red;

        public static bool IsEnabled { get; private set; } = false;
        public static bool ShowBounds { get; set; } = true;
        public static bool ShowCollisionBox { get; set; } = true;
        public static bool ShowAttackBox { get; set; } = true;
        public static bool ShowText { get; set; } = true;        

        public static void EnableDebug(bool value)
        {
            IsEnabled = value;
        }

        public static void Draw(Game game, SpriteBatch spriteBatch)
        {
            if (poly == null)
                poly = new Polygon3D(game, new Polygon(), Color.White);

            for (int i = 0; i < Polygons.Count; i++)
            {
                poly.Color = Polygons[i].Item2;
                poly.Poly = Polygons[i].Item1;

                poly.Draw();                
            }

            Polygons.Clear();
        }
    }
}