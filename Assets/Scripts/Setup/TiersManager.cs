using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TiersManager : MonoBehaviour {

	public GameObject itemList; // SampleListItem Prefab
	public Transform contentPanelTierStrings;
	public GameObject inputField; //SampleInputField Prefab
	public Transform contentPanelNewTiers;

	private Dictionary<string, List<string>> newTiersConfig;
	private AnnotationSetupManager annotationSetupManager;
	private Dictionary<string, string> tiersConfig;
	private Dictionary<GameObject, List<GameObject>> itemListByInputField;
	private GameObject currentInputFieldGO;
	private GameObject currentButtonGO;


	// Use this for initialization
	void Start () {
		newTiersConfig = new Dictionary<string, List<string>> ();
		itemListByInputField = new Dictionary<GameObject, List<GameObject>> ();
		currentInputFieldGO = null;
		currentButtonGO = null;

		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		Simulation boss = annotationSetupManager.GetBoss ();
		tiersConfig = boss.tiersConfig;

		foreach (string t in tiersConfig.Keys) {
			GameObject tierStringGO = GameObject.Instantiate (itemList);
			Button currentButton = tierStringGO.GetComponent<Button> ();
			tierStringGO.GetComponent<SampleItemButton> ().SetItemListText (t);
			tierStringGO.transform.SetParent (contentPanelTierStrings);

			EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}
	}


	public void ClickedAddNewTier(){

		Debug.Log ("Add new tier");
		GameObject newInputFieldGO = GameObject.Instantiate (inputField);
		InputField currentInputField = newInputFieldGO.GetComponent<InputField> ();
		currentInputField.onEndEdit.AddListener (delegate {EditedInputField (); });
		newInputFieldGO.transform.SetParent (contentPanelNewTiers);
		itemListByInputField.Add (newInputFieldGO, new List<GameObject>());

		EventTrigger trigger = currentInputField.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => { OnPointerClickInputField((PointerEventData)data); });
		trigger.triggers.Add(entry);
	}

	public void DeleteNewTier(){
	
		// Delete new Tiers
		string selectedTier = currentInputFieldGO.GetComponent<InputField>().text.ToString();
		if (newTiersConfig.ContainsKey (selectedTier))
			newTiersConfig.Remove(selectedTier);

		// Delete GameObjects
		foreach (GameObject inputFieldGO in itemListByInputField.Keys) {
			if (inputFieldGO == currentInputFieldGO) {
				foreach (GameObject itemListGO in itemListByInputField[inputFieldGO])
					itemListGO.transform.SetParent (contentPanelTierStrings);

				Destroy (inputFieldGO);
			}
		}
	}

	private void EditedInputField(){
		InputField currentInputField = currentInputFieldGO.GetComponent<InputField> ();
		Debug.Log ("End Edit = " + currentInputField.text.ToString());

		if(!newTiersConfig.ContainsKey(currentInputField.text.ToString()))
			newTiersConfig.Add (currentInputField.text.ToString (), new List<string> ());
	}

	public void OnPointerClickButton(PointerEventData data)
	{
		GameObject currentButtonGO = data.selectedObject;
		Debug.Log("OnPointerClickButton called = " + currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());

		if (currentInputFieldGO == null)
			return;

		string selectedTier = currentInputFieldGO.GetComponent<InputField>().text.ToString ();
		foreach (string tier in newTiersConfig.Keys) {
			if (selectedTier.Equals (tier)) {
				newTiersConfig [tier].Add (currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());
				itemListByInputField [currentInputFieldGO].Add (currentButtonGO);
				currentButtonGO.transform.SetParent (contentPanelNewTiers);
			}
		}
	}

	public void OnPointerClickInputField(PointerEventData data)
	{
		currentInputFieldGO = data.selectedObject;
		Debug.Log("OnPointerClickInputField called = " + currentInputFieldGO.GetComponent<InputField>().text.ToString ());
	}

	public void OnClickExit(){
		annotationSetupManager.SetNewTierConfig (newTiersConfig);

		// TODO: this showed be done by the EventSystem
		GameObject tiersSetupPanel = GameObject.Find ("TiersSetupPanel");
		tiersSetupPanel.SetActive (false);

		annotationSetupManager.GetActionSetupPanel ().SetActive (true);


	}

}
