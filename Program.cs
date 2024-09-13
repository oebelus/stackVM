namespace vm
{
    class Program
    {
        public static void Main()
        {
            var mn = Mnemonics.Mnemonic("PUSH 23 CALL <isPrime> HALT isPrime: PUSH 0 STORE PUSH 0 LOAD PUSH 2 LT CJUMP <953R2> PUSH 2 PUSH 1 STORE KO9DX: PUSH 1 LOAD PUSH 1 LOAD MUL PUSH 0 LOAD GT NOT PUSH 0 EQ CJUMP <0H0Z8> PUSH 0 LOAD PUSH 1 LOAD MOD PUSH 0 EQ CJUMP <2YR83> JUMP <D860S> 2YR83: PUSH 0 RET D860S: PUSH 1 LOAD PUSH 1 ADD PUSH 1 STORE JUMP <KO9DX> 0H0Z8: JUMP <L2K0Z> 953R2: PUSH 0 RET L2K0Z: PUSH 1 RET");

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
