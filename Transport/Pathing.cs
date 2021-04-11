using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;
using Transport.Extensions;

namespace Transport
{
    class Pathing
    {
       

        public static List<Vector2i> Find(SquareGrid grid, Vector2i startPosition, Vector2i endPosition)
        {
            var manhattanDistance = (endPosition - startPosition).ManhattanDistance();

            var startNode = new Node(startPosition)
            {
                GCost = 0,
                HCost = manhattanDistance
            };

            var endNode = new Node(endPosition)
            {
                GCost = manhattanDistance,
                HCost = 0
            };

            var openSet = new HashSet<Node>();
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.OrderBy(node => node.FCost).First();
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode.Equals(endNode))
                {
                    var path = new List<Vector2i>();
                    while (!currentNode.Equals(startNode))
                    {
                        path.Add(currentNode.Position);
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();
                    return path;
                }

                foreach (var direction in Directions.All)
                {
                    var neighbourPosition = currentNode.Position + direction;
                    if (!grid.IsWalkable(neighbourPosition)) continue;

                    var neighbourNode = new Node(neighbourPosition);
                    if (closedSet.Contains(neighbourNode)) continue;

                    var gCost = currentNode.GCost + 1;
                    if (gCost < neighbourNode.GCost || !openSet.Contains(neighbourNode))
                    {
                        neighbourNode.GCost = gCost;
                        neighbourNode.HCost = (endPosition - neighbourPosition).ManhattanDistance();
                        neighbourNode.Parent = currentNode;
                        if (!openSet.Contains(neighbourNode))
                        {
                            openSet.Add(neighbourNode);
                        }
                    }
                }
            }

            return new List<Vector2i>();
        }
    }
}