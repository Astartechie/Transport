using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Transport
{
    internal class Program
    {
        private static void Main()
        {
            var grid = new SquareGrid(10, 10);

            var lineY = 0;
            foreach (var line in File.ReadLines("Assets\\map.txt"))
            {
                var lineX = 0;
                foreach (var s in line.Split(","))
                {
                    grid.SetWalkable(lineX, lineY, s == "1");
                    lineX++;
                }

                lineY++;
            }

            const int tileWidth = 64;
            const int tileHeight = 64;

            const int speed = 128;


            var window = new RenderWindow(new VideoMode((uint) grid.Width * tileWidth, (uint) grid.Height * tileHeight), "Transport");

            var terrainTexture = new Texture("Assets\\terrainTiles_default.png");

            var tankTexture = new Texture("Assets\\tank_red.png");
            var tankSprite = new Sprite(tankTexture)
            {
                Origin = new Vector2f(19, 23),
                Position = new Vector2f(96, 96)
            };

            var terrainSprites = new List<Sprite>();

            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    var tileOffset = grid.IsWalkable(x, y) ? new IntRect(tileWidth * 2, tileHeight, tileWidth, tileHeight) : new IntRect(0, 0, tileWidth, tileHeight);
                    terrainSprites.Add(new Sprite(terrainTexture, tileOffset)
                    {
                        Position = new Vector2f(x * tileWidth, y * tileHeight)
                    });
                }
            }

            var queue = new Queue<Vector2f>();

            window.MouseButtonReleased += (sender, args) =>
            {
                if (args.Button != Mouse.Button.Left) return;

                var tileX = args.X / tileWidth;
                var tileY = args.Y / tileHeight;

                if (!grid.IsWalkable(tileX, tileY)) return;

                var tankX = (int) (tankSprite.Position.X / tileWidth);
                var tankY = (int) (tankSprite.Position.Y / tileHeight);

                var path = Pathing.Find(grid, new Vector2i(tankX, tankY), new Vector2i(tileX, tileY));

                Console.Write($"[{tankX},{tankY}] -> [{tileX},{tileY}]: ");
                if (!path.Any())
                {
                    Console.Write("No Path");
                }
                else
                {
                    Console.Write(string.Join(" -> ", path.Select(point => $"[{point.X},{point.Y}]")));
                }

                Console.WriteLine();

                queue.Clear();
                foreach (var point in path)
                {
                    queue.Enqueue(new Vector2f((point.X + 0.5f) * tileWidth, (point.Y + 0.5f) * tileHeight));
                }
            };

            window.SetActive();
            var clock = new Clock();
            while (window.IsOpen)
            {
                var deltaTime = clock.Restart().AsSeconds();
                window.Clear();
                window.DispatchEvents();
                foreach (var terrainSprite in terrainSprites)
                {
                    window.Draw(terrainSprite);
                }

                if (queue.Count > 0)
                {
                    var direction = queue.Peek() - tankSprite.Position;
                    if (direction.Length() > 1)
                    {
                        tankSprite.Position += direction.Normalized() * speed * deltaTime;
                    }
                    else
                    {
                        queue.Dequeue();
                    }
                }

                window.Draw(tankSprite);
                window.Display();
            }
        }
    }
}