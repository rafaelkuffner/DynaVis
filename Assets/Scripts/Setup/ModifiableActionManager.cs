using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModifiableActionManager : MonoBehaviour {


	public Transform contentTiers;
	public Transform contentActions;
	public GameObject itemPrefab;
	public Dictionary<string, List<string>> TiersByModifiableAction { get; set; }

	private List<string> tierList;
	private List<Action> actionList;


	private SetupButton setup;
	private Dictionary<string, List<string>>  newTierConfig;
	private Dictionary<string, List<string>> tiersByAction;

	private GameObject currentActionGO;
	private GameObject currentTierGO;

	private int currentActionIndex;
	private List<Transform> contentActionItemList;

	void Start(){
        setup = GameObject.Find("Button Setup").GetComponent<SetupButton>();
        contentActionItemList = new List<Transform>();
		TiersByModifiableAction = new Dictionary<string, List<string>> ();
		currentActionIndex = 0;

		actionList = setup.ActionList;
		newTierConfig = setup.GetNewTiersConfig ();
		tiersByAction = setup.ActionManager.tiersByAction;

		// Tiers not assigned yet
		tierList = new List<string> ();
		GetTiersNotAssigned ();

		foreach (Action action in actionList) {
			GameObject actionStringGO = GameObject.Instantiate (itemPrefab);
			Button actionStringButton = actionStringGO.GetComponent<Button> ();
			actionStringGO.GetComponent<SampleItemButton> ().SetItemListText (action.GetClassName());
			actionStringGO.transform.SetParent (contentActions);

			EventTrigger trigger = actionStringButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickActionButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}
			
		foreach (string tier in tierList) {
			GameObject tierGO = GameObject.Instantiate (itemPrefab);
			Button tierButton = tierGO.GetComponent<Button> ();
			tierGO.GetComponent<SampleItemButton> ().SetItemListText (tier);
			tierGO.transform.SetParent (contentTiers);

			EventTrigger trigger = tierButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickTierButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}
	}

	void GetTiersNotAssigned(){

		if (tiersByAction.Count != 0) {
			foreach (string action in tiersByAction.Keys) {
				List<string> tiers = tiersByAction [action];
				foreach (string tier in tiers) {
					foreach (string newTier in newTierConfig.Keys) {
						if (newTier != tier)
							tierList.Add (newTier);
					}
				}
			}
		} else
			tierList.AddRange (newTierConfig.Keys);
	}

	public void OnPointerClickActionButton(PointerEventData data){
		currentActionGO = data.selectedObject;
	}

	public void OnPointerClickTierButton(PointerEventData data){
		currentTierGO = data.selectedObject;

		string currentTierName = currentTierGO.transform.GetComponent<SampleItemButton> ().GetItemListText ();
		string currentActionName = currentActionGO.transform.GetComponent<SampleItemButton> ().GetItemListText ();

		int numberOfItems = contentActions.childCount;
		for (int i = 0; i < numberOfItems; i++) {
			Transform item = contentActions.GetChild (i);
			if (item.GetComponent<SampleItemButton> ().GetItemListText() == currentActionName) {
				currentActionIndex = i;
				break;
			}
		}

		if (TiersByModifiableAction.ContainsKey (currentActionName))
			TiersByModifiableAction [currentActionName].Add (currentTierName);
		else {
			TiersByModifiableAction.Add (currentActionName, new List<string> ());
			TiersByModifiableAction [currentActionName].Add (currentTierName);
		}

		RedrawContentActions ();
	}

	public void RedrawContentActions(){

		contentActionItemList.Clear ();
		int numberOfItems = contentActions.childCount;
		for (int i = 0; i < numberOfItems; i++) {
			contentActionItemList.Add (contentActions.GetChild (i));
			//Destroy (contentActions.GetChild (i));
		}

		contentActionItemList.Insert (++currentActionIndex, currentTierGO.transform);

		contentActions.DetachChildren ();
		foreach (Transform transform in contentActionItemList)
			transform.SetParent (contentActions);

		currentActionIndex = 0;
	}

	// Update is called once per frame
	public void ClickedNext () {
		setup.ModifiableActionSetupPanel.SetActive (false);
		setup.ParameterSetupPanel.SetActive (true);
	}
}
