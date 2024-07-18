class Parse
{
    readonly static string[] separators = ["->", ":"];

    public static Dictionary<string, string> ParseFunction(string str)
    {
        string[] parsed = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, string> function = [];

        function.Add("name", parsed[0].Trim());
        function.Add("args", parsed[1].Trim());
        function.Add("body", parsed[2].Trim());

        return function;
    }
}

/*
Add: x, y -> x + y;
fct: ____ -> _____;
*/

/*

separators , -> : 
EOL ;

*/