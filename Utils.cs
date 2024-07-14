class Utils
{
    public static byte ToUint32(byte[] arr)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(arr);

        return (byte)BitConverter.ToUInt32(arr, 0);
    }

    public static byte[] Mnemonic(string mnemo)
    {
        string[] arr = mnemo.Split(" ");
        List<byte> buffer = [];
        int length = arr.Length;

        for (int i = 0; i < length; i++)
        {
            var val = arr[i];
            if (Program.instruction.TryGetValue(val, out int value))
            {
                buffer.Add((byte)value);
            }
            else
            {
                string[] c = arr[i].Split("x");
                int opp = 5 - c[1].Length;
                string hex = string.Concat(Enumerable.Repeat('0', opp)) + c[1];
                byte[] bytes = new byte[5];

                int j = 0;
                while (j < 4)
                {
                    buffer.Add(Convert.ToByte(hex.Substring(j, 2), 16));
                    j++;
                }
            }
        }

        return [.. buffer];
    }
}