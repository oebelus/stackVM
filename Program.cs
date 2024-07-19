using iLang.Parser;

var code = @"
Max : [a, b] -> (a > b) ? a : b; 
Min : [a, b] -> (a < b) ? a : b; 
Sum : [a, b] -> (a = b) ? b : (a + Sum(a + 1, b)); 
Main: Sum(Min(23, 69), Max(23, 69));
";

Parsers.ParseCompilationUnit(code, out var function);
byte[] bytes = Compilers.Compile(function);

VirtualMachine vm = new(bytes);

vm.Execute();

vm.Logger();