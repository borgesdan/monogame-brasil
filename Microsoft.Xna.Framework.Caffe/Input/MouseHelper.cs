//---------------------------------------//
// Danilo Borges Santos, 2020       -----//
// danilo.bsto@gmail.com            -----//
// MonoGame.Caffe [1.0]             -----//
//---------------------------------------//

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
        
        private int clickTimeL = 0;
        private byte clickCountL = 0;
        private bool hasDoubleClickL = false;
        
        private int clickTimeR = 0;
        private byte clickCountR = 0;
        private bool hasDoubleClickR = false;        

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        
        /// <sumary>Obtém ou define se esta instância está disponível para ser atualizada.</sumary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>Obtém ou define o tempo para reconhecimento de um duplo clique.</summary>
        public int DoubleClickDelay { get; set; } = 900;
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

            //Verifica o duplo clique no botão esquerdo e direito
            
            //botão esquerdo
            hasDoubleClickL = false;
            clickTimeL += gameTime.ElapsedGameTime.Milliseconds;            

            if (IsPress(MouseButtons.Left))
            {
                if(clickTimeL <= DoubleClickDelay)
                {
                    clickCountL++;
                    
                    if (clickCountL == 2)
                    {
                        clickCountL = 0;
                        clickTimeL = 0;
                        hasDoubleClickL = true;
                    }
                }
                else
                {
                    clickCountL = 0;
                    clickTimeL = 0;
                }
            }

            //botão direito

            hasDoubleClickR = false;
            clickTimeR += gameTime.ElapsedGameTime.Milliseconds;

            if (IsPress(MouseButtons.Right))
            {
                if (clickTimeR <= DoubleClickDelay)
                {
                    clickCountR++;

                    if (clickCountR == 2)
                    {
                        clickCountR = 0;
                        clickTimeR = 0;
                        hasDoubleClickR = true;
                    }
                }
                else
                {
                    clickCountR = 0;
                    clickTimeR = 0;
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
        public bool CheckLefDoubleClick()
        {
            return hasDoubleClickL;
        }

        /// <summary>Verifica se houve duplo clique no botão esquerdo.</summary>
        public bool CheckRightDoubleClick()
        {
            return hasDoubleClickR;
        }

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