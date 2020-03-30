// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

using System;

namespace Game2
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
