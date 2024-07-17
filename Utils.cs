class Utils
{
    public static byte ToUint32(byte[] arr)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(arr);

        return (byte)BitConverter.ToUInt32(arr, 0);
    }

    public static string NumberToHex(string nbr)
    {
        int opp = 8 - nbr.Length;
        string hex = string.Concat(Enumerable.Repeat('0', opp)) + nbr;

        return hex;
    }
}