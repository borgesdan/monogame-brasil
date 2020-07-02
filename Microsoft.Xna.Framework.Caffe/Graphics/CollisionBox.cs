using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public struct CollisionBox
    {
        /// <summary>O index relativo ao frame do sprite.</summary>
        public readonly int Index;
        /// <summary>Define se o box recebe colisão com outro CollisionBox.</summary>
        public readonly bool CanCollide;
        /// <summary>Define pode receber dano de um AttackBox.</summary>
        public readonly bool CanTakeDamage;
        /// <summary>Define a porcentagem do dano sofrido, 1f equivale a 100%.</summary>
        public readonly float DamagePercentage;

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
    }
}