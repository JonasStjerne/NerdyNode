public class Graph
{
    public List<Node> nodes;

    public Graph()
    {
        nodes = new List<Node>();
    }
    public void AddNode(Node node)
    {
        nodes.Add(node);
    }
    public void RemoveNode(Node node)
    {
        nodes.Remove(node);
    }
    public List<Node> GetNodes()
    {
        return nodes;
    }

    public override string ToString()
    {

        return "Graph: " + string.Join("| |", nodes);
    }
}