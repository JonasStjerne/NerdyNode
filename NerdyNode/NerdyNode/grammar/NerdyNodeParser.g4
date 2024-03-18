parser grammar NerdyNodeParser;
options {
	tokenVocab = NerdyNodeLexer;
}
program: block EOF;

block: BEGIN (statement SEMI)* END;

statement:
	forstmt
	| ifstmt
	| declaration
	| assignment
	| funccall
	| graphfunc
	| print;

forstmt: FOR IDENTIFIER IN list block;

ifstmt: IF expr block (ELSE block)?;

declaration: type assignment;

assignment: IDENTIFIER EQ expr;

type:
	TYPEINT
	| TYPESTRING
	| TYPEBOOL
	| TYPENODE
	| TYPEEDGE
	| TYPEGRAPH
	| TYPENODESET
	| TYPEEDGESET;

expr:
	value
	| IDENTIFIER
	| funccall
	| expr numop expr
	| expr boolop expr
	| expr graphop expr
	| PARANSTART expr PARANEND
	| LABEL expr LABEL
	| SETSTART IDENTIFIER arrow IDENTIFIER SETEND;

value: INT | STRING | bool | graph | nodeset | edgeset;

bool: TRUE | FALSE;

arrow: RIGHTDIRECTION | LEFTDIRECTION | UNDIRECTED;

list:
	LISTSTART expr ELLIPSIS expr LISTEND
	| nodeset
	| edgeset
	| IDENTIFIER;

numop: PLUS | MINUS | DIVIDE | TIMES | MODOLUS;

boolop:
	EQUALS
	| NOTEQUAL
	| LESSEQUAL
	| GRATEREQUAL
	| LESSTHAN
	| GREATERTHAN
	| AND
	| OR;

graphop: UNION;

graph: PARANSTART nodeset COMMA edgeset PARANEND;

nodeset: SETSTART identlist SETEND;
edgeset: SETSTART identlist SETEND;

identlist: IDENTIFIER? | (IDENTIFIER COMMA)+ IDENTIFIER;

funccall:
	IDENTIFIER DOT IDENTIFIER PARANSTART paramlist PARANEND;

graphfunc:
	IDENTIFIER ADDEDGETOGRAPH funccall
	| funccall ADDEDGETOGRAPH IDENTIFIER
	| funccall ADDEDGETOGRAPH funccall
	| IDENTIFIER ADDEDGETOGRAPH IDENTIFIER;

paramlist: expr? | (expr COMMA)* expr;

print: PRINT expr;