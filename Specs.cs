record Specs()
{
    public int MEMORY_SIZE = 1024;
    public int STACK_SIZE = 256;
    public int WORD_SIZE = 4;
    public int STACK_FRAMES_SIZE = 16;
    public int CALL_STACK_SIZE = 16;
    public int HEAP_INDEX { get; set; } = 0;
}