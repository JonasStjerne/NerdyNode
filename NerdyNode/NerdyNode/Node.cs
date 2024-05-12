public class Node
{
    static int _counter = 1;

    public int Id { get; }

    public string? Label { get; set; }

    public Graph? Graph;

    public List<Edge> Edges { get; }

    public Node(string? label, Graph? graph = null, List<Edge>? edges = null)
    {
        this.Id = _counter++;

        if (edges == null)
        {
            this.Edges = new List<Edge>();
        }
        else
        {
            this.Edges = edges;
        }
        this.Label = label;
        this.Graph = graph;
    }
    public void AddEdge(Edge edge)
    {
        Edges.Add(edge);
    }
    public void RemoveEdge(Edge edge)
    {
        Edges.Remove(edge);
    }
    public List<Edge> GetEdges()
    {
        return Edges;
    }

    public string GetAttributes()
    {
        return $"[label=\"{Label}\"]";
    }

    // can be used from NerdyNode program
    public void UpdateLabel(string label)
    {
        this.Label = label;
    }

    public override string ToString()
    {
        return Label != null ? Label : "<Node>";
    }

}