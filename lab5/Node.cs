using System;
using System.Collections.Generic;
using System.Text;

namespace lab5
{   
    [Serializable]
    class Node
    {
        public int number;
        public bool captured;
        public List<int> neighbours;

        public Node()
        {
            number = -1;
            captured = false;
            neighbours = new List<int>();
        }

        public bool connect(Node neighbour, int maxConnections)
        {
            if (!this.neighbours.Contains(neighbour.number) && !(neighbour.neighbours.Count == maxConnections))
            {
                this.neighbours.Add(neighbour.number);
                neighbour.neighbours.Add(this.number);
                return true;
            }
            return false;
        }
    }
}
