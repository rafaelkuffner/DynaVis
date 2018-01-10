using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionManager : MonoBehaviour {

	public Transform contentTiers;
	public Transform contentActions;
	public GameObject itemPrefab;

	private List<Configuration> actions;
	private AnnotationSetupManager annotationSetupManager;
	private Dictionary<string, List<string>> newTiersConfig;
	private GameObject currentTierGO;
	private GameObject currentActionGO;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();

		actions = annotationSetupManager.GetBoss ().actionsConfig;

		newTiersConfig = annotationSetupManager.GetNewTiersConfig ();

		foreach (Configuration actionConfiguration in actions) {
			GameObject actionStringGO = GameObject.Instantiate (itemPrefab);
			Button actionStringButton = actionStringGO.GetComponent<Button> ();
			actionStringGO.GetComponent<SampleItemButton> ().SetItemListText (actionConfiguration.className);
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
	}

	public void OnPointerClickActionButton(PointerEventData data){
		currentActionGO = data.selectedObject;
		// reorder items!
		currentActionGO.transform.SetParent (currentTierGO.transform);
	}

	public void OnPointerClickTierButton(PointerEventData data){
		currentTierGO = data.selectedObject;
		Button button = currentTierGO.GetComponent<Button> ();
		ColorBlock buttonColors = button.colors;
		buttonColors.highlightedColor = new Color (0.0f, 0.2f, 0.3f);
		button.colors = buttonColors;
	}

	public void OnClickNext(){
		annotationSetupManager.ActionSetupPanel.SetActive (false);
		annotationSetupManager.SpriteSetupPanel.SetActive (true);
	}
}
