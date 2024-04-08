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
    int width = 4;
    int height = 4;
    graph latticeGraph = ({},{});
    for y in [1..height] begin
        for x in [1..width] begin
            node newNode = |x+","+y|;
            latticeGraph.AddNode(newNode);
            if x > 1 begin
                node n = latticeGraph.GetNode((x-1)+","+y);
                newNode <--> n;
            end;
            if y > 1 begin
                node n = latticeGraph.GetNode(x+","+(y-1));
                newNode <--> n;
            end;
        end;
    end;
    print latticeGraph;
end
```

### Binary Tree

```
begin
    int depth = 4;
    node initialNode = |"1"|;
    graph binaryTree = ({initialNode},{});
    for i in [2..depth] begin
        graph left = binaryTree;
        graph right = binaryTree.Copy();
        binaryTree = left union right;
        node newRootNode = |i|;
        binaryTree.AddNode(newRootNode);
        nodeset ns = binaryTree.GetNodes((i-1)+"");
        for n in ns begin
            newRootNode --> n;
        end;
    end;
    print binaryTree;
end
```

### Binary tree 2.0

```
begin
    int depth = 4;
    node initialNode = "1";
    graph binaryTree = ({initialNode},{});
    for i in [2..depth] begin
        graph left = binaryTree;
        graph right = binaryTree.copy();
        binaryTree = left union right;
        node newRootNode = [i];
        binaryTree.addNode(newRootNode);
        nodeset ns = binaryTree.getNodes(i-1);
        for n in ns begin
            edge e = {newRootNode -> n};
            binaryTree.addEdge(e);
        end;
    end;
end
```

### Lattice graph 2.0

```
// Lattice graph 10x10
begin
    int width = 10;
    int height = 10;
    graph latticeGraph = ({},{});
    for y in [1..height] begin
        for x in [1..width] begin
            node newNode = [x+","+y];
            latticeGraph.addNode(newNode);
            if x > 1 begin
                node n = latticeGraph.getNode((x-1)+","+y);
                edge e = {newNode <-> n};
                latticeGraph.addEdge(e);
            end;
            if y > 1 begin
                node n = latticeGraph.getNode(x+","+(y-1));
                edge e = {newNode <-> n};
                latticeGraph.addEdge(e);
            end;
        end;
    end;
end
```
