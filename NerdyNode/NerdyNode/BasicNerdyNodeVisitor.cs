using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Antlr4.Runtime.Misc;

public class BasicNerdyNodeVisitor : NerdyNodeParserBaseVisitor<object>
{
    private Stack<Scope> symbolTable = new Stack<Scope>();
    public BasicNerdyNodeVisitor()
    {
        var globalScope = new Scope();
        symbolTable.Push(globalScope);
    }

    public override object VisitProgram([NotNull] NerdyNodeParser.ProgramContext context)
    {
        Visit(context.block());
        return null;
    }

    public override object VisitIfstmt(NerdyNodeParser.IfstmtContext context)
    {
        var condition = (bool)Visit(context.expr());
        if (condition)
        {
            Visit(context.block(0));
        }
        else if (context.block(1) != null)
        {
            Visit(context.block(1));
        }
        return null;
    }

    public override object VisitBlock([NotNull] NerdyNodeParser.BlockContext context)
    {
        //Get the top most scope from the stack
        var parentScope = symbolTable.Peek();
        var newScope = new Scope(parentScope);
        symbolTable.Push(newScope);

        foreach (var statement in context.statement())
        {
            Visit(statement);
        }
        symbolTable.Pop();
        return null;
    }


    public override object VisitStatement([NotNull] NerdyNodeParser.StatementContext context)
    {
        if (context.forstmt() != null)
        {
            Visit(context.forstmt());
        }
        else if (context.ifstmt() != null)
        {
            Visit(context.ifstmt());
        }
        else if (context.declaration() != null)
        {
            Visit(context.declaration());
        }
        else if (context.assignment() != null)
        {
            Visit(context.assignment());
        }
        else if (context.print() != null)
        {
            Visit(context.print());
        }
        else if (context.funccall() != null)
        {
            Visit(context.funccall());
        }
        else if (context.graphfunc() != null)
        {
            Visit(context.graphfunc());
        }
        return null;
    }

    public override object VisitForstmt(NerdyNodeParser.ForstmtContext context)
    {
        Scope forLoopWrapperScope = new Scope(symbolTable.Peek());
        symbolTable.Push(forLoopWrapperScope);
        var start = (int)Visit(context.list().expr(0));
        var end = (int)Visit(context.list().expr(1));
        forLoopWrapperScope.Declare(context.IDENTIFIER().GetText(), null);
        for (int i = start; i <= end; i++)
        {
            forLoopWrapperScope.Assign(context.IDENTIFIER().GetText(), i);
            Visit(context.block());
        }
        symbolTable.Pop();
        return null;
    }

    public override object VisitDeclaration(NerdyNodeParser.DeclarationContext context)
    {
        var type = context.type().GetText();
        var variableName = context.assignment().IDENTIFIER().GetText();
        symbolTable.Peek().Declare(variableName, null);
        Visit(context.assignment());

        return null;
    }

    public override object VisitAssignment(NerdyNodeParser.AssignmentContext context)
    {
        if (symbolTable.Peek().HasVariable(context.IDENTIFIER().GetText()))
        {
            var variableName = context.IDENTIFIER().GetText();
            var value = Visit(context.expr());
            symbolTable.Peek().Assign(variableName, value);
        }
        else
        {
            throw new Exception("Variable not declared");
        }

        return null;
    }

    public override object VisitExpr(NerdyNodeParser.ExprContext context)
    {
        if (context.value() != null)
        {
            return Visit(context.value());
        }
        else if (context.arrow() != null)
        {
            // var graph = (Graph)Visit(context.graph());
            // var start = (Node)Visit(context.identlist().IDENTIFIER(0));
            // var end = (Node)Visit(context.identlist().IDENTIFIER(1));
            // return graph.FindPath(start, end);
            throw new NotImplementedException();
        }
        else if (context.IDENTIFIER(0) != null)
        {
            return symbolTable.Peek().Retrieve(context.IDENTIFIER(0).GetText());
        }
        else if (context.numop() != null)
        {
            switch (context.numop().GetText())
            {
                case "+":
                    var left = Visit(context.expr(0));
                    var right = Visit(context.expr(1));
                    if (left is string || right is string)
                    {
                        return left.ToString() + right.ToString();
                    }
                    return (int)Visit(context.expr(0)) + (int)Visit(context.expr(1));
                case "-":
                    return (int)Visit(context.expr(0)) - (int)Visit(context.expr(1));
                case "*":
                    return (int)Visit(context.expr(0)) * (int)Visit(context.expr(1));
                case "/":
                    return (int)Visit(context.expr(0)) / (int)Visit(context.expr(1));
                case "%":
                    return (int)Visit(context.expr(0)) % (int)Visit(context.expr(1));
            }
        }
        else if (context.boolop() != null)
        {
            switch (context.boolop().GetText())
            {
                case "==":
                    return (int)Visit(context.expr(0)) == (int)Visit(context.expr(1));
                case "!=":
                    return (int)Visit(context.expr(0)) != (int)Visit(context.expr(1));
                case ">":
                    return (int)Visit(context.expr(0)) > (int)Visit(context.expr(1));
                case "<":
                    return (int)Visit(context.expr(0)) < (int)Visit(context.expr(1));
                case ">=":
                    return (int)Visit(context.expr(0)) >= (int)Visit(context.expr(1));
                case "<=":
                    return (int)Visit(context.expr(0)) <= (int)Visit(context.expr(1));
            }
        }
        else if (context.PARANSTART() != null)
        {
            return Visit(context.expr(0));
        }
        else if (context.funccall() != null)
        {
            var result = Visit(context.funccall());
            return result;
        }
        else if (context.LABEL() != null)
        {
            var label = Visit(context.expr(0)).ToString();
            var node = new Node(label);
            return node;
        }

        return null;
    }

    public override object VisitPrint([NotNull] NerdyNodeParser.PrintContext context)
    {

        if (context.expr() != null)
        {
            var eval = Visit(context.expr());
            Console.WriteLine(eval);
        }
        return null;
    }

    public override object VisitValue([NotNull] NerdyNodeParser.ValueContext context)
    {
        if (context.INT() != null)
        {
            return int.Parse(context.INT().GetText());
        }
        else if (context.@bool() != null)
        {
            return bool.Parse(context.@bool().GetText());
        }
        else if (context.STRING() != null)
        {
            return context.STRING().GetText().Trim('"');
        }
        else if (context.graph() != null)
        {
            return Visit(context.graph());
        }
        return null;
    }

    public override object VisitGraph([NotNull] NerdyNodeParser.GraphContext context)
    {
        var graph = new Graph();
        var nodes = (List<Node>)Visit(context.nodeset());
        foreach (var node in nodes)
        {
            graph.AddNode(node);
        }
        return graph;
    }

    public override object VisitNodeset([NotNull] NerdyNodeParser.NodesetContext context)
    {
        var nodes = new List<Node>();
        foreach (var identifier in context.identlist().IDENTIFIER())
        {
            var identifierName = identifier.GetText();
            var value = symbolTable.Peek().Retrieve(identifierName);
            if (value is Node)
            {
                nodes.Add((Node)value);
            }
            else
            {
                throw new Exception("Object in nodeset is not a node");
            }
        }
        return nodes;
    }

    public override object VisitFunccall([NotNull] NerdyNodeParser.FunccallContext context)
    {
        var value = symbolTable.Peek().Retrieve(context.IDENTIFIER(0).GetText());
        var method = context.IDENTIFIER(1).GetText();
        var methodInfo = value.GetType().GetMethod(method);
        if (methodInfo != null)
        {
            var parameters = new List<object>();
            if (context.paramlist() != null)
            {
                foreach (var expr in context.paramlist().expr())
                {
                    parameters.Add(Visit(expr));
                }
            }
            return methodInfo.Invoke(value, parameters.ToArray());
        }
        else
        {
            throw new Exception("Method not found");
        }
    }

    public override object VisitGraphfunc([NotNull] NerdyNodeParser.GraphfuncContext context)
    {
        var left = context.IDENTIFIER(0) == null ? VisitFunccall(context.funccall(0)) : symbolTable.Peek().Retrieve(context.IDENTIFIER(0).GetText());
        if (!(left is Node))
        {
            throw new Exception("Left side of graph function is not a node");
        }
        var right = context.IDENTIFIER(1) == null ? VisitFunccall(context.funccall(1)) : symbolTable.Peek().Retrieve(context.IDENTIFIER(1).GetText());
        if (!(right is Node))
        {
            throw new Exception("Right side of graph function is not a node");
        }
        if (((Node)left).graph != ((Node)right).graph)
        {
            throw new Exception("Nodes are not in the same graph");
        }

        switch (context.addtograph().GetText())
        {
            case "<-->":
                var toRight1 = new Edge((Node)right);
                ((Node)left).AddEdge(toRight1);
                var toLeft1 = new Edge((Node)left);
                ((Node)right).AddEdge(toLeft1);
                break;
            case "-->":
                var toRight2 = new Edge((Node)right);
                ((Node)left).AddEdge(toRight2);
                break;
            case "<--":
                var toLeft3 = new Edge((Node)left);
                ((Node)right).AddEdge(toLeft3);
                break;
        }

        return null;
    }

}