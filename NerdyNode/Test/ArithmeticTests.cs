using Antlr4.Runtime;

namespace Test;

[Collection("Sequential")]
public class ArithmeticTests
{
    private string Setup(string input)
    {
        input = "begin print " + input + "; end";
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

    [Fact]
    public void Plus()
    {
        var result = Setup("2+2");
        Assert.Equal(4, Int32.Parse(result));
    }

    [Fact]
    public void PlusParenthesis()
    {
        var result = Setup("2+(2+2)");
        Assert.Equal(6, Int32.Parse(result));
    }

    [Fact]
    public void Minus()
    {
        var result = Setup("2-2");
        Assert.Equal(0, Int32.Parse(result));
    }

    [Fact]
    public void MinusParenthesis()
    {
        var result = Setup("2-(2-2)");
        Assert.Equal(2, Int32.Parse(result));
    }

    [Fact]
    public void Multiply()
    {
        var result = Setup("2*2");
        Assert.Equal(4, Int32.Parse(result));
    }

    [Fact]
    public void MultiplyParenthesis()
    {
        var result = Setup("2*(2*2)");
        Assert.Equal(8, Int32.Parse(result));
    }

    [Fact]
    public void Divide()
    {
        var result = Setup("2/2");
        Assert.Equal(1, Int32.Parse(result));
    }

    [Fact]
    public void DivideParenthesis()
    {
        var result = Setup("2/(2/2)");
        Assert.Equal(2, Int32.Parse(result));
    }

    [Fact]
    public void Modulus()
    {
        var result = Setup("2%2");
        Assert.Equal(0, Int32.Parse(result));
    }


    [Fact]
    public void Parthenthesis()
    {
        var result = Setup("(2+2)*2");
        Assert.Equal(8, Int32.Parse(result));
    }


    // [Fact]
    // public void ModulusParenthesis()
    // {
    //     var result = Setup("2%(2%2)");
    //     Assert.Equal(2, Int32.Parse(result));
    // }

    // [Fact]
    // public void Exponent()
    // {
    //     var result = Setup("2^2");
    //     Assert.Equal(4, Int32.Parse(result));
    // }

    // [Fact]
    // public void ExponentParenthesis()
    // {
    //     var result = Setup("2^(2^2)");
    //     Assert.Equal(16, Int32.Parse(result));
    // }

    // [Fact]
    // public void ExponentParenthesis2()
    // {
    //     var result = Setup("(2^2)^2");
    //     Assert.Equal(16, Int32.Parse(result));
    // }

}