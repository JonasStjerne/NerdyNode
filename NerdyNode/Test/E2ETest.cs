using Antlr4.Runtime;

namespace Test;

[Collection("Sequential")]
public class E2ETest
{
    private StringWriter consoleOutput;
    private string setup(string input)
    {
        AntlrInputStream inputStream = new AntlrInputStream(input);
        NerdyNodeLexer NerdyNodeLexer = new NerdyNodeLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(NerdyNodeLexer);
        NerdyNodeParser nerdyNodeParser = new NerdyNodeParser(commonTokenStream);

        NerdyNodeParser.ProgramContext programContext = nerdyNodeParser.program();
        BasicNerdyNodeVisitor visitor = new BasicNerdyNodeVisitor();
        consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        visitor.Visit(programContext);
        return consoleOutput.ToString();
    }

    [Fact]
    public void TestLatticeGraph()
    {
        const string LatticeGraphAlgo =
        @"
        begin
            int width = 4;
            int height = 3;
            graph latticeGraph = ({},{});
            for y in [1..height] begin
                for x in [1..width] begin
                    node newNode = |x+"",""+y|;
                    latticeGraph.AddNode(newNode);
                    if x > 1 begin
                        node n = latticeGraph.GetNode((x-1)+"",""+y);
                        newNode <--> n;
                    end;
                    if y > 1 begin
                        node n = latticeGraph.GetNode(x+"",""+(y-1));
                        newNode <--> n;
                    end;
                end;
            end;
            print latticeGraph;
        end
        ";
        var result = setup(LatticeGraphAlgo);
        Assert.Equal("1,1 -> 2,1, 1,2\n2,1 -> 1,1, 3,1, 2,2\n3,1 -> 2,1, 4,1, 3,2\n4,1 -> 3,1, 4,2\n1,2 -> 1,1, 2,2, 1,3\n2,2 -> 1,2, 2,1, 3,2, 2,3\n3,2 -> 2,2, 3,1, 4,2, 3,3\n4,2 -> 3,2, 4,1, 4,3\n1,3 -> 1,2, 2,3\n2,3 -> 1,3, 2,2, 3,3\n3,3 -> 2,3, 3,2, 4,3\n4,3 -> 3,3, 4,2\n", result);
    }



    [Fact]
    public void TestBinaryTree()
    {
        const string BinaryTreeAlgo =
        @"
        begin
            int depth = 4;
            node initialNode = |""1""|;
            graph binaryTree = ({initialNode},{});
            for i in [2..depth] begin
                graph left = binaryTree;
                graph right = binaryTree.Copy();
                binaryTree = left union right;
                node newRootNode = |i|;
                binaryTree.AddNode(newRootNode);⁄
                nodeset ns = binaryTree.GetNodes((i-1)+"""");
                for n in ns begin
                    newRootNode --> n;
                end;
            end;
            print binaryTree;
        end
        ";
        var result = setup(BinaryTreeAlgo);
        Assert.Equal("1 -> \n1 -> \n2 -> 1, 1\n1 -> \n1 -> \n2 -> 1, 1\n3 -> 2, 2\n1 -> \n1 -> \n2 -> 1, 1\n1 -> \n1 -> \n2 -> 1, 1\n3 -> 2, 2\n4 -> 3, 3\n", result);
    }

}