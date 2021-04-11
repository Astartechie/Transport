using System;
using System.Collections.Generic;
using System.Text;

namespace Transport
{
    public class Tile
    {
        public static readonly Tile Empty = new Tile(0);

        public Tile(int movementCost)
        {
            MovementCost = movementCost;
        }

        public int MovementCost { get; }

        public bool IsWalkable => MovementCost > 0;
    }
}
