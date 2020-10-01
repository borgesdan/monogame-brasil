// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Componente que calcula se os limites do ator se encontram dentro ou fora de um retângulo informado.
    /// </summary>
    public class BoundsComponent : ActorComponent
    {
        //----------------------------------------//
        //-----         PROPRIEDADES         -----//
        //----------------------------------------//

        /// <summary>Obtém ou define o retângulo que representa os limites em que o ator pode se encontrar fora ou dentro.</summary>
        public Rectangle Bounds { get; set; } = Rectangle.Empty;
        /// <summary>
        /// Obtém um resultado dos cálculos em um objeto Vector2, em que o valor do vetor será a diferença em que a entidade se encontra fora ou dentro dos limites do retângulo informado.
        /// </summary>
        public Vector2 Result { get; private set; } = Vector2.Zero;

        /// <summary>
        /// Encapsula um método a ser chamado no fim do método Update deste component.
        /// <list type="number">
        /// <item>Actor é o ator que implementa esse componente.</item>
        /// <item>GameTime são os valores de tempo do jogo.</item>
        /// <item>Vector2 é o retorno da propriedade Result.</item>
        /// </list>
        /// </summary>
        public Action<Actor, GameTime, Vector2> OnUpdate;

        //----------------------------------------//
        //-----         CONSTRUTOR           -----//
        //----------------------------------------//

        /// <summary>
        /// Inicializa uma nova instância de BoundsComponent.
        /// </summary>
        /// <param name="actor">Define o ator o qual esse componente será associado.</param>
        /// <param name="bounds">Define o retângulo que representa os limites em que o ator pode se encontrar fora ou dentro.</param>
        public BoundsComponent(Actor actor, Rectangle bounds): base(actor) 
        {
            Bounds = bounds;
            Name = nameof(BoundsComponent);
        }

        /// <summary>
        /// Inicializa uma nova instância de BoundsComponent como cópia de outra instância
        /// </summary>
        /// <param name="destination">O ator o qual esse componente será associado.</param>
        /// <param name="source">O componente a ser copiado.</param>
        public BoundsComponent(Actor destination, BoundsComponent source) : base(destination, source)
        {
            Bounds = source.Bounds;
            Result = source.Result;
        }

        //----------------------------------------//
        //-----         MÉTODOS              -----//
        //----------------------------------------//        

        ///<inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            var ebounds = Actor.Bounds;

            if (Bounds.Width < ebounds.Width || Bounds.Height < ebounds.Height)
                throw new Exception("O tamanho dos limites informado não pode ser menor que os limites do ator.");

            Vector2 result = Vector2.Zero;

            //Cálculo do retângulo com e sem rotação.
            if (Actor.Transform.Rotation == 0)
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
                var boundsR = Actor.BoundsR;

                foreach (var p in boundsR.Points)
                {
                    if (p.X < Bounds.Left)
                    {
                        result.X -= p.X - Bounds.Left;
                    }
                    else if (p.X > Bounds.Right)
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

            Result = result;

            OnUpdate?.Invoke(Actor, gameTime, result);
            base.Update(gameTime);
        }

        //---------------------------------------//
        //-----         DISPOSE             -----//
        //---------------------------------------//

        private bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                OnUpdate = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}