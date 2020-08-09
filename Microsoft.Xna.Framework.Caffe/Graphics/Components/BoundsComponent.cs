// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que calcula se os limites da entidade se encontram dentro ou fora dos limites informados.
    /// </summary>
    public class BoundsComponent : EntityComponent
    {
        /// <summary>Os limites a serem usados para cálculo.</summary>
        public Rectangle Bounds { get; set; }

        /// <summary>Encapsula um metodo com os parâmetros definidos e que expõe um resultado final no formato Vector2.
        /// Em que o valor do vetor será a diferença em que a entidade se encontra fora dos limites.</summary>
        public ResultAction<Vector2> OnOutOfBounds;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe BoundsComponent.
        /// </summary>
        /// <param name="bounds">Os limites a serem usados para cálculo.</param>
        public BoundsComponent(Rectangle bounds) : base()
        {
            Name = nameof(BoundsComponent);
            Bounds = bounds;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe BoundsComponent.
        /// </summary>
        /// <param name="bounds">Os limites a serem usados para cálculo.</param>
        /// <param name="action">Encapsula um metodo com os parâmetros definidos</param>
        public BoundsComponent(Rectangle bounds, ResultAction<Vector2> action) : base()
        {
            Name = nameof(BoundsComponent);
            Bounds = bounds;
            OnOutOfBounds += action;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe BoundsComponent como uma cópia.
        /// </summary>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        /// <param name="source">A origem para cópia.</param>
        public BoundsComponent(Entity2D destination, BoundsComponent source) : base(destination, source)
        {
            Bounds = source.Bounds;
            OnOutOfBounds = source.OnOutOfBounds;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria uma nova instância de BoundsComponent quando não é possível utilizar o construtor de cópia.
        /// </summary>
        /// <typeparam name="T">O tipo a ser informado.</typeparam>
        /// <param name="source">O origem a ser copiada.</param>
        /// <param name="destination">A entidade a ser associada a esse componente.</param>
        public override T Clone<T>(T source, Entity2D destination)
        {
            if (source is BoundsComponent)
                return (T)Activator.CreateInstance(typeof(BoundsComponent), destination, source);
            else
                throw new InvalidCastException();
        }

        /// <summary>Atualiza o componente.</summary>
        /// <param name="gameTime">Fornece acesso aos valores de tempo do jogo.</param>
        public override void Update(GameTime gameTime)
        {
            var ebounds = Entity.Bounds;

            if (Bounds.Width < ebounds.Width || Bounds.Height < ebounds.Height)
                throw new Exception("O tamanho dos limites informado não pode ser menor que os limites da entidade.");

            Vector2 result = Vector2.Zero;

            //Cálculo do retângulo com e sem rotação.
            if(Entity.Transform.Rotation == 0)
            {
                if (ebounds.Left < Bounds.Left)
                {
                    result.X -= ebounds.Left - Bounds.Left;
                }
                else if (ebounds.Right > Bounds.Right)
                {
                    result.X -= ebounds.Right - Bounds.Right;
                }
                if (ebounds.Top < Bounds.Top)
                {
                    result.Y -= ebounds.Top - Bounds.Top;
                }
                else if (ebounds.Bottom > Bounds.Bottom)
                {
                    result.Y -= ebounds.Bottom - Bounds.Bottom;
                }
            }
            else
            {
                var boundsR = Entity.BoundsR;

                foreach(var p in boundsR.Points)
                {
                    if(p.X < Bounds.Left)
                    {
                        result.X -= p.X - Bounds.Left;
                    }
                    else if(p.X > Bounds.Right)
                    {
                        result.X -= p.X - Bounds.Right;
                    }
                    if (p.Y < Bounds.Top)
                    {
                        result.Y -= p.Y - Bounds.Top;
                    }
                    else if (p.Y > Bounds.Bottom)
                    {
                        result.Y -= p.Y - Bounds.Bottom;
                    }
                }
            }

            OnOutOfBounds?.Invoke(Entity, gameTime, result);

            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                OnOutOfBounds = null;
            }

            base.Dispose(disposing);
        }
    }
}