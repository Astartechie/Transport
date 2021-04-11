using SFML.System;

namespace Transport
{
    public class SquareTileGrid
    {
        public SquareTileGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new Tile[width, height];
        }

        public int Width { get; }
        public int Height { get; }

        public Tile GetTile(Vector2i position) => GetTile(position.X, position.Y);

        public Tile GetTile(int x, int y) => IsWithin(x, y) ? _tiles[x, y] : Tile.Empty;
        
        public void SetTile(Vector2i position, Tile tile) => SetTile(position.X, position.Y, tile);

        public void SetTile(int x, int y, Tile tile)
        {
            if (!IsWithin(x, y)) return;
            _tiles[x, y] = tile;
        }

        private bool IsWithin(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
        
        private readonly Tile[,] _tiles;
    }
}