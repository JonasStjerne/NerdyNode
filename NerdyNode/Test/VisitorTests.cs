using Antlr4.Runtime;
using Moq;
using Moq.AutoMock;

namespace Test;

public class VisitorTests
{
    private NerdyNodeParser setup(string Input)
    {
        AntlrInputStream inputStream = new AntlrInputStream(Input);
        NerdyNodeLexer NerdyNodeLexer = new NerdyNodeLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(NerdyNodeLexer);
        NerdyNodeParser nerdyNodeParser = new NerdyNodeParser(commonTokenStream);

        return nerdyNodeParser;
    }

    [Fact]
    public void TestVisitTypeInt()
    {
        var context = setup("int");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Equal(typeof(int), type);
    }

    [Fact]
    public void TestVisitTypeString()
    {
        var context = setup("string");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Equal(typeof(string), type);
    }

    [Fact]
    public void TestVisitTypeBool()
    {
        var context = setup("boolean");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Equal(typeof(bool), type);
    }

    [Fact]
    public void TestVisitTypeGraph()
    {
        var context = setup("graph");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Equal(typeof(Graph), type);
    }

    [Fact]
    public void TestVisitTypeNode()
    {
        var context = setup("node");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Equal(typeof(Node), type);
    }

    [Fact]
    public void TestVisitTypeEdge()
    {
        var context = setup("edge");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Equal(typeof(Edge), type);
    }

    [Fact]
    public void TestVisitTypeNodeSet()
    {
        var context = setup("nodeset");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Equal(typeof(List<Node>), type);
    }

    [Fact]
    public void TestVisitTypeUnknownType()
    {
        var context = setup("notarealtype");
        BasicNerdyNodeVisitor Visitor = new BasicNerdyNodeVisitor();
        var type = Visitor.VisitType(context.type());
        Assert.Null(type);
    }

    // [Fact]
    // public void TestVisitDeclarationInvalid()
    // {
    //     var context = setup("int x = \"teststring\";");
    //     var mocker = new AutoMocker();
    //     mocker.Setup<BasicNerdyNodeVisitor, object>(x => x.VisitType(It.IsAny<NerdyNodeParser.TypeContext>())).Returns(typeof(int));
    //     mocker.Setup<BasicNerdyNodeVisitor, object>(x => x.VisitAssignment(It.IsAny<NerdyNodeParser.AssignmentContext>()));
    //     mocker.Setup<BasicNerdyNodeVisitor, object>(x => x.symbolTable.Peek().Retrieve("x")).Returns("teststring");
    //     var visitor = mocker.CreateInstance<BasicNerdyNodeVisitor>();
    //     try
    //     {
    //         visitor.Visit(context.declaration());
    //     }
    //     catch (Exception e)
    //     {
    //         Assert.Equal("Type mismatch in declaration", e.Message);
    //     }
    //     Assert.False(true);
    // }

}