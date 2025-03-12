using System.Text;

class ByteManipulation
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

    public static byte[] SerializeString(string str)
    {
        List<byte> strBytes = [.. Encoding.UTF8.GetBytes(str)];

        // if (strBytes.Count > 4)
        // {
        //     strBytes.AddRange(strBytes[..3]);
        // }
        // else
        // {
        //     int zeros = 4 - strBytes.Count;

        //     for (int i = 0; i < zeros; i++)
        //     {
        //         strBytes.Insert(0, 0);
        //     }


        //     foreach (var item in strBytes)
        //     {
        //         Console.WriteLine(item);
        //     }
        // }

        return [.. strBytes];
    }

    public static string DeserializeString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    public static char DeserializeChar(byte[] bytes)
    {
        return (char)int.Parse(string.Join("", bytes.Where(x => x != 0)));
    }
}