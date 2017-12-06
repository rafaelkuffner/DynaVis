using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AnnotationSetupManager : MonoBehaviour {

	public string setupFilename;
	public List<string> SetupTiers { get; set; }

	Simulation boss;
	Dictionary<string, List<string>> newTiersConfig;
	List<SetupData> setupDataList;

	//DELETE
	public GameObject TierSetupPanel { get; set; }
	public GameObject ActionSetupPanel { get; set; }
	public GameObject SpriteSetupPanel { get; set; }
	public GameObject ModifiersSetupPanel { get; set; }
	public GameObject OutputSetupPanel { get; set; }
	public GameObject ParameterSetupPanel { get; set; }

	public Simulation GetBoss(){
		return boss;
	}

	public void SetNewTierConfig(Dictionary<string, List<string>> tiersConfig){
		newTiersConfig = tiersConfig;
	}

	public Dictionary<string, List<string>> GetNewTiersConfig(){
		return newTiersConfig;
	}

	void loadFile(){
		StreamReader sr = new StreamReader (setupFilename);
		//Skipping header;
		string line = sr.ReadLine ();
		char[] delim = {','};
		List<Modifier> modifs = new List<Modifier>();
		while((line = sr.ReadLine()) != null)
		{
			string[] components = line.Split(delim);
			if (components.Length == 5) {
				int startTime;
				int endTime;
				if (int.TryParse (components [2], out startTime) && int.TryParse (components [3], out endTime)) {
					SetupData setupData = new SetupData (components [0], components [1], startTime, endTime, components [4]);
					setupDataList.Add (setupData);

					// Debug
					//setupData.DebugSetupData();
				} 
				else {
					Debug.Log ("ERROR READING SETUP FILE: INCORRECT START TIME OR END TIME");
					break;
				}
			} 
			else {
				Debug.Log ("ERROR READING SETUP FILE: WRONG NUMBER OF PARAMENTERS");
				break;
			}

		}

	}

	void GetUniqueSetupTiers(){
		
		foreach (SetupData setupData in setupDataList) {
			if (!SetupTiers.Contains (setupData.Tier)) {
				SetupTiers.Add (setupData.Tier);
				Debug.Log ("Tier = " + setupData.Tier);
			}
		}
	}

	// Use this for initialization
	void Awake () {
		boss = GameObject.Find ("Boss Object").GetComponent<Simulation> ();

		TierSetupPanel = GameObject.Find ("TierSetupPanel");

		ActionSetupPanel = GameObject.Find ("ActionSetupPanel");
		ActionSetupPanel.SetActive(false);

		SpriteSetupPanel = GameObject.Find ("SpritesSetupPanel");
		SpriteSetupPanel.SetActive (false);

		ModifiersSetupPanel = GameObject.Find ("ModifierSetupPanel");
		ModifiersSetupPanel.SetActive (false);

		OutputSetupPanel = GameObject.Find ("OutputSetupPanel");
		OutputSetupPanel.SetActive (false);

		ParameterSetupPanel = GameObject.Find ("ParameterSetupPanel");
		ParameterSetupPanel.SetActive (false);

		setupDataList = new List<SetupData> ();
		SetupTiers = new List<string> ();
		loadFile ();
		GetUniqueSetupTiers ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
