using SFML.System;

namespace Transport
{
    public class SquareGrid
    {
        public SquareGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _squares = new bool[width, height];
        }

        public int Width { get; }
        public int Height { get; }

        public bool IsWalkable(Vector2i position) => IsWalkable(position.X, position.Y);

        public bool IsWalkable(int x, int y) => IsWithin(x, y) && _squares[x, y];

        public void SetWalkable(Vector2i position, bool walkable) => SetWalkable(position.X, position.Y, walkable);

        public void SetWalkable(int x, int y, bool walkable)
        {
            if (!IsWithin(x, y)) return;
            _squares[x, y] = walkable;
        }

        private bool IsWithin(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;


        private readonly bool[,] _squares;
    }
}