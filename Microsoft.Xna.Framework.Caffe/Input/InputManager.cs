// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Classe de gerenciamento de entradas do usuário.
    /// </summary>
    public class InputManager : IUpdate
    {
        /// <summary>Obtém ou define o GamePad do Player 1.</summary>
        public GamePadHelper One { get; set; }
        /// <summary>Obtém ou define o GamePad do Player 2.</summary>
        public GamePadHelper Two { get; set; }
        /// <summary>Obtém ou define o GamePad do Player 3.</summary>
        public GamePadHelper Three { get; set; }
        /// <summary>Obtém ou define o GamePad do Player 4.</summary>
        public GamePadHelper Four { get; set; }
        /// <summary>Obtém ou define o gerenciamento do teclado.</summary>
        public KeyboardHelper Keyboard { get; set; }
        /// <summary>Obtém ou define o gerenciamento do Mouse.</summary>
        public MouseHelper Mouse { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe InputManager.
        /// </summary>
        public InputManager()
        {
            One = new GamePadHelper(PlayerIndex.One, null);
            Two = new GamePadHelper(PlayerIndex.Two, null);
            Three = new GamePadHelper(PlayerIndex.Three, null);
            Four = new GamePadHelper(PlayerIndex.Four, null);
            Keyboard = new KeyboardHelper();
            Mouse = new MouseHelper();
        }        

        /// <summary>
        /// Obtém o GamePadHelper pelo seu index.
        /// </summary>
        /// <param name="index">O index do jogador.</param>
        public GamePadHelper this[PlayerIndex index]
        {
            get
            {
                return index switch                 //switch(index)
                {
                    PlayerIndex.One => One,         //case PlayerIndex.One: return One; break;
                    PlayerIndex.Two => Two,
                    PlayerIndex.Three => Three,
                    PlayerIndex.Four => Four,
                    _ => null,                      //case default: return null; break;
                };
            }
        }
        
        /// <summary>Atualiza os estados das entradas do usuário.</summary>
        /// <param name="gameTime">Uma instância de GameTime.</param>
        public void Update(GameTime gameTime)
        {
            One.Update(gameTime);
            Two.Update(gameTime);
            Three.Update(gameTime);
            Four.Update(gameTime);
            Keyboard.Update(gameTime);
            Mouse.Update(gameTime);
        }       
    }
}