// See https://aka.ms/new-console-template for more information
using System.Text;
using Antlr4.Runtime;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Running NerdyNode");

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a file to parse.");
            return;
        }

        string filePath = args[0];
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File does not exist.");
            return;
        }

        string fileContents = "";

        try
        {
            fileContents = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the file: {ex.Message}");
        }

        try
        {

            AntlrInputStream inputStream = new AntlrInputStream(fileContents);
            NerdyNodeLexer NerdyNodeLexer = new NerdyNodeLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(NerdyNodeLexer);
            NerdyNodeParser nerdyNodeParser = new NerdyNodeParser(commonTokenStream);

            NerdyNodeParser.BlockContext blockContext = nerdyNodeParser.block();
            BasicNerdyNodeVisitor visitor = new BasicNerdyNodeVisitor();
            visitor.Visit(blockContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
        }
    }
}
