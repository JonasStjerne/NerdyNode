public class Scope
{
    public Dictionary<string, object> variables;
    private Scope? parent;
    Scope()
    {
        this.variables = new Dictionary<string, object>();
        this.parent = null;
    }
    Scope(Scope parent)
    {
        this.variables = new Dictionary<string, object>();
        this.parent = parent;
    }

}