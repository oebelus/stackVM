using System.Text;

namespace vm
{
    class Program
    {
        public static void Main()
        {
            byte[] program = [0x0, 0x0, 0x0, 0x0, 0x2,
            0x0, 0x0, 0x0, 0x0, 0x17,
            0x2,
            ];

            var mn = Utils.Mnemonic("PUSH 0x2 PUSH 0x2 ADD PUSH 0x23 ADD");

            VirtualMachine vm = new(mn);

            vm.Execute();

            vm.Logger();

        }
    }
}

// "PUSH 0x2 PUSH 0x2 ADD"


