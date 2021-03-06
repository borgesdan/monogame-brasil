﻿// Danilo Borges Santos, 2020.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>Classe que gerencia e auxilia nas entradas do usuário com o GamePad.</summary>
    public class GamePadHelper : IUpdate
    {
        private KeyboardState keyboardState = new KeyboardState();
        private KeyboardState lastKeyboardState = new KeyboardState();

        /// <sumary>Obtém ou define se esta instância está disponível para ser atualizada.</sumary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>Obtém o estado atual do GamePad.</summary>
        public GamePadState State { get; private set; } = new GamePadState();
        /// <summary>Obtém o estado anterior do GamePad antes da atualização.</summary>
        public GamePadState OldState { get; private set; } = new GamePadState();

        /// <summary>Obtém o index do GamePad.</summary>
        public PlayerIndex Index { get; private set; } = PlayerIndex.One;
        /// <summary>Obtém ou define o mapeamento de teclas para botões do GamePad.</summary>
        public Dictionary<Buttons, Keys?> KeyboardMap { get; set; } = new Dictionary<Buttons, Keys?>();

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//

        /// <summary>Inicializa uma nova instância de GamePadHelper.</summary>
        /// <param name="playerIndex">O index do GamePad.</param>
        public GamePadHelper(PlayerIndex playerIndex) : this(playerIndex, null) { }

        /// <summary>Inicializa uma nova instância de GamePadHelper.</summary>
        /// <param name="playerIndex">O index do GamePad.</param>
        /// <param name="keyboardMap">O mapeamento de teclas para botões do GamePad.</param>
        /// <param name="padState">Define o estdo do GamePad.</param>
        public GamePadHelper(PlayerIndex playerIndex, KeyboardMap keyboardMap, GamePadState padState) : this(playerIndex, keyboardMap)
        {
            State = padState;
        }

        /// <summary>Inicializa uma nova instância de GamePadHelper.</summary>
        /// <param name="playerIndex">O index do GamePad.</param>
        /// <param name="keyboardMap">o mapeamento de teclas para botões do GamePad.</param>
        public GamePadHelper(PlayerIndex playerIndex, KeyboardMap keyboardMap)
        {
            Index = playerIndex;
            KeyboardMap = keyboardMap?.GetKeyboardMap();
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>Atualiza os estados do GamePad.</summary>
        /// <param name="gameTime">Uma instância de GameTime.</param>
        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;

            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            OldState = State;
            State = GamePad.GetState(Index);
        }

        /// <summary>
        /// <summary>Define o mapeamento de teclas para botões do GamePad.</summary>
        /// </summary>
        /// <param name="keyboardMap">o mapeamento de teclas para botões do GamePad.</param>
        public void SetMap(KeyboardMap keyboardMap)
        {
            KeyboardMap = keyboardMap.GetKeyboardMap();
        }

        /// <summary>Verifica se o botão selecionado está pressionado.</summary>
        /// <param name="button">O botão do GamePad a ser verificado.</param>
        public bool IsDown(Buttons button)
        {
            bool result = false;

            if (State.IsConnected)
            {
                if (State.IsButtonDown(button))
                    result = true;
            }
            else if (KeyboardMap != null)
            {
                var k = KeyboardMap[button];

                if (k != null)
                    if (keyboardState.IsKeyDown(k.Value))
                        result = true;
            }

            return result;
        }

        /// <summary>Verifica se o botão selecionado foi pressionado.</summary>
        /// <param name="button">O botão do GamePad a ser verificado.</param>
        public bool IsPress(Buttons button)
        {
            bool result = false;

            if (State.IsConnected)
            {
                if (OldState.IsButtonUp(button) && State.IsButtonDown(button))
                    result = true;
            }
            else if (KeyboardMap != null)
            {
                var k = KeyboardMap[button];

                if (k != null)
                    if (lastKeyboardState.IsKeyUp(k.Value) && keyboardState.IsKeyDown(k.Value))
                        result = true;
            }

            return result;
        }

        /// <summary>Verifica se o botão estava pressionada e foi liberada.</summary>
        /// <param name="key">A tecla a ser verificada.</param>
        public bool IsReleased(Buttons button)
        {
            bool result = false;

            if (State.IsConnected)
            {
                if (OldState.IsButtonDown(button) && State.IsButtonUp(button))
                    result = true;
            }
            else if (KeyboardMap != null)
            {
                var k = KeyboardMap[button];

                if (k != null)
                    if (lastKeyboardState.IsKeyDown(k.Value) && keyboardState.IsKeyUp(k.Value))
                        result = true;
            }

            return result;
        }

        /// <summary>Verifica se o botão selecionado está liberado.</summary>     
        /// <param name="button">O botão do GamePad a ser verificado.</param>
        public bool IsUp(Buttons button)
        {
            bool result = false;

            if (State.IsConnected)
            {
                if (State.IsButtonUp(button))
                    result = true;
            }
            else if (KeyboardMap != null)
            {
                var k = KeyboardMap[button];

                if (k != null)
                    if (keyboardState.IsKeyUp(k.Value))
                        result = true;
            }

            return result;
        }

        /// <summary>Verifica o estado da analógico esquerdo.</summary>        
        public Vector2 GetLeftThumbState()
        {
            Vector2 thumb = Vector2.Zero;

            if (State.IsConnected)
            {
                thumb = State.ThumbSticks.Left;
            }
            else if (KeyboardMap != null)
            {
                var kUp = KeyboardMap[Buttons.LeftThumbstickUp];
                var kDown = KeyboardMap[Buttons.LeftThumbstickDown];
                var kRight = KeyboardMap[Buttons.LeftThumbstickRight];
                var kLeft = KeyboardMap[Buttons.LeftThumbstickLeft];

                if (kUp != null)
                {
                    if (keyboardState.IsKeyDown(kUp.Value))
                    {
                        thumb.Y = 1;
                    }                        
                    else if (kDown != null)
                    {
                        if (keyboardState.IsKeyDown(kDown.Value))
                        {
                            thumb.Y = -1;
                        }                            
                    }
                }

                if (kRight != null)
                {
                    if (keyboardState.IsKeyDown(kRight.Value))
                    {
                        thumb.X = 1;
                    }
                    else if (kLeft != null)
                    {
                        if (keyboardState.IsKeyDown(kLeft.Value))
                        {
                            thumb.X = -1;
                        }
                    }
                }
            }

            return thumb;
        }

        /// <summary>Verifica o estado da analógico direito.</summary>
        public Vector2 GetRightThumbState()
        {
            Vector2 thumb = Vector2.Zero;

            if (State.IsConnected)
            {
                thumb = State.ThumbSticks.Left;
            }
            else if (KeyboardMap != null)
            {
                var kUp = KeyboardMap[Buttons.RightThumbstickUp];
                var kDown = KeyboardMap[Buttons.RightThumbstickDown];
                var kRight = KeyboardMap[Buttons.RightThumbstickRight];
                var kLeft = KeyboardMap[Buttons.RightThumbstickLeft];

                if (kUp != null)
                {
                    if (keyboardState.IsKeyDown(kUp.Value))
                    {
                        thumb.Y = 1;
                    }
                    else if (kDown != null)
                    {
                        if (keyboardState.IsKeyDown(kDown.Value))
                        {
                            thumb.Y = -1;
                        }
                    }
                }

                if (kRight != null)
                {
                    if (keyboardState.IsKeyDown(kRight.Value))
                    {
                        thumb.X = 1;
                    }
                    else if (kLeft != null)
                    {
                        if (keyboardState.IsKeyDown(kLeft.Value))
                        {
                            thumb.X = -1;
                        }
                    }
                }
            }

            return thumb;
        }
    }
}