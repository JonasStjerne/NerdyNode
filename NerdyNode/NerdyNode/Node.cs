public class Node
{
    string? label;
    List<Edge> edges;
    public Node(string? label, List<Edge>? edges)
    {
        if (edges == null)
        {
            this.edges = new List<Edge>();
        }
        else
        {
            this.edges = edges;
        }
        this.label = label;
    }
    public void addEdge(Edge edge)
    {
        edges.Add(edge);
    }
    public void removeEdge(Edge edge)
    {
        edges.Remove(edge);
    }
    public List<Edge> getEdges()
    {
        return edges;
    }


    public string? getLabel()
    {
        return label;
    }
    public void setLabel(string? label)
    {
        this.label = label;
    }
    public override string ToString()
    {
        return label;
    }

}