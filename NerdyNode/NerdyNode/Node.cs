public class Node
{
    string? label;
    public Graph? graph;
    List<Edge> edges;
    public Node(string? label, List<Edge>? edges = null)
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
    public void AddEdge(Edge edge)
    {
        edges.Add(edge);
    }
    public void RemoveEdge(Edge edge)
    {
        edges.Remove(edge);
    }
    public List<Edge> GetEdges()
    {
        return edges;
    }


    public string? GetLabel()
    {
        return label;
    }
    public void SetLabel(string? label)
    {
        this.label = label;
    }
    public override string ToString()
    {
        return label;
    }

}