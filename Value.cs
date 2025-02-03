interface IValue { }

record Number(long Value) : IValue
{
}

record String(string Value) : IValue
{
}

record Nil() : IValue
{
}