using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class AnnotationSetupManager : MonoBehaviour {

	public string setupFilename;
	public List<string> SetupTiersList { get; set; }
	public List<string> ParametersList { get; set; }
	public Dictionary<string, List<string>> TiersByAction { get; set; }
	public List<string> ActionList { get; set; } // implemented actions: see folder Scripts>Actions
	public List<string> ModifierList { get; set;} // implemented modifiers: see folder Scripts>Actions>Modifiers

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
	private ModifiersManager modifierManager;

	public Simulation GetBoss(){
		return boss;
	}

	public void SetNewTierConfig(Dictionary<string, List<string>> tiersConfig){
		newTiersConfig = tiersConfig;
	}

	public Dictionary<string, List<string>> GetNewTiersConfig(){
		return newTiersConfig;
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
		SetupTiersList = new List<string> ();
		ParametersList = new List<string> ();

		ActionList = new List<string> ();
		ActionList.Add ("UpperBodyAction");
		ActionList.Add ("LowerBodyAction");
		ActionList.Add ("HeadFaceAction");
		ActionList.Add ("LocationAction");
		ActionList.Add ("GazeAction");

		ModifierList = new List<string> ();
		ModifierList.Add ("ColorModifier");

		spriteManager = SpriteSetupPanel.GetComponent<SpriteManager> ();
		modifierManager = ModifiersSetupPanel.GetComponent<ModifiersManager> ();

		loadFile ();
		GetUniqueSetupTiers ();
		GetUniqueParameters ();

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
			if (!SetupTiersList.Contains (setupData.Tier)) {
				SetupTiersList.Add (setupData.Tier);
				Debug.Log ("Tier = " + setupData.Tier);
			}
		}
		SetupTiersList.Sort ();
	}

	void GetUniqueParameters(){

		foreach (SetupData setupData in setupDataList) {
			if (!ParametersList.Contains (setupData.Parameter)) {
				ParametersList.Add (setupData.Parameter);
			}	
		}
		ParametersList.Sort ();
	}
		
	public List<string> getParametersByTierString(List<string> tiers){

		List<string> parametersByTier = new List<string> ();

		foreach (string tier in tiers) {
			List<string> tierStrings = newTiersConfig [tier];
			foreach (string tierString in tierStrings) {
				foreach (SetupData data in setupDataList) {
					if (tierString == data.Tier) {
						if (!parametersByTier.Contains (data.Parameter))
							parametersByTier.Add (data.Parameter);
					}
				}
			}
		}
		return parametersByTier;
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

		Dictionary<string, Dictionary<string, string>> newSpriteTranslateTableByAction = spriteManager.NewSpriteTranslationTableByAction;
		foreach (string action in TiersByAction.Keys) {
			List<string> tiers = TiersByAction [action];

			XmlNode actionNode = xmlDoc.CreateElement("Action");
			XmlAttribute attributeName = xmlDoc.CreateAttribute("name");
			attributeName.Value = action;
			actionNode.Attributes.Append (attributeName);
			actionsNode.AppendChild (actionNode);

			XmlNode applicableTiersNode = xmlDoc.CreateElement("ApplicableTiers");
			actionNode.AppendChild (applicableTiersNode);

			foreach (string tier in tiers) {
				XmlNode applicableTierNode = xmlDoc.CreateElement("ApplicableTier");
				applicableTierNode.InnerText = tier;
				applicableTiersNode.AppendChild (applicableTierNode);
			}

			XmlNode parametersNode = xmlDoc.CreateElement("Parameters");
			actionNode.AppendChild (parametersNode);
			Dictionary<string, string> newSpriteTranslateTable = newSpriteTranslateTableByAction [action];
			foreach (string oldSpriteName in newSpriteTranslateTable.Keys) {
				XmlNode parameterNode = xmlDoc.CreateElement ("Parameter");
				XmlAttribute attributeInput = xmlDoc.CreateAttribute ("input");
				attributeInput.Value = oldSpriteName;
				parameterNode.Attributes.Append (attributeInput);
				XmlAttribute attributeOutput = xmlDoc.CreateAttribute ("output");
				attributeOutput.Value = newSpriteTranslateTable[oldSpriteName];
				parameterNode.Attributes.Append (attributeOutput);
				parametersNode.AppendChild (parameterNode);
			}
		}

		XmlNode modifiersNode = xmlDoc.CreateElement("Modifiers");
		rootNode.AppendChild (modifiersNode);

		Dictionary<string, List<string>> actionsByModifiers = modifierManager.ActionsByModifier;
		foreach (string modifier in actionsByModifiers.Keys) {
			List<string> actions = actionsByModifiers[modifier];

			XmlNode modifierNode = xmlDoc.CreateElement("Modifier");
			modifiersNode.AppendChild (modifierNode);
			XmlAttribute attributeModifierName = xmlDoc.CreateAttribute ("name");
			attributeModifierName.Value = modifier;
			modifierNode.Attributes.Append (attributeModifierName);

			XmlNode modifiableActionsNode = xmlDoc.CreateElement("modifiableActions");
			modifierNode.AppendChild (modifiableActionsNode);
			foreach (string action in actions) {
				XmlNode modifiableActionNode = xmlDoc.CreateElement("ModifiableAction");
				modifiableActionsNode.AppendChild (modifiableActionNode);
				XmlAttribute attributeName = xmlDoc.CreateAttribute ("name");
				attributeName.Value = action;
				modifiableActionNode.Attributes.Append (attributeName);
			}
		}
		xmlDoc.Save("newSetup.xml");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
