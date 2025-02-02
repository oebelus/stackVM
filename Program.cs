namespace vm
{
    class Program
    {
        public static void Main()
        {
            // var mn = Mnemonics.Mnemonic("PUSH hello PUSH 0 STORE E71DN: PUSH 1 PUSH 0 EQ CJUMP <CLWQ4> PUSH 0 LOAD PUSH 2 MOD PUSH 0 EQ CJUMP <NGGVE> JUMP <IYUPO> NGGVE: PUSH 0 LOAD PUSH 1 ADD PUSH 0 STORE JUMP <E71DN> IYUPO: PUSH 0 LOAD PUSH 10 LT NOT CJUMP <WAINV> JUMP <OMCTI> WAINV: JUMP <CLWQ4> OMCTI: PUSH 0 LOAD PUSH 0 LOAD PUSH 1 ADD PUSH 0 STORE JUMP <E71DN> CLWQ4: HALT");

            var mn = Mnemonics.Mnemonic("PUSH_CHAR i PUSH_STR 2 ne CONCAT");

            Console.WriteLine();

            List<string> toMnemo = MnemoHelpers.ByteCodeToMnemonics(mn);

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
