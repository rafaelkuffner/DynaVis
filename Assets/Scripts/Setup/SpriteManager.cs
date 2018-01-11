﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteManager : MonoBehaviour {

	public Transform contentSprites;
	public GameObject itemPrefab;
	public Transform contentSpriteName;
	public GameObject inputFieldPrefab;
	public Dictionary<string, string> NewSpriteTranslationTable { get; set; }

	private AnnotationSetupManager annotationSetupManager;
	private List<string> actionList;
	private Dictionary<string, string> spriteTranslationTable;
	private GameObject spriteNameGO;
	private GameObject currentButtonGO;
	private GameObject currentItemListButtonGO;
	private GameObject currentItemListInputFieldGO;
	private Dictionary<string, List<string>> tiersByAction;
	private List<string> actions;


	// Use this for initialization
	void Start () {
		NewSpriteTranslationTable = new Dictionary<string, string> ();

		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		actionList = annotationSetupManager.ActionList;
		tiersByAction = annotationSetupManager.TiersByAction;
		actions = new List<string>(tiersByAction.Keys);

		if (actions.Count > 0) {
			string action = actions [0];
			List<string> tiers = tiersByAction [action];
			List<string> parameterList = annotationSetupManager.getParametersByTierString (tiers);

			foreach (string param in parameterList) {
				GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
				Button currentButton = spriteNameGO.GetComponent<Button> ();
				spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (param);
				spriteNameGO.transform.SetParent (contentSprites);

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
		/*
		foreach (string action in actions) {
			spriteTranslationTable = actionConfiguration.translationTable;
			foreach (string spriteName in spriteTranslationTable.Keys) {
				GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
				Button currentButton = spriteNameGO.GetComponent<Button> ();
				spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (spriteName);
				spriteNameGO.transform.SetParent (contentSprites);

				EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerDown;
				entry.callback.AddListener((data) => { OnPointerClickButton((PointerEventData)data); });
				trigger.triggers.Add(entry);
			}
		}*/
	}

	private void LoadContent(){
		contentSprites.DetachChildren();
		contentSpriteName.DetachChildren();

		GameObject[] contentListDetached = GameObject.FindGameObjectsWithTag ("SampleListItem") as GameObject[];
		for (int i = 0; i < contentListDetached.Length; i++) {
			Destroy(contentListDetached[i]);
		}

		if (actions.Count <= 0)
			return;

		string action = actions [0];
		List<string> tiers = tiersByAction [action];
		List<string> parameterList = annotationSetupManager.getParametersByTierString (tiers);

		foreach (string param in parameterList) {
			GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
			Button currentButton = spriteNameGO.GetComponent<Button> ();
			spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (param);
			spriteNameGO.transform.SetParent (contentSprites);

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

	public void ClickedAddNewSpriteName(){
		Debug.Log ("Added new sprite name");
		GameObject newInputFieldGO = GameObject.Instantiate (inputFieldPrefab);
		InputField currentInputField = newInputFieldGO.GetComponentInChildren<InputField> ();
		Button currentInputButton = newInputFieldGO.GetComponentInChildren<Button> ();
		currentInputField.onEndEdit.AddListener (delegate {EditedInputField (); });
		newInputFieldGO.transform.SetParent (contentSpriteName);
		//itemListByInputField.Add (newInputFieldGO, new List<GameObject>());

		EventTrigger triggerInputField = currentInputField.GetComponent<EventTrigger>();
		EventTrigger.Entry entryInputField = new EventTrigger.Entry();
		entryInputField.eventID = EventTriggerType.PointerExit;
		entryInputField.callback.AddListener((data) => { OnPointerExitInputField((PointerEventData)data); });
		triggerInputField.triggers.Add(entryInputField);

		EventTrigger triggerButton = currentInputButton.GetComponent<EventTrigger>();
		EventTrigger.Entry entryButton = new EventTrigger.Entry();
		entryButton.eventID = EventTriggerType.PointerClick;
		entryButton.callback.AddListener((data) => { OnPointerClickItemListButton((PointerEventData)data); });
		triggerButton.triggers.Add(entryButton);
	}

	private void EditedInputField(){
		InputField currentInputField = currentItemListInputFieldGO.GetComponent<InputField> ();

		Debug.Log("EditedInputField = " + currentItemListInputFieldGO.GetComponentInChildren<Text>().text.ToString ());

		if (!NewSpriteTranslationTable.ContainsKey (currentInputField.text.ToString())) {
			NewSpriteTranslationTable.Add (currentInputField.text.ToString (), "");
		}
	}

	public void OnPointerExitInputField(PointerEventData data)
	{
		currentItemListInputFieldGO = data.selectedObject;
	}

	public void OnPointerClickItemListButton(PointerEventData data)
	{
		currentItemListButtonGO = data.selectedObject;
	}

	public void OnPointerClickButton(PointerEventData data)
	{
		currentButtonGO = data.selectedObject;
		Debug.Log("OnPointerClickButton called = " + currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());

		if (currentItemListInputFieldGO == null)
			return;

		string selectedSprite = currentItemListInputFieldGO.GetComponentInChildren<InputField>().text.ToString ();
		string key = "";
		string value = "";
		foreach (string newSpriteName in NewSpriteTranslationTable.Keys) {
			if (selectedSprite.Equals (newSpriteName)) {
				key = newSpriteName;
				value = currentButtonGO.GetComponentInParent<SampleItemButton> ().GetItemListText ();
				Debug.Log ("currentItemListInputFieldGO = " + currentItemListInputFieldGO.name);
				currentItemListButtonGO.GetComponent<SampleItemButton> ().SetItemListText(value);
				//itemListByInputField [currentInputFieldGO].Add (currentButtonGO);
				Destroy(currentButtonGO);
			}
		}
		NewSpriteTranslationTable.Remove (key);
		NewSpriteTranslationTable.Add(key, value);
	}

	public void DeleteNewSpriteName(){
		Debug.Log ("Delete new sprite name");
	}

	public void OnClickNextAction(){

		LoadContent ();

		//annotationSetupManager.SpriteSetupPanel.SetActive (false);
		//annotationSetupManager.ModifiersSetupPanel.SetActive (true);
	}
}
