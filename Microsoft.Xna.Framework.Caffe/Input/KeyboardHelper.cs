//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>Classe que gerencia e auxilia nas entradas do jogador com um teclado.</summary>
    public class KeyboardHelper
    {
        /// <sumary>Obtém ou define se esta instância está disponível para ser atualizada.</sumary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>Obtém o estado atual do teclado.</summary>
        public KeyboardState State { get; private set; }
        /// <summary>Obtém o estado anterior do teclado antes da atualização.</summary>
        public KeyboardState OldState { get; private set; }

        /// <summary>
        /// Inicializa uma nova instância de KeyboardHelper.
        /// </summary>
        public KeyboardHelper() { }

        /// <summary>Atualiza os estados do GamePad.</summary>
        /// <param name="gameTime">Uma instância de GameTime.</param>
        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;

            OldState = State;
            State = Keyboard.GetState();
        }

        /// <summary>Verifica se a tecla selecionada está pressionada.</summary>
        /// <param name="key">A tecla a ser verificada.</param>
        public bool IsDown(Keys key)
        {
            bool result = false;

            if (State.IsKeyDown(key))
                result = true;

            return result;
        }

        /// <summary>Verifica se a tecla selecionada foi pressionada.</summary>
        /// <param name="key">A tecla a ser verificada.</param>
        public bool IsPress(Keys key)
        {
            bool result = false;

            if (OldState.IsKeyUp(key) && State.IsKeyDown(key))
                result = true;

            return result;
        }

        /// <summary>Verifica se a tecla estava pressionada e foi liberada.</summary>
        /// <param name="key">A tecla a ser verificada.</param>
        public bool IsReleased(Keys key)
        {
            bool result = false;

            if (OldState.IsKeyDown(key) && State.IsKeyUp(key))
                result = true;

            return result;
        }

        /// <summary>Verifica se a tecla selecionada está liberada.</summary>   
        /// <param name="key">A tecla a ser verificada.</param>
        public bool IsUp(Keys key)
        {
            bool result = false;

            if (State.IsKeyUp(key))
                result = true;

            return result;
        }
    }
}