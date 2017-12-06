using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputManager : MonoBehaviour {

	public List<string> Modifiers { get; set; }
	public List<string> Output { get; set; }
	public GameObject inputFieldModifier;
	public Transform contentPanelModifiers;
	public GameObject inputFieldOutput;
	public Transform contentPanelOutput;

	private AnnotationSetupManager annotationSetupManager;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();

		Modifiers = new List<string> ();
		Output = new List<string> ();
	}
	
	public void AddNewModifier(){
		GameObject newInputFieldModifierGO = GameObject.Instantiate (inputFieldModifier);
		InputField currentInputField = newInputFieldModifierGO.GetComponent<InputField> ();
		currentInputField.onEndEdit.AddListener (delegate {EditedInputFieldModifier (); });
		newInputFieldModifierGO.transform.SetParent (contentPanelModifiers);
	}

	void EditedInputFieldModifier(){
	
	}

	public void DeleteNewModifier(){
	
	}

	public void AddNewOutput(){
		GameObject newInputFieldOutputGO = GameObject.Instantiate (inputFieldOutput);
		InputField currentInputField = newInputFieldOutputGO.GetComponent<InputField> ();
		currentInputField.onEndEdit.AddListener (delegate {EditedInputFieldOutput (); });
		newInputFieldOutputGO.transform.SetParent (contentPanelOutput);
	}

	void EditedInputFieldOutput(){

	}

	public void DeleteNewOutput(){


	}

	public void OnClickNext(){
		// TODO: this showed be done by the EventSystem
		annotationSetupManager.ModifiersSetupPanel.SetActive (false);
		annotationSetupManager.OutputSetupPanel.SetActive(true);
	}
	

}
