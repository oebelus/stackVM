interface IValue { }

class Number(long number) : IValue
{
    public long Value => number;
}

class String(string str) : IValue
{
    public string Value => str;
}