using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Statements;
using Djinn.Syntax.Biding;
using Djinn.Syntax.Biding.Statements;

// var source = $$"""
//                function void hello(void) {
//                    ret printf("Hello World!");
//                }
//                """;

var source = $$"""
               function void hello(int a) {
                   ret 1 + a;
               }
               """;

// var source = $$"""1+2+3+4""";

// var source = "";

var lexer = new Lexer(source);
var parser = new Parser(lexer);
var tree = parser.Parse();
var binder = new Binder();

var a = binder.Visit((BlockStatement)tree.Statements.First());
var test = (BoundBlockStatement)a;
// IDjinn.Compile(source);

return;