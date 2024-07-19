using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;


namespace iLang.Parser;

public record SyntaxTree;

public record Expression : SyntaxTree;
public record Atom : Expression;
public record Identifier(string Value) : Atom;
public record Number(double Value) : Atom;
public record Boolean(bool Value) : Atom;
public record Operation(char op);
public record ArgumentList(Identifier[] Items) : SyntaxTree {
    public override string ToString() => $"[{string.Join(", ", Items.Select(x => x.ToString()))}]";

}
public record Function(Identifier Name, ArgumentList Args, Expression Body) : SyntaxTree;
public record BinaryOp(Expression Left, Operation Op, Expression Right) : Expression;
public record UnaryOp(Operation Op, Expression Right) : Expression;
public record Conditional(Expression Condition, Expression True, Expression False) : Expression;
public record Parenthesis(Expression Body) : Expression;
public record ParameterList(Expression[] Items) : SyntaxTree {
    public override string ToString() => $"({string.Join(", ", Items.Select(x => x.ToString()))})";
}
public record Call(SyntaxTree Function, ParameterList Args) : Expression;
public record CompilationUnit(SyntaxTree[] Body) : SyntaxTree {
    public override string ToString() => string.Join("\n", Body.Select(x => x.ToString()));
}

public static class Compilers {
    class Instruction(byte instruction, Operand Operand) {
        public override string ToString() => $"{(Opcodes)instruction} {Operand}";

        public byte Op { get; } = instruction;
        public Operand Operand { get; set; } = Operand;
    }
    record Operand;
    record Value(Number operand) : Operand {
        public override string ToString() => operand.Value.ToString();
    }

    record None : Operand {
        public override string ToString() => "";
    }
    record Placeholder(string atom) : Operand {
        public override string ToString() => atom;
    }
    record Bytecode(List<Instruction> Instructions) {
        public void Add(Opcodes Opcodes) => Instructions.Add(new Instruction((byte)Opcodes, new None()));
        public void Add(Opcodes Opcodes, Number operand) => Instructions.Add(new Instruction((byte)Opcodes, new Value(operand)));
        public void Add(Opcodes Opcodes, Operand operand) => Instructions.Add(new Instruction((byte)Opcodes, operand));

        public void AddRange(Bytecode bytecode) => Instructions.AddRange(bytecode.Instructions);

        public void RemoveRange(int start, int count) => Instructions.RemoveRange(start, count);
        public void RemoveRange(int start) => Instructions.RemoveRange(start, Instructions.Count - start);

        public int Size => Instructions.Sum(x => {
            if(x.Operand is Value) return 1 + 4;
            else if (x.Operand is Placeholder) return 1 + 4;
            return 1;
        });

        public override string ToString() => string.Join("\n", Instructions.Select((x, i) => $"{new Bytecode(Instructions.Slice(0, i)).Size} : {x.ToString()}"));
    }

    private class Context(string name) {
        public string Name { get; set; } = name;
        public Bytecode Bytecode { get; } = new(new List<Instruction>());
        public Dictionary<string, int> Variables { get; } = new();
    }

    private class FunctionContext() : Context(String.Empty) {
        public Dictionary<string, Bytecode> Functions { get; } = new();
        public Bytecode MachineCode { get; } = new(new List<Instruction>());

        public byte[] Collapse() {
            int AbsoluteValue(int value) => value < 0 ? -value : value; 
            int Address(string name) => AbsoluteValue(name.GetHashCode() % 1024);

            Dictionary<string, int> functionOffsets = new();
            int offsetRegionSet = Functions.Count  * 11 + 5;
            
            MachineCode.Add(Opcodes.PUSH, new Value(new Number(offsetRegionSet)));
            MachineCode.Add(Opcodes.PUSH, new Value(new Number(Address("Main"))));
            MachineCode.Add(Opcodes.GSTORE);

            int acc = Functions["Main"].Size + offsetRegionSet;
            foreach (var function in Functions)
            {
                if(function.Key == "Main") continue;
                MachineCode.Add(Opcodes.PUSH, new Value(new Number(acc)));
                MachineCode.Add(Opcodes.PUSH, new Value(new Number(Address(function.Key))));
                MachineCode.Add(Opcodes.GSTORE);
                
                acc += function.Value.Size;
            }

            MachineCode.Add(Opcodes.CALL, new Placeholder("Main"));

            functionOffsets["Main"] = offsetRegionSet;
            MachineCode.AddRange(Functions["Main"]);

            foreach(var function in Functions) {
                if(function.Key == "Main") continue;
                functionOffsets[function.Key] = MachineCode.Size;
                MachineCode.AddRange(function.Value);
            }

            foreach(var instruction in MachineCode.Instructions) {
                if(instruction.Operand is Placeholder placeholder) {
                    if(!functionOffsets.ContainsKey(placeholder.atom)) {
                        throw new Exception($"Function {placeholder.atom} not found");
                    }
                    instruction.Operand = new Value(new Number(functionOffsets[placeholder.atom]));
                }
            }

            Console.WriteLine(MachineCode);

            return MachineCode.Instructions.SelectMany(x => {
                if(x.Operand is Value value) {
                    return [ x.Op, .. BitConverter.GetBytes((int)value.operand.Value).Reverse() ];
                }
                return new byte[] { x.Op };
            }).ToArray();
        }
    }

    private static void CompileIdentifier(Identifier identifier, Context context, FunctionContext functionContext) {
        if(context.Variables.ContainsKey(identifier.Value)) {
            context.Bytecode.Add(Opcodes.PUSH, new Value(new Number(context.Variables[identifier.Value])));
            context.Bytecode.Add(Opcodes.LOAD);

        } else {
            throw new Exception($"Variable {identifier.Value} not found");
        }
    }

    private static void CompileBoolean(Boolean boolean, Context context, FunctionContext functionContext) {
        context.Bytecode.Add(Opcodes.PUSH, boolean.Value ? new Number(1) : new Number(0));
    }

    private static void CompileNumber(Number number, Context context, FunctionContext _) {
        context.Bytecode.Add((byte)Opcodes.PUSH, number);
    }

    private static void CompileCall(Call call, Context context, FunctionContext functionContext) {
        foreach(var arg in call.Args.Items) {
            CompileExpression(arg, context, functionContext);
        }
        context.Bytecode.Add(Opcodes.CALL, new Placeholder(((Identifier)call.Function).Value));
    }

    private static void CompileBinaryOp(BinaryOp binaryOp, Context context, FunctionContext functionContext) {
        CompileExpression(binaryOp.Right, context, functionContext);
        CompileExpression(binaryOp.Left, context, functionContext);
        switch (binaryOp.Op.op)
        {
            case '+':
                context.Bytecode.Add(Opcodes.ADD);
                break;
            case '-':
                context.Bytecode.Add(Opcodes.SUB);
                break;
            case '*':
                context.Bytecode.Add(Opcodes.MUL);
                break;
            case '/':
                context.Bytecode.Add(Opcodes.DIV);
                break;
            case '%':
                context.Bytecode.Add(Opcodes.MOD);
                break;
            case '<':
                context.Bytecode.Add(Opcodes.LT);
                break;
            case '>':
                context.Bytecode.Add(Opcodes.GT);
                break;
            case '=':
                context.Bytecode.Add(Opcodes.EQ);
                break;
            case '&':
                context.Bytecode.Add(Opcodes.AND);
                break;
            case '|':
                context.Bytecode.Add(Opcodes.OR);
                break;
            case '^':
                context.Bytecode.Add(Opcodes.XOR);
                break;
            default:
                throw new Exception($"Unknown binary operator {binaryOp.Op.op}");
        }
    }

    private static void CompileUnaryOp(UnaryOp unaryOp, Context context, FunctionContext functionContext) {
        CompileExpression(unaryOp.Right, context, functionContext);
        switch (unaryOp.Op.op)
        {
            case '+':
                break;
            case '-':
                context.Bytecode.Add(Opcodes.NEG);
                break;
            case '!':
                context.Bytecode.Add(Opcodes.NOT);
                break;
            case '~':
                context.Bytecode.Add(Opcodes.XOR);
                break;
            default:
                throw new Exception($"Unknown unary operator {unaryOp.Op.op}");
        }
    }

    private static void CompileParenthesis(Parenthesis parenthesis, Context context, FunctionContext functionContext) {
        CompileExpression(parenthesis.Body, context, functionContext);
    }

    private static void CompileExpression(Expression expression, Context context, FunctionContext functionContext) {
        switch (expression)
        {
            case Atom atom:
                CompileAtom(atom, context, functionContext);
                break;
            case Call call:
                CompileCall(call, context, functionContext);
                break;
            case BinaryOp binaryOp:
                CompileBinaryOp(binaryOp, context, functionContext);
                break;
            case UnaryOp unaryOp:
                CompileUnaryOp(unaryOp, context, functionContext);
                break;
            case Conditional conditional:
                CompileConditional(conditional, context, functionContext);
                break;
            case Parenthesis parenthesis:
                CompileParenthesis(parenthesis, context, functionContext);
                break;
            default:
                throw new Exception($"Unknown expression type {expression.GetType()}");
        }
    }

    private static void CompileAtom(Atom tree, Context context, FunctionContext functionContext) {
        switch (tree)
        {
            case Identifier identifier:
                if(!context.Variables.ContainsKey(identifier.Value)) {
                    context.Variables[identifier.Value] = context.Variables.Count;
                }
                CompileIdentifier(identifier, context, functionContext);
                break;
            case Number number:
                CompileNumber(number, context, functionContext);
                break;
            case Boolean boolean:
                CompileBoolean(boolean, context, functionContext);
                break;
            default:
                throw new Exception($"Unknown atom type {tree.GetType()}");
        }
    }

    private static void CompileConditional(Conditional conditional, Context context, FunctionContext functionContext) {
        int AbsoluteValue(int value) => value < 0 ? -value : value; 
        int Address(string name) => AbsoluteValue(name.GetHashCode() % 1024);

        CompileExpression(conditional.Condition, context, functionContext);
        context.Bytecode.Add(Opcodes.PUSH, new Value(new Number(14)));
        context.Bytecode.Add(Opcodes.STORE);

        int current = context.Bytecode.Instructions.Count;
        
        CompileExpression(conditional.True, context, functionContext);
        var trueSlice = new Bytecode(context.Bytecode.Instructions[current..]);
        context.Bytecode.RemoveRange(current);

        CompileExpression(conditional.False, context, functionContext);
        var falseSlice = new Bytecode(context.Bytecode.Instructions[current..]);
        context.Bytecode.RemoveRange(current);

        int offsetIndex = Address(context.Name);

        context.Bytecode.Add(Opcodes.PUSH, new Value(new Number(offsetIndex)));
        context.Bytecode.Add(Opcodes.GLOAD);
        context.Bytecode.Add(Opcodes.PUSH, new Value(new Number(context.Bytecode.Size + falseSlice.Size + 26)));
        context.Bytecode.Add(Opcodes.ADD);

        context.Bytecode.Add(Opcodes.PUSH, new Value(new Number(14)));
        context.Bytecode.Add(Opcodes.LOAD);

        context.Bytecode.Add(Opcodes.CJUMP);

        context.Bytecode.AddRange(falseSlice);

        context.Bytecode.Add(Opcodes.PUSH, new Value(new Number(offsetIndex)));
        context.Bytecode.Add(Opcodes.GLOAD);
        context.Bytecode.Add(Opcodes.PUSH, new Value(new Number(context.Bytecode.Size + trueSlice.Size + 7)));
        context.Bytecode.Add(Opcodes.ADD);
        context.Bytecode.Add(Opcodes.JUMP);

        context.Bytecode.AddRange(trueSlice);
    }

    private static void CompileFunction(Function function, FunctionContext functionContext) {
        if(functionContext.Functions.ContainsKey(function.Name.Value)) {
            throw new Exception($"Function {function.Name.Value} already defined");
        }

        var localContext = new Context(function.Name.Value);
        
        var functionArgs = function.Args.Items.Reverse().ToArray();
        for(int i = 0; i < functionArgs.Length; i++) {
            localContext.Variables[functionArgs[i].Value] = i;
            localContext.Bytecode.Add(Opcodes.PUSH, new Value(new Number(i)));
            localContext.Bytecode.Add(Opcodes.STORE);
        }

        CompileExpression(function.Body, localContext, functionContext);

        if(function.Name.Value == "Main") {
            localContext.Bytecode.Add(Opcodes.HALT);
        } else {
            localContext.Bytecode.Add(Opcodes.RET);
        }

        functionContext.Functions[function.Name.Value] = localContext.Bytecode;
    }

    public static byte[] Compile(CompilationUnit compilationUnit) {
        var functionContext = new FunctionContext();
        foreach(var tree in compilationUnit.Body) {
            if(tree is Function function) {
                CompileFunction(function, functionContext);
            } else {
                throw new Exception($"Unknown tree type {tree.GetType()}");
            }
        }

        return functionContext.Collapse();
    }
}
public static class Parsers {

    private static bool Fail(string code, ref int index, int start) {
        index = start;
        return false;
    }
    private static bool ParseIdentifier(string code, ref int index, out Atom? identifier) {
        identifier = null;
        var start = index;
        while (index < code.Length && char.IsLetter(code[index])) {
            index++;
        }
        if(start == index) return Fail(code, ref index, start);
        identifier = new Identifier(code[start..index]);
        return true;
    }

    private static bool ParseNumber(string code, ref int index, out Atom? number) {
        number = null;
        var start = index;
        while (index < code.Length && char.IsDigit(code[index])) {
            index++;
        }
        if(start == index) return Fail(code, ref index, start);
        number = new Number(double.Parse(code[start..index]));
        return true;
    }

    private static bool ParseConditional(string code, ref int index, out Conditional? conditional) {
        conditional = null;
        int start = index;
        if(!ParseExpression(code, ref index, out Expression? condition, excludeTernary: true, excludeBinary: true)) return Fail(code, ref index, start);

        if(code[index++] != '?') return Fail(code, ref index, start);
        if(!ParseExpression(code, ref index, out Expression? trueBranch, excludeTernary: true, excludeBinary: true)) return Fail(code, ref index, start);

        if(code[index++] != ':') return Fail(code, ref index, start);
        if(!ParseExpression(code, ref index, out Expression? falseBranch, excludeTernary: true, excludeBinary: true)) return Fail(code, ref index, start);

        conditional = new Conditional(condition, trueBranch, falseBranch);
        return true;
    }

    private static bool ParseAtom(string code, ref int index, out Atom? token) {
        token = null;
        int start = index;
        if (index < code.Length && char.IsLetter(code[index])) {
            if(ParseBoolean(code, ref index, out token)) return true;
            return ParseIdentifier(code, ref index, out token);
        }
        if (index < code.Length && char.IsDigit(code[index])) {
            return ParseNumber(code, ref index, out token);
        }
        return Fail(code, ref index, start);
    }

    private static bool ParseArgumentList(string code, ref int index, out Atom[]? argList) {
        argList = null;
        int start = index;

        if(code[index++] != '[') return Fail(code, ref index, start);
        List<Atom> args = new();
        bool parseNextToken = true;
        while(parseNextToken && ParseIdentifier(code, ref index, out var token)) {
            args.Add(token);
            if(code[index] != ',' && code[index] != ']') return Fail(code, ref index, start); 
            else {
                parseNextToken = code[index++] != ']'; 
            }
        }

        if(args.Count == 0) return Fail(code, ref index, start);

        argList = args.ToArray();
        return true;
    }    

    private static bool ParseUnaryOperation(string code, ref int index, out UnaryOp? unaryOp) {
        unaryOp = null;
        Expression? atom = null;
        int start = index;


        Operation? op = code[index++] switch
        {
            '+' => new Operation('+'),
            '-' => new Operation('-'),
            '!' => new Operation('!'),
            '~' => new Operation('~'),
            _ => null
        };

        if(op == null) return Fail(code, ref index, start);

        if(ParseExpression(code, ref index, out atom, excludeBinary: true, excludeTernary: true)) {
            unaryOp = new UnaryOp(op, atom);
            return true;
        }
        return Fail(code, ref index, start);
    }

    private static bool ParseParenthesis(string code, ref int index, out Parenthesis? parenthesis) {
        parenthesis = null;
        int start = index;
        if(code[index++] != '(') return Fail(code, ref index, start);
        if(!ParseExpression(code, ref index, out Expression? body)) return Fail(code, ref index, start);
        if(code[index++] != ')') return Fail(code, ref index, start);
        parenthesis = new Parenthesis(body);
        return true;
    }

    private static bool ParseBoolean(string code, ref int index, out Atom? boolean) {
        boolean = null;
        int start = index;
        if(ParseIdentifier(code, ref index, out Atom? token) && token is Identifier identifier && (identifier.Value == "true" || identifier.Value == "false")) {
            boolean = new Boolean(identifier.Value == "true");
            return true;
        } 
        return Fail(code, ref index, start);
    }

    private static bool ParseBinaryOperation(string code, ref int index, out BinaryOp? binaryOp) {
        binaryOp = null;
        Operation op = null;
        int start = index;

        if(!ParseExpression(code, ref index, out Expression? leftAtom, excludeBinary: true, excludeTernary: true)) {
            return Fail(code, ref index, start);
        }
        if(code[index] != '+' && code[index] != '-' && code[index] != '*' && code[index] != '/' && code[index] != '%' && code[index] != '<' && code[index] != '>' && code[index] != '=' && code[index] != '&' && code[index] != '|' && code[index] != '^') {
            return Fail(code, ref index, start);
        } ;

        op = new Operation(code[index++]);

        if(!ParseExpression(code, ref index, out Expression? rightAtom)) {
            return Fail(code, ref index, start);
        }
        binaryOp = new BinaryOp(leftAtom, op, rightAtom);
        return true;
    }
    private static bool ParseParameters(string code, ref int index, out Expression[]? argsList) {
        int start = index;
        argsList = null;
        if(code[index++] != '(') return Fail(code, ref index, start);
        List<Expression> args = new();
        bool parseNextToken = true;
        while(parseNextToken && ParseExpression(code, ref index, out var token)) {
            args.Add(token);
            if(code[index] != ',' && code[index] != ')') return Fail(code, ref index, start); 
            else {
                parseNextToken = code[index++] != ')'; 
            }
        }
        if(args.Count == 0) return Fail(code, ref index, start);

        argsList = args.ToArray();
        return true;
        
    }

    private static bool ParseCall(string code, ref int index, out Call? call) {

        int start = index;

        call = null;
        if(!ParseIdentifier(code, ref index, out Atom? identifier)) return Fail(code, ref index, start);
        if(!ParseParameters(code, ref index, out Expression[]? argsList)) return Fail(code, ref index, start);

        call = new Call(identifier, new ParameterList(argsList));
        return true;
    }

    private static bool ParseExpression(string code, ref int index, out Expression? expression, bool excludeBinary = false, bool excludeTernary = false) {
        expression = null;
        if(!excludeBinary && ParseBinaryOperation(code, ref index, out BinaryOp? binaryOp)) {
            expression = binaryOp;
            return true;
        } 
        else if(!excludeTernary && ParseConditional(code, ref index, out Conditional? conditional)) {
            expression = conditional;
            return true;
        }
        else if(ParseParenthesis(code, ref index, out Parenthesis? parenthesis)) {
            expression = parenthesis;
            return true;
        }
        else if(ParseCall(code, ref index, out Call? call)) {
            expression = call;
            return true;
        }
        else if(ParseUnaryOperation(code, ref index, out UnaryOp? unaryOp)) {
            expression = unaryOp;
            return true;
        }
        else if(ParseAtom(code, ref index, out Atom? atom)) {
            expression = atom;
            return true;
        }
        return false;
    }

    public static bool ParseFunction(string code, ref int index, out Function function) {
        
        function = null;
        if(!ParseIdentifier(code, ref index, out Atom? identifier)) {
            return false;
        }
        if(code[index++] != ':') return false;
        
        Atom[]? argsList;
        if(identifier is Identifier name && name.Value != "Main") {

            if(!ParseArgumentList(code, ref index, out argsList)) {
                return false;
            }
            if(code[index++] != '-' || code[index++] != '>') {
                return false;
            }
        }
        else {
            argsList = Array.Empty<Atom>();
        }
        if(!ParseExpression(code, ref index, out Expression? body)) {
            return false;
        }
        if(code[index++] != ';') return false;
        function = new Function((Identifier)identifier, new ArgumentList(argsList.Cast<Identifier>().ToArray()), body);
        return true;
    }

    public static bool ParseCompilationUnit(string code, out CompilationUnit compilationUnit) {
        code = code.Replace(" ", "").Replace("\n", "").Replace("\r", "");

        int index = 0;
        compilationUnit = null;
        List<SyntaxTree> body = new();
        while(index < code.Length) {
            if(!ParseFunction(code, ref index, out Function? function)) {
                return false;
            }
            body.Add(function);
        }
        compilationUnit = new CompilationUnit(body.ToArray());
        return true;
    }
}