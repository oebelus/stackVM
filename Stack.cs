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
        while (count < n)
        {
            Console.Write(stack[count] + " ");
            count++;
        }
    }

    public T ElementAt(int index) {
        return stack[index];
    }
}