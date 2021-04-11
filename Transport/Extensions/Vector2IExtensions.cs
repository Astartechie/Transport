using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace Transport.Extensions
{
    public static class Vector2IExtensions
    {
        public static int ManhattanDistance(this Vector2i vector) => Math.Abs(vector.X) + Math.Abs(vector.Y);
    }
}
