using System;
using SFML.System;

namespace Transport
{
    public static class Vector2FExtensions
    {
        public static float Length(this Vector2f vector) => MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);

        public static Vector2f Normalized(this Vector2f vector) => vector / vector.Length();
    }
}