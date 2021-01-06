using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que duplica a imagem do ator por um período de tempo específico.
    /// </summary>
    public class GhostComponent : ActorComponent
    {
        /// <summary>
        /// Representa a imagem duplicada do ator.
        /// </summary>
        public class Ghost
        {
            public Sprite Sprite { get; set; } = null;
            public SpriteFrame Frame { get; set; } = new SpriteFrame();
            public Vector2 Position { get; set; } = Vector2.Zero;
            public float Rotation { get; set; } = 0;
            public Vector2 Origin { get; set; } = Vector2.Zero;
            public Vector2 Scale { get; set; } = Vector2.One;
            public float LayerDepth { get; set; } = 0;
            public SpriteEffects Effects { get; set; } = SpriteEffects.None;

            public float ElapsedTime { get; set; } = 0;            
        }

        AnimatedActor entity = null;
        List<Ghost> ghosts = new List<Ghost>();
        int delayTime = 0;

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
        /// Obtém ou define a cor para desenho do componente.
        /// </summary>
        public Color Color { get; set; } = new Color(100, 100, 100, 100);

        /// <summary>
        /// Encapsula um método a ser chamado quando for criado um objeto Ghost.
        /// </summary>
        public event Action<AnimatedActor, Ghost> OnCreate;
        /// <summary>
        /// Encapsula um método a ser chamado quando for removido um objeto Ghost.
        /// </summary>
        public event Action<AnimatedActor, Ghost> OnRemove;

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

        protected override void _Update(GameTime gameTime)
        {            
            delayTime += gameTime.ElapsedGameTime.Milliseconds;             

            if (delayTime >= Delay)
            {
                Ghost ghost = new Ghost();
                ghost.Sprite = entity.CurrentAnimation.CurrentSprite;
                ghost.Frame = entity.CurrentAnimation.CurrentFrame;
                ghost.Position = entity.Transform.Position;
                ghost.Rotation = entity.Transform.Rotation;
                ghost.Scale = entity.Transform.Scale;
                ghost.LayerDepth = entity.Transform.LayerDepth;
                ghost.Effects = entity.Transform.SpriteEffects;
                ghost.Origin = entity.Transform.Origin;
                
                ghosts.Add(ghost);
                delayTime = 0;

                OnCreate?.Invoke(entity, ghost);
            }            

            for(int i = 0; i < ghosts.Count; i++)
            {
                ghosts[i].ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (ghosts[i].ElapsedTime >= Time)
                {
                    OnRemove?.Invoke(entity, ghosts[i]);
                    ghosts.RemoveAt(i);
                }                    
            }
        }

        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ghosts.Count; i++)
            {
                Ghost g = ghosts[i];

                spriteBatch.Draw(g.Sprite.Texture, g.Position, g.Frame.Bounds, Color, g.Rotation, g.Origin, g.Scale, g.Effects, g.LayerDepth);
            }
        }
    }
}