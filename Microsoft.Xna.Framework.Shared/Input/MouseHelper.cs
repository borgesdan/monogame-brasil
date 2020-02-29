// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

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

    /// <summary>Classe que auxilia o uso do mouse.</summary>
    public class MouseHelper
    {
        //---------------------------------------//
        //-----         VARIAVEIS           -----//
        //---------------------------------------//
        private Game game;
        
        private int clickTimeL = 0;
        private byte clickCountL = 0;
        private bool hasDoubleClickL = false;
        
        private int clickTimeR = 0;
        private byte clickCountR = 0;
        private bool hasDoubleClickR = false;        

        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//

        /// <summary>Obtém ou define se o mouse está visível em tela.</summary>
        public bool IsVisible { get => game.IsMouseVisible; set => game.IsMouseVisible = value; }
        /// <sumary>Obtém ou define se esta instância está apta a executar os trabalhos.</sumary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>Obtém ou define o tempo de execução para um duplo clique.</summary>
        public int DoubleClickDelay { get; set; } = 900;
        /// <summary>Obtém o estado atual do mouse.</summary>
        public MouseState State { get; private set; }
        /// <summary>Obtém o último estado do mouse.</summary>
        public MouseState LastState { get; private set; }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância da classe MouseHelper.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        public MouseHelper(Game game)
        {
            this.game = game;
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

            LastState = State;
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
            LastState = State;
            State = state;
        }

        /// <summary>Verifica se houve duplo clique no botão direito.</summary>
        /// <returns>Retorna True se ocorreu o duplo clique.</returns>
        public bool CheckLefDoubleClick()
        {
            return hasDoubleClickL;
        }

        /// <summary>Verifica se houve duplo clique no botão esquerdo.</summary>
        /// <returns>Retorna True se ocorreu o duplo clique.</returns>
        public bool CheckRightDoubleClick()
        {
            return hasDoubleClickR;
        }

        /// <summary>Checa se o botão do mouse está clicado.</summary>
        /// <param name="button">O botão do mouse a ser checado.</param>
        /// <returns>Retorna True se o botão estiver clicado.</returns>
        public bool IsDown(MouseButtons button)
        {
            bool rtn = false;
            ButtonState s = Check(button, false);

            if (s == ButtonState.Pressed)
                rtn = true;

            return rtn;
        }

        /// <summary>Checa se o botão do mouse está levantado.</summary>
        /// <param name="button">O botão do mouse a ser checado.</param>
        /// <returns>Retorna True se o botão estiver levantado.</returns>
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
        /// <returns>Retorna True se o último estado do botão for 'IsUp' e o atual 'IsDown'.</returns>
        public bool IsPress(MouseButtons button)
        {
            bool rtn = false;
            ButtonState s = Check(button, false);
            ButtonState ls = Check(button, true);

            if (s == ButtonState.Pressed && ls == ButtonState.Released)
                rtn = true;

            return rtn;
        }

        private ButtonState Check(MouseButtons button, bool checkLastState)
        {
            MouseState stt;

            if (checkLastState)
                stt = LastState;
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