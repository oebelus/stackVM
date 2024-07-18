class Tokens
{
    Dictionary<string, Func<int, int, int>> Operations = new()
    {
        { "add", (a, b) => a + b },
        { "sub", (a, b) => a - b },
        { "div", (a, b) => a / b },
        { "mul", (a, b) => a * b }
    };
}