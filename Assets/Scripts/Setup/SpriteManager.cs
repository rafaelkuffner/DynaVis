using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteManager : MonoBehaviour {

	private SpriteHelper sh;
	private SpriteCollection dudeSprite;
	private SpriteCollection faceMUSprite;
	private SpriteCollection upperBodySprite;
	private SpriteCollection lowerBodySprite;
	private SpriteCollection settlingSprite;
	private SpriteCollection errorSprite;
	private AnnotationSetupManager annotationSetupManager;
	private List<Configuration> actions;
	private Dictionary<string, string> spriteTranslationTable;
	private GameObject spriteNameGO;
	private Dictionary<string, string> newSpriteTranslationTable;
	private GameObject currentButtonGO;
	private GameObject currentItemListButtonGO;
	private GameObject currentItemListInputFieldGO;
	public Transform contentSprites;
	public GameObject itemPrefab;
	public Transform contentSpriteName;
	public GameObject inputFieldPrefab;

	// Use this for initialization
	void Start () {
		sh = GameObject.Find ("Sprites").GetComponent<SpriteHelper> ();
		dudeSprite = sh.dude;
		faceMUSprite = sh.faceMU;
		upperBodySprite = sh.upperBody;
		lowerBodySprite = sh.lowerBody;
		settlingSprite = sh.settling;
		errorSprite = sh.error;

		newSpriteTranslationTable = new Dictionary<string, string> ();

		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		actions = annotationSetupManager.GetBoss ().actionsConfig;

		foreach (Configuration actionConfiguration in actions) {
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
		}
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

		if (!newSpriteTranslationTable.ContainsKey (currentInputField.text.ToString())) {
			newSpriteTranslationTable.Add (currentInputField.text.ToString (), "");
		}
	}

	public void OnPointerExitInputField(PointerEventData data)
	{
		currentItemListInputFieldGO = data.selectedObject;
		Debug.Log("OnPointerExitInputField called = " + currentItemListInputFieldGO.GetComponentInChildren<Text>().text.ToString ());
	}

	public void OnPointerClickItemListButton(PointerEventData data)
	{
		currentItemListButtonGO = data.selectedObject;
		Debug.Log("OnPointerClickItemListButton called = " + currentItemListButtonGO.GetComponentInChildren<Text>().text.ToString ());
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
		foreach (string newSpriteName in newSpriteTranslationTable.Keys) {
			if (selectedSprite.Equals (newSpriteName)) {
				key = newSpriteName;
				value = currentButtonGO.GetComponentInParent<SampleItemButton> ().GetItemListText ();
				Debug.Log ("currentItemListInputFieldGO = " + currentItemListInputFieldGO.name);
				currentItemListButtonGO.GetComponent<SampleItemButton> ().SetItemListText(value);
				//itemListByInputField [currentInputFieldGO].Add (currentButtonGO);
				Destroy(currentButtonGO);
			}
		}
		newSpriteTranslationTable.Remove (key);
		newSpriteTranslationTable.Add(key, value);
	}

	public void DeleteNewSpriteName(){
		Debug.Log ("Delete new sprite name");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
