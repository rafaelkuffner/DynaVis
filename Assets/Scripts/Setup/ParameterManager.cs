using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ParameterManager : MonoBehaviour {

	public Transform contentPanelModifiers;
	public Transform contentPanelParameters;
	public GameObject dropdownItem;
	public GameObject itemList;

	private AnnotationSetupManager annotationSetupManager;
	private GameObject currentDropdownGO;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();

		OutputManager outputManager = annotationSetupManager.OutputSetupPanel.GetComponent<OutputManager> ();
		List<string> modifierList = outputManager.ModifiersList;

		dropdownItem.GetComponent<Dropdown> ().ClearOptions ();
		dropdownItem.GetComponent<Dropdown> ().AddOptions (modifierList);

		List<string> parameters = annotationSetupManager.ParametersList;

		foreach (string parameter in parameters) {
			GameObject paramGO = GameObject.Instantiate (itemList);
			Button currentButton = paramGO.GetComponent<Button> ();
			paramGO.GetComponent<SampleItemButton> ().SetItemListText (parameter);
			paramGO.transform.SetParent (contentPanelParameters);
		}
	}


	public void ClickedAddNewModifier(){
		GameObject newDropdowGO = GameObject.Instantiate (dropdownItem);
		Dropdown currentInputField = newDropdowGO.GetComponent<Dropdown> ();
		newDropdowGO.transform.SetParent (contentPanelModifiers);

		EventTrigger trigger = newDropdowGO.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => { OnPointerClickDropdown((PointerEventData)data); });
		trigger.triggers.Add(entry);

		entry.eventID = EventTriggerType.Select;
		entry.callback.AddListener((data) => { OnSelect((PointerEventData)data); });
		trigger.triggers.Add(entry);
	
	}

	public void ClickedDeleteModifier(){
		if (currentDropdownGO != null)
			Destroy (currentDropdownGO);
	}

	public void OnPointerClickDropdown(PointerEventData data)
	{
		currentDropdownGO = data.selectedObject;
	
	}

	public void OnSelect(PointerEventData data)
	{
		currentDropdownGO = data.selectedObject;
		Dropdown currentDropdown = currentDropdownGO.GetComponent<Dropdown> ();
		Debug.Log("dropbox = " + currentDropdown.captionText.text.ToString ());

	}


	public void OnClickExit(){
		// TODO: this showed be done by the EventSystem
		annotationSetupManager.WriteXMLFile ();
		annotationSetupManager.ParameterSetupPanel.SetActive (false);
		//annotationSetupManager.ModifiersSetupPanel.SetActive(true);
	}
}
