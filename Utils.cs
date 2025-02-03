using Instruction = vm.Instruction;

class Utils
{
    public static int ToUint32(byte[] arr)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(arr);

        return (int)BitConverter.ToUInt32(arr, 0);
    }

    public static byte[] ToByteArray(string str)
    {
        int nbr = int.Parse(str);

        byte[] nbrArray = BitConverter.GetBytes(nbr);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(nbrArray);
        }

        return nbrArray;
    }

    public static List<string> ByteCodeToMnemonics(byte[] bytecode)
    {
        List<string> mnemonic = [];
        int length = bytecode.Length;

        int i = 0;
        while (i < length)
        {
            if (bytecode[i] == 0 || bytecode[i] == 24)
            {
                mnemonic.Add($"{i}: {Instruction.vInstruction.FirstOrDefault(x => x.Value == bytecode[i]).Key} {ToUint32(bytecode[i..(i + 5)])}");
                i += 5;
                continue;
            }

            mnemonic.Add($"{i}: {Instruction.vInstruction.FirstOrDefault(x => x.Value == bytecode[i]).Key}");
            i++;
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