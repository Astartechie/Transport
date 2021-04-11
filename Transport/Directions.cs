using SFML.System;

namespace Transport
{
    public class Directions
    {
        public static readonly Vector2i Up = new Vector2i(0, -1);
        public static readonly Vector2i Right = new Vector2i(1, 0);
        public static readonly Vector2i Down = new Vector2i(0, 1);
        public static readonly Vector2i Left = new Vector2i(-1, 0);

        public static readonly Vector2i[] All = {Up, Right, Down, Left};
    }
}