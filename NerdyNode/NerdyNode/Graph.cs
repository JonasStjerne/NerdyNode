public class Graph
{
    public List<Node> nodes;

    public Graph()
    {
        nodes = new List<Node>();
    }
    public void addNode(Node node)
    {
        nodes.Add(node);
    }
    public void removeNode(Node node)
    {
        nodes.Remove(node);
    }
    public List<Node> getNodes()
    {
        return nodes;
    }
}