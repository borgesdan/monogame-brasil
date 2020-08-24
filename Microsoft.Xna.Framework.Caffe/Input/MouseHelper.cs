// Danilo Borges Santos, 2020.

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>Enumera os botões do mouse.</summary>
    public enum MouseButtons : byte
    {
        Left = 1,
        Right = 2,
        Middle = 3,
        X1 = 4,
        X2 = 5
    }

    /// <summary>Classe que auxilia no gerenciamento de entradas do mouse.</summary>
    public class MouseHelper
    {
        //---------------------------------------//
        //-----         VARIAVEIS           -----//
        //---------------------------------------//   

        private int clickTime = 0;
        private byte clicks = 0;
        private bool hasDoubleClick = false;               

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        
        /// <sumary>Obtém ou define se esta instância está disponível para ser atualizada.</sumary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>Obtém ou define o tempo para reconhecimento de um duplo clique em milisegundos.</summary>
        public int DoubleClickDelay { get; set; } = 900;
        /// <summary>Obtém ou define o botão a ser verificado o duplo clique.</summary>
        public MouseButtons DoubleClickButton { get; set; } = MouseButtons.Left;
        /// <summary>Obtém o estado atual do mouse.</summary>
        public MouseState State { get; private set; }
        /// <summary>Obtém estado anterior do mouse.</summary>
        public MouseState OldState { get; private set; }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância da classe MouseHelper.</summary>
        public MouseHelper()
        {
            State = Mouse.GetState();
        }

        //---------------------------------------//
        //-----         METÓDOS             -----//
        //---------------------------------------//

        /// <summary>Atualiza o estado do mouse.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;

            OldState = State;
            State = Mouse.GetState();

            //Verifica o duplo clique
            CheckDoubleClick(gameTime);
        }

        //Verifica o duplo clique do botão definido
        private void CheckDoubleClick(GameTime gameTime)
        {
            hasDoubleClick = false;
            clickTime += gameTime.ElapsedGameTime.Milliseconds;
            bool isLeftPress = IsPress(DoubleClickButton);

            if (isLeftPress)
            {
                if (clickTime <= DoubleClickDelay)
                {
                    clicks++;

                    if (clicks == 2)
                    {
                        clicks = 0;
                        clickTime = 0;
                        hasDoubleClick = true;
                    }
                }
                else
                {
                    clicks = 0;
                    clickTime = 0;
                    hasDoubleClick = false;
                }
            }
        }

        /// <summary>Define o estado do mouse.</summary>
        /// <param name="state">O estado do mouse a ser definido.</param>
        public void SetState(MouseState state)
        {
            OldState = State;
            State = state;
        }

        /// <summary>Verifica se houve duplo clique no botão direito.</summary>
        public bool CheckDoubleClick() => hasDoubleClick;

        /// <summary>Checa se o botão do mouse está pressionado.</summary>
        /// <param name="button">O botão do mouse a ser checado.</param>
        public bool IsDown(MouseButtons button)
        {
            bool rtn = false;
            ButtonState s = Check(button, false);

            if (s == ButtonState.Pressed)
                rtn = true;

            return rtn;
        }

        /// <summary>Checa se o botão do mouse está liberado.</summary>
        /// <param name="button">O botão do mouse a ser checado.</param>
        public bool IsUp(MouseButtons button)
        {
            bool rtn = false;
            ButtonState s = Check(button, false);

            if (s == ButtonState.Released)
                rtn = true;

            return rtn;
        }

        /// <summary>Checa se o botão do mouse foi pressionado.</summary>
        /// <param name="button">O botão do mouse a ser checado.</param>
        public bool IsPress(MouseButtons button)
        {
            bool rtn = false;
            ButtonState s = Check(button, false);
            ButtonState ls = Check(button, true);

            if (s == ButtonState.Pressed && ls == ButtonState.Released)
                rtn = true;

            return rtn;
        }

        //Checa os estados dos botões
        private ButtonState Check(MouseButtons button, bool checkLastState)
        {
            MouseState stt;

            if (checkLastState)
                stt = OldState;
            else
                stt = State;

            if (button == MouseButtons.Left)
                return stt.LeftButton;
            else if (button == MouseButtons.Middle)
                return stt.MiddleButton;
            else if (button == MouseButtons.Right)
                return stt.RightButton;
            else if (button == MouseButtons.X1)
                return stt.XButton1;
            else 
                return stt.XButton2;
        }
    }    
}