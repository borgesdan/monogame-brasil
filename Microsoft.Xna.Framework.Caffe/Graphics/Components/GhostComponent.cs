using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que duplica a imagem com ator quantas vezes necessário e por um período de tempo específico.
    /// </summary>
    public class GhostComponent : ActorComponent
    {
        class Ghost
        {
            public Animation Animation { get; set; }
            public Sprite Sprite { get; set; }
            public SpriteFrame Frame { get; set; }    
            public Vector2 Position { get; set; }
        }

        AnimatedActor entity = null;
        List<Ghost> ghosts = new List<Ghost>();
        int delayTime = 0;
        int elapsedTime = 0;

        //----------------------------------------//
        //-----         PROPRIEDADES         -----//
        //----------------------------------------//

        /// <summary>
        /// Obtém ou define o tempo em milisegundos em que as imagens fantasmas ficaram desenhadas na tela.
        /// </summary>
        public int Time { get; set; } = 2000;        
        /// <summary>
        /// Obtém ou define o tempo em milisegundos em que cada imagem do ator será recuperada.
        /// </summary>
        public int Delay { get; set; } = 1000;
        /// <summary>
        /// Obtém ou define o número máximo de fantasmas.
        /// </summary>
        public int Max { get; set; } = 5;
        /// <summary>
        /// Obtém ou define a cor para desenho do componente.
        /// </summary>
        public Color Color { get; set; } = new Color(100, 100, 100, 100);

        //----------------------------------------//
        //-----         CONSTRUTOR           -----//
        //----------------------------------------//        
        
        /// <summary>
        /// Inicializa uma nova instância de GhostComponent.
        /// </summary>
        /// <param name="actor">O ator associado a esse component.</param>
        public GhostComponent(AnimatedActor actor) : base(actor)
        {
            entity = actor;
        }

        /// <summary>
        /// Inicializa uma nova instância de GhostComponent como cópia de outra instâcia
        /// </summary>
        /// <param name="destination">O ator a ser associado.</param>
        /// <param name="source">O componente a ser copiado.</param>
        public GhostComponent(AnimatedActor destination, ActorComponent source) : base(destination, source)
        {
            entity = destination;
        }

        //----------------------------------------//
        //-----         FUNÇÕES              -----//
        //----------------------------------------//

        public override void Update(GameTime gameTime)
        {
            if (!Enable.IsEnabled)
                return;

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            delayTime += gameTime.ElapsedGameTime.Milliseconds;

            //Inicializamos a lista de fantasmas caso não haja nenhum
            if(ghosts.Count == 0)
            {
                Ghost ghost = new Ghost();
                ghost.Animation = entity.CurrentAnimation;
                ghost.Sprite = entity.CurrentAnimation.CurrentSprite;
                ghost.Frame = entity.CurrentAnimation.CurrentFrame;
                ghost.Position = entity.Transform.Position;

                ghosts.Add(ghost);
            } 
            
            if(delayTime >= Delay)
            {
                Ghost ghost = new Ghost();
                ghost.Animation = entity.CurrentAnimation;
                ghost.Sprite = entity.CurrentAnimation.CurrentSprite;
                ghost.Frame = entity.CurrentAnimation.CurrentFrame;
                ghost.Position = entity.Transform.Position;

                ghosts.Add(ghost);

                delayTime = 0;
            }

            if(elapsedTime >= Time)
            {
                ghosts.RemoveAt(0);
                elapsedTime = 0;
                elapsedTime += delayTime;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enable.IsVisible)
                return;

            for (int i = 0; i < ghosts.Count; i++)
            {
                Ghost g = ghosts[i];

                spriteBatch.Draw(g.Animation.CurrentSprite.Texture, g.Position, g.Frame.Bounds, Color);
            }

            base.Draw(gameTime, spriteBatch);
        }
    }
}