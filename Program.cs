namespace vm
{
    class Program
    {
        public static void Main()
        {
            byte[] program = [0, 0, 0, 0, 25,
            0, 0, 0, 0, 25,
            ];

            var mn = Mnemonics.Mnemonic("PUSH 0 PUSH 0 GSTORE CZ8BS: PUSH 100 PUSH 0 GLOAD LT PUSH 0 EQ JUMP <PJTPO> PUSH 1 PUSH 0 GLOAD ADD PUSH 0 GSTORE JUMP <CZ8BS> PJTPO: HALT");

            /*
            PUSH 0
            PUSH 1
            JUMP ADDRESS
            PUSH 23
            PUSH 32
            ADD 
            ADDRESS: 
            ADD
            HALT
            */

            // PUSH 10 PUSH 10 EQ PUSH 23 CJUMP PUSH 1 HALT PUSH 0 HALT");

            // PUSH 10 PUSH 5 CALL <div> PUSH 7 PUSH 23 CALL <add> HALT div: DIV RET add: ADD RET

            foreach (var item in mn)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();

            List<string> toMnemo = Utils.ByteCodeToMnemonics(mn);

            foreach (var item in toMnemo)
            {
                Console.WriteLine(item);
            }

            VirtualMachine vm = new(mn);

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

/* CODES:

Equality: PUSH 5 PUSH 5 EQ PUSH 23 CJUMP PUSH 0 HALT PUSH 1 HALT
Even: PUSH 2 PUSH 7 MOD PUSH 23 CJUMP PUSH 1 HALT PUSH 0 HALT
Max: PUSH 23 PUSH 7 PUSH 4 PUSH 69 CALL <func> CALL <func> CALL <func> HALT </func> PUSH 0 STORE PUSH 1 STORE PUSH 0 LOAD PUSH 1 LOAD GT PUSH 74 CJUMP PUSH 0 LOAD RET PUSH 1 LOAD RET
Min: PUSH 23 PUSH 7 PUSH 4 PUSH 0 CALL <func> CALL <func> CALL <func> HALT </func> PUSH 0 STORE PUSH 1 STORE PUSH 0 LOAD PUSH 1 LOAD GT PUSH 74 CJUMP PUSH 1 LOAD RET PUSH 0 LOAD RET


- indexing: 

    string[] arr = code.Split(" ");
    int length = arr.Length;
    int inc = 0;

    for (int i = 0; i < length; i++)
    {
        if (Instruction.instruction.TryGetValue(arr[i].ToString(), out int _)) inc++;
        else if (int.TryParse(arr[i], out int _)) inc += 4;
        else if (arr[i].StartsWith("</")) inc += 1;
    }
            
*/
