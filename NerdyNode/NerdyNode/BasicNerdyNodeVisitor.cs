using System.Diagnostics;
using Antlr4.Runtime.Misc;

public class BasicNerdyNodeVisitor : NerdyNodeParserBaseVisitor<object>
{
    public bool Boolean;
    public override object VisitBool(NerdyNodeParser.BoolContext context)
    {
        Boolean = bool.Parse(context.GetText());
        return Boolean;
    }

    public override object VisitIfstmt(NerdyNodeParser.IfstmtContext context)
    {
        var condition = (bool)Visit(context.expr());
        if (condition)
        {
            Visit(context.block());
        }
        return null;
    }

    public override object VisitBlock([NotNull] NerdyNodeParser.BlockContext context)
    {
        foreach (var statement in context.statement())
        {
            Visit(statement);
        }
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
        var start = (int)Visit(context.list().expr(0));
        var end = (int)Visit(context.list().expr(1));
        for (int i = start; i <= end; i++)
        {
            Visit(context.block());
        }
        return null;
    }

    public override object VisitDeclaration(NerdyNodeParser.DeclarationContext context)
    {
        Dictionary<String, dynamic> variables = new Dictionary<String, dynamic>();

        var type = context.type().GetText();
        var variableName = context.assignment().IDENTIFIER().GetText();
        var value = Visit(context.assignment());

        // //The code should return the different values somehow
        // switch (type)
        // {
        //     case "int":
        //         int intVal = (int)value;
        //         return intVal;
        //     case "bool":
        //         bool boolVal = (bool)value;
        //         return boolVal;
        //     case "string":
        //         string stringVal = (string)value;
        //         return stringVal;
        //         // case "TYPEGRAPH": // TO DO implement these
        // }

        return null;
    }

    public override object VisitAssignment(NerdyNodeParser.AssignmentContext context)
    {
        var variableName = context.IDENTIFIER().GetText();
        var value = Visit(context.expr());
        Console.WriteLine($"Variable: {variableName} set to Value: {value}");
        return null;
    }

    public override object VisitExpr(NerdyNodeParser.ExprContext context)
    {
        if (context.value() != null)
        {
            return Visit(context.value());
        }
        else if (context.IDENTIFIER() != null)
        {
            //Return value of the variable with the name of the identifier Probably not working in this state as the text is returned
            return context.IDENTIFIER().GetText();

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
        Console.WriteLine(context.value().GetText().Trim('"'));
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
        return null;
    }

}