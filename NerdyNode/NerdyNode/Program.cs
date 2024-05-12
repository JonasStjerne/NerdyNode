// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;

[assembly: CLSCompliant(false)]

class Program
{
    static void Main(string[] args)
    {

        if (args.Length == 0)
        {
            Console.WriteLine("No path to file provided. Defaulting to test.0-0");
            args = new string[] { "samples/test.0-0" };
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

            NerdyNodeParser.ProgramContext programContext = nerdyNodeParser.program();
            BasicNerdyNodeVisitor visitor = new BasicNerdyNodeVisitor();
            visitor.Visit(programContext);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
        }
    }
}
