class Mnemonics
{
    public static byte[] Mnemonic(string mnemo)
    {
        string[] arr = mnemo.Split(" ");
        List<byte> buffer = [];
        Dictionary<string, int> functions = [];
        int length = arr.Length;

        for (int i = 0; i < length; i++)
        {
            var val = arr[i];

            if (Instruction.instruction.TryGetValue(val, out int value))
            {
                buffer.Add((byte)value);
            }
            else if (val[0] == '<')
            {
                string function = val;

                if (!functions.ContainsKey(function))
                {
                    if (val[1] == '/') continue;
                    int k = i + 1;
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
                            if (arr[k][2..] != val[1..] && !arr[k].StartsWith("</"))
                            {
                                inc += 4;
                                // Console.WriteLine($"{arr[k]}, {inc}, {k}");
                            }
                        }
                        else
                        {
                            inc += 4;
                            // Console.WriteLine($"{arr[k]}, {inc}, {k}");
                        }
                        k++;
                    }

                    functions.Add(function, inc + buffer.Count + 2);
                }

                // adding index
                byte[] nbrArray = Utils.ToByteArray(functions[function].ToString());

                foreach (var item in nbrArray)
                {
                    buffer.Add(item);
                }
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
}