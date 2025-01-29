

class Stack<T>(int size)
{
    public int head = 0;
    private readonly T[] stack = new T[size];

    public void Push(T el)
    {
        stack[head] = el;
        head++;
    }

    public T Pop()
    {
        head--;
        T popped = stack[head];
        return popped;
    }

    public void Clear()
    {
        head = 0;
    }

    public bool IsEmpty()
    {
        return head == 0;
    }

    public void StackLogger(int n)
    {
        int count = 0;

        foreach (var item in stack)
        {
            Console.Write(item + " ");
            count++;
        }
        while (count < n)
        {
            Console.Write(0 + " ");
            count++;
        }
    }

    public T ElementAt(int index)
    {
        return stack[index];
    }

    public int Length()
    {
        return size;
    }

    public int Count()
    {
        return head;
    }

    public T Peek()
    {
        return stack[0];
    }

    public T Pook()
    {
        if (head == 0)
        {
            return stack[0];
        }

        return stack[head - 1];
    }

    public Stack<S> Map<S>(Func<T, S> Mapper)
    {
        Stack<S> result = new(head);

        for (int i = 0; i < head; i++)
        {
            result.Push(Mapper(stack[i]));
        }

        return result;
    }
}