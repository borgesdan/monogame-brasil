// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>Classe que gerencia e auxilia nas entradas do jogador com um teclado.</summary>
    public class KeyboardHelper
    {
        /// <sumary>Obtém ou define se esta instância está apta a executar os trabalhos.</sumary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>Obtém o estado atual do teclado.</summary>
        public KeyboardState State { get; private set; }
        /// <summary>Obtém o último estado do teclado antes da atualização.</summary>
        public KeyboardState LastState { get; private set; }

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

            LastState = State;
            State = Keyboard.GetState();
        }

        /// <summary>Verifica se o botão selecionado está pressionado.</summary>
        /// <param name="key">A tecla a ser verificada.</param>
        public bool IsDown(Keys key)
        {
            bool result = false;

            if (State.IsKeyDown(key))
                result = true;

            return result;
        }

        /// <summary>Verifica se o botão selecionado não estava pressionado no estado anterior, mas sim no atual.</summary>
        /// <param name="key">A tecla a ser verificada.</param>
        public bool IsPress(Keys key)
        {
            bool result = false;

            if (LastState.IsKeyUp(key) && State.IsKeyDown(key))
                result = true;

            return result;
        }

        /// <summary>Verifica se o botão selecionado não está pressionado.</summary>   
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