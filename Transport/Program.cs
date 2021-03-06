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
            var grid = new SquareTileGrid(10, 10);

            var roadTile = new Tile(1);
            var sandRoadTile = new Tile(2);

            var lineY = 0;
            foreach (var line in File.ReadLines("Assets\\map.txt"))
            {
                var lineX = 0;
                foreach (var s in line.Split(","))
                {
                    if (s == "r")
                    {
                        grid.SetTile(lineX, lineY, roadTile);
                    }
                    else if (s == "s")
                    {
                        grid.SetTile(lineX, lineY, sandRoadTile);
                    }
                    else
                    {
                        grid.SetTile(lineX, lineY, Tile.Empty);
                    }


                    lineX++;
                }

                lineY++;
            }

            const int tileWidth = 64;
            const int tileHeight = 64;

            const int MaxSpeed = 128;


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
                    var tileRect = new IntRect(0, 0, tileWidth, tileHeight);
                    var position = new Vector2i(x, y);

                    var tileIndex = 0;
                    var tile = grid.GetTile(position);
                    if (tile.IsWalkable)
                    {
                        var neighbourTile = grid.GetTile(position + Directions.Up);
                        if (neighbourTile.IsWalkable)
                        {
                            tileIndex += 1;
                        }

                        neighbourTile = grid.GetTile(position + Directions.Right);
                        if (neighbourTile.IsWalkable)
                        {
                            tileIndex += 2;
                        }

                        neighbourTile = grid.GetTile(position + Directions.Down);
                        if (neighbourTile.IsWalkable)
                        {
                            tileIndex += 4;
                        }

                        neighbourTile = grid.GetTile(position + Directions.Left);
                        if (neighbourTile.IsWalkable)
                        {
                            tileIndex += 8;
                        }

                        switch (tileIndex)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 4:
                            case 8:
                                tileRect = new IntRect(0, 128, tileWidth, tileHeight);
                                break;

                            case 3:
                                tileRect = new IntRect(320, 64, tileWidth, tileHeight);
                                break;

                            case 5:
                                tileRect = new IntRect(64, 0, tileWidth, tileHeight);
                                break;

                            case 6:
                                tileRect = new IntRect(192, 64, tileWidth, tileHeight);
                                break;

                            case 7:
                                tileRect = new IntRect(192, 0, tileWidth, tileHeight);
                                break;
                            case 9:
                                tileRect = new IntRect(384, 64, tileWidth, tileHeight);
                                break;
                            case 10:
                                tileRect = new IntRect(128, 0, tileWidth, tileHeight);
                                break;
                            case 11:
                                tileRect = new IntRect(320, 0, tileWidth, tileHeight);
                                break;

                            case 12:
                                tileRect = new IntRect(256, 64, tileWidth, tileHeight);
                                break;

                            case 13:
                                tileRect = new IntRect(256, 0, tileWidth, tileHeight);
                                break;

                            case 14:
                                tileRect = new IntRect(384, 0, tileWidth, tileHeight);
                                break;

                            default:
                                tileRect = new IntRect(128, 64, tileWidth, tileHeight);
                                break;
                        }
                    }

                    if (tile == sandRoadTile)
                    {
                        tileRect = new IntRect(tileRect.Left, tileRect.Top + 128, tileRect.Width, tileRect.Height);
                    }

                    terrainSprites.Add(new Sprite(terrainTexture, tileRect)
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
                        var tankX = (int)(tankSprite.Position.X / tileWidth);
                        var tankY = (int)(tankSprite.Position.Y / tileHeight);
                        var tile = grid.GetTile(tankX, tankY);
                        var speed = (float) MaxSpeed / tile.MovementCost;
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