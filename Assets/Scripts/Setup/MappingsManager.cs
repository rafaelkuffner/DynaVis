using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MappingsManager : MonoBehaviour {

	public GameObject inputFieldMappingInput;
	public Transform contentPanelMappingInput;
	public GameObject inputFieldMappingOutput;
	public Transform contentPanelMappingOutput;
	public Dictionary<string, string> MappingInputOutput { get; set; }
	//public List<string> MappingInputList { get; set; }
	///public List<string> MappingOutputList { get; set; }

	private SetupButton setup;
	private GameObject currentMappingInputGO = null;
	private GameObject currentMappingOutputGO = null;
	private string currentModifier;

	// Use this for initialization
	void Start () {
        setup = GameObject.Find("Button Setup").GetComponent<SetupButton>();
		MappingInputOutput = new Dictionary<string, string> ();
		//MappingInputList = new List<string> ();
		//MappingOutputList = new List<string> ();
	}
	
	public void AddNewItem(){
		GameObject newInputFieldModifierGO = GameObject.Instantiate (inputFieldMappingInput);
		InputField modifierInputField = newInputFieldModifierGO.GetComponent<InputField> ();
		modifierInputField.onEndEdit.AddListener (delegate {EditedInputFieldModifier (); });
		newInputFieldModifierGO.transform.SetParent (contentPanelMappingInput);

		EventTrigger triggerModifierInputField = modifierInputField.GetComponent<EventTrigger>();
		EventTrigger.Entry entryModifierInputField = new EventTrigger.Entry();
		entryModifierInputField.eventID = EventTriggerType.PointerExit;
		entryModifierInputField.callback.AddListener((data) => { OnPointerExitModifierInputField((PointerEventData)data); });
		triggerModifierInputField.triggers.Add(entryModifierInputField);

		GameObject newInputFieldOutputGO = GameObject.Instantiate (inputFieldMappingOutput);
		InputField outputInputField = newInputFieldOutputGO.GetComponent<InputField> ();
		outputInputField.onEndEdit.AddListener (delegate {EditedInputFieldOutput (); });
		newInputFieldOutputGO.transform.SetParent (contentPanelMappingOutput);

		EventTrigger triggerOutputInputField = outputInputField.GetComponent<EventTrigger>();
		EventTrigger.Entry entryOutputInputField = new EventTrigger.Entry();
		entryOutputInputField.eventID = EventTriggerType.PointerExit;
		entryOutputInputField.callback.AddListener((data) => { OnPointerExitOutputInputField((PointerEventData)data); });
		triggerOutputInputField.triggers.Add(entryOutputInputField);

		//AddMapping ();
	}

	public void DeleteItem(){


	}

	public void OnPointerExitModifierInputField(PointerEventData data)
	{
		currentMappingInputGO = data.selectedObject;
	}

	public void OnPointerExitOutputInputField(PointerEventData data)
	{
		currentMappingOutputGO = data.selectedObject;
	}

	void EditedInputFieldModifier(){
		InputField currentModifier = currentMappingInputGO.GetComponent<InputField> ();

		if (!MappingInputOutput.ContainsKey (currentModifier.text))
			MappingInputOutput.Add (currentModifier.text, "");
		
	}

	void EditedInputFieldOutput(){
		InputField currentOutput = currentMappingOutputGO.GetComponent<InputField> ();

		if(MappingInputOutput.ContainsKey(currentMappingInputGO.GetComponent<InputField> ().text))
			MappingInputOutput[currentMappingInputGO.GetComponent<InputField> ().text] = currentOutput.text;
		//MappingOutputList.Add (currentOutput.text); 
	}

	public void AddMapping(){

		if (currentMappingInputGO != null && currentMappingOutputGO != null) {
			string inputText = currentMappingInputGO.GetComponent<InputField> ().text;
			string outputText = currentMappingOutputGO.GetComponent<InputField> ().text;

			if (!MappingInputOutput.ContainsKey (inputText)) {
				MappingInputOutput.Add (inputText, outputText);
			} 
			else {
				MappingInputOutput [inputText] = outputText;
			}
		}
	}

	public void OnClickNext(){
		// TODO: this showed be done by the EventSystem
		setup.MappingsSetupPanel.SetActive (false);
		setup.ModifiersSetupPanel.SetActive(true);
	}
	

}
