// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    public struct CollisionBox : IEquatable<CollisionBox>
    {
        /// <summary>O index relativo ao frame do sprite.</summary>
        public int Index;
        /// <summary>Define se o box recebe colisão com outro CollisionBox.</summary>
        public bool CanCollide;
        /// <summary>Define pode receber dano de um AttackBox.</summary>
        public bool CanTakeDamage;
        /// <summary>Define a porcentagem do dano sofrido, 1f equivale a 100%.</summary>
        public float DamagePercentage;

        /// <summary>A posição no eixo Y relativo ao frame.</v>
        public int X;
        /// <summary>A posição no eixo X relativo ao frame.</summary>
        public int Y;
        /// <summary>A largura do box.</summary>
        public int Width;
        /// <summary>A altura do box.</summary>
        public int Height;

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

        /// <summary>
        /// Cria um novo objeto CollisionBox.
        /// </summary>
        /// <param name="index">O index relativo ao SpriteFrame.</param>
        /// <param name="rectangle">O retângulo que representa a posição e o tamanho do box.</param>
        public CollisionBox(int index, Rectangle rectangle) : this(index, rectangle, true, true, 1f) 
        { }

        /// <summary>
        /// Cria um novo objeto CollisionBox.
        /// </summary>
        /// <param name="index">O index relativo ao SpriteFrame.</param>
        /// <param name="rectangle">O retângulo que representa a posição e o tamanho do box.</param>
        /// <param name="canCollide">True se o box deve está habilitado para colisão.</param>
        /// <param name="canTakeDamage">True se o box deve receber dano.</param>
        /// <param name="damagePercentage">O valor em porcentagem para a colisão.</param>
        /// <param name="tags">Os valores das tags.</param>
        public CollisionBox(int index, Rectangle rectangle, bool canCollide, bool canTakeDamage, float damagePercentage, params byte[] tags)
        {
            Index = index;
            X = rectangle.X;
            Y = rectangle.Y;
            Width = rectangle.Width;
            Height = rectangle.Height;
            CanCollide = canCollide;
            CanTakeDamage = canTakeDamage;
            DamagePercentage = MathHelper.Clamp(damagePercentage, 0, 1f);

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
        /// <param name="scale">O valor da escala do target.</param>
        /// <param name="flip">O SpriteEffect pertencente ao estado atual da entidade.</param>
        public CollisionBox GetRelativePosition(Rectangle frame, Rectangle target, Vector2 scale, SpriteEffects flip)
        {
            int x = frame.X - this.X;
            int y = frame.Y - this.Y;
            int w = frame.Width - this.Width;
            int h = frame.Height - this.Height;            

            Rectangle rectangle = new Rectangle(target.X - (int)(x * scale.X), target.Top - (int)(y * scale.Y), target.Width - (int)(w * scale.X), target.Height - (int)(h * scale.Y));

            if(flip == SpriteEffects.FlipHorizontally)
            {
                Point rotated = Rotation.GetRotation(rectangle.Location, target.Center.ToVector2(), MathHelper.ToRadians(180));
                rectangle.X = rotated.X - rectangle.Width;
            }
            if (flip == SpriteEffects.FlipVertically)
            {
                Point rotated = Rotation.GetRotation(rectangle.Location, target.Center.ToVector2(), MathHelper.ToRadians(180));
                rectangle.Y = rotated.Y - rectangle.Height;
            }

            CollisionBox cb = new CollisionBox(Index, rectangle, CanCollide, CanTakeDamage, DamagePercentage,
                T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12);

            return cb;
        }

        public static bool operator ==(CollisionBox left, CollisionBox right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CollisionBox left, CollisionBox right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is SpriteFrame frame && Equals(frame);
        }

        public bool Equals(CollisionBox box)
        {
            return Index == box.Index &&
                   CanCollide == box.CanCollide &&
                   CanTakeDamage == box.CanTakeDamage &&
                   DamagePercentage == box.DamagePercentage &&
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
            int hashCode = -856949754;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + CanCollide.GetHashCode();
            hashCode = hashCode * -1521134295 + CanTakeDamage.GetHashCode();
            hashCode = hashCode * -1521134295 + DamagePercentage.GetHashCode();
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
    }
}