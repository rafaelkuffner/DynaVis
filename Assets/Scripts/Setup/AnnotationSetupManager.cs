using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationSetupManager : MonoBehaviour {

	Simulation boss;
	Dictionary<string, List<string>> newTiersConfig;

	//DELETE
	GameObject actionSetupPanel;

	public Simulation GetBoss(){
		return boss;
	}

	public void SetNewTierConfig(Dictionary<string, List<string>> tiersConfig){
		newTiersConfig = tiersConfig;
	}

	public Dictionary<string, List<string>> GetNewTiersConfig(){
		return newTiersConfig;
	}

	public GameObject GetActionSetupPanel(){
		return actionSetupPanel;
	}

	// Use this for initialization
	void Awake () {
		boss = GameObject.Find ("Boss Object").GetComponent<Simulation> ();

		actionSetupPanel = GameObject.Find ("ActionSetupPanel");
		actionSetupPanel.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
