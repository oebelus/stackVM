namespace vm;

public enum Instructions : byte
{
    PUSH,
    POP,

    ADD,
    SUB,
    MUL,
    DIV,
    NEG,
    EXP,
    MOD,
    LT,
    GT,
    EQ,

    AND,
    OR,
    NOT,
    XOR,
    LS,
    RS,

    LOAD, // load local val or fct arg
    GLOAD,
    STORE,
    GSTORE,

    JUMP,
    CJUMP,

    CALL,
    RET,

    HALT
}

class Instruction
{
    public static Dictionary<string, int> vInstruction = new()
    {
        {"PUSH", 0},
        {"POP", 1},
        {"ADD", 2},
        {"SUB", 3},
        {"MUL", 4},
        {"DIV", 5},
        {"NEG", 6},
        {"EXP", 7},
        {"MOD", 8},
        {"LT", 9},
        {"GT", 10},
        {"EQ", 11},
        {"AND", 12},
        {"OR", 13},
        {"NOT", 14},
        {"XOR", 15},
        {"LS", 16},
        {"RS", 17},
        {"LOAD", 18},
        {"GLOAD", 19},
        {"STORE", 20},
        {"GSTORE", 21},
        {"JUMP", 22},
        {"CJUMP", 23},
        {"CALL", 24},
        {"RET", 25},
        {"HALT", 26},
    };

    // public static Dictionary<TokenType, string> cOperation = new()
    // {
    //     {TokenType.PLUS, "ADD"},
    //     {TokenType.MINUS, "SUB"},
    //     {TokenType.MOD, "MOD"},
    //     {TokenType.STAR, "MUL"},
    //     {TokenType.SLASH, "DIV"},
    //     {TokenType.LESS, "LT"},
    //     {TokenType.GREATER, "GT"},
    //     {TokenType.EQUAL_EQUAL, "EQ"},
    //     {TokenType.AND, "AND"},
    //     {TokenType.OR, "OR"},
    //     {TokenType.BANG, "NEG"},
    // };

    public static Dictionary<Instructions, string> cInstruction = new()
    {
        {Instructions.PUSH, "PUSH"},
        {Instructions.POP, "POP"},
        {Instructions.LOAD, "LOAD"},
        {Instructions.GLOAD, "GLOAD"},
        {Instructions.STORE, "STORE"},
        {Instructions.GSTORE, "GSTORE"},
        {Instructions.RET, "RET"},
        {Instructions.CJUMP, "CJUMP"},
        {Instructions.JUMP, "JUMP"},
        {Instructions.EQ, "EQ"},
        {Instructions.GT, "GT"},
        {Instructions.LT, "LT"},
        {Instructions.NEG, "LT"},
        {Instructions.NOT, "NOT"},
    };
}

