interface IValue
{
    int Size { get; set; }
    string Type => GetType().Name;
    object Value { get; }
}

record Number(int Value) : IValue
{
    public int Size { get; set; } = 4;

    object IValue.Value => Value.ToString();
}

record String(string Value) : IValue
{
    public int Size { get; set; } = Value.Length;
    object IValue.Value => Value.ToString();
}

record Char(char Value) : IValue
{
    public int Size { get; set; } = 1;
    object IValue.Value => Value.ToString();
}

record Nil() : IValue
{
    public int Size { get; set; } = 0;
    object IValue.Value => "Nil";

}

record Boolean(bool Value) : IValue
{
    // Since bools are converted to ints : ) 
    public int Size { get; set; } = 4;
    object IValue.Value => Value.ToString();
}

/*

int charSize = 2;
int stringSize = charSize * strLength;

*/