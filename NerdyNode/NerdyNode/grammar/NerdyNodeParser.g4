parser grammar NerdyNodeParser;
options {
	tokenVocab = NerdyNodeLexer;
}

program: block EOF;

block: BEGIN (statement SEMI)* END;

statement: forstmt | ifstmt | declaration | assignment | print;

forstmt: FOR IDENTIFIER IN list block;

ifstmt: IF expr block;

declaration: type assignment;

assignment: IDENTIFIER EQ expr;

type: | TYPEINT | TYPESTRING | TYPEBOOL | TYPEGRAPH;

expr:
	value
	| IDENTIFIER
	| expr numop expr
	| expr boolop expr
	| PARANSTART expr PARANEND;

value: | INT | STRING | bool | graph | nodeset | edgeset;

bool: TRUE | FALSE;

list: LISTSTART expr ELLIPSIS expr LISTEND;

numop: PLUS | MINUS | DIVIDE | TIMES | MODOLUS;

boolop: EQUALS;

graph: PARANSTART nodeset COMMA edgeset PARANEND;

nodeset: SETSTART identlist SETEND;
edgeset: SETSTART identlist SETEND;

identlist: IDENTIFIER | (IDENTIFIER COMMA)+;

print: PRINT STRING;