using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionManager : MonoBehaviour {

	public Transform contentTiers;
	public Transform contentActions;
	public GameObject itemPrefab;
	public Dictionary<string, List<string>> tiersByAction;

	private List<string> actionList;
	private AnnotationSetupManager annotationSetupManager;
	private Dictionary<string, List<string>> newTiersConfig;
	private GameObject currentTierGO;
	private GameObject currentActionGO;
	private int currentActionIndex;
	private List<Transform> contentActionItemList;


	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		contentActionItemList = new List<Transform> ();
		tiersByAction = new Dictionary<string, List<string>> ();
		actionList = annotationSetupManager.ActionList;

		newTiersConfig = annotationSetupManager.GetNewTiersConfig ();

		foreach (string action in actionList) {
			GameObject actionStringGO = GameObject.Instantiate (itemPrefab);
			Button actionStringButton = actionStringGO.GetComponent<Button> ();
			actionStringGO.GetComponent<SampleItemButton> ().SetItemListText (action);
			actionStringGO.transform.SetParent (contentActions);

			EventTrigger trigger = actionStringButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickActionButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}

		foreach (string tier in newTiersConfig.Keys) {
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

		currentActionIndex = 0;
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

		if (tiersByAction.ContainsKey (currentActionName))
			tiersByAction [currentActionName].Add (currentTierName);
		else {
			tiersByAction.Add (currentActionName, new List<string> ());
			tiersByAction [currentActionName].Add (currentTierName);
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

	public void OnClickNext(){
		annotationSetupManager.TiersByAction = tiersByAction;
		annotationSetupManager.ActionSetupPanel.SetActive (false);
		annotationSetupManager.SpriteSetupPanel.SetActive (true);
	}
}
