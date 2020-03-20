// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Classe gerenciadora as telas do jogo.</summary>
    public class ScreenManager : IDisposable, IUpdateDrawable
    {
        //---------------------------------------//
        //-----         VARIÁVEIES          -----//
        //---------------------------------------//        
        private bool disposed = false;
        private bool callLoadScreen = false;
        private Task taskLoading = null;

        //----------------------------------------//
        //-----         PROPRIEDADES         -----//
        //----------------------------------------//

        /// <summary>Obtém a instância corrente da classe Game.</summary>
        public Game Game { get; private set; } = null;
        /// <summary>Obtém ou define a lista de telas.</summary>
        public List<Screen> Screens { get; set; } = new List<Screen>();
        /// <summary>Obtém a tela ativa.</summary>
        public Screen Active { get; protected set; } = null;
        /// <summary>Obtém ou define o gerenciador de eventos de entrada do usuário.</summary>
        public InputManager Input { get; set; } = null;
        /// <summary>Obtém a tela que está sendo carregada de modo assíncrono.</summary>
        public Screen LoadingScreen { get; protected set; } = null;
        /// <summary>Obtém True se o gerenciador está carregando uma tela de modo assíncrono.</summary>
        public bool IsLoadingAsync { get; protected set; } = false;

        //-----------------------------------------//
        //-----         CONSTRUTOR            -----//
        //-----------------------------------------//

        /// <summary>Inicializa uma nova instância da classe ScreenManager.</summary>
        /// <param name="game">A instância atual da classe Game.</param>
        public ScreenManager(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game), "O argumento não pode ser nulo.");
            Input = new InputManager(game);
        }

        //---------------------------------------//
        //-----         INDEXADOR           -----//
        //---------------------------------------//


        /// <summary>Obtém uma tela informando seu nome.</summary>  
        public Screen this[string name]
        {
            get
            {
                return Find(name);
            }
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Carrega uma tela em uma thread separada e troca a tela atual para essa quando o carregamento estiver terminado.
        /// </summary>
        /// <param name="name">O nome da tela a ser carregada</param>
        /// <param name="callWhenIsFinished">True, se a tela deverá ser chamada quando o carregamento estiver terminado.</param>
        /// <returns>Retorna um objeto Task se a execução for bem sucedida, se não, retorna null.</returns>        
        public Task LoadAsyc(string name, bool callWhenIsFinished)
        {
            if (taskLoading != null)
                throw new InvalidOperationException("Já existe uma tarefa de carregamento de tela em andamento.");

            var s = this[name];

            if (s.LoadState == ScreenLoadState.UnLoaded)
            {
                var t = Task.Run(s.Load);
                LoadingScreen = s;                
                IsLoadingAsync = true;
                callLoadScreen = callWhenIsFinished;

                s.ChangeState(this, ScreenLoadState.Loading);

                taskLoading = t;
                return t;
            }
            else
            {
                throw new InvalidOperationException("A tela informada " + nameof(name) + " já está carregada ou está carregando.");
            }                
        }

        /// <summary>
        /// Define uma cena carregada ao seu estado inicial.
        /// </summary>
        /// <param name="name">O nome da tela a ser redefinida.</param>
        public void Reset(string name)
        {
            var s = this[name];

            if (s.LoadState == ScreenLoadState.Loaded)
                s.Reset();
        }

        /// <summary>
        /// Descarrega uma cena anteriormente carregada.
        /// </summary>
        /// <param name="name">O nome da tela a ser descarregada.</param>
        public void Unload(string name)
        {
            var s = this[name];

            if (s.LoadState == ScreenLoadState.Loaded)
                s.Unload();
        }

        /// <summary>Adiciona telas para esse gerenciador.</summary>
        /// <param name="screens">A quantidade desejada de telas a serem adicionadas.</param>
        public void Add(params Screen[] screens)
        {
            foreach (var s in screens)
            {
                if (s.Manager == null)
                    s.Manager = this;

                Screens.Add(s);

                if (Active == null)
                {
                    if (s.LoadState == ScreenLoadState.Loaded)
                        Active = s;
                }
            }             
        }

        /// <summary>Remove uma tela do gerenciador.</summary>
        /// <param name="name">Nome da tela.</param>
        public void Remove(string name)
        {
            Screen finder = this[name];
            Screens.Remove(finder);
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
            Active = finder ?? throw new ArgumentException("Não foi encontrada um mundo com esse nome", nameof(name));

            if (reset)
                old.Reset();
        }        

        /// <summary>Retorna uma tela definida pelo nome.</summary>
        /// <param name="name">O nome da tela.</param>
        public Screen Find(string name)
        {
            var s = Screens.Find(x => x.Name.Equals(name));
            return s;
        }        

        /// <summary>Atualiza o gerenciador de telas.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public virtual void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
            Active?.Update(gameTime);

            if(IsLoadingAsync)
            {
                if(taskLoading != null)
                {
                    if(taskLoading.IsCompleted)
                    {
                        IsLoadingAsync = false;

                        if (callLoadScreen)
                            Change(LoadingScreen.Name);

                        LoadingScreen = null;
                        taskLoading = null;
                    }                    
                }
            }
        }

        /// <summary>Desenha o gerenciador de telas com a sua tela ativa.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Um objeto SpriteBatch para desenho.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Active?.Draw(gameTime, spriteBatch);
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//

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
                Game = null;

                Screens.Clear();
                Screens = null;

                Active = null;
                
                LoadingScreen = null;
            }                

            disposed = true;
        }
    }
}