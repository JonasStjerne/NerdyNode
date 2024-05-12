using System.Diagnostics;

public class Graph
{
    public List<Node> Nodes { get; }

    public Graph()
    {
        Nodes = new List<Node>();
    }
    public void AddNode(Node node)
    {
        node.Graph = this;
        Nodes.Add(node);
    }
    public void RemoveNode(Node node)
    {
        node.Graph = null;
        Nodes.Remove(node);
    }
    public Node? GetNode(string label)
    {
        var node = Nodes.Find(n => n.Label == label);
        return node;
    }

    public List<Node> GetNodes(string label)
    {
        return Nodes.FindAll(n => n.Label == label);
    }

    public void AddEdge(Node from, Node to, bool directed, int? weight)
    {
        to.Graph = from.Graph;
        if (directed)
        {
            from.AddEdge(new Edge(from, to, directed, weight));
        }
        else
        {
            from.AddEdge(new Edge(from, to, directed, weight));
            to.AddEdge(new Edge(to, from, directed, weight));
        }
    }

    public override string ToString()
    {
        var printList = new List<string>();
        foreach (var node in Nodes)
        {
            var str = node.Label + " -> " + string.Join("; ", node.GetEdges().Select(e => e.EndNode.Label).ToArray());
            printList.Add(str);
        }
        return string.Join("\n", printList);
    }

    public Graph Copy()
    {
        var newGraph = new Graph();
        var nodeMap = new Dictionary<Node, Node>();
        foreach (var node in Nodes)
        {
            var newNode = new Node(node.Label, newGraph);
            newGraph.AddNode(newNode);
            nodeMap[node] = newNode;
        }
        foreach (var node in Nodes)
        {
            var newNode = nodeMap[node];
            foreach (var edge in node.GetEdges())
            {
                var newEdge = new Edge(newNode, nodeMap[edge.EndNode], edge.Directed, edge.Weight);
                newNode.AddEdge(newEdge);
            }
        }
        return newGraph;
    }

    public Graph Union(Graph graphToUnion)
    {
        var thisGraphCopy = this.Copy();
        var graphToUnionCopy = graphToUnion.Copy();
        foreach (var node in graphToUnionCopy.Nodes)
        {
            node.Graph = thisGraphCopy;
            thisGraphCopy.Nodes.Add(node);
        }
        return thisGraphCopy;
    }

    // A very simple implementation for looking up if an edge is already in a set
    // This should be optimized to use a better search algorithm
    private bool HasEdge(List<Edge> edges, Edge edge)
    {
        foreach (var e in edges)
        {
            if (e.Directed && edge.Directed)
            {
                if (e.StartNode == edge.StartNode && e.EndNode == edge.EndNode)
                {
                    return true;
                }
            }
            else
            {
                if ((e.StartNode == edge.StartNode && e.EndNode == edge.EndNode)
                || (e.StartNode == edge.EndNode && e.EndNode == edge.StartNode))
                {
                    return true;
                }

            }
        }
        return false;
    }

    //
    // Returns all unique edges. Duplicates for undirected edgegs are removed
    //
    public List<Edge> GetEdges()
    {
        List<Edge> edges = new List<Edge>();
        foreach (var n in Nodes)
        {
            foreach (var e in n.Edges)
            {
                if (!HasEdge(edges, e))
                {
                    edges.Add(e);
                }
            }
        }
        return edges;
    }

    // 
    // Generate a diagram for the graph
    // Uses https://graphviz.org to create the visualization.
    // Graphviz uses a syntax for describing a graph that matches our internal model
    // so this method transforms the internal model to the graphviz format.
    //
    public void Draw(String name, String fileName)
    {
        var printList = new List<string>
        {
            $"digraph {name} {{"
        };

        // First list all nodes with their attributes
        foreach (var node in Nodes)
        {
            var str = $"{node.Id} {node.GetAttributes()} ;";
            printList.Add(str);
        }

        // Now list all edges
        foreach (var edge in GetEdges())
        {
            var str = $"{edge.StartNode.Id} -> {edge.EndNode.Id} {edge.GetAttributes()};";
            printList.Add(str);
        }

        printList.Add("}");
        var graphizDef = string.Join("\n", printList);

        //Console.WriteLine(graphizDef);

        // Using Graphviz to generate a png file
        // graphviz must be installed on computer (MacOS: >brew install graphviz)
        using (Process graphiz = new Process())
        {
            graphiz.StartInfo.FileName = "dot";
            graphiz.StartInfo.UseShellExecute = false;
            graphiz.StartInfo.Arguments = " -Tpng -o" + fileName;
            graphiz.StartInfo.RedirectStandardInput = true;

            graphiz.Start();
            StreamWriter stdin = graphiz.StandardInput;
            stdin.WriteLine(graphizDef);
            stdin.Close();
            graphiz.WaitForExit();
        }

        // Using shell command 'open' to display the generated image
        // This works on MacOS - on Windows?
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "open";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Arguments = fileName;

            process.Start();
            process.WaitForExit();
        }
    }
}