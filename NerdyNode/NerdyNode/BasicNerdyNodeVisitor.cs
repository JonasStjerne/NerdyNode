public class BasicNerdyNodeVisitor : NerdyNodeParserBaseVisitor<object>
{
    public bool Boolean;
    public override object VisitBool(NerdyNodeParser.BoolContext context)
    {            
        Boolean = bool.Parse(context.GetText());
        return Boolean;
    }
}