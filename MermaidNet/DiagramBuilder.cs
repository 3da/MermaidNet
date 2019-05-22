using System;
using System.Collections.Generic;
using System.Text;

namespace MermaidNet
{
    public class DiagramBuilder
    {

        public string Build(Graph graph)
        {
            var sb = new StringBuilder();
            sb.AppendLine("graph TD");

            sb.AppendLine(BuildSubGraph(graph));

            return sb.ToString();
        }

        public IDictionary<string, string> GetDescriptions(Graph graph)
        {
            var result = new Dictionary<string, string>();

            foreach (var graphNode in graph.Nodes)
            {
                if (string.IsNullOrWhiteSpace(graphNode.Description))
                    continue;

                result[graphNode.Id] = graphNode.Description;
            }

            foreach (var graphSubGraph in graph.SubGraphs)
            {
                foreach (var description in GetDescriptions(graphSubGraph))
                {
                    result[description.Key] = description.Value;
                }
            }

            return result;
        }

        public string BuildSubGraph(Graph graph)
        {
            var sb = new StringBuilder();
            foreach (var graphNode in graph.Nodes)
            {
                sb.Append(graphNode.Id);
                switch (graphNode.Style)
                {
                    case NodeStyle.Rounded:
                        sb.Append('(');
                        break;
                    case NodeStyle.Circle:
                        sb.Append("((");
                        break;
                    case NodeStyle.Rhombus:
                        sb.Append('{');
                        break;
                    default:
                        sb.Append('[');
                        break;
                }

                sb.Append(graphNode.Name);

                switch (graphNode.Style)
                {
                    case NodeStyle.Rounded:
                        sb.Append(')');
                        break;
                    case NodeStyle.Circle:
                        sb.Append("))");
                        break;
                    case NodeStyle.Rhombus:
                        sb.Append('}');
                        break;
                    default:
                        sb.Append(']');
                        break;
                }

                sb.AppendLine();
            }

            foreach (var subGraph in graph.SubGraphs)
            {
                sb.AppendLine($"subgraph {subGraph.Name}");
                sb.AppendLine(BuildSubGraph(subGraph));
                sb.AppendLine("end");
            }

            foreach (var graphRelation in graph.Relations)
            {

                sb.Append(graphRelation.A.Id);

                switch (graphRelation.Direction)
                {
                    case Direction.None:
                        sb.Append("---");
                        break;
                    case Direction.AB:
                        sb.Append("-->");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!string.IsNullOrEmpty(graphRelation.Text))
                    sb.Append($"|\"{graphRelation.Text} &zwnj; &zwnj; &zwnj;\"|");

                sb.AppendLine(graphRelation.B.Id);

            }

            return sb.ToString();
        }

    }
}
