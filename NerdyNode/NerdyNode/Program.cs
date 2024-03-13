// See https://aka.ms/new-console-template for more information
using System.Text;
using Antlr4.Runtime;

Console.WriteLine("Running NerdyNode");

    try
    {
        string input = "";
        StringBuilder text = new StringBuilder();
        Console.WriteLine("Input the chat.");
        
        // to type the EOF character and end the input: use CTRL+D, then press <enter>
        while ((input = Console.ReadLine()) != "u0004")
        {
            text.AppendLine(input);
        }
        
        AntlrInputStream inputStream = new AntlrInputStream(text.ToString());
        NerdyNodeLexer NerdyNodeLexer = new NerdyNodeLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(NerdyNodeLexer);
        NerdyNodeParser nerdyNodeParser = new NerdyNodeParser(commonTokenStream);

        NerdyNodeParser.BoolContext boolContext = nerdyNodeParser.@bool();
        BasicNerdyNodeVisitor visitor = new BasicNerdyNodeVisitor();   
        visitor.Visit(boolContext);
        Console.WriteLine(visitor.Boolean);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);                
    }
