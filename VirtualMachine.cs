using System.Numerics;

class VirtualMachine(byte[] program)
{
    private readonly int[] memory = new int[1024];
    private readonly Stack<int> stack = new(1024);
    private readonly Stack<int[]> stackFrames = new(1024);
    private readonly Stack<int> call_stack = new(1024);
    private readonly byte[] program = program;
    private int counter = 0;

    public VirtualMachine Execute()
    {
        int operand_1;
        int operand_2;

        while (counter < program.Length)
        {
            var instruction = (Opcodes)program[counter];

            switch (instruction)
            {
                case Opcodes.PUSH:
                    byte result = Utils.ToUint32(program.Skip(counter + 1).Take(4).ToArray());
                    stack.Push(result);
                    counter += 5;
                    break;

                case Opcodes.POP:
                    counter++;
                    break;

                case Opcodes.ADD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 + operand_2);
                    counter++;
                    break;

                case Opcodes.SUB:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 - operand_2));
                    counter++;
                    break;

                case Opcodes.DIV:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 / operand_2));
                    counter++;
                    break;

                case Opcodes.MUL:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 * operand_2));
                    counter++;
                    break;

                case Opcodes.MOD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 % operand_2));
                    counter++;
                    break;

                case Opcodes.EXP:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((int)Math.Pow(operand_1, operand_2));
                    counter++;
                    break;

                case Opcodes.LT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 < operand_2 ? 1 : 0));
                    counter++;
                    break;

                case Opcodes.GT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 > operand_2 ? 1 : 0));
                    counter++;
                    break;

                case Opcodes.EQ:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 == operand_2 ? 1 : 0));
                    counter++;
                    break;

                case Opcodes.NEG:
                    stack.Push(-stack.Pop());
                    counter++;
                    break;

                case Opcodes.NOT:
                    stack.Push(~stack.Pop());
                    counter++;
                    break;

                case Opcodes.OR:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 | operand_2));
                    counter++;
                    break;

                case Opcodes.AND:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 & operand_2));
                    counter++;
                    break;

                case Opcodes.LS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 << operand_2));
                    counter++;
                    break;

                case Opcodes.RS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((operand_1 << operand_2));
                    counter++;
                    break;

                case Opcodes.LOAD:
                    int index = stack.Pop();

                    int[] arr = stackFrames.Pook();

                    stack.Push(arr[index]);

                    counter++;
                    break;

                case Opcodes.STORE:
                    index = stack.Pop();
                    int val = stack.Pop();

                    arr = stackFrames.Pook();

                    arr[index] = val;

                    counter++;
                    break;


                case Opcodes.GLOAD:
                    index = stack.Pop();
                    val = memory[index];

                    stack.Push(val);
                    counter++;
                    break;

                case Opcodes.GSTORE:
                    index = stack.Pop();
                    val = stack.Pop();

                    memory[index] = val;
                    counter++;
                    break;

                case Opcodes.JUMP:
                    int destination = stack.Pop();

                    counter = destination;
                    break;

                case Opcodes.CJUMP:
                    int condition = stack.Pop();
                    destination = stack.Pop();

                    counter = condition != 0 ? destination : (int)(counter + 1);
                    break;

                case Opcodes.CALL:

                    call_stack.Push((int)(counter + 5));
                    stackFrames.Push(new int[32]);
                    destination = Utils.ToUint32(program.Skip(counter + 1).Take(4).ToArray());

                    counter = destination;
                    break;

                case Opcodes.RET:
                    index = call_stack.Pop();
                    stackFrames.Pop();
                    counter = index;
                    break;

                case Opcodes.HALT:
                    counter = program.Length;
                    break;
            }
        }
        return this;
    }

    public void Logger()
    {
        Console.Write($"\nSTACK:\n\n[ ");

        stack.StackLogger(50);

        Console.Write("]\n\n");

        Console.WriteLine($"Head: {stack.head}\n");

        Console.Write("MEMORY:\n\n[ ");

        int count = 0;
        while (count < 50)
        {
            Console.Write(memory[count] + " ");
            count++;
        }

        Console.Write("]\n\n");

        Console.Write("STACK FRAME:\n\n[\n");

        for(int i = 0; i < stackFrames.head; i++)
        {
            Console.Write("[ ");
            for(int j = 0; j < 32; j++)
            {
                Console.Write(stackFrames.ElementAt(i)[j] + " ");
            }
            Console.Write("]\n");

        }



        Console.Write("]\n\n");

        Console.WriteLine($"Frame Pointer: {stackFrames.head}\n");

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
}