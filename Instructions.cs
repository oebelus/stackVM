namespace vm;

public enum Instructions : byte
{
    PUSH, // int
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

    HALT,

    PUSH_STR,
    PUSH_CHAR,
    CONCAT
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
        {"PUSH_STR", 27},
        {"PUSH_CHAR", 28},
        {"CONCAT", 29},
    };

    public static Dictionary<TokenType, string> cOperation = new()
    {
        {TokenType.PLUS, "ADD"},
        {TokenType.MINUS, "SUB"},
        {TokenType.MOD, "MOD"},
        {TokenType.STAR, "MUL"},
        {TokenType.SLASH, "DIV"},
        {TokenType.LESS, "LT"},
        {TokenType.GREATER, "GT"},
        {TokenType.EQUAL_EQUAL, "EQ"},
        {TokenType.AND, "AND"},
        {TokenType.OR, "OR"},
        {TokenType.BANG, "NEG"},
    };

    public static Dictionary<Instructions, string> cInstruction = new()
    {
        {Instructions.PUSH, "PUSH"},
        {Instructions.PUSH_STR, "PUSH_STR"},
        {Instructions.PUSH_CHAR, "PUSH_CHAR"},
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

