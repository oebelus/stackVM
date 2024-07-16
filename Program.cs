namespace vm
{
    class Program
    {
        public static void Main()
        {
            byte[] program = [0x0, 0x0, 0x0, 0x0, 0x5,
            0x0, 0x0, 0x0, 0x0, 0x5,
            0x18, 0x0, 0x0, 0x0, 0x10,
            0x1A,
            0x02,
            0x19
            ];

            //var mn = Utils.Mnemonic("PUSH 0x5 PUSH 0x5 CALL <add> HALT <add> ADD RET");

            VirtualMachine vm = new(program);

            vm.Execute();

            vm.Logger();

        }
    }
}

/*

.main
    PUSH 5
    PUSH 5
    CALL

.add
    POP
    ADD
    RET


=> PUSH 0x5 PUSH 0x5 CALL ADD RET 
*/


