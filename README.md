```
    _   __              __      _   __          __
   / | / /__  _________/ /_  __/ | / /___  ____/ /__
  /  |/ / _ \/ ___/ __  / / / /  |/ / __ \/ __  / _ \
 / /|  /  __/ /  / /_/ / /_/ / /|  / /_/ / /_/ /  __/
/_/ |_/\___/_/   \__,_/\__, /_/ |_/\____/\__,_/\___/
                      /____/
```

# NerdyNode

The next big thing in the world of unwanted programming languages

## Development

-   Follow the getting started guide for antlr4 [here](https://github.com/antlr/antlr4/blob/master/doc/getting-started.md).

-   Download Visual Studio Code extension [here](https://marketplace.visualstudio.com/items?itemName=mike-lischke.vscode-antlr4).

## Examples

```
begin
    int n = 2+(3/5*4);
    graph g = ({N1},{E1});
    string m = "hej";
    boolean a = true;
    for i in [1..4] begin
        for j in [i..6] begin
        n = 4*5;
        end;
    end;
    if a == 3 begin
    string e = "ja";
    end;
    print "nej";
end
```

### Lattice graph

```
begin
    graph latticeGraph = Graph;
    for y in [0..height] begin
        for x in [0..width] begin
            node newNode = Node(x+","+y);
            latticeGraph.addNode(newNode)
            if x > 0 begin
                newNode <--> latticeGraph.getNodeByLabel(x-1+","+y);
            end;
            if y > 0 begin
                newNode <--> latticeGraph.getNodeByLabel(x+","+y-1);
            end;
        end;
    end;
end
```
