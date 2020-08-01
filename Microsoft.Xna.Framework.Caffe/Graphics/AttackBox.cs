using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public struct AttackBox : IEquatable<AttackBox>
    {
        /// <summary>O index relativo ao frame do sprite.</summary>
        public readonly int Index;
        /// <summary>Define a potência do dano se houver uma colisão com um CollisionBox.</summary>
        public readonly int Power;

        /// <summary>A posição no eixo Y relativo ao frame.</v>
        public readonly int X;
        /// <summary>A posição no eixo X relativo ao frame.</summary>
        public readonly int Y;
        /// <summary>A largura do box.</summary>
        public readonly int Width;
        /// <summary>A altura do box.</summary>
        public readonly int Height;

        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 01.</summary>
        public byte T01;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 02.</summary>
        public byte T02;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 03.</summary>
        public byte T03;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 04.</summary>
        public byte T04;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 05.</summary>
        public byte T05;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 06.</summary>
        public byte T06;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 07.</summary>
        public byte T07;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 08.</summary>
        public byte T08;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 09.</summary>
        public byte T09;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 10.</summary>
        public byte T10;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 11.</summary>
        public byte T11;
        /// <summary>Define uma valor que pode ser recuperado futuramente na tag 12.</summary>
        public byte T12;

        /// <summary>Obtém um retângulo com a posição e tamanho do frame dentro do SpriteSheet.</summary>
        public Rectangle Bounds
        {
            get => new Rectangle(X, Y, Width, Height);
        }

        public AttackBox(int index, int power, Rectangle rectangle, params byte[] tags)
        {
            Index = index;
            X = rectangle.X;
            Y = rectangle.Y;
            Width = rectangle.Width;
            Height = rectangle.Height;
            Power = power;

            T01 = 0;
            T02 = 0;
            T03 = 0;
            T04 = 0;
            T05 = 0;
            T06 = 0;
            T07 = 0;
            T08 = 0;
            T09 = 0;
            T10 = 0;
            T11 = 0;
            T12 = 0;

            for (int i = 0; i < tags.Length; i++)
            {
                if (i == 0)
                    T01 = tags[i];
                else if (i == 1)
                    T02 = tags[i];
                else if (i == 2)
                    T03 = tags[i];
                else if (i == 3)
                    T04 = tags[i];
                else if (i == 4)
                    T05 = tags[i];
                else if (i == 5)
                    T06 = tags[i];
                else if (i == 6)
                    T07 = tags[i];
                else if (i == 7)
                    T08 = tags[i];
                else if (i == 8)
                    T09 = tags[i];
                else if (i == 9)
                    T10 = tags[i];
                else if (i == 10)
                    T11 = tags[i];
                else if (i == 11)
                    T12 = tags[i];
                else
                    break;
            }
        }

        /// <summary>
        /// Obtém a posição relativa da caixa no frame independente da escala deste.
        /// </summary>
        /// <param name="frame">O frame que o mesmo index do box.</param>
        /// <param name="target">O frame do mesmo index mas com o tamanho final para o calculo.</param>
        /// <param name="scale">O tamanho da escala do target.</param>
        /// <param name="flip">O SpriteEffect pertencente ao estado atual da entidade.</param>
        public AttackBox GetRelativePosition(Rectangle frame, Rectangle target, Vector2 scale, SpriteEffects flip)
        {
            int x = frame.X - this.X;
            int y = frame.Y - this.Y;
            int w = frame.Width - this.Width;
            int h = frame.Height - this.Height;

            Rectangle rectangle = new Rectangle(target.X - (int)(x * scale.X), target.Top - (int)(y * scale.Y), target.Width - (int)(w * scale.X), target.Height - (int)(h * scale.Y));

            if (flip == SpriteEffects.FlipHorizontally)
            {
                Point rotated = Rotation.GetRotation(rectangle.Location, target.Center.ToVector2(), MathHelper.ToRadians(180));
                rectangle.X = rotated.X - rectangle.Width;
            }
            if (flip == SpriteEffects.FlipVertically)
            {
                Point rotated = Rotation.GetRotation(rectangle.Location, target.Center.ToVector2(), MathHelper.ToRadians(180));
                rectangle.Y = rotated.Y - rectangle.Height;
            }

            AttackBox cb = new AttackBox(Index, Power, rectangle,
                T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12);

            return cb;
        }

        public override bool Equals(object obj)
        {
            return obj is AttackBox frame && Equals(frame);
        }

        public bool Equals(AttackBox box)
        {
            return Index == box.Index &&
                   Power == box.Power &&
                   X == box.X &&
                   Y == box.Y &&
                   Width == box.Width &&
                   Height == box.Height &&
                   T01 == box.T01 &&
                   T02 == box.T02 &&
                   T03 == box.T03 &&
                   T04 == box.T04 &&
                   T05 == box.T05 &&
                   T06 == box.T06 &&
                   T07 == box.T07 &&
                   T08 == box.T08 &&
                   T09 == box.T09 &&
                   T10 == box.T10 &&
                   T11 == box.T11 &&
                   T12 == box.T12 &&
                   Bounds.Equals(box.Bounds);
        }

        public override int GetHashCode()
        {
            int hashCode = 2131157796;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Power.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + T01.GetHashCode();
            hashCode = hashCode * -1521134295 + T02.GetHashCode();
            hashCode = hashCode * -1521134295 + T03.GetHashCode();
            hashCode = hashCode * -1521134295 + T04.GetHashCode();
            hashCode = hashCode * -1521134295 + T05.GetHashCode();
            hashCode = hashCode * -1521134295 + T06.GetHashCode();
            hashCode = hashCode * -1521134295 + T07.GetHashCode();
            hashCode = hashCode * -1521134295 + T08.GetHashCode();
            hashCode = hashCode * -1521134295 + T09.GetHashCode();
            hashCode = hashCode * -1521134295 + T10.GetHashCode();
            hashCode = hashCode * -1521134295 + T11.GetHashCode();
            hashCode = hashCode * -1521134295 + T12.GetHashCode();
            hashCode = hashCode * -1521134295 + Bounds.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(AttackBox left, AttackBox right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AttackBox left, AttackBox right)
        {
            return !(left == right);
        }
    }
}