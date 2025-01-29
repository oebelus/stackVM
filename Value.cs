interface IValue
{
    int Size { get; set; }
    string Type => GetType().Name;
}

record Number(long Value) : IValue
{
    public int Size { get; set; } = 4;
}

record String(string Value) : IValue
{
    public int Size { get; set; } = Value.Length * sizeof(char);
}

record Nil() : IValue
{
    public int Size { get; set; } = 0;
}

record Boolean(bool Value) : IValue
{
    public int Size { get; set; } = 1;
}