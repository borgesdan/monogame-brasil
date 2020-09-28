// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Representa uma recorte da imagem [frame] de um Sprite.</summary>
    public struct SpriteFrame : IEquatable<SpriteFrame>
    {
        //---------------------------------------//
        //-----         VARIAVEIS           -----//
        //---------------------------------------//   

        /// <summary>A posição no eixo X na Textura.</v>
        public int X;
        /// <summary>A posição no eixo Y na Textura.</summary>
        public int Y;
        /// <summary>A largura do frame.</summary>
        public int Width;
        /// <summary>A altura do frame.</summary>
        public int Height;
        /// <summary>Valor de correção para alinhamento deste recorte em uma série de recortes de uma animação no eixo X, caso necessário.</summary>
        public float AlignX;
        /// <summary>Valor de correção para alinhamento deste recorte em uma série de recortes de uma animação no eixo Y, caso necessário.</summary>
        public float AlignY;

        /// <summary>Obtém um retângulo com a posição e tamanho do frame dentro do SpriteSheet.</summary>
        public Rectangle Bounds { get => new Rectangle(X, Y, Width, Height); }
        /// <summary>Obtém um vetor com os valores do alinhamento.</summary>
        public Vector2 Align { get => new Vector2(AlignX, AlignY); }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>
        /// Cria um novo objeto SpriteFrame.
        /// </summary>
        /// <param name="x">A posição no eixo X do recorte na Textura.</param>
        /// <param name="y">A posição no eixo Y do recorte na Textura.</param>
        /// <param name="width">A largura do recorte.</param>
        /// <param name="height">A altura do recorte.</param>
        /// <param name="alignX">Valor de correção para alinhamento deste recorte em uma série de recortes de uma animação no eixo X, caso necessário.</param>
        /// <param name="alignY">Valor de correção para alinhamento deste recorte em uma série de recortes de uma animação no eixo Y, caso necessário.</param>
        public SpriteFrame(int x, int y, int width, int height, float alignX = 0, float alignY = 0)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            AlignX = alignX;
            AlignY = alignY;
        }

        //---------------------------------------//
        //-----         METÓDOS             -----//
        //---------------------------------------//

        /// <summary>
        /// Cria um novo objeto SpriteFrame através de um objeto Rectangle.
        /// </summary>
        /// <param name="rectangle">O objeto Rectangle a ser utilizado.</param>
        /// <param name="align">Um vetor com o alinhamento para correção em animações, caso necessário.</param>
        public static SpriteFrame Create(Rectangle rectangle, Vector2 align)
        {
            return new SpriteFrame(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, align.X, align.Y);
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SpriteFrame frame && Equals(frame);
        }

        /// <summary>Retorna True caso os dois objetos forem iguais.</summary>
        public bool Equals(SpriteFrame other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Width == other.Width &&
                   Height == other.Height &&
                   AlignX == other.AlignX &&
                   AlignY == other.AlignY;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height, AlignX, AlignY);
        }

        ///<inheritdoc/>
        public static bool operator ==(SpriteFrame left, SpriteFrame right)
        {
            return left.Equals(right);
        }

        ///<inheritdoc/>
        public static bool operator !=(SpriteFrame left, SpriteFrame right)
        {
            return !(left == right);
        }
    }
}