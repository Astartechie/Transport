using System;
using SFML.System;

namespace Transport
{
    internal class Node : IEquatable<Node>
    {
        public Node(Vector2i position)
        {
            Position = position;
        }

        public Vector2i Position { get; }

        public Node Parent { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }

        public float FCost => GCost + HCost;

        public bool Equals(Node other)
        {
            if (other is null) return false;
            return Position == other.Position;
        }

        public override bool Equals(object obj) => Equals(obj as Node);

        public override int GetHashCode() => Position.GetHashCode();
    }
}