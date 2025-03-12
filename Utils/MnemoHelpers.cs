using vm;

class MnemoHelpers
{
    public static List<string> ByteCodeToMnemonics(byte[] bytecode)
    {
        List<string> mnemonic = [];
        int length = bytecode.Length;

        foreach (var item in bytecode)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();

        int i = 0;
        while (i < length)
        {
            if (bytecode[i] == 0 || bytecode[i] == 24)
            {
                mnemonic.Add($"{i}: {Instruction.vInstruction.FirstOrDefault(x => x.Value == bytecode[i]).Key} {ByteManipulation.ToUint32(bytecode[i..(i + 5)])}");
                i += 5;
                continue;
            }
            else if (bytecode[i] == 27)
            {
                byte instruction = bytecode[i];
                int step = i;

                int len = ByteManipulation.ToUint32(bytecode[i..(i + 5)]);
                i += 5;

                // Console.WriteLine($"length: {len}");

                string str = ByteManipulation.DeserializeString([.. bytecode[i..(i + 4)].Where(x => x != 0)]);
                i += 4;

                // Console.WriteLine($"length: {str}");

                mnemonic.Add($"{step}: {Instruction.vInstruction.FirstOrDefault(x => x.Value == instruction).Key} {str} of size {len}");
            }
            else if (bytecode[i] == 28)
            {
                byte instruction = bytecode[i];
                i++;

                char c = ByteManipulation.DeserializeChar(bytecode[i..(i + 4)]);
                i += 4;

                mnemonic.Add($"{i}: {Instruction.vInstruction.FirstOrDefault(x => x.Value == instruction).Key} {c}");
            }
            else if (bytecode[i] == 30)
            {
                byte instruction = bytecode[i];
                int size = bytecode[i - 1];
                i++;

                string s = ByteManipulation.DeserializeString(bytecode[i..(i + size)]);
                i += size;

                mnemonic.Add($"{i}: {Instruction.vInstruction.FirstOrDefault(x => x.Value == instruction).Key} {s}");
            }
            else
            {
                mnemonic.Add($"{i}: {Instruction.vInstruction.FirstOrDefault(x => x.Value == bytecode[i]).Key}");
                i++;
            }
        }

        return mnemonic;
    }

    public static int GetIndex(string bytecode)
    {
        string[] arr = bytecode.Split(" ");
        int length = arr.Length;
        int inc = 0;

        for (int i = 0; i < length; i++)
        {
            if (Instruction.vInstruction.TryGetValue(arr[i].ToString(), out int _)) inc++;
            else if (int.TryParse(arr[i], out int _)) inc += 4;
            else if (arr[i].StartsWith("</")) inc += 1;
        }

        return inc;
    }
}