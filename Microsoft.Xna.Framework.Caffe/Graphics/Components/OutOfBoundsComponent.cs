// Danilo Borges Santos, 2020. Contato: danilo.bsto@gmail.com

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que calcula se os limites da entidade se encontram dentro do limite informado.
    /// </summary>
    public class OutOfBoundsComponent : EntityComponent
    {
        /// <summary>Os limites da entidade em que a entidade pode se encontrar dentro.</summary>
        public Rectangle Bounds { get; set; }
        /// <summary>Encapsula um metodo com os parâmetros definidos e que expõe um resultado final no formato Vector2.
        /// Em que o vetor com valor 0 significa que a entidade se encontra dentro dos limites informado, caso isso não ocorra,
        /// o valor no vetor será a diferença em que a entidade se encontra fora dos limites.</summary>
        public ResultAction<Vector2> OnOutOfBounds;

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância da classe OutOfBoundsComponent.
        /// </summary>
        /// <param name="bounds">Os limites em que a entidade se encontra dentro.</param>
        public OutOfBoundsComponent(Rectangle bounds) : base()
        {
            Name = nameof(OutOfBoundsComponent);
            Bounds = bounds;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe OutOfBoundsComponent.
        /// </summary>
        /// <param name="bounds">Os limites em que a entidade se encontra dentro.</param>
        /// <param name="action">Encapsula um metodo com os parâmetros definidos</param>
        public OutOfBoundsComponent(Rectangle bounds, ResultAction<Vector2> action) : base()
        {
            Name = nameof(OutOfBoundsComponent);
            Bounds = bounds;
            OnOutOfBounds += action;
        }           

        /// <summary>
        /// Inicializa uma nova instância da classe OutOfBoundsComponent como uma cópia.
        /// </summary>
        /// <param name="source">A origem para cópia.</param>
        public OutOfBoundsComponent(OutOfBoundsComponent source) : base(source)
        {
            Bounds = source.Bounds;
            OnOutOfBounds = source.OnOutOfBounds;
        }

        //---------------------------------------//
        //-----         FUNÇÕES             -----//
        //---------------------------------------//

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
            if (disposing)
            {
                OnOutOfBounds = null;
            }

            base.Dispose(disposing);
        }
    }
}