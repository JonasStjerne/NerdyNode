using System.Collections;
using Antlr4.Runtime.Misc;

public class BasicNerdyNodeVisitor : NerdyNodeParserBaseVisitor<object>
{

    private Stack<Scope> symbolTable = new Stack<Scope>();
    private Scope globalScope;


    public BasicNerdyNodeVisitor()
    {
        globalScope = new Scope();
        globalScope.Declare("INFINITY", int.MaxValue);
        symbolTable.Push(globalScope);
    }

    public override object VisitProgram([NotNull] NerdyNodeParser.ProgramContext context)
    {
        // we need to handle func definitions before we evaluate main block - as the functions defined will be used in the main block
        foreach (var funcdef in context.funcdeclaration())
        {
            Visit(funcdef);
        }

        // Now evaluate the main program block
        return Visit(context.block());
    }

    public override object VisitFuncdeclaration([NotNull] NerdyNodeParser.FuncdeclarationContext context)
    {
        var variableName = context.IDENTIFIER().GetText();
        symbolTable.Peek().Declare(variableName, context);
        return context;
    }

    public override object VisitIfstmt(NerdyNodeParser.IfstmtContext context)
    {
        var condition = AssertBool(Visit(context.expr()), context.expr().GetText());
        if (condition)
        {
            return Visit(context.block(0));
        }
        else if (context.block(1) != null)
        {
            return Visit(context.block(1));
        }
        else
        {
            return false;
        }
    }

    public override object VisitBlock([NotNull] NerdyNodeParser.BlockContext context)
    {
        //Get the top most scope from the stack
        var parentScope = symbolTable.Peek();
        var newScope = new Scope(parentScope);
        symbolTable.Push(newScope);

        try
        {
            object returnValue = false;
            foreach (var statement in context.statement())
            {
                returnValue = Visit(statement);
            }
            return returnValue;
        }
        finally
        {
            symbolTable.Pop();
        }
    }


    public override object VisitStatement([NotNull] NerdyNodeParser.StatementContext context)
    {
        if (context.forstmt() != null)
        {
            return Visit(context.forstmt());
        }
        else if (context.ifstmt() != null)
        {
            return Visit(context.ifstmt());
        }
        else if (context.declaration() != null)
        {
            return Visit(context.declaration());
        }
        else if (context.assignment() != null)
        {
            return Visit(context.assignment());
        }
        else if (context.print() != null)
        {
            return Visit(context.print());
        }
        else if (context.draw() != null)
        {
            return Visit(context.draw());
        }
        else if (context.funccall() != null)
        {
            return Visit(context.funccall());
        }
        else if (context.graphfunc() != null)
        {
            return Visit(context.graphfunc());
        }
        else if (context.returnstmt() != null)
        {
            return Visit(context.returnstmt());
        }
        throw new NotImplementedException("Statement type not yet implemented: " + context.GetText());
    }

    public override object VisitReturnstmt([NotNull] NerdyNodeParser.ReturnstmtContext context)
    {
        var value = Visit(context.expr());
        throw new ReturnFromBlock(value);
    }

    public override object VisitForstmt(NerdyNodeParser.ForstmtContext context)
    {
        object returnValue = false;
        Scope forLoopWrapperScope = new Scope(symbolTable.Peek());
        symbolTable.Push(forLoopWrapperScope);
        try
        {
            if (context.list().expr(0) != null)
            {
                var start = AssertInt(Visit(context.list().expr(0)), context.list().GetText());
                var end = AssertInt(Visit(context.list().expr(1)), context.list().GetText());
                forLoopWrapperScope.Declare(context.IDENTIFIER().GetText(), start);
                if (start < end)
                {
                    for (int i = start; i <= end; i++)
                    {
                        forLoopWrapperScope.Assign(context.IDENTIFIER().GetText(), i);
                        returnValue = Visit(context.block());
                    }

                }
                else
                {
                    for (int i = start; i >= end; i--)
                    {
                        forLoopWrapperScope.Assign(context.IDENTIFIER().GetText(), i);
                        returnValue = Visit(context.block());
                    }

                }
            }
            else if (context.list().IDENTIFIER() != null)
            {
                //list of type object or node or edge
                var listSymbol = context.list().IDENTIFIER().GetText();
                var symbolValue = symbolTable.Peek().Retrieve(listSymbol);
                var list = symbolValue as IEnumerable;
                if (list != null)
                {
                    forLoopWrapperScope.Declare(context.IDENTIFIER().GetText(), symbolValue);
                    foreach (var item in list)
                    {
                        forLoopWrapperScope.Assign(context.IDENTIFIER().GetText(), item);
                        returnValue = Visit(context.block());
                    }
                }
                else
                {
                    throw new Exception("Symbol in for loop ('" + listSymbol + "') must be enumerable. Type is: " + symbolValue.GetType());
                }
            }
            return returnValue;

        }
        finally
        {
            symbolTable.Pop();

        }
    }

    public override object VisitDeclaration(NerdyNodeParser.DeclarationContext context)
    {
        var type = Visit(context.type());
        var variableName = context.assignment().IDENTIFIER().GetText();
        var value = Visit(context.assignment().expr());
        symbolTable.Peek().Declare(variableName, value);
        return value;
    }

    public override object VisitAssignment(NerdyNodeParser.AssignmentContext context)
    {
        var symbolName = context.IDENTIFIER().GetText();
        if (symbolTable.Peek().HasVariable(symbolName))
        {
            var variableName = symbolName;
            var value = Visit(context.expr());
            symbolTable.Peek().Assign(variableName, value);
            return value;
        }
        else
        {
            throw new Exception("Variable '" + symbolName + "' not declared");
        }
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
        else if (context.MINUS() != null)
        {
            var value = Visit(context.expr(0));
            return -1 * AssertInt(value, context.expr(0).GetText());
        }
        else if (context.PLUS() != null)
        {
            var value = Visit(context.expr(0));
            return AssertInt(value, context.expr(0).GetText());
        }
        else if (context.numop1() != null)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));
            switch (context.numop1().GetText())
            {
                case "*":
                    return AssertInt(left, context.expr(0).GetText()) * AssertInt(right, context.expr(1).GetText());
                case "/":
                    return AssertInt(left, context.expr(0).GetText()) / AssertInt(right, context.expr(1).GetText());
                case "%":
                    return AssertInt(left, context.expr(0).GetText()) % AssertInt(right, context.expr(1).GetText());
            }
        }
        else if (context.numop2() != null)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));
            switch (context.numop2().GetText())
            {
                case "+":
                    if (left is string || right is string)
                    {
                        return left.ToString() + right.ToString();
                    }
                    else
                    {
                        return AssertInt(left, context.expr(0).GetText()) + AssertInt(right, context.expr(1).GetText());
                    }
                case "-":
                    return AssertInt(left, context.expr(0).GetText()) - AssertInt(right, context.expr(1).GetText());
            }
        }
        else if (context.boolop() != null)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));
            switch (context.boolop().GetText())
            {
                case "==":
                    return left.Equals(right);
                case "!=":
                    return !left.Equals(right);
                case ">":
                    return (AssertInt(left, context.expr(0).GetText()) > AssertInt(right, context.expr(1).GetText()));
                case "<":
                    return (AssertInt(left, context.expr(0).GetText()) < AssertInt(right, context.expr(1).GetText()));
                case ">=":
                    return (AssertInt(left, context.expr(0).GetText()) >= AssertInt(right, context.expr(1).GetText()));
                case "<=":
                    return (AssertInt(left, context.expr(0).GetText()) <= AssertInt(right, context.expr(1).GetText()));
            }
        }
        else if (context.graphop() != null)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));

            switch (context.graphop().GetText())
            {
                case "union":
                    if (left is Graph && right is Graph)
                    {

                        return ((Graph)left).Union((Graph)right);
                    }
                    else
                    {
                        throw new Exception("Both sides of graph operation must be a graph");
                    }
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

        throw new NotImplementedException("Expression not yet implemented: " + context.GetText());
    }

    public override object VisitPrint([NotNull] NerdyNodeParser.PrintContext context)
    {
        if (context.expr() != null)
        {
            var value = Visit(context.expr());
            Console.WriteLine(value);
            return value;
        }
        return false;
    }

    public override object VisitDraw([NotNull] NerdyNodeParser.DrawContext context)
    {
        if (context.expr() != null)
        {
            var value = Visit(context.expr());
            var graph = value as Graph;
            if (graph == null)
            {
                throw new Exception("Only graphs are valid for drawing - is: " + value.GetType());
            }
            var graphName = context.expr().GetText(); //FIXME: how to find the name of the graph, if expr contains more than just a symbol
            graph.Draw(graphName, graphName + ".png");
            return graph;
        }
        return false;
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
        else if (context.nodeset() != null)
        {
            return Visit(context.nodeset());
        }
        // else if (context.edg() != null)
        // {
        //     return symbolTable.Peek().Retrieve(context.IDENTIFIER().GetText());
        // }

        throw new NotImplementedException("Value type not yet implemented: " + context.GetText());
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
            var value = symbolTable.Peek().Retrieve(identifierName) as Node;
            if (value != null)
            {
                nodes.Add(value);
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
        // if both identifier1 and identifier2 are present, the semantic meaning is that identifier1 is an object and identifier2 is a method name
        if (context.IDENTIFIER().Length == 2)
        {
            return HandleObjectMethodCall(context.IDENTIFIER(0).GetText(), context.IDENTIFIER(1).GetText(), context.paramlist());
        }
        // if identifier1 is missing, then identifier2 is a plain function identifier
        else if (context.IDENTIFIER().Length == 1)
        {
            return HandleFunctionCall(context.IDENTIFIER(0).GetText(), context.paramlist());
        }
        else
        {
            throw new Exception("Missing identififiers in function call statement: " + context.GetText());
        }
    }

    private Object HandleObjectMethodCall(String objectIdentifier, String methodName, NerdyNodeParser.ParamlistContext paramList)
    {
        var value = symbolTable.Peek().Retrieve(objectIdentifier);
        if (value == null)
        {
            throw new Exception("Variable '" + objectIdentifier + "' not found in current scope");
        }
        var methodInfo = value.GetType().GetMethod(methodName);
        if (methodInfo != null)
        {
            var parameters = new List<object>();
            if (paramList != null)
            {
                foreach (var expr in paramList.expr())
                {
                    parameters.Add(Visit(expr));
                }
            }
            var returnValue = methodInfo.Invoke(value, parameters.ToArray());
            return returnValue != null ? returnValue : false;
        }
        else
        {
            throw new Exception("Method '" + methodName + "' not found on the value " + objectIdentifier + " of type " + value.GetType().Name);
        }
    }

    private Object HandleFunctionCall(String functionName, NerdyNodeParser.ParamlistContext paramList)
    {
        var symbolValue = symbolTable.Peek().Retrieve(functionName);

        var functionDecl = symbolTable.Peek().Retrieve(functionName) as NerdyNodeParser.FuncdeclarationContext;
        if (functionDecl == null)
        {
            throw new Exception("Variable '" + functionName + "' does not refer to a function in current scope");
        }

        if (paramList.expr().Length != functionDecl.paramdecllist().paramdecl().Length)
        {
            throw new Exception("Number of arguments (" + paramList.expr().Length + ") does not match declaration (" + functionDecl.paramdecllist().paramdecl().Length + ")");
        }


        // First Setup a new symbol table with all parameter names set
        // The function symbol table should inherit from root table and not the current symbol table
        var functionCallScope = new Scope(globalScope);

        SetFunctionBlockSymbols(paramList, functionDecl, functionCallScope);

        symbolTable.Push(functionCallScope);

        try
        {
            return Visit(functionDecl.block());
        }
        catch (ReturnFromBlock val)
        {
            return val.value;
        }
        finally
        {
            symbolTable.Pop();
        }
    }

    private void SetFunctionBlockSymbols(NerdyNodeParser.ParamlistContext paramList, NerdyNodeParser.FuncdeclarationContext? functionDecl, Scope functionCallScope)
    {
        if (functionDecl == null)
        {
            return;
        }

        for (var n = 0; n < functionDecl.paramdecllist().paramdecl().Length; n++)
        {
            var param = functionDecl.paramdecllist().paramdecl()[n];

            // First find destination symbol and type for parameter
            var paramType = param.type();
            var paramIdent = param.IDENTIFIER().GetText();

            // Next find source symbol and type
            var arg = paramList.expr()[n];
            var argValue = Visit(arg);

            //Console.WriteLine($"{n} ; {paramIdent}; {paramType} -> {arg}; {argValue} ");

            // Call-by-value or call-by-reference
            object paramValue;
            if (paramType.TYPEINT() != null)
            {
                paramValue = AssertInt(argValue, arg.GetText()); // call-by-value
            }
            else if (paramType.TYPESTRING() != null)
            {
                // make a new string as copy of argument
                paramValue = new string(AssertString(argValue, arg.GetText()));  // call-by-value
            }
            else if (paramType.TYPEBOOL() != null)
            {
                paramValue = AssertBool(argValue, arg.GetText()); // call-by-value
            }
            else if (paramType.TYPEGRAPH() != null)
            {
                paramValue = AssertGraph(argValue, arg.GetText());  // call-by-reference
            }
            else if (paramType.TYPENODE() != null)
            {
                paramValue = AssertNode(argValue, arg.GetText());  // call-by-reference
            }
            else if (paramType.TYPEEDGE() != null)
            {
                paramValue = AssertEdge(argValue, arg.GetText());  // call-by-reference
            }
            else if (paramType.TYPENODESET() != null)
            {
                paramValue = AssertNodeSet(argValue, arg.GetText());  // call-by-reference
            }
            else
            {
                throw new NotImplementedException("Parameter type not yet implemented: " + paramType.GetText());
            }

            // Set value in function blocks symbol table
            functionCallScope.Declare(paramIdent, paramValue);
        }
    }

    public override object VisitGraphfunc([NotNull] NerdyNodeParser.GraphfuncContext context)
    {
        if (context.addtograph().GetText() == "<<")
        {
            Graph left = GetGraphValue(context, 0) ?? throw new Exception("Left side of graph function is not a graph: " + context.GetText());
            Node right = GetNodeValue(context, 1) ?? throw new Exception("Right side of graph function is not a node: " + context.GetText());
            left.AddNode(right);
            return left;
        }
        else
        {
            Node left = GetNodeValue(context, 0) ?? throw new Exception("Left side of graph function is not a node: " + context.GetText());
            Node right = GetNodeValue(context, 1) ?? throw new Exception("Right side of graph function is not a node: " + context.GetText());

            if (left.Graph != right.Graph)
            {
                throw new Exception("Nodes are not in the same graph");
            }

            switch (context.addtograph().GetText())
            {
                case "<<":
                    break;
                case "<-->":
                    if (left.Graph == null)
                    {
                        throw new Exception("Nodes must be connected to a graph");
                    }
                    left.Graph.AddEdge(left, right, false, 0);
                    break;
                case "-->":
                    if (left.Graph == null)
                    {
                        throw new Exception("Left node must be connected to a graph");
                    }
                    left.Graph.AddEdge(left, right, true, 0);
                    break;
                case "<--":
                    if (right.Graph == null)
                    {
                        throw new Exception("Left node must be connected to a graph");
                    }
                    right.Graph.AddEdge(right, left, true, 0);
                    break;
            }
            return left;
        }
    }

    private Graph? GetGraphValue(NerdyNodeParser.GraphfuncContext context, int index)
    {
        if (context.IDENTIFIER(index) == null && context.funccall(index) != null)
        {
            return VisitFunccall(context.funccall(index)) as Graph;
        }
        else if (context.IDENTIFIER(index) != null && context.funccall(index) == null)
        {
            return symbolTable.Peek().Retrieve(context.IDENTIFIER(index).GetText()) as Graph;
        }
        else
        {
            throw new Exception($"Missing identifier or function call: " + context.GetText());
        }
    }

    private Node? GetNodeValue(NerdyNodeParser.GraphfuncContext context, int index)
    {
        if (context.IDENTIFIER(index) == null && context.funccall(index) != null)
        {
            return VisitFunccall(context.funccall(index)) as Node;
        }
        else if (context.IDENTIFIER(index) != null && context.funccall(index) == null)
        {
            return symbolTable.Peek().Retrieve(context.IDENTIFIER(index).GetText()) as Node;
        }
        else
        {
            throw new Exception($"Missing identifier or function call: " + context.GetText());
        }
    }

    //map types from antlr to c# types
    public override object VisitType([NotNull] NerdyNodeParser.TypeContext context)
    {
        if (context.TYPEINT() != null)
        {
            return typeof(int);
        }
        else if (context.TYPESTRING() != null)
        {
            return typeof(string);
        }
        else if (context.TYPEBOOL() != null)
        {
            return typeof(bool);
        }
        else if (context.TYPEGRAPH() != null)
        {
            return typeof(Graph);
        }
        else if (context.TYPENODE() != null)
        {
            return typeof(Node);
        }
        else if (context.TYPEEDGE() != null)
        {
            return typeof(Edge);
        }
        else if (context.TYPENODESET() != null)
        {
            return typeof(List<Node>);
        }

        throw new NotImplementedException("Type not yet implemented: " + context.GetText());
    }

    private static int AssertInt(object value, string text)
    {
        if (value == null)
        {
            throw new Exception("Expression is null");
        }
        else if (value.GetType() != typeof(int))
        {
            throw new Exception("Expression type should be integer - is: " + value.GetType() + " in " + text);
        }
        return (int)value;
    }

    private static bool AssertBool(object value, string text)
    {
        if (value == null)
        {
            throw new Exception("Expression is null");
        }
        else if (value.GetType() != typeof(bool))
        {
            throw new Exception("Expression type should be bool - is: " + value.GetType() + " in " + text);
        }
        return (bool)value;
    }

    private static string AssertString(object value, string text)
    {
        if (value == null)
        {
            throw new Exception("Expression is null");
        }
        else if (value.GetType() != typeof(string))
        {
            throw new Exception("Expression type should be string - is: " + value.GetType() + " in " + text);
        }
        return (string)value;
    }

    private static Graph AssertGraph(object value, string text)
    {
        if (value == null)
        {
            throw new Exception("Expression is null");
        }
        else if (value.GetType() != typeof(Graph))
        {
            throw new Exception("Expression type should be Graph - is: " + value.GetType() + " in " + text);
        }
        return (Graph)value;
    }

    private static Edge AssertEdge(object value, string text)
    {
        if (value == null)
        {
            throw new Exception("Expression is null");
        }
        else if (value.GetType() != typeof(Edge))
        {
            throw new Exception("Expression type should be Edge - is: " + value.GetType() + " in " + text);
        }
        return (Edge)value;
    }

    private static Node AssertNode(object value, string text)
    {
        if (value == null)
        {
            throw new Exception("Expression is null");
        }
        else if (value.GetType() != typeof(Node))
        {
            throw new Exception("Expression type should be Node - is: " + value.GetType() + " in " + text);
        }
        return (Node)value;
    }

    private static List<Node> AssertNodeSet(object value, string text)
    {
        if (value == null)
        {
            throw new Exception("Expression is null");
        }
        else if (value.GetType() != typeof(List<Node>))
        {
            throw new Exception("Expression type should be NodeSet - is: " + value.GetType() + " in " + text);
        }
        return (List<Node>)value;
    }

    public class ReturnFromBlock : Exception
    {
        private Object exprValue;
        public Object value
        {
            get { return exprValue; }
        }

        public ReturnFromBlock(Object value)
        {
            exprValue = value;
        }
    }

}