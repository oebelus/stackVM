class Parse {
    readonly string[] separators = [",", "->", ","];
    
    public Dictionary<int, string> ParseFunction(string str) {
        int count = 0;
        List<string> parts = [];

        for (int i = 0; i < str.Length; i++) {
            if (separators.Contains(str[i].ToString())) {

            }
        }
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