lexer grammar NerdyNodeLexer;

//Seperators 
EQ: '=';
SEMI: ';';
COL: ':';
DOT: '.';
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
TYPESTRING: 'string';
TYPEBOOL: 'boolean';

//values:
INT: [0-9]+;
STRING: '"' .*? '"';
TRUE: 'true';
FALSE: 'false';

//Graph types
TYPEGRAPH: 'graph';
TYPENODE: 'node';
TYPEEDGE: 'edge';
TYPENODESET: 'nodeset';
TYPEEDGESET: 'edgeset';

// Program constructs
FOR: 'for';
IN: 'in';
IF: 'if';
ELSE: 'else';

//Numop
PLUS: '+';
MINUS: '-';
DIVIDE: '/';
TIMES: '*';
MODOLUS: '%';

//Boolop
EQUALS: '==';
NOTEQUAL: '/=';
LESSEQUAL: '<=';
GRATEREQUAL: '>=';
LESSTHAN: '<';
GREATERTHAN: '>';
AND: '&&';
OR: '||';

//Graph operators
UNION: 'union';
//INTERSECTION: 'inter'; COMPLIMENT: '\\'; CARTESIANPRODUCT: 'product'; SUPSET: 'subset'; SUPERSET:
// 'superset';

RIGHTDIRECTION: '->';
LEFTDIRECTION: '<-';
UNDIRECTED: '<->';
ADDEDGETOGRAPH: '<-->';

PRINT: 'print';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;

WS: [ \t\n\r\f]+ -> skip;
COMMENT: '//' ~[\n]* -> skip;
BLOCKCOMMENT: '/*' (.)*? '*/' -> skip;