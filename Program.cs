namespace vm
{
    class Program
    {
        public static void Main()
        {
            // byte[] program = [0x0, 0x0, 0x0, 0x0, 0x5,
            // 0x0, 0x0, 0x0, 0x0, 0x5,
            // 0x18, 0x0, 0x0, 0x0, 0x1A,
            // 0x0, 0x0, 0x0, 0x0, 0x0B,
            // 0x18, 0x0, 0x0, 0x0, 0x1B,
            // 0x1A,
            // 0x02,
            // 0x19,
            // 0x03,
            // 0x19,
            // ];

            var mn = Mnemonics.Mnemonic("PUSH 0x5 PUSH 0x2 STORE");

            VirtualMachine vm = new(mn);

            vm.Execute();

            vm.Logger();

            List<string> list = Parse.ParseFunction("Main: \nAdd 5, 7  \nAdd: x, y -> x + y;");

            foreach (var val in list)
            {
                Console.WriteLine(val);
            }

        }
    }
}

/*
Main: 
    Add 5, 7;
    
Add: x, y -> x + y;
*/

