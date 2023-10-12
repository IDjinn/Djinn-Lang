using Djinn;

// var source = $$"""
//                function void hello(void) {
//                    ret printf("Hello World!");
//                }
//                """;

var source = $$"""
               function void hello(void) {
                   ret 1 + 2;
               }
               """;

// var source = $$"""1+2+3+4""";

// var source = "";

IDjinn.Compile(source);