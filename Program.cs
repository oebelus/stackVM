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

            // var mn = Mnemonics.Mnemonic("PUSH 0x5 PUSH 0x2 STORE");

            // VirtualMachine vm = new(mn);

            // vm.Execute();

            // vm.Logger();

            List<Function> list = Parse.ParseFunctions("Main: \nAdd 5, 7;  \nAdd: x, y -> x + y;\nSub: x, y -> x - y\nSome_gibberish: x, y, z -> x + y - z * z");

            Console.WriteLine(Parse.ToByteCode("Main: \n\tAdd 5, 7\n\tSub 4, 9  \n\tAdd: x, y -> x + y;\nSub: x, y -> x - y\nSome_gibberish: x, y, z -> x + y - z * z"));

            // PUSH 5 PUSH 7 CALL _ HALT ADD RET SUB RET 

        }
    }
}

/*
Main: 
    Add 5, 7
    Sub 4, 9
    
Add: x, y -> x + y;
SUB: x, y -> x - y;
*/

