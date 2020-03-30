// Danilo Borges Santos, 2020. 
// Email: danilo.bsto@gmail.com
// Versão: Conillon [1.0]

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Gerencia telas que se encontram dentro de uma tela principal.</summary>
    public class SubScreenManager : IManager
    {
        //---------------------------------------//
        //-----         VARIÁVEIES          -----//
        //---------------------------------------//  

        private bool disposed = false;

        //----------------------------------------//
        //-----         PROPRIEDADES         -----//
        //----------------------------------------//
        
        /// <summary>Obtém a instância corrente da tela gerenciadora.</summary>
        public Screen MainScreen { get; set; } = null;
        /// <summary>Obtém a instância corrente do gerenciador de telas.</summary>
        public ScreenManager ScreenManager { get => MainScreen.Manager; }
        /// <summary>Obtém a instância corrente da classe Game.</summary>
        public Game Game { get => MainScreen.Game; }
        /// <summary>Obtém o InputManager do ScreenManager.</summary>
        public InputManager Input { get => ScreenManager.Input; }
        /// <summary>Obtém ou define a lista de sub-telas.</summary>
        public List<Screen> SubScreens { get; set; } = new List<Screen>();
        /// <summary>Obtém a tela ativa.</summary>
        public Screen Active { get; private set; } = null;

        /// <summary>
        /// Inicializa uma nova instância da classe SubScreenManager.
        /// </summary>
        /// <param name="mainScreen">A tela em que o gerenciador está associado.</param>
        public SubScreenManager(Screen mainScreen)
        {
            MainScreen = mainScreen;
        }

        /// <summary>Obtém uma tela informando seu nome.</summary>  
        public Screen this[string name]
        {
            get
            {
                return SubScreens.Find(x => x.Name.Equals(name));
            }
        }

        /// <summary>
        /// Define uma cena carregada ao seu estado inicial.
        /// </summary>
        /// <param name="name">O nome da tela a ser redefinida.</param>
        public void Reset(string name)
        {
            var s = this[name];            
            s.Reset();
        }

        /// <summary>Adiciona telas para esse gerenciador.</summary>
        /// <param name="screens">A quantidade desejada de telas a serem adicionadas.</param>
        public void Add(params Screen[] screens)
        {
            foreach (var s in screens)
            {
                SubScreens.Add(s);

                if (Active == null)
                    Active = s;
            }
        }

        /// <summary>Remove uma tela do gerenciador.</summary>
        /// <param name="name">Nome da tela.</param>
        public void Remove(string name)
        {
            Screen finder = this[name];
            SubScreens.Remove(finder);
        }

        /// <summary>Ativa uma nova tela para ser atualização e desenho preservando o estado atual da atual tela ativa.</summary>
        /// <param name="name">O nome da tela que será ativada.</param>
        public void Change(string name) => Change(name, false);

        /// <summary>Ativa uma nova tela para ser atualização e desenho</summary>
        /// <param name="name">O nome da tela que será ativada.</param>
        /// <param name="reset">True se deseja que o gerenciador chame o método Reset() da tela atual.</param>
        public void Change(string name, bool reset)
        {
            Screen old = Active;

            Screen finder = this[name];
            Active = finder ?? throw new ArgumentException("Não foi encontrada uma tela com esse nome", nameof(name));

            if (reset)
                old.Reset();
        }

        /// <summary>
        /// Troca para a próxima tela da lista.
        /// </summary>
        /// <param name="reset">True se deseja que o gerenciador chame o método Reset() da tela atual.</param>
        public void Next(bool reset)
        {
            int index = SubScreens.FindIndex(x => x.Equals(Active));

            if (index >= SubScreens.Count - 1)
                index = 0;
            else
                index++;

            Change(SubScreens[index].Name, reset);
        }

        /// <summary>
        /// Troca para a tela anterior da lista de telas.
        /// </summary>
        /// <param name="reset">True se deseja que o gerenciador chame o método Reset() da tela atual.</param>
        public void Back(bool reset)
        {
            //Voltar a tela.
            int index = SubScreens.FindIndex(x => x.Equals(Active));

            if (index <= 0)
                index = SubScreens.Count - 1;
            else
                index--;

            Change(SubScreens[index].Name, reset);
        }

        /// <summary>Atualiza o gerenciador de telas.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public void Update(GameTime gameTime)
        {
            Active?.Update(gameTime);
        }

        /// <summary>Desenha o gerenciador de telas com a sua tela ativa.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Active?.Draw(gameTime, spriteBatch);
        }

        /// <summary>Libera os recursos da classe.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                MainScreen = null;
                SubScreens.Clear();
                SubScreens = null;

                Active = null;
            }

            disposed = true;
        }
    }
}