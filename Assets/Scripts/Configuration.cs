using System.Collections;
using System.Collections.Generic;

public class Configuration  {

    public string className;
    public List<string> tiers;
    public Dictionary<string, string> translationTable;

    public Configuration(string className)
    {
        this.className = className;
		tiers = new List<string>();
        translationTable = new Dictionary<string, string>();   
    }

}
