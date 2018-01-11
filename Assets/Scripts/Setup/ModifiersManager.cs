using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifiersManager : MonoBehaviour {

	public Transform contentActions;
	public Transform contentModifiers;
	public GameObject itemList;

	private AnnotationSetupManager annotationSetupManager;


	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();

		List<string> actionList = annotationSetupManager.ActionList;
		List<string> modifierList = annotationSetupManager.ModifierList;

		foreach (string action in actionList) {
			GameObject actionGO = GameObject.Instantiate (itemList);
			Button currentButton = actionGO.GetComponent<Button> ();
			actionGO.GetComponent<SampleItemButton> ().SetItemListText (action);
			actionGO.transform.SetParent (contentActions);
		}

		foreach (string modifier in modifierList) {
			GameObject modifierGO = GameObject.Instantiate (itemList);
			Button currentButton = modifierGO.GetComponent<Button> ();
			modifierGO.GetComponent<SampleItemButton> ().SetItemListText (modifier);
			modifierGO.transform.SetParent (contentModifiers);
		}

		//foreach(string action in 
	}
	
	public void OnClickNext(){
		// TODO: this showed be done by the EventSystem
		annotationSetupManager.ModifiersSetupPanel.SetActive (false);
		annotationSetupManager.OutputSetupPanel.SetActive(true);
	}
}
