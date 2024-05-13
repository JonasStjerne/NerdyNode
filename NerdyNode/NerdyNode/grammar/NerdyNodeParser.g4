//
// antlr4 -lib . -Dlanguage=CSharp -o ../antlr4 -visitor NerdyNodeParser.g4 NerdyNodeLexer.g4
//
parser grammar NerdyNodeParser;
options {
	tokenVocab = NerdyNodeLexer;
}
program: block (funcdeclaration)* EOF;

block: BEGIN (statement SEMI)* END;

funcdeclaration:
	type IDENTIFIER PARANSTART paramdecllist PARANEND block;

paramdecllist: paramdecl? | (paramdecl COMMA)+ paramdecl;

paramdecl: type IDENTIFIER;

statement:
	forstmt
	| ifstmt
	| declaration
	| assignment
	| funccall
	| graphfunc
	| print
	| draw
	| returnstmt;

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
	| MINUS expr
	| PLUS expr
	| expr numop1 expr
	| expr numop2 expr
	| expr boolop expr
	| expr graphop expr
	| PARANSTART expr PARANEND
	| IDENTIFIER
	| funccall
	| LABEL expr LABEL
	| SETSTART IDENTIFIER arrow IDENTIFIER SETEND;

value: INT | STRING | bool | graph | nodeset | edgeset;

bool: TRUE | FALSE;

arrow: RIGHTDIRECTION | LEFTDIRECTION | UNDIRECTED;

list:
	LISTSTART expr ELLIPSIS expr LISTEND
	//| nodeset | edgeset
	| IDENTIFIER;

numop1: DIVIDE | TIMES | MODOLUS;
numop2: PLUS | MINUS;

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
	(IDENTIFIER DOT)? IDENTIFIER PARANSTART paramlist PARANEND;

paramlist: expr? | (expr COMMA)+ expr;

graphfunc:
	(IDENTIFIER | funccall) addtograph (IDENTIFIER | funccall);

addtograph:
	ADDUNDIRECTED
	| ADDLEFTDIRECTION
	| ADDRIGHTDIRECTION
	| ADD_TO
	;

returnstmt: RETURN expr;

print: PRINT expr;
draw: DRAW expr;