// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que implementa a funcionalidade de mundança de cor gradativa de um ator.
    /// </summary>
    public class ColorComponent : ActorComponent
    {
        private float elapsedTime = 0;

        /// <summary>Obtém ou define a cor final a ser alcançada. </summary>
        public Color FinalColor { get; set; } = Color.White;
        /// <summary>Obtém ou define o tempo em milisegundos a ser atrasada para cada mudança de cor (default = 0).</summary>
        public float Delay { get; set; } = 0;
        /// <summary>Encapsula um método que será chamado quando a cor final for alcançada.</summary>
        /// <list type="bullet">
        /// <item>GameTime são os valores de tempo do jogo.</item>
        /// </list>
        /// </summary>
        public event Action<GameTime> OnChangeColor;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de ColorComponent.
        /// </summary>
        public ColorComponent(Actor actor) : base(actor)
        {
            Name = nameof(ColorComponent);
        }

        /// <summary>
        /// Inicializa uma nova instância de ColorComponent como cópia de outra instância.
        /// </summary>
        /// <param name="destination">O ator a ser associado.</param>
        /// <param name="source">O componente a ser copiado.</param>
        public ColorComponent(Actor destination, ColorComponent source) : base(destination, source)
        {
            Delay = source.Delay;
            FinalColor = source.FinalColor;
            OnChangeColor = source.OnChangeColor;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//        

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        protected override void _Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > Delay)
            {
                Color active = Actor.Transform.Color;
                Color final = FinalColor;

                if (!final.Equals(active))
                {
                    if (active.R > final.R)
                        active.R--;
                    else if (active.R < final.R)
                        active.R++;

                    if (active.G > final.G)
                        active.G--;
                    else if (active.G < final.G)
                        active.G++;

                    if (active.B > final.B)
                        active.B--;
                    else if (active.B < final.B)
                        active.B++;

                    if (active.A > final.A)
                        active.A--;
                    else if (active.A < final.A)
                        active.A++;

                    Actor.Transform.Color = active;
                }

                if (active == final)
                {
                    OnChangeColor?.Invoke(gameTime);
                }

                elapsedTime = 0;
            }            
        }

        protected override void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {            
        }
    }
}