class Tokens
{
    public static Dictionary<char, Instructions> Operations = new()
    {
        { '+', Instructions.ADD },
        { '-',  Instructions.SUB },
        { '/', Instructions.DIV },
        { '*', Instructions.MUL }
    };
}