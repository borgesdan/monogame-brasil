namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Classe que representa um modo de visualização do desenvolvedor.
    /// </summary>
    public static class DEBUG
    {
        public static Color BoundsColor = Color.Red;
        public static Color CollisionBoxColor = Color.Green;

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