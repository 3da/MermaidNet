using System;

namespace MermaidNet
{
    public class Node
    {
        public Node(string id)
        {
            Id = id;
        }

        public Node()
        {
            Id = "id" + Guid.NewGuid().ToString().Replace("-", "");
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public NodeStyle Style { get; set; }
        public string Description { get; set; }
    }
}
