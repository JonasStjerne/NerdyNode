using Antlr4.Runtime;

namespace Test;

public class ScopeTests
{
    private string Setup(string input)
    {
        input = "begin " + input + " end";
        AntlrInputStream inputStream = new AntlrInputStream(input);
        NerdyNodeLexer NerdyNodeLexer = new NerdyNodeLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(NerdyNodeLexer);
        NerdyNodeParser nerdyNodeParser = new NerdyNodeParser(commonTokenStream);

        NerdyNodeParser.ProgramContext programContext = nerdyNodeParser.program();
        BasicNerdyNodeVisitor visitor = new BasicNerdyNodeVisitor();
        StringWriter consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        visitor.Visit(programContext);
        return consoleOutput.ToString().Trim();
    }

    // [Fact]
    // public void TestScope()
    // {
    //     string input = "int x = 2; print x + 2;";
    //     string output = Setup(input);
    //     Assert.Equal("4", output);
    // }

    // [Fact]
    // public void TestScope2()
    // {
    //     string input = "let x = 10; let y = 20; begin let x = 30; print x + y; end;";
    //     string output = Setup(input);
    //     Assert.Equal("50", output);
    // }





}