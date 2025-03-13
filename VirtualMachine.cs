using Instructions = vm.Instructions;

class VirtualMachine(byte[] program)
{
    static readonly Specs VMSpecs = new();
    private readonly byte[] memory = new byte[VMSpecs.MEMORY_SIZE];
    private readonly Stack<int> stack = new(VMSpecs.STACK_SIZE);
    private readonly Stack<int[]> stackFrames = new(VMSpecs.STACK_FRAMES_SIZE);
    private readonly Stack<int> call_stack = new(VMSpecs.CALL_STACK_SIZE);
    private readonly byte[] program = program;
    private int counter = 0;
    private Dictionary<int, int> Allocated = [];

    public VirtualMachine Execute()
    {
        int operand_1;
        int operand_2;

        stackFrames.Push(new int[32]);

        while (counter < program.Length)
        {
            byte instruction = program[counter];

            switch ((Instructions)instruction)
            {
                case Instructions.PUSH:
                    int value = ByteManipulation.ToUint32([.. program.Skip(counter + 1).Take(4)]);

                    stack.Push(value);

                    counter += 5;
                    break;

                case Instructions.PUSH_CHAR:
                    byte[] c = program[(counter + 1)..(counter + 5)];

                    stack.Push(ByteManipulation.DeserializeChar(c));

                    counter += 5;
                    break;

                case Instructions.PUSH_STR:
                    int pointer = ByteManipulation.ToUint32(program[counter..(counter + 4)]);
                    stack.Push(pointer);

                    counter += 5;

                    int size = ByteManipulation.ToUint32(program[counter..(counter + 4)]);
                    stack.Push(size);

                    counter += 4;
                    break;

                case Instructions.CONCAT:
                    int size_2 = stack.Pop();
                    int pointer_2 = stack.Pop();

                    byte[] str_2 = memory[pointer_2..(pointer_2 + size_2)];

                    int size_1 = stack.Pop();
                    int pointer_1 = stack.Pop();

                    byte[] str_1 = memory[pointer_1..(pointer_1 + size_1)];

                    string concat = ByteManipulation.DeserializeString(str_1) + ByteManipulation.DeserializeString(str_2);

                    byte[] bytes_str = ByteManipulation.SerializeString(concat);

                    int ptr = VMSpecs.HEAP_INDEX;

                    // Console.WriteLine($"pointer to the heap: {ptr}");

                    foreach (var b in bytes_str)
                    {
                        if (ptr < VMSpecs.MEMORY_SIZE)
                        {
                            memory[ptr] = b;
                            ptr++;
                        }
                    }

                    Allocated.Add(VMSpecs.HEAP_INDEX, ptr);

                    VMSpecs.HEAP_INDEX = ptr + 1;

                    counter += 17;
                    break;

                case Instructions.POP:
                    counter++;
                    break;

                case Instructions.ADD:
                    operand_1 = stack.Pop();
                    operand_2 = stack.Pop();

                    try
                    {
                        stack.Push(operand_1 + operand_2);
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

                    stack.Push(operand_1 - operand_2);
                    counter++;
                    break;

                case Instructions.DIV:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

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
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    counter++;
                    break;

                case Instructions.EXP:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push((int)Math.Pow(operand_1, operand_2));
                    counter++;
                    break;

                case Instructions.LT:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(operand_1 < operand_2 ? 1 : 0);
                    counter++;
                    break;

                case Instructions.GT:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

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
                    if (stack.Pop() != 0)
                        stack.Push(0);
                    else stack.Push(1);

                    counter++;
                    break;

                case Instructions.OR:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(operand_1 | operand_2);
                    counter++;
                    break;

                case Instructions.AND:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(operand_1 & operand_2);
                    counter++;
                    break;

                case Instructions.LS:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(operand_1 << operand_2);
                    counter++;
                    break;

                case Instructions.RS:
                    operand_2 = stack.Pop();
                    operand_1 = stack.Pop();

                    stack.Push(operand_1 >> operand_2);
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

                case Instructions.GSTORE_STR:
                    size = stack.Pop();
                    index = stack.Pop();

                    if (index % 4 != 0)
                    {
                        throw new Exception("Invalid memory address");
                    }

                    if (VMSpecs.HEAP_INDEX + size > VMSpecs.MEMORY_SIZE)
                    {
                        throw new Exception("Out of memory");
                    }

                    while (index < VMSpecs.HEAP_INDEX)
                    {
                        index += 4;
                    }

                    // Console.WriteLine($"\nIndex of GSTORE_STR: {index}");

                    counter++;

                    byte[] str = program[counter..(counter + size)];

                    foreach (var s in str)
                    {
                        memory[index] = s;
                        index++;
                    }

                    int increment = index % 4 == 0 ? index : index + (4 - index % 4);

                    // Console.WriteLine($"\nAllocation Index: {index}\nHeap Index: {VMSpecs.HEAP_INDEX}\nSize: {size}\nIncrement: {increment}\nNew Heap Index: {increment}\n");

                    counter += size;
                    VMSpecs.HEAP_INDEX += increment;

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

                    byte[] val_bytes = BitConverter.GetBytes(val);

                    foreach (byte b in val_bytes)
                    {
                        memory[index] = b;
                        index++;
                    }

                    counter++;
                    break;

                case Instructions.JUMP:
                    int destination = stack.Pop();

                    counter = destination;
                    break;

                case Instructions.CJUMP:
                    destination = stack.Pop();
                    int condition = stack.Pop();

                    counter = (int)(condition != 0 ? destination : counter + 1);
                    break;

                case Instructions.CALL:
                    call_stack.Push(counter + 5);
                    stackFrames.Push(new int[32]);
                    destination = ByteManipulation.ToUint32([.. program.Skip(counter + 1).Take(4)]);

                    counter = destination;
                    break;

                case Instructions.RET:
                    try
                    {
                        index = call_stack.Pop();
                        stackFrames.Pop();
                        counter = index;
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

        stack.StackLogger(50);

        Console.Write("]\n\n");

        Console.WriteLine($"Head: {stack.head}\n");

        Console.Write("MEMORY:\n\n[ ");

        int count = 0;

        while (count < 200)
        {
            Console.Write(memory[count] + " ");
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

                int[] el = stackFrames.ElementAt(count);

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

    // public static Stack<string> StackToLoggable(Stack<int> stack)
    // {
    //     return stack.Map(x =>
    //     {
    //         return x switch
    //         {
    //             int num => num.ToString(),
    //             char str => str.Value.ToString(),
    //             Char chr => chr.Value.ToString(),
    //             _ => 0.ToString(),
    //         };
    //     });
    // }

    // public static string[] ArrToLoggable(IValue[] arr)
    // {
    //     return arr.Select(x =>
    //     {
    //         return x switch
    //         {
    //             Number num => num.Value.ToString(),
    //             String str => str.Value.ToString(),
    //             _ => 0.ToString(),
    //         };
    //     }).ToArray();
    // }
}