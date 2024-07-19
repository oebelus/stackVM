namespace vm
{
    class Program
    {
        public static void Main()
        {
            byte[] program = [0, 0, 0, 0, 25,
            0, 0, 0, 0, 25,
            ];

            var mn = Mnemonics.Mnemonic("PUSH 25 PUSH 25 CALL");

            VirtualMachine vm = new(mn);

            vm.Execute();

            vm.Logger();

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

