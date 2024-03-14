public class Scope
{
    public Dictionary<string, object> variables = new Dictionary<string, object>();
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

    public object retrieve(string name)
    {
        if (this.variables.ContainsKey(name))
        {
            return this.variables[name];
        }
        else if (this.parent != null)
        {
            return this.parent.retrieve(name);
        }
        else
        {
            throw new Exception("Variable not found");
        }
    }

    public void assign(string name, object value)
    {
        if (this.variables.ContainsKey(name))
        {
            this.variables[name] = value;
        }
        else if (this.parent != null)
        {
            this.parent.assign(name, value);
        }
        else
        {
            throw new Exception("Variable not found");
        }
    }

    public void declare(string name, object value)
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

    public bool hasVariable(string name)
    {
        if (this.variables.ContainsKey(name))
        {
            return true;
        }
        else if (this.parent != null)
        {
            return this.parent.hasVariable(name);
        }
        else
        {
            return false;
        }
    }


}