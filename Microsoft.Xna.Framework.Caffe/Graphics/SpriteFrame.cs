// Danilo Borges Santos, 2020.

using System;
using System.Collections.Generic;

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

        /*
        * Sobre OriginCorrection
        * Se uma entidade tem uma altura de 90 no primeiro frame e este valor é a base para as demais correções; e se
        * o próximo frame tiver a altura 95, então deve-se adicionar o valor new Vector (0, 5) em OriginCorrection. Se
        * o próximo frame tiver a altura de 85, então deve-se adicionar o valor new Vector(0, -5).
        * 
        */

        /// <summary>
        /// Necessário para um alinhamento caso nem todos os frames de uma animação sejam de tamanhos iguais.
        /// <para>
        /// Exemplo: Se o primeiro frame de uma animação tem o valor 90px de altura e o próximo 95px;        
        /// </para>
        /// <para>
        /// Então deve-se adicionar o valor new Vector(0,5) em OriginCorrection no segundo frame para um melhor alinhamento.
        /// </para>
        /// </summary>        
        public Vector2 OriginCorrection;       

        /// <summary>Obtém um retângulo com a posição e tamanho do frame dentro do SpriteSheet.</summary>
        public Rectangle Bounds
        {
            get => new Rectangle(X, Y, Width, Height);
        }

        //---------------------------------------//
        //-----         CONSTRUTOR          -----//
        //---------------------------------------//

        /// <summary>Cria um objeto da estrutura SpriteFrame.</summary>
        /// <param name="frame">Um retângulo com a posição e o tamanho do frame.</param>        
        public SpriteFrame(Rectangle frame) : this(frame, Vector2.Zero) { }

        /// <summary>Cria um objeto da estrutura SpriteFrame.</summary>
        /// <param name="frame">Um retângulo com a posição e o tamanho do frame.</param>
        /// <param name="originCorrection">Necessário para um alinhamento caso nem todos os frames de uma animação sejam iguais.</param>
        public SpriteFrame(Rectangle frame, Vector2 originCorrection)
        {
            X = frame.X;
            Y = frame.Y;
            Width = frame.Width;
            Height = frame.Height;

            OriginCorrection = originCorrection;
        }

        /// <summary>Cria um objeto da estrutura SpriteFrame.</summary>
        /// <param name="x">A posição no eixo Y na Textura.</param>
        /// <param name="y">A posição no eixo X na Textura.</param>
        /// <param name="width">A largura do frame.</param>
        /// <param name="height">A altura do frame.</param>
        public SpriteFrame(int x, int y, int width, int height) : this(new Rectangle(x, y, width, height)) { }

        /// <summary>Cria um objeto da estrutura SpriteFrame.</summary>
        /// <param name="x">A posição no eixo Y na Textura.</param>
        /// <param name="y">A posição no eixo X na Textura.</param>
        /// <param name="width">A largura do frame.</param>
        /// <param name="height">A altura do frame.</param>
        /// <param name="originCorrection">Necessário para um alinhamento caso nem todos os frames de uma animação sejam iguais.</param> 
        public SpriteFrame(int x, int y, int width, int height, Vector2 originCorrection) : this(new Rectangle(x, y, width, height), originCorrection) { }

        //---------------------------------------//
        //-----         METÓDOS             -----//
        //---------------------------------------//

        public override bool Equals(object obj)
        {
            return obj is SpriteFrame frame && Equals(frame);
        }

        public bool Equals(SpriteFrame other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Width == other.Width &&
                   Height == other.Height &&
                   OriginCorrection.Equals(other.OriginCorrection) &&
                   Bounds.Equals(other.Bounds);
        }

        public override int GetHashCode()
        {
            var hashCode = 1024957483;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(OriginCorrection);
            hashCode = hashCode * -1521134295 + EqualityComparer<Rectangle>.Default.GetHashCode(Bounds);
            return hashCode;
        }

        public static bool operator ==(SpriteFrame left, SpriteFrame right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SpriteFrame left, SpriteFrame right)
        {
            return !(left == right);
        }
    }
}