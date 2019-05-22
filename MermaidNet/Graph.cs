using System.Collections.Generic;

namespace MermaidNet
{
    public class Graph
    {
        public string Name { get; set; }

        public IList<Graph> SubGraphs { get; set; } = new List<Graph>();

        public IList<Node> Nodes { get; set; } = new List<Node>();

        public IList<Relation> Relations { get; set; } = new List<Relation>();



        public Node AddNode(Node node)
        {
            Nodes.Add(node);
            return node;
        }

        public Node AddNode(string text)
        {
            var node = new Node() { Name = text };

            Nodes.Add(node);

            return node;
        }

        public Relation Connect(Node A, Node B, Direction direction = Direction.AB, string text = null)
        {
            var rel = new Relation()
            {
                A = A,
                B = B,
                Direction = direction,
                Text = text
            };

            Relations.Add(rel);

            return rel;
        }

        public Graph AddGraph(string name)
        {
            var gr = new Graph() { Name = name };

            SubGraphs.Add(gr);

            return gr;
        }
    }
}
