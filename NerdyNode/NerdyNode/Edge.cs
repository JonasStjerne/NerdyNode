public class Edge
{
    Node endNode { get; }
    public int? weight { get; }
    public Edge(Node endNode, int? weight = null)
    {
        this.endNode = endNode;
        this.weight = weight;
    }

    public Node GetEndNode()
    {
        return endNode;
    }
}