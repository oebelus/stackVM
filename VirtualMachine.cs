class VirtualMachine(byte[] program)
{
    private readonly byte[] memory = new byte[1024];
    private readonly Stack<byte> stack = new(1024);
    private static readonly byte frame_start = 20;
    private static readonly byte frame_end = 42;
    private byte frame_pointer = frame_start;
    private readonly Stack<byte> frame = new(frame_end - frame_start);
    private readonly Stack<byte> call_stack = new(frame_start - frame_end);
    private readonly byte[] program = program;
    private List<string> messages = [];
    private int counter = 0;

    public VirtualMachine Execute()
    {
        byte operand_1;
        byte operand_2;

        while (counter < program.Length)
        {
            byte instruction = program[counter];

            switch ((Instructions)instruction)
            {
                case Instructions.PUSH:
                    byte result = Utils.ToUint32(program.Skip(counter + 1).Take(4).ToArray());
                    stack.Push(result);
                    counter += 5;
                    break;

                case Instructions.POP:
                    counter++;
                    break;

                case Instructions.ADD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 + operand_2));
                    counter++;
                    break;

                case Instructions.SUB:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 - operand_2));
                    counter++;
                    break;

                case Instructions.DIV:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 / operand_2));
                    counter++;
                    break;

                case Instructions.MUL:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 * operand_2));
                    counter++;
                    break;

                case Instructions.MOD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 % operand_2));
                    counter++;
                    break;

                case Instructions.EXP:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)Math.Pow(operand_1, operand_2));
                    counter++;
                    break;

                case Instructions.LT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 < operand_2 ? 1 : 0));
                    counter++;
                    break;

                case Instructions.GT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 > operand_2 ? 1 : 0));
                    counter++;
                    break;

                case Instructions.EQ:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 == operand_2 ? 1 : 0));
                    counter++;
                    break;

                case Instructions.NEG:
                    stack.Push((byte)-stack.Pop());
                    counter++;
                    break;

                case Instructions.NOT:
                    stack.Push((byte)~stack.Pop());
                    counter++;
                    break;

                case Instructions.OR:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 | operand_2));
                    counter++;
                    break;

                case Instructions.AND:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 & operand_2));
                    counter++;
                    break;

                case Instructions.LS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 << operand_2));
                    counter++;
                    break;

                case Instructions.RS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 << operand_2));
                    counter++;
                    break;

                case Instructions.LOAD:
                    byte index = frame_pointer;

                    stack.Push(stack.ElementAt(index));

                    frame_pointer = (byte)((frame_pointer - 1) % frame_end);

                    counter++;
                    break;

                case Instructions.STORE:
                    byte val = stack.Pop();

                    frame.Push(val);

                    memory[frame_pointer] = val;

                    frame_pointer = (byte)((frame_pointer + 1) % frame_end);
                    stack.head--;
                    counter++;
                    break;

                case Instructions.GLOAD:
                    index = stack.Pop();
                    val = memory[index];

                    stack.Push(val);
                    counter++;
                    break;

                case Instructions.GSTORE:
                    index = stack.Pop();
                    val = stack.Pop();

                    memory[index] = val;
                    counter++;
                    break;

                case Instructions.JUMP:
                    byte destination = stack.Pop();

                    counter = destination;
                    break;

                case Instructions.CJUMP:
                    byte condition = stack.Pop();
                    destination = stack.Pop();

                    counter = condition != 0 ? destination : counter;
                    break;

                case Instructions.CALL:
                    break;

                case Instructions.RET:
                    break;
            }
        }
        return this;
    }

    public void Logger()
    {
        Console.WriteLine();

        Console.WriteLine("Stack:");

        Console.Write("[ ");

        stack.StackLogger(50);

        Console.Write("]");

        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Frame:");

        Console.Write("[ ");

        frame.StackLogger(frame_end - frame_start);

        Console.Write("]");

        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Memory:");

        int count = 0;

        Console.Write("[ ");
        while (count < 50)
        {
            Console.Write(memory[count] + " ");
            count++;
        }
        Console.Write("]");

        Console.WriteLine();

        Console.WriteLine();

        Console.WriteLine("Program:");

        foreach (var item in program)
        {
            Console.Write(item + " ");
        }

        Console.WriteLine();

        Console.WriteLine();

        Console.WriteLine("Counter: " + counter);

        Console.WriteLine();

        Console.WriteLine("Head: " + stack.head);

        Console.WriteLine();

        Console.WriteLine("Frame Pointer: " + frame.head);
        Console.WriteLine("Frame pointer in Memory: " + frame_pointer);

        Console.WriteLine();

        foreach (var message in messages)
        {
            Console.WriteLine(message);
        }
    }
}