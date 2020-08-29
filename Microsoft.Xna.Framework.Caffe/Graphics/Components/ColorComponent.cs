// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que implementa a funcionalidade de mundança de cor gradativa de uma entidade.
    /// </summary>
    public class ColorComponent : EntityComponent
    {
        private float elapsedTime = 0;

        /// <summary>Obtém ou define a cor final a ser alcançada. A propriedade Enable será setada como EnableGroup.Unavailable 
        /// quando a cor desejada for alcançada.</summary>
        public Color FinalColor { get; set; } = Color.White;
        /// <summary>Obtém ou define o tempo em milisegundos a ser atrasada para cada mudança de cor (default = 0).</summary>
        public float Delay { get; set; } = 0;
        /// <summary>Encapsula um método que será chamado quando a cor final for alcançada.</summary>
        public Action<GameTime> OnChangeColor;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de ColorComponent.
        /// </summary>
        public ColorComponent() : base()
        {
            Name = nameof(ColorComponent);
        }

        /// <summary>
        /// Inicializa uma nova instância de ColorComponent como cópia de outra instância.
        /// </summary>
        /// <param name="destination">A entidade a ser associada.</param>
        /// <param name="source">O componente a ser copiado.</param>
        public ColorComponent(Entity2D destination, ColorComponent source) : base(destination, source)
        {
            Delay = source.Delay;
            FinalColor = source.FinalColor;
            OnChangeColor = source.OnChangeColor;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância quando não for possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">O objeto a ser copiado</param>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        public override T Clone<T>(T source, Entity2D destination)
        {
            if (source is ColorComponent)
                return (T)Activator.CreateInstance(typeof(ColorComponent), destination, source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;            

            if (elapsedTime > Delay)
            {
                Color active = Entity.Transform.Color;
                Color final = FinalColor;

                if(!final.Equals(active))
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

                    Entity.Transform.Color = active;                    
                }

                if (active == final)
                {
                    OnChangeColor?.Invoke(gameTime);
                }

                elapsedTime = 0;
            }            

            base.Update(gameTime);
        }
    }
}