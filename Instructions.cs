enum Instructions : byte
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

    LOAD,
    STORE,

    JUMP,
    CJUMP,
}

class Program
{
    public static Dictionary<string, int> instruction = new()
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
        {"STORE", 19},
        {"JUMP", 20},
        {"CJUMP", 21}
    };
}

