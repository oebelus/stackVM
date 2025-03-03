using System.Text;
using Instructions = vm.Instructions;

class VirtualMachine(byte[] program)
{
    private readonly IValue[] memory = new IValue[1024];
    private readonly Stack<IValue> stack = new(1024);
    private readonly Stack<IValue[]> stackFrames = new(16);
    private readonly Stack<int> call_stack = new(16);
    private readonly byte[] program = program;
    private int counter = 0;

    public VirtualMachine Execute()
    {
        IValue operand_1;
        IValue operand_2;

        stackFrames.Push(new IValue[32]);

        while (counter < program.Length)
        {
            byte instruction = program[counter];

            switch ((Instructions)instruction)
            {
                case Instructions.PUSH:
                    int value = ByteManipulation.ToUint32([.. program.Skip(counter + 1).Take(4)]);

                    stack.Push(new Number(value));

                    counter += 5;
                    break;

                case Instructions.PUSH_CHAR:
                    byte[] c = program[(counter + 1)..(counter + 5)];

                    stack.Push(new Char(ByteManipulation.DeserializeChar(c)));

                    counter += 5;
                    break;

                case Instructions.PUSH_STR:
                    counter += 5;

                    byte[] s = program[counter..(counter + 4)];

                    stack.Push(new String(ByteManipulation.DeserializeString(s)));

                    counter += 4;
                    break;

                case Instructions.CONCAT:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    string concat = operand_1.Value.ToString() + operand_2.Value.ToString();

                    stack.Push(new String(concat));
                    counter += 9;
                    break;

                case Instructions.POP:
                    counter++;
                    break;

                case Instructions.ADD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    Console.WriteLine($"{operand_1.Value} + {operand_2.Value}");

                    try
                    {
                        stack.Push(new Number(int.Parse((operand_1.Value as string)!) + int.Parse((operand_2.Value as string)!)));
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Type Mismatch");
                    }

                    counter++;
                    break;

                case Instructions.SUB:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value - (operand_2 as Number)!.Value));
                    counter++;
                    break;

                case Instructions.DIV:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value / (operand_2 as Number)!.Value));
                    counter++;
                    break;

                case Instructions.MUL:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value * (operand_2 as Number)!.Value));
                    counter++;
                    break;

                case Instructions.MOD:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    Console.WriteLine($"{operand_1.Value}, {operand_2.Value}");

                    counter++;
                    break;

                case Instructions.EXP:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((int)Math.Pow((operand_1 as Number)!.Value, (operand_2 as Number)!.Value)));
                    counter++;
                    break;

                case Instructions.LT:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value < (operand_2 as Number)!.Value ? 1 : 0));
                    counter++;
                    break;

                case Instructions.GT:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value > (operand_2 as Number)!.Value ? 1 : 0));
                    counter++;
                    break;

                case Instructions.EQ:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value == (operand_2 as Number)!.Value ? 1 : 0));
                    counter++;
                    break;

                case Instructions.NEG:
                    stack.Push(new Number(-(stack.Pop() as Number)!.Value));
                    counter++;
                    break;

                case Instructions.NOT:
                    if ((stack.Pop() as Number)!.Value != 0)
                        stack.Push(new Number(0));
                    else stack.Push(new Number(1));

                    counter++;
                    break;

                case Instructions.OR:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value | (operand_2 as Number)!.Value));
                    counter++;
                    break;

                case Instructions.AND:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value & (operand_2 as Number)!.Value));
                    counter++;
                    break;

                case Instructions.LS:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value << (int)(operand_2 as Number)!.Value));
                    counter++;
                    break;

                case Instructions.RS:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(new Number((operand_1 as Number)!.Value >> (int)(operand_2 as Number)!.Value));
                    counter++;
                    break;

                case Instructions.LOAD:
                    Number index = (Number)stack.Pop();

                    IValue[] arr = stackFrames.Pook();

                    stack.Push(arr[index.Value]);

                    counter++;
                    break;

                case Instructions.STORE:
                    index = (Number)stack.Pop();
                    IValue val = stack.Pop();

                    arr = stackFrames.Pook();

                    arr[index.Value] = val;

                    counter++;
                    break;


                case Instructions.GLOAD:
                    index = (Number)stack.Pop();
                    val = memory[index.Value];

                    stack.Push(val);
                    counter++;
                    break;

                case Instructions.GSTORE:
                    index = (Number)stack.Pop();
                    val = stack.Pop();

                    memory[index.Value] = val;
                    counter++;
                    break;

                case Instructions.JUMP:
                    Number destination = (Number)stack.Pop();

                    counter = (int)destination.Value;
                    break;

                case Instructions.CJUMP:
                    destination = (Number)stack.Pop();
                    Number condition = (Number)stack.Pop();

                    counter = (int)(condition.Value != 0 ? destination.Value : counter + 1);
                    break;

                case Instructions.CALL:
                    call_stack.Push(counter + 5);
                    stackFrames.Push(new IValue[32]);
                    destination = new Number(ByteManipulation.ToUint32(program.Skip(counter + 1).Take(4).ToArray()));

                    counter = (int)destination.Value;
                    break;

                case Instructions.RET:
                    try
                    {
                        index = new Number(call_stack.Pop());
                        stackFrames.Pop();
                        counter = (int)index.Value;
                    }
                    catch (Exception)
                    {
                        counter = program.Length;
                    }

                    break;

                case Instructions.HALT:
                    counter = program.Length;
                    break;
            }
        }
        return this;
    }

    public void Logger()
    {
        Console.Write($"\nSTACK:\n\n[ ");

        StackToLoggable(stack).StackLogger(50);

        Console.Write("]\n\n");

        Console.WriteLine($"Head: {stack.head}\n");

        Console.Write("MEMORY:\n\n[ ");

        int count = 0;
        string[] LoggableMemory = ArrToLoggable(memory);

        while (count < 50)
        {
            Console.Write(LoggableMemory[count] + " ");
            count++;
        }

        Console.Write("]\n\n");

        count = 0;

        if (stackFrames.head > 0)
        {
            Console.Write("STACK FRAMES:\n\n");

            while (count < stackFrames.head)
            {
                Console.Write($"- Stack frame no. {count}\n\n[ ");

                string[] el = ArrToLoggable(stackFrames.ElementAt(count));

                foreach (var item in el)
                    Console.Write(item + " ");

                Console.Write("]\n\n");

                count++;
            }

            Console.WriteLine($"Frame Pointer: {stackFrames.head}, length: {stackFrames.Length()}\n");
        }

        Console.Write("PROGRAM:\n\n[ ");

        foreach (var item in program)
        {
            Console.Write(item + " ");
        }

        Console.Write("]\n\n");

        Console.WriteLine($"Counter: {counter} \n");

        Console.Write("CALL STACK:\n\n[ ");

        call_stack.StackLogger(16);

        Console.Write("]\n\n");

        Console.WriteLine($"Call Stack Pointer: {call_stack.head}\n");
    }

    public static Stack<string> StackToLoggable(Stack<IValue> stack)
    {
        return stack.Map(x =>
        {
            return x switch
            {
                Number num => num.Value.ToString(),
                String str => str.Value.ToString(),
                Char chr => chr.Value.ToString(),
                _ => 0.ToString(),
            };
        });
    }

    public static string[] ArrToLoggable(IValue[] arr)
    {
        return arr.Select(x =>
        {
            return x switch
            {
                Number num => num.Value.ToString(),
                String str => str.Value.ToString(),
                _ => 0.ToString(),
            };
        }).ToArray();
    }
}