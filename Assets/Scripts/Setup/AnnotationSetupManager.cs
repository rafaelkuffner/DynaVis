using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class AnnotationSetupManager : MonoBehaviour {

	public string setupFilename;
	public List<string> SetupTiers { get; set; }

	//DELETE
	public GameObject TierSetupPanel { get; set; }
	public GameObject ActionSetupPanel { get; set; }
	public GameObject SpriteSetupPanel { get; set; }
	public GameObject ModifiersSetupPanel { get; set; }
	public GameObject OutputSetupPanel { get; set; }
	public GameObject ParameterSetupPanel { get; set; }

	private Simulation boss;
	private Dictionary<string, List<string>> newTiersConfig;
	private List<SetupData> setupDataList;
	private SpriteManager spriteManager;

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

		spriteManager = SpriteSetupPanel.GetComponent<SpriteManager> ();

		loadFile ();
		GetUniqueSetupTiers ();

	}


	public void WriteXMLFile(){
	
		XmlDocument xmlDoc = new XmlDocument();
		XmlNode rootNode = xmlDoc.CreateElement("Definitions");
		xmlDoc.AppendChild(rootNode);

		XmlNode tiersNode = xmlDoc.CreateElement("Tiers");
		rootNode.AppendChild (tiersNode);

		foreach (string newTier in newTiersConfig.Keys) {
			XmlNode newTierNode = xmlDoc.CreateElement("Tier");
			XmlAttribute attribute = xmlDoc.CreateAttribute("name");
			attribute.Value = newTier;
			newTierNode.Attributes.Append(attribute);
			tiersNode.AppendChild (newTierNode);
			foreach (string tierString in newTiersConfig[newTier]) {
				XmlNode newTierString = xmlDoc.CreateElement("TierString");
				newTierString.InnerText = tierString;
				newTierNode.AppendChild (newTierString);
			}
		}

		XmlNode actionsNode = xmlDoc.CreateElement("Actions");
		rootNode.AppendChild (actionsNode);

		XmlNode parametersNode = xmlDoc.CreateElement("Parameters");
		actionsNode.AppendChild (parametersNode);
		Dictionary<string, string> newSpriteTranslateTable = spriteManager.NewSpriteTranslationTable;
		foreach (string newSprinteName in newSpriteTranslateTable.Keys) {
			XmlNode parameterNode = xmlDoc.CreateElement("Parameter");
			XmlAttribute attributeInput = xmlDoc.CreateAttribute("input");
			attributeInput.Value = newSpriteTranslateTable[newSprinteName];
			parameterNode.Attributes.Append (attributeInput);
			XmlAttribute attributeOutput = xmlDoc.CreateAttribute("output");
			attributeOutput.Value = newSprinteName;
			parameterNode.Attributes.Append (attributeOutput);
			parametersNode.AppendChild (parameterNode);
		}

		xmlDoc.Save("newSetup.xml");
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
