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

    public override string ToString()
    {

        return "Graph: |" + string.Join("| |", nodes) + "|";
    }

    public void PrintEdges()
    {
        foreach (var node in nodes)
        {
            Console.WriteLine(node.GetLabel() + " -> " + string.Join(", ", node.GetEdges().Select(e => e.GetEndNode().GetLabel()).ToArray()));
        }
    }
}