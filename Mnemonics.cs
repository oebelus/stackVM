using System.Text;
using Instruction = vm.Instruction;

class Mnemonics
{
    public static Dictionary<string, int> MapAddress(string[] mnemonics)
    {
        Dictionary<string, int> addresses = [];
        int length = mnemonics.Length;

        for (int i = 0; i < length; i++)
        {
            if (mnemonics[i].StartsWith('<') && !addresses.ContainsKey(mnemonics[i][1..mnemonics[i].Length]))
            {
                int index = GetIndex(mnemonics, mnemonics[i]);
                addresses.TryAdd(mnemonics[i][1..(mnemonics[i].Length - 1)], index);
            }
        }
        return addresses;
    }
    public static byte[] Mnemonic(string mnemo)
    {
        string[] mnemonics = mnemo.Split(' ');
        Dictionary<string, int> addresses = MapAddress(mnemonics);
        List<byte> buffer = [];
        int length = mnemonics.Length;

        for (int i = 0; i < length; i++)
        {
            var val = mnemonics[i];

            if (Instruction.vInstruction.TryGetValue(val, out int value))
            {
                if (value == 23 || value == 22)
                {
                    byte[] nbrArray = Utils.ToByteArray(addresses[mnemonics[i + 1][1..(mnemonics[i + 1].Length - 1)]].ToString());

                    buffer.Add(0);

                    foreach (var item in nbrArray)
                    {
                        buffer.Add(item);
                    }
                    buffer.Add((byte)value);

                    i++;
                }
                else if (value == 27)
                {
                    buffer.Add(27);

                    int strSize;
                    string str;

                    try
                    {
                        strSize = int.Parse(mnemonics[i + 1]);
                        str = mnemonics[i + 2];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new Exception("Missing arguments for PUSH_STR_CONST");
                    }

                    i += 2;

                    byte[] nbrArray = Utils.ToByteArray(strSize.ToString());

                    foreach (var item in nbrArray)
                    {
                        buffer.Add(item);
                    }

                    int words = (int)(strSize % 4 == 0 ? strSize / 4 : Math.Ceiling((float)strSize / 4));

                    byte[] byteStr = Utils.SerializeString(str, words);

                    foreach (var item in byteStr)
                    {
                        buffer.Add(item);
                    }
                }
                else buffer.Add((byte)value);
            }
            else if (val[0] == '<')
            {
                int index = addresses[val[1..(val.Length - 1)]];

                byte[] nbrArray = Utils.ToByteArray(index.ToString());

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
                byte[] nbrArray = Utils.ToByteArray(mnemonics[i]);

                foreach (var item in nbrArray)
                {
                    buffer.Add(item);
                }
            }
        }

        return [.. buffer];
    }

    private static int GetIndex(string[] mnemonics, string value)
    {
        int inc = 0;
        int length = mnemonics.Length;

        for (int i = 0; i < length; i++)
        {
            string val = mnemonics[i];

            if (val.Length > 1 && val.EndsWith(':') && val[..(val.Length - 1)] == value[1..(value.Length - 1)])
            {
                return inc;
            }

            if (int.TryParse(val, out int _)) inc += 4;
            else if (Instruction.vInstruction.ContainsKey(val.Trim()))
            {
                if (val == "JUMP" || val == "CJUMP") inc += 5;
                if (val == "CALL") inc += 5;
                else inc += 1;
            }
        }
        return -1;
    }
}