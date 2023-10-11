

using Djinn;

var source = $$"""
               function void hello(void) {
                   ret printf("Hello World!");
               }
               """;

// var source = $$"""1+2+3+4""";

// var source = "";
            
IDjinn.Compile(source);