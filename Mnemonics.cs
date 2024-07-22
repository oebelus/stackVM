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
                    int k = i;
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
                            else break;
                        }
                        else
                        {
                            inc += 4;
                            // Console.WriteLine($"{arr[k]}, {inc}, {k}");
                        }
                        k++;
                    }

                    functions.Add(function, inc + buffer.Count);
                }

                string index = Utils.NumberToHex(functions[function].ToString());
                int z = 0;
                while (z < 8)
                {
                    buffer.Add((byte)int.Parse(index.Substring(z, 2)));
                    z += 2;
                }
            }
            else
            {
                string[] c = arr[i].Split("x");
                if (c.Length > 0)
                {
                    string nbr = c.Length == 1 ? c[0] : c[1];
                    string hex = Utils.NumberToHex(nbr);

                    int j = 0;
                    while (j < 8)
                    {
                        buffer.Add((byte)int.Parse(hex.Substring(j, 2)));
                        j += 2;
                    }
                }
            }
        }

        return [.. buffer];
    }
}