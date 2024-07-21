namespace vm
{
    class Program
    {
        public static void Main()
        {
            // byte[] program = [0, 0, 0, 0, 25,
            // 0, 0, 0, 0, 25,
            // ];

            var mn = Mnemonics.Mnemonic("PUSH 5 PUSH 7 CALL <add> PUSH 4 PUSH 8 CALL <sub> HALT </add> ADD RET </sub> SUB RET");

            // VirtualMachine vm = new(mn);

            // vm.Execute();

            // vm.Logger();

            string code = @"Main:
    Add 5, 7
    Sub 4, 8
Add: x, y -> x + y
Sub: x, y -> x - y
            ";

            List<Function> list = Parse.ParseFunctions("Main: \n  \nAdd: x, y -> x + y;");

            // "Main: \n\tAdd 5, 7\n\tSub 4, 8\nAdd: x, y -> x + y\nSub: x, y -> x - y"

            string program = Parse.ToByteCode(code);

            Console.WriteLine(program);

            var mne = Mnemonics.Mnemonic(program);

            VirtualMachine vm = new(mne);

            vm.Execute();

            vm.Logger();

        }
    }
}

/*
Main: 
    Add 5, 7
    Sub 4, 9
    
Add: x, y -> x + y;
Sub: x, y -> x - y;
*/

