public class VirtualMachine(byte[] program)
{
    private readonly int[] memory = new int[1024];
    private readonly Stack<int> stack = new(1024);
    private readonly Stack<int[]> stackFrames = new(16);
    private readonly Stack<int> call_stack = new(16);
    private readonly byte[] program = program;
    private int counter = 0;

    public VirtualMachine Execute()
    {
        int operand_1;
        int operand_2;

        while (counter < program.Length)
        {
            byte instruction = program[counter];

            switch ((Instructions)instruction)
            {
                case Instructions.PUSH:
                    int result = Utils.ToUint32(program.Skip(counter + 1).Take(4).ToArray());
                    stack.Push(result);
                    counter += 5;
                    break;

                case Instructions.POP:
                    counter++;
                    break;

                case Instructions.ADD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 + operand_2);
                    counter++;
                    break;

                case Instructions.SUB:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 - operand_2);
                    counter++;
                    break;

                case Instructions.DIV:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 / operand_2);
                    counter++;
                    break;

                case Instructions.MUL:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 * operand_2);
                    counter++;
                    break;

                case Instructions.MOD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 % operand_2);
                    counter++;
                    break;

                case Instructions.EXP:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push((int)Math.Pow(operand_1, operand_2));
                    counter++;
                    break;

                case Instructions.LT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 < operand_2 ? 1 : 0);
                    counter++;
                    break;

                case Instructions.GT:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 > operand_2 ? 1 : 0);
                    counter++;
                    break;

                case Instructions.EQ:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 == operand_2 ? 1 : 0);
                    counter++;
                    break;

                case Instructions.NEG:
                    stack.Push(-stack.Pop());
                    counter++;
                    break;

                case Instructions.NOT:
                    stack.Push(~stack.Pop());
                    counter++;
                    break;

                case Instructions.OR:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 | operand_2);
                    counter++;
                    break;

                case Instructions.AND:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 & operand_2);
                    counter++;
                    break;

                case Instructions.LS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 << operand_2);
                    counter++;
                    break;

                case Instructions.RS:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    stack.Push(operand_1 << operand_2);
                    counter++;
                    break;

                case Instructions.LOAD:
                    int index = stack.Pop();

                    int[] arr = stackFrames.Pook();

                    stack.Push(arr[index]);

                    counter++;
                    break;

                case Instructions.STORE:
                    index = stack.Pop();
                    int val = stack.Pop();

                    arr = stackFrames.Pook();

                    arr[index] = val;

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
                    int destination = stack.Pop();

                    counter = destination;
                    break;

                case Instructions.CJUMP:
                    destination = stack.Pop();
                    int condition = stack.Pop();

                    counter = condition != 0 ? destination : counter + 1;
                    break;

                case Instructions.CALL:
                    call_stack.Push(counter + 5);
                    stackFrames.Push(new int[32]);
                    destination = Utils.ToUint32(program.Skip(counter + 1).Take(4).ToArray());

                    counter = destination;
                    break;

                case Instructions.RET:
                    index = call_stack.Pop();
                    stackFrames.Pop();
                    counter = index;

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

        if (stackFrames.head > 0)
        {
            Console.Write("STACK FRAME:\n\n[ ");

            var el = stackFrames.ElementAt(0);

            foreach (var item in el)
                Console.Write(item + " ");

            Console.Write("]\n\n");

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
}