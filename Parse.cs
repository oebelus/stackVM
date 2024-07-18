class Parse
{
    readonly static string[] separators = ["->", ":"];

    public static List<string> ParseFunction(string str)
    {
        Dictionary<string, List<int>> function = [];
        List<string> functions = [];
        int length = str.Length;

        string subs = "";

        for (int i = 0; i < str.Length; i++) {
            if (str[i] == ':') {
                int j = i - 1;
                while (j >= 0 && str[j] != '\n') {
                    subs = str[j] + subs;
                    j--;
                }

                functions.Add(subs.Trim());

                if (!subs.Equals("main", StringComparison.CurrentCultureIgnoreCase)) {
                    string func = "";

                    int k = i + 1;
                    while (str[k] != '\n' && k + 1 < length) {
                        func += str[k];
                        k++;
                    }

                    string[] funcTokens = func.Split("->", StringSplitOptions.RemoveEmptyEntries);
                    int arguments = funcTokens[0].Split(',').Length;
                    int operation = '0'; 
                     
                    Console.WriteLine(funcTokens[1].Length);
                    //Console.WriteLine("op: " + operation);

                    foreach (var item in funcTokens)
                    {
                        Console.WriteLine(item);
                    }

                }
            }
            
            subs = "";
        }


        return functions;
    }
}

/*

Main: 
    Add 5, 7  
    
Add: x, y -> x + y;

*/