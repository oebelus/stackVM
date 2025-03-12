namespace vm
{
    class Program
    {
        public static void Main()
        {
            // var mn = Mnemonics.Mnemonic("PUSH 5 PUSH 0 STORE E71DN: PUSH 1 PUSH 0 EQ CJUMP <CLWQ4> PUSH 0 LOAD PUSH 2 MOD PUSH 0 EQ CJUMP <NGGVE> JUMP <IYUPO> NGGVE: PUSH 0 LOAD PUSH 1 ADD PUSH 0 STORE JUMP <E71DN> IYUPO: PUSH 0 LOAD PUSH 10 LT NOT CJUMP <WAINV> JUMP <OMCTI> WAINV: JUMP <CLWQ4> OMCTI: PUSH 0 LOAD PUSH 0 LOAD PUSH 1 ADD PUSH 0 STORE JUMP <E71DN> CLWQ4: HALT");
            int l = ByteManipulation.SerializeString("Hello, my name is Imane").Length;
            int r = ByteManipulation.SerializeString("I am a String").Length;

            var mn = Mnemonics.Mnemonic($"PUSH 32 PUSH {l} GSTORE_STR \"Hello, my name is Imane\" PUSH 64 PUSH {r} GSTORE_STR \"I am a String\"");

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