using System.Collections;
using System.Collections.Generic;
using System;

public class Configuration  {

    public string className;
    public List<string> tiers;
    public Dictionary<Tuple<string,string>, string> translationTable;

    public Configuration(string className)
    {
        this.className = className;
		tiers = new List<string>();
        translationTable = new Dictionary<Tuple<string, string>, string>();   
    }

}
