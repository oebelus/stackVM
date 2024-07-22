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
        Console.WriteLine(nbr);

        byte[] nbrArray = BitConverter.GetBytes(nbr);
        foreach (var item in nbrArray)
        {
            Console.WriteLine(item);
        }

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(nbrArray);
        }

        return nbrArray;
    }
}