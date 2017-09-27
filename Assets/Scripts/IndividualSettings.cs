 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndividualSettings {

	public Dictionary<string,bool> actionFunction;



	public IndividualSettings(){
		Simulation boss = GameObject.Find ("Boss Object").GetComponent<Simulation> ();
		actionFunction = new Dictionary<string, bool> ();
		foreach (Configuration c in boss.actionsConfig) {
			actionFunction.Add(c.className,true);
            foreach (KeyValuePair<string, Dictionary<string, string>> kp in boss.modifiersMapping)
            {
                List<Configuration> mappedActions = boss.modifiersConfig[kp.Key];
                if (mappedActions.Exists(x => x.className == c.className))
                {
                    foreach (string s in kp.Value.Keys)
                    {
                        actionFunction.Add(c.className + s, true);
                    }
                }
            }
		}

	}

	public void setAll(bool set){
		List<string> akeys = new List<string>(actionFunction.Keys);
		foreach (string k1 in akeys )
			actionFunction[k1] = set;
	}
}
