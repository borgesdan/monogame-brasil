// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Ator que representa uma entidade personalizável.</summary>
    public abstract class Entity2D : Actor
    {
        //---------------------------------------//
        //-----         PROPRIEDADES        -----//
        //---------------------------------------//
        /// <summary>Obtém ou define a tela a qual a entidade está associada.</summary>
        public Screen Screen { get; set; } = null;        

        //---------------------------------------//
        //-----         EVENTOS             -----//
        //---------------------------------------//

        /// <summary>Encapsula métodos que serão invocados na função Update.</summary>        
        public event Action<Entity2D, GameTime> OnUpdate;
        /// <summary>Encapsula métodos que serão invocados na função Draw.</summary>
        public event Action<Entity2D, GameTime, SpriteBatch> OnDraw;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Inicializa uma nova instância de Entity2D.</summary>
        /// <param name="game">Instância atual da classe Game.</param>
        /// <param name="name">Nome da entidade.</param>
        protected Entity2D(Game game, string name) : base(game) 
        {
            Name = name;
        }

        /// <summary>Inicializa uma nova instância de Entity2D como cópia de outro Entity2D.</summary>
        /// <param name="source">A entidade a ser copiada.</param>
        protected Entity2D(Entity2D source) : base(source)
        {
            Screen = source.Screen;
            OnUpdate = source.OnUpdate;
            OnDraw = source.OnDraw;
        }

        //---------------------------------------//
        //-----         MÉTODOS             -----//
        //---------------------------------------//        

        /// <summary>Atualiza a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            Transform.Update();

            //Chama o evento.
            OnUpdate?.Invoke(this, gameTime);

            //Atualiza os componentes.
            Components.Update(gameTime);

            //Atualiza os limites da entidade.
            UpdateBounds();
        }

        /// <summary>Desenha a entidade.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        /// <param name="spriteBatch">Uma instância da classe SpriteBath para a entidade ser desenhada.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            OnDraw?.Invoke(this, gameTime, spriteBatch);
            base.Draw(gameTime, spriteBatch);
        } 

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//
        bool disposed = false;
        
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if(disposing)
            {
                Screen = null;
            }

            disposed = true;

            base.Dispose(disposing);
        } 
    }
}