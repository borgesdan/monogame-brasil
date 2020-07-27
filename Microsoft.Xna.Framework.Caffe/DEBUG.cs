namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Classe que representa um modo de visualização do desenvolvedor.
    /// </summary>
    public static class DEBUG
    {
        public static Color BoundsColor { get; set; } = Color.DarkBlue;
        public static Color CollisionBoxColor { get; set; } = Color.Green;
        public static Color AttackBoxColor { get; set; } = Color.Red;

        public static bool IsEnabled { get; private set; } = false;
        public static bool ShowBounds { get; set; } = false;
        public static bool ShowCollisionBox { get; set; } = false;
        public static bool ShowAttackBox { get; set; } = false;

        public static void EnableDebug(bool value)
        {
            IsEnabled = value;
        }
    }
}