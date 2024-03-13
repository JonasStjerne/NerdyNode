lexer grammar GraphLexer;

//Seperators 
EQ: '=';
SEMI: ';';
COL: ':';
BEGIN: 'begin';
END: 'end';
LISTSTART: '[';
LISTEND: ']';
SETSTART: '{';
SETEND: '}';
PARANSTART: '(';
PARANEND: ')';
COMMA: ',';
ELLIPSIS: '..';
LABEL: '"';

//Types
TYPEINT: 'int';
INT: [0-9]+;
TYPESTRING: 'string';
STRING: '"' .*? '"';
TYPEBOOL: 'boolean';
TRUE: 'true';
FALSE: 'false';

//Graph types
TYPEGRAPH: 'graph';
TYPENODE: 'node';
TYPEEDGE: 'edge';

FOR: 'for';
FOREACH: 'foreach';
IF: 'if';

//Operators
PLUS: '+';
MINUS: '-';
DIVIDE: '/';
TIMES: '*';
MODOLUS: '%';
EQUALS: '==';
NOTEQUAL: '/=';
LESSEQUAL: '<=';
GRATEREQUAL: '>=';
LESSTHAN: '<';
GREATERTHAN: '>';
AND: '&&';
OR: '||';
NOT: 'not';
IN: 'in';

//Graph operators
UNION: 'union';
INTERSECTION: 'inter';
COMPLIMENT: '\\';
CARTESIANPRODUCT: 'product';
SUPSET: 'subset';
SUPERSET: 'superset';
NOTIN: 'not' 'in';

ADDTOGRAPH: '<<';

//Asignments
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
//Exstra
WS: [ \t\n\r\f]+ -> skip;