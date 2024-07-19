class Parse
{
    readonly static string[] separators = ["->", ":"];
    readonly static char[] binaryOperations = ['=', '-', '/', '*', '%', '/', '&', '|'];

    public static List<Function> ParseFunctions(string str)
    {
        List<Function> functions = [];
        int length = str.Length;

        string subs = "";

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ':')
            {
                int j = i - 1;
                while (j >= 0 && str[j] != '\n')
                {
                    subs = str[j] + subs;
                    j--;
                }

                if (!subs.Equals("main", StringComparison.CurrentCultureIgnoreCase))
                {
                    string func = "";

                    int k = i + 1;
                    while (str[k] != '\n' && k + 1 < length)
                    {
                        func += str[k];
                        k++;
                    }

                    string[] funcTokens = func.Split("->", StringSplitOptions.RemoveEmptyEntries);

                    int arguments = funcTokens[0].Split(',').Length;

                    List<char> operations = [];

                    foreach (var item in funcTokens[1])
                    {
                        if (item != ' ' && binaryOperations.Contains(item))
                        {
                            operations.Add(item);
                        }
                    }

                    functions.Add(new Function([.. operations], arguments, subs.Trim()));
                }
            }

            subs = "";
        }


        return functions;
    }

    public static string ToByteCode(string code)
    {
        string bytecode = "";
        List<Function> functions = ParseFunctions(code);

        int index = code.IndexOf("Main") + 5;
        string[] arr = code.Split("\n\t");
        string[] mainBody = arr[1..(arr.Length - 1)];

        foreach (var item in mainBody)
        {
            string[] init = item.Split(' ');

            if (init.Length > 0)
            {
                Function function = functions.Where(function => function.Name == init[0].Trim()).ToArray()[0];
                Console.WriteLine(function.Name);

                string[] args = string.Join(' ', init[1..]).Split(",").Where(x => x.Trim() != "").ToArray();

                int length = string.Join(' ', init[1..]).Split(",").Where(x => x.Trim() != "").ToArray().Length;

                if (function.Arguments == length)
                {
                    foreach (var op in args)
                    {
                        bytecode += " PUSH " + op.Trim();
                    }
                    bytecode += " CALL " + function.Name.Trim();
                }
            }

        }

        return bytecode.Trim();
    }
}

/*

Main: 
    Add 5, 7  
    
Add: x, y -> x + y;

*/