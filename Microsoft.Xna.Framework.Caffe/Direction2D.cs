using System;

namespace Microsoft.Xna.Framework
{
    [Flags]
    public enum Direction2D : byte
    {
        Up          = 2,
        Left        = 8,
        Down        = 16,
        Right       = 32
    }
}