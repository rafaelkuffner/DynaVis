using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModifiersManager : MonoBehaviour {

	public Transform contentActions;
	public Transform contentModifiers;
	public GameObject itemList;
	public Dictionary<string, List<string>> ActionsByModifier { get; set; }
	public Dictionary<string, List<string>> MappingsByModifier { get; set; }

	private SetupButton setup;
	private GameObject currentModifierGO;
	private GameObject currentActionGO;
	GameObject currentMappingGO;
	private int currentModifierIndex;
	private List<Transform> contentModifiersItemList;

	// Use this for initialization
	void Start () {
        setup = GameObject.Find("Button Setup").GetComponent<SetupButton>();

		List<Action> actionList = new List<Action> ();
		foreach (Action a in setup.ActionList) 
		{
			if (a is ColorChangeableAction) {
				actionList.Add (a);
			}
		}
			
		List<string> modifierList = setup.ModifierList;
		contentModifiersItemList = new List<Transform> ();
		ActionsByModifier = new Dictionary<string, List<string>> ();
		MappingsByModifier = new Dictionary<string, List<string>> ();

		// Add button to separate actions from mappings
		GameObject actionSeparatorGO = GameObject.Instantiate(itemList);
		Button buttonAction = actionSeparatorGO.GetComponent<Button> ();
		actionSeparatorGO.GetComponent<SampleItemButton> ().SetItemListText ("ACTION");
		actionSeparatorGO.transform.SetParent (contentActions);

		// ACTIONS
		foreach (Action action in actionList) {
			GameObject actionGO = GameObject.Instantiate (itemList);
			Button currentButton = actionGO.GetComponent<Button> ();
			actionGO.GetComponent<SampleItemButton> ().SetItemListText (action.GetClassName());
			actionGO.transform.SetParent (contentActions);

			EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickActionButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}

		GameObject mappingSeparatorGO = GameObject.Instantiate(itemList);
		Button buttonMapping = mappingSeparatorGO.GetComponent<Button> ();
		mappingSeparatorGO.GetComponent<SampleItemButton> ().SetItemListText ("MAPPING");
		mappingSeparatorGO.transform.SetParent (contentActions);

		//MAPPINGS
		List<string> mappings = new List<string>(setup.MappingsManager.MappingInputOutput.Keys);
		foreach (string mapping in mappings) {
			GameObject mappingGO = GameObject.Instantiate (itemList);
			Button currentButton = mappingGO.GetComponent<Button> ();
			mappingGO.GetComponent<SampleItemButton> ().SetItemListText (mapping);
			mappingGO.transform.SetParent (contentActions);

			EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickMappingButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}

		foreach (string modifier in modifierList) {
			GameObject modifierGO = GameObject.Instantiate (itemList);
			Button currentButton = modifierGO.GetComponent<Button> ();
			modifierGO.GetComponent<SampleItemButton> ().SetItemListText (modifier);
			modifierGO.transform.SetParent (contentModifiers);

			EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickModifierButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}
	}

	public void OnPointerClickMappingButton(PointerEventData data){
		currentMappingGO = data.selectedObject;
		//MappingsByModifier
	
		string currentModifierName = currentModifierGO.GetComponent<SampleItemButton> ().GetItemListText ();
		string currentMappingName = currentMappingGO.GetComponent<SampleItemButton> ().GetItemListText ();

		int numberOfItems = contentModifiers.childCount;
		for (int i = 0; i < numberOfItems; i++) {
			Transform item = contentModifiers.GetChild (i);
			if (item.GetComponent<SampleItemButton> ().GetItemListText() == currentModifierName) {
				currentModifierIndex = i;
				break;
			}
		}

		if (MappingsByModifier.ContainsKey (currentModifierName))
			MappingsByModifier [currentModifierName].Add (currentMappingName);
		else {
			MappingsByModifier.Add (currentModifierName, new List<string> ());
			MappingsByModifier [currentModifierName].Add (currentMappingName);
		}
		RedrawContentModifiers (currentMappingGO);
	}

	public void OnPointerClickActionButton(PointerEventData data){
		currentActionGO = data.selectedObject;

		string currentModifierName = currentModifierGO.GetComponent<SampleItemButton> ().GetItemListText ();
		string currentActionName = currentActionGO.GetComponent<SampleItemButton> ().GetItemListText ();

		int numberOfItems = contentModifiers.childCount;
		for (int i = 0; i < numberOfItems; i++) {
			Transform item = contentModifiers.GetChild (i);
			if (item.GetComponent<SampleItemButton> ().GetItemListText() == currentModifierName) {
				currentModifierIndex = i;
				break;
			}
		}

		if (ActionsByModifier.ContainsKey (currentModifierName))
			ActionsByModifier [currentModifierName].Add (currentActionName);
		else {
			ActionsByModifier.Add (currentModifierName, new List<string> ());
			ActionsByModifier [currentModifierName].Add (currentActionName);
		}
		RedrawContentModifiers (currentActionGO);
	}

	public void OnPointerClickModifierButton(PointerEventData data){
		currentModifierGO = data.selectedObject;
	}

	public void RedrawContentModifiers(GameObject newItem){

		contentModifiersItemList.Clear ();
		int numberOfItems = contentModifiers.childCount;
		for (int i = 0; i < numberOfItems; i++) {
			contentModifiersItemList.Add (contentModifiers.GetChild (i));
			//Destroy (contentActions.GetChild (i));
		}

		contentModifiersItemList.Insert (++currentModifierIndex, newItem.transform);

		contentModifiers.DetachChildren ();
		foreach (Transform transform in contentModifiersItemList)
			transform.SetParent (contentModifiers);

		currentModifierIndex = 0;
	}
	
	public void OnClickNext(){
		// TODO: this showed be done by the EventSystem
		setup.ModifiersSetupPanel.SetActive (false);
		setup.ModifiableActionSetupPanel.SetActive(true);
	}
}
