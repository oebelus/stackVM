namespace vm
{
    class Program
    {
        public static void Main()
        {
            byte[] program = [0x0, 0x0, 0x0, 0x0, 0x5,
            0x0, 0x0, 0x0, 0x0, 0x5,
            0x18, 0x0, 0x0, 0x0, 0x1A,
            0x0, 0x0, 0x0, 0x0, 0x0B,
            0x18, 0x0, 0x0, 0x0, 0x1B,
            0x1A,
            0x02,
            0x19,
            0x03,
            0x19,
            ];

            /*
                [ 5 5 11 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ]

                [ 0 0 0 0 5 0 0 0 0 5 24 0 0 0 26 0 0 0 0 11 24 0 0 0 27 26 2 25 3 25 ] 
                
                [ 0 0 0 0 5 0 0 0 0 5 24 0 0 0 26 0 0 0 0 11 24 0 0 0 27 26 2 25 3 25 ]
            */

            //var mn = Utils.Mnemonic("PUSH 0x5 PUSH 0x5 CALL <sub> HALT <sub> SUB RET");
            var mn = Utils.Mnemonic("PUSH 5 PUSH 5 CALL <add> PUSH 11 CALL <sub> HALT </add> ADD RET </sub> SUB RET");
            //                                      11    4    5   9  10    14   15                 

            /*
                
            */

            /*
                0 0 0 0 5 0 0 0 0 5 24 0 0 0 26 0 0 0 0 10 24 0 0 0 32 26 3 25 2 25
                PUSH 5    PUSH 5    CALL 26     PUSH 10    CALL 32   HALT SUB RET SUB ADD
            */

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


