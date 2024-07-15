class VirtualMachine(byte[] program)
{
    private readonly byte[] memory = new byte[1024];
    private readonly Stack<byte> stack = new(1024);
    private readonly Stack<byte> frame = new(1024);
    private readonly byte[] program = program;

    private int counter = 0;

    public VirtualMachine Execute()
    {
        byte operand_1;
        byte operand_2;

        while (counter < program.Length)
        {
            byte instruction = program[counter];

            switch (instruction)
            {
                case (byte)Instructions.PUSH:
                    byte result = Utils.ToUint32(program.Skip(counter + 1).Take(4).ToArray());
                    stack.Push(result);
                    counter += 5;
                    break;

                case (byte)Instructions.POP:
                    counter++;
                    break;

                case (byte)Instructions.ADD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 + operand_2));
                    counter++;
                    break;

                case (byte)Instructions.SUB:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 - operand_2));
                    counter++;
                    break;

                case (byte)Instructions.DIV:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 / operand_2));
                    counter++;
                    break;

                case (byte)Instructions.MUL:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 * operand_2));
                    counter++;
                    break;

                case (byte)Instructions.MOD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 % operand_2));
                    counter++;
                    break;

                case (byte)Instructions.EXP:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)Math.Pow(operand_1, operand_2));
                    counter++;
                    break;

                case (byte)Instructions.LT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 < operand_2 ? 1 : 0));
                    counter++;
                    break;

                case (byte)Instructions.GT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 > operand_2 ? 1 : 0));
                    counter++;
                    break;

                case (byte)Instructions.EQ:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 == operand_2 ? 1 : 0));
                    counter++;
                    break;

                case (byte)Instructions.NEG:
                    stack.Push((byte)-stack.Pop());
                    counter++;
                    break;

                case (byte)Instructions.NOT:
                    stack.Push((byte)~stack.Pop());
                    counter++;
                    break;

                case (byte)Instructions.OR:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 | operand_2));
                    counter++;
                    break;

                case (byte)Instructions.AND:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 & operand_2));
                    counter++;
                    break;

                case (byte)Instructions.LS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 << operand_2));
                    counter++;
                    break;

                case (byte)Instructions.RS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((byte)(operand_1 << operand_2));
                    counter++;
                    break;

                case (byte)Instructions.LOAD: 
                    byte offset = stack.Pop();
                    byte index = (byte)(frame.head + offset);

                    stack.Push(stack.ElementAt(index));
                    counter++;
                    break;

                case (byte)Instructions.STORE:
                    offset = stack.Pop();
                    byte val = stack.Pop();
                    offset = (byte)(frame.head + offset);

                    memory[stack.ElementAt(offset)] = val;
                    counter++;
                    break;

                case (byte)Instructions.GLOAD:
                    index = stack.Pop();
                    val = memory[index];

                    stack.Push(val);
                    counter++;
                    break;

                case (byte)Instructions.GSTORE:
                    index = stack.Pop();
                    val = stack.Pop();

                    memory[index] = val;
                    counter++;
                    break;

                case (byte)Instructions.JUMP:
                    byte destination = stack.Pop();

                    counter = destination;
                    break;

                case (byte)Instructions.CJUMP:
                    byte condition = stack.Pop();
                    destination = stack.Pop();

                    counter = condition != 0 ? destination : counter;
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

        frame.StackLogger(50);

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
    }
}