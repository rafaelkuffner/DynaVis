using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OutputManager : MonoBehaviour {

	public GameObject inputFieldModifier;
	public Transform contentPanelModifiers;
	public GameObject inputFieldOutput;
	public Transform contentPanelOutput;
	public List<string> ModifiersList { get; set; }
	public List<string> OutputList { get; set; }

	private AnnotationSetupManager annotationSetupManager;
	private GameObject currentModifierGO;
	private GameObject currentOutputGO;
	private string currentModifier;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		ModifiersList = new List<string> ();
		OutputList = new List<string> ();
	}
	
	public void AddNewItem(){
		GameObject newInputFieldModifierGO = GameObject.Instantiate (inputFieldModifier);
		InputField modifierInputField = newInputFieldModifierGO.GetComponent<InputField> ();
		modifierInputField.onEndEdit.AddListener (delegate {EditedInputFieldModifier (); });
		newInputFieldModifierGO.transform.SetParent (contentPanelModifiers);

		EventTrigger triggerModifierInputField = modifierInputField.GetComponent<EventTrigger>();
		EventTrigger.Entry entryModifierInputField = new EventTrigger.Entry();
		entryModifierInputField.eventID = EventTriggerType.PointerExit;
		entryModifierInputField.callback.AddListener((data) => { OnPointerExitModifierInputField((PointerEventData)data); });
		triggerModifierInputField.triggers.Add(entryModifierInputField);

		GameObject newInputFieldOutputGO = GameObject.Instantiate (inputFieldOutput);
		InputField outputInputField = newInputFieldOutputGO.GetComponent<InputField> ();
		outputInputField.onEndEdit.AddListener (delegate {EditedInputFieldOutput (); });
		newInputFieldOutputGO.transform.SetParent (contentPanelOutput);

		EventTrigger triggerOutputInputField = outputInputField.GetComponent<EventTrigger>();
		EventTrigger.Entry entryOutputInputField = new EventTrigger.Entry();
		entryOutputInputField.eventID = EventTriggerType.PointerExit;
		entryOutputInputField.callback.AddListener((data) => { OnPointerExitOutputInputField((PointerEventData)data); });
		triggerOutputInputField.triggers.Add(entryOutputInputField);
	}

	public void DeleteItem(){


	}

	public void OnPointerExitModifierInputField(PointerEventData data)
	{
		currentModifierGO = data.selectedObject;
	}

	public void OnPointerExitOutputInputField(PointerEventData data)
	{
		currentOutputGO = data.selectedObject;
	}

	void EditedInputFieldModifier(){
		InputField currentModifier = currentModifierGO.GetComponent<InputField> ();
		ModifiersList.Add (currentModifier.text);
	}

	void EditedInputFieldOutput(){
		InputField currentOutput = currentOutputGO.GetComponent<InputField> ();
		OutputList.Add (currentOutput.text);
	}

	public void OnClickNext(){
		// TODO: this showed be done by the EventSystem
		annotationSetupManager.OutputSetupPanel.SetActive (false);
		annotationSetupManager.ParameterSetupPanel.SetActive(true);
	}
	

}
