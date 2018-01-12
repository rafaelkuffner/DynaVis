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

	private AnnotationSetupManager annotationSetupManager;
	private GameObject currentModifierGO;
	private GameObject currentActionGO;
	private int currentModifierIndex;
	private List<Transform> contentModifiersItemList;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();

		List<string> actionList = annotationSetupManager.ActionList;
		List<string> modifierList = annotationSetupManager.ModifierList;
		contentModifiersItemList = new List<Transform> ();
		ActionsByModifier = new Dictionary<string, List<string>> ();

		foreach (string action in actionList) {
			GameObject actionGO = GameObject.Instantiate (itemList);
			Button currentButton = actionGO.GetComponent<Button> ();
			actionGO.GetComponent<SampleItemButton> ().SetItemListText (action);
			actionGO.transform.SetParent (contentActions);

			EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickActionButton((PointerEventData)data); });
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
		RedrawContentModifiers ();
	}

	public void OnPointerClickModifierButton(PointerEventData data){
		currentModifierGO = data.selectedObject;
	}

	public void RedrawContentModifiers(){

		contentModifiersItemList.Clear ();
		int numberOfItems = contentModifiers.childCount;
		for (int i = 0; i < numberOfItems; i++) {
			contentModifiersItemList.Add (contentModifiers.GetChild (i));
			//Destroy (contentActions.GetChild (i));
		}

		contentModifiersItemList.Insert (++currentModifierIndex, currentActionGO.transform);

		contentModifiers.DetachChildren ();
		foreach (Transform transform in contentModifiersItemList)
			transform.SetParent (contentModifiers);

		currentModifierIndex = 0;
	}
	
	public void OnClickNext(){
		// TODO: this showed be done by the EventSystem
		annotationSetupManager.ModifiersSetupPanel.SetActive (false);
		annotationSetupManager.OutputSetupPanel.SetActive(true);
	}
}
