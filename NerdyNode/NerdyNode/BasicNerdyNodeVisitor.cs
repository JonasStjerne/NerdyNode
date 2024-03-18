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
        return null;
    }

    public override object VisitForstmt(NerdyNodeParser.ForstmtContext context)
    {
        Scope forLoopWrapperScope = new Scope(symbolTable.Peek());
        symbolTable.Push(forLoopWrapperScope);
        var start = (int)Visit(context.list().expr(0));
        var end = (int)Visit(context.list().expr(1));
        forLoopWrapperScope.declare(context.IDENTIFIER().GetText(), null);
        for (int i = start; i <= end; i++)
        {
            forLoopWrapperScope.assign(context.IDENTIFIER().GetText(), i);
            Visit(context.block());
        }
        symbolTable.Pop();
        return null;
    }

    public override object VisitDeclaration(NerdyNodeParser.DeclarationContext context)
    {
        var type = context.type().GetText();
        var variableName = context.assignment().IDENTIFIER().GetText();
        symbolTable.Peek().declare(variableName, null);
        Visit(context.assignment());

        return null;
    }

    public override object VisitAssignment(NerdyNodeParser.AssignmentContext context)
    {
        if (symbolTable.Peek().hasVariable(context.IDENTIFIER().GetText()))
        {
            var variableName = context.IDENTIFIER().GetText();
            var value = Visit(context.expr());
            symbolTable.Peek().assign(variableName, value);
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
            return symbolTable.Peek().retrieve(context.IDENTIFIER(0).GetText());
        }
        else if (context.numop() != null)
        {
            switch (context.numop().GetText())
            {
                case "+":
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
            }
        }
        else if (context.PARANSTART() != null)
        {
            return Visit(context.expr(0));
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
        foreach (var node in context.identlist().IDENTIFIER().ToList())
        {
            nodes.Add((Node)Visit(node));
        }
        return nodes;
    }

}