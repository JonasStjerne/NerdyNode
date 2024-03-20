using System.Runtime.CompilerServices;

public class Graph
{
    public List<Node> nodes;

    public Graph()
    {
        nodes = new List<Node>();
    }
    public void AddNode(Node node)
    {
        node.graph = this;
        nodes.Add(node);
    }
    public void RemoveNode(Node node)
    {
        node.graph = null;
        nodes.Remove(node);
    }
    public Node GetNode(string label)
    {
        var node = nodes.Find(n => n.GetLabel() == label);
        return node;
    }

    public List<Node> GetNodes(string label)
    {
        return nodes.FindAll(n => n.GetLabel() == label);
    }

    public override string ToString()
    {
        var printList = new List<string>();
        foreach (var node in nodes)
        {
            var str = node.GetLabel() + " -> " + string.Join(", ", node.GetEdges().Select(e => e.GetEndNode().GetLabel()).ToArray());
            printList.Add(str);
        }
        return string.Join("\n", printList);
    }

    public Graph Copy()
    {
        var newGraph = new Graph();
        var nodeMap = new Dictionary<Node, Node>();
        foreach (var node in nodes)
        {
            var newNode = new Node(node.GetLabel(), newGraph);
            newGraph.AddNode(newNode);
            nodeMap[node] = newNode;
        }
        foreach (var node in nodes)
        {
            var newNode = nodeMap[node];
            foreach (var edge in node.GetEdges())
            {
                var newEdge = new Edge(nodeMap[edge.GetEndNode()], edge.weight);
                newNode.AddEdge(newEdge);
            }
        }
        return newGraph;
    }

    public Graph Union(Graph graphToUnion)
    {
        var thisGraphCopy = this.Copy();
        var graphToUnionCopy = graphToUnion.Copy();
        foreach (var node in graphToUnionCopy.nodes)
        {
            node.graph = thisGraphCopy;
            thisGraphCopy.nodes.Add(node);
        }
        return thisGraphCopy;
    }


}