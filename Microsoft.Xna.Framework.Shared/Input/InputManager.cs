// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Classe de gerenciamento de entradas do usuário.
    /// </summary>
    public class InputManager 
    {
        /// <summary>Obtém ou define o GamePad com o PlayerIndex 1.</summary>
        public GamePadHelper One { get; set; }
        /// <summary>Obtém ou define o GamePad com o PlayerIndex 2.</summary>
        public GamePadHelper Two { get; set; }
        /// <summary>Obtém ou define o GamePad com o PlayerIndex 3.</summary>
        public GamePadHelper Three { get; set; }
        /// <summary>Obtém ou define o GamePad com o PlayerIndex 4.</summary>
        public GamePadHelper Four { get; set; }
        /// <summary>Obtém ou define o gerenciamento do teclado.</summary>
        public KeyboardHelper Keyboard { get; set; }
        /// <summary>Obtém ou define o gerenciamento do Mouse.</summary>
        public MouseHelper Mouse { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe InputManager.
        /// </summary>
        /// <param name="game">A instância atual da classe Game.</param>
        public InputManager(Game game)
        {
            One = new GamePadHelper(PlayerIndex.One, null);
            Two = new GamePadHelper(PlayerIndex.Two, null);
            Three = new GamePadHelper(PlayerIndex.Three, null);
            Four = new GamePadHelper(PlayerIndex.Four, null);
            Keyboard = new KeyboardHelper();
            Mouse = new MouseHelper(game);
        }

        /// <summary>
        /// Obtém o GamePadHelper pelo seu index.
        /// </summary>
        /// <param name="index">O index do jogador.</param>
        /// <returns>Retorna o GamePadHelper selecionado.</returns>
        public GamePadHelper this[PlayerIndex index]
        {
            get
            {
                switch(index)
                {
                    case PlayerIndex.One:
                        return One;
                    case PlayerIndex.Two:
                        return Two;
                    case PlayerIndex.Three:
                        return Three;
                    case PlayerIndex.Four:
                        return Four;
                    default:
                        return null;
                }
            }
        }
        
        /// <summary>Atualiza os estados do GamePad.</summary>
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
        
        /// <summary>
        /// Adiciona um mapa do teclado a um GamePadHelper.
        /// </summary>
        /// <param name="index">O index do GamePad.</param>
        /// <param name="map">O mapa do teclado.</param>
        public void AddMap(PlayerIndex index, KeyboardMap map)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    One.KeyboardMap = map.GetKeyboardMap();
                    break;
                case PlayerIndex.Two:
                    Two.KeyboardMap = map.GetKeyboardMap();
                    break;
                case PlayerIndex.Three:
                    Three.KeyboardMap = map.GetKeyboardMap();
                    break;
                case PlayerIndex.Four:
                    Four.KeyboardMap = map.GetKeyboardMap();
                    break;
            }
        }
    }
}