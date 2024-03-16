public class Edge
{
    Node endNode { get; }
    int? weight { get; }
    public Edge(Node endNode, int? weight)
    {
        this.endNode = endNode;
        this.weight = weight;
    }

}