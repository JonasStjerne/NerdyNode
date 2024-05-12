public class Edge
{
    public bool Directed { get; }
    public Node StartNode { get; }
    public Node EndNode { get; }
    public int? Weight { get; }

    public Edge(Node startNode, Node endNode, bool directed, int? weight = null)
    {
        this.StartNode = startNode;
        this.EndNode = endNode;
        this.Directed = directed;
        this.Weight = weight;
    }

    public string GetAttributes()
    {
        string dir = Directed ? "forward" : "none";
        string attributes = $"[dir={dir}";
        if (Weight != null)
        {
            attributes += $", weight={Weight}";
        }
        attributes += "]";
        return attributes;
    }


}