class Mnemonics
{
    public static byte[] Mnemonic(string mnemo)
    {
        string[] arr = mnemo.Split(' ');
        List<byte> buffer = [];
        Dictionary<string, int> functions = [];
        int length = arr.Length;

        for (int i = 0; i < length; i++)
        {
            var val = arr[i];

            if (Instruction.instruction.TryGetValue(val, out int value))
            {
                if (value == 23 || value == 22)
                {
                    int inc = GetIndex(i + 1, length, arr, arr[i + 1]);
                    int jumps = 1;
                    int cjumps = 1;

                    for (int j = i + 1; j < length; j++)
                    {
                        if (arr[j] == "JUMP") jumps++;
                        else if (arr[j] == "CJUMP") cjumps++;
                    }

                    byte[] nbrArray = Utils.ToByteArray((inc + buffer.Count + jumps + 1).ToString()); // 1 for PUSH

                    buffer.Add(0);

                    foreach (var item in nbrArray)
                    {
                        buffer.Add(item);
                    }
                    buffer.Add((byte)value);

                    i++;
                }
                else buffer.Add((byte)value);
            }
            else if (val[0] == '<')
            {
                string function = val;

                if (!functions.ContainsKey(function))
                {
                    if (val[1] == '/') continue;
                    int k = i;
                    int inc = GetIndex(k, length, arr, val);
                    functions.Add(function, inc + buffer.Count);
                }

                // adding index
                byte[] nbrArray = Utils.ToByteArray(functions[function].ToString());

                foreach (var item in nbrArray)
                {
                    buffer.Add(item);
                }
            }
            else if (!int.TryParse(val, out int _))
            {
                continue;
            }
            else
            {
                byte[] nbrArray = Utils.ToByteArray(arr[i]);

                foreach (var item in nbrArray)
                {
                    buffer.Add(item);
                }
            }
        }

        return [.. buffer];
    }

    private static int GetIndex(int k, int length, string[] arr, string val)
    {
        int inc = 0;

        while (k < length/* && val.Length > 1 && arr[k][2..] != val[1..]*/)
        {
            if (Instruction.instruction.ContainsKey(arr[k].Trim()))
            {
                inc += 1;
                // Console.WriteLine($"{arr[k]}, {inc}, {k}");
            }
            else if (arr[k].Length > 1 && !int.TryParse(arr[k], out _))
            {
                if (arr[k][2..] != val[1..] && !arr[k].EndsWith(':'))
                {
                    inc += 4;
                    // Console.WriteLine($"{arr[k]}, {inc}, {k}");
                }
                else if (arr[k].EndsWith(':') && arr[k][0..(arr[k].Length - 2)] == val[1..(val.Length - 2)])
                {
                    break;
                }
            }
            else
            {
                inc += 4;
                // Console.WriteLine($"{arr[k]}, {inc}, {k}");
            }
            k++;
        }

        return inc;
    }
}