public class Scope
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();
    private Scope? parent;
    public Scope()
    {
        this.variables = new Dictionary<string, object>();
        this.parent = null;
    }
    public Scope(Scope parent)
    {
        this.variables = new Dictionary<string, object>();
        this.parent = parent;
    }

    public Scope CreateScope()
    {
        return new Scope(this);
    }

    public object Retrieve(string name)
    {
        if (this.variables.ContainsKey(name))
        {
            return this.variables[name];
        }
        else if (this.parent != null)
        {
            return this.parent.Retrieve(name);
        }
        else
        {
            throw new Exception("Variable not found");
        }
    }

    public void Assign(string name, object value)
    {
        if (this.variables.ContainsKey(name))
        {
            this.variables[name] = value;
        }
        else if (this.parent != null)
        {
            this.parent.Assign(name, value);
        }
        else
        {
            throw new Exception("Variable not found");
        }
    }

    public void Declare(string name, object value)
    {
        if (this.variables.ContainsKey(name))
        {
            throw new Exception("Variable already declared");
        }
        else
        {
            this.variables[name] = value;
        }
    }

    public bool HasVariable(string name)
    {
        if (this.variables.ContainsKey(name))
        {
            return true;
        }
        else if (this.parent != null)
        {
            return this.parent.HasVariable(name);
        }
        else
        {
            return false;
        }
    }


}