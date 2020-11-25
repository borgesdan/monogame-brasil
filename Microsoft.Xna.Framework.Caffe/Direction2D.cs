// Danilo Borges Santos, 2020.

using System;

namespace Microsoft.Xna.Framework
{
    [Flags]
    public enum Direction2D : byte
    {
        Up          = 2,
        Down        = 4,
        Right       = 8,
        Left        = 16
    }
}