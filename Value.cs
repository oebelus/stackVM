interface IValue
{
    public int Size { get; set; }
    public string Type => GetType().Name;
    public object Value { get; }

}

record Number(int Value) : IValue
{
    public int Size { get; set; } = 4;

    object IValue.Value => Value.ToString();
}

record Obj(object Value, int Size, int Pointer) : IValue
{
    public int Pointer { get; set; } = Pointer;
    int IValue.Size { get; set; } = Size;
    object IValue.Value => Value;

}

record String(string Val, int Pointer) : Obj(Val, Val.Length, Pointer)
{
    public new int Pointer = Pointer;
    public new int Size { get; set; } = Val.Length;
    public new object Value => Value;
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