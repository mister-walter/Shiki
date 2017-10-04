using System.Collections.Generic;
using Nett;
using System.IO;

public class StringLoader {
    private static StringLoader loader;
    private Dictionary<string, string> strings;

    public static StringLoader Instance() {
        if(loader == null) {
            loader = new StringLoader();
        }
        return loader;
    }

    public StringLoader() {
        strings = new Dictionary<string, string>();
    }

    public void LoadStrings(Stream stream) {
        var table = Toml.ReadStream(stream);
        foreach(var key in table.Keys) {
            var value = table.Get<TomlString>(key).Value;
            strings.Add(key, value);
        }
    }

    public string GetString(string key) {
        if(this.strings.Count == 0) {
            throw new System.Exception("Strings file not yet loaded!");
        }
        return strings[key];
    }
}
