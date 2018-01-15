using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ParameterManager : MonoBehaviour {

	public Transform contentPanelMappings;
	public Transform contentPanelParameters;
	public GameObject dropdownItem;
	public GameObject itemPrefab;
	public Vector3 contentMappingsPosition;
	public Button buttonNext;

	private AnnotationSetupManager annotationSetupManager;
	private GameObject currentDropdownGO;
	private Dictionary<string, List<string>> tiersByModifiableAction;
	private List<string> actions;
	private string currentAction;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		tiersByModifiableAction = annotationSetupManager.ModifiableActionManager.TiersByModifiableAction;

		MappingsManager mappingsManager = annotationSetupManager.MappingsSetupPanel.GetComponent<MappingsManager> ();
		List<string> mappingsOutputManager = new List<string>(mappingsManager.MappingInputOutput.Values);

		contentMappingsPosition = contentPanelMappings.position;

		actions = new List<string>(tiersByModifiableAction.Keys);

		if (actions.Count > 0) {
			currentAction = actions [0];

			List<string> tiers = tiersByModifiableAction [currentAction];
			List<string> parameterList = annotationSetupManager.getParametersByTierString (tiers);

			foreach (string param in parameterList) {
				GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
				Button currentButton = spriteNameGO.GetComponent<Button> ();
				spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (param);
				spriteNameGO.transform.SetParent (contentPanelParameters);
			}
			actions.RemoveAt (0);

		}
		dropdownItem.GetComponent<Dropdown> ().ClearOptions ();
		dropdownItem.GetComponent<Dropdown> ().AddOptions (mappingsOutputManager);
	}

	private void LoadContent(){
		contentPanelMappings.DetachChildren();
		contentPanelParameters.DetachChildren();
		contentPanelMappings.position = contentMappingsPosition;

		GameObject[] contentListDetached = GameObject.FindGameObjectsWithTag ("SampleListItem") as GameObject[];
		for (int i = 0; i < contentListDetached.Length; i++) {
			Destroy(contentListDetached[i]);
		}

		if (actions.Count <= 0)
			return;

		currentAction = actions [0];
		List<string> tiers = tiersByModifiableAction [currentAction];
		List<string> parameterList = annotationSetupManager.getParametersByTierString (tiers);

		foreach (string param in parameterList) {
			GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
			Button currentButton = spriteNameGO.GetComponent<Button> ();
			spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (param);
			spriteNameGO.transform.SetParent (contentPanelParameters);

			EventTrigger trigger = currentButton.GetComponent<EventTrigger> ();
			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener ((data) => {
				OnPointerClickButton ((PointerEventData)data);
			});
			trigger.triggers.Add (entry);
		}
		actions.RemoveAt (0);
	} 

	public void ClickedAddNewModifier(){
		GameObject newDropdowGO = GameObject.Instantiate (dropdownItem);
		Dropdown currentInputField = newDropdowGO.GetComponent<Dropdown> ();
		newDropdowGO.transform.SetParent (contentPanelMappings);

		EventTrigger trigger = newDropdowGO.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => { OnPointerClickDropdown((PointerEventData)data); });
		trigger.triggers.Add(entry);

		EventTrigger trigger2 = newDropdowGO.GetComponent<EventTrigger>();
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.Select;
		entry2.callback.AddListener((data) => { OnSelect((PointerEventData)data); });
		trigger2.triggers.Add(entry);
	
	}

	public void ClickedDeleteModifier(){
		if (currentDropdownGO != null)
			Destroy (currentDropdownGO);
	}

	public void OnPointerClickDropdown(PointerEventData data)
	{
		currentDropdownGO = data.selectedObject;

	}

	public void OnPointerClickButton(PointerEventData data)
	{
		
	
	}

	public void OnSelect(PointerEventData data)
	{
		currentDropdownGO = data.selectedObject;
		Dropdown currentDropdown = currentDropdownGO.GetComponent<Dropdown> ();
		Debug.Log("dropbox = " + currentDropdown.captionText.text.ToString ());

	}


	public void OnClickNextAction(){
		
		LoadContent ();
		if (actions.Count == 0) {
			buttonNext.GetComponentInChildren<Text> ().text = "Exit";
			buttonNext.onClick.AddListener(OnClickExit);
		}
	}

	public void OnClickExit(){

		GameObject[] contentItemDetached = GameObject.FindGameObjectsWithTag ("SampleItem") as GameObject[];
		for (int i = 0; i < contentItemDetached.Length; i++) {
			Destroy(contentItemDetached[i]);
		}

		annotationSetupManager.WriteXMLFile ();
		annotationSetupManager.ParameterSetupPanel.SetActive (false);
	}
}
