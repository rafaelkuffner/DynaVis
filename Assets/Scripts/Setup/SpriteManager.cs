using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteManager : MonoBehaviour {

	public Transform contentSprites;
	public GameObject itemPrefab;
	public Transform contentSpriteName;
	public Vector3 contentSpriteNamePosition;
	public GameObject inputFieldPrefab;
	public Dictionary<string, Dictionary<string,  string>> NewSpriteTranslationTableByAction { get; set; }
	public Button buttonNext;

	private AnnotationSetupManager annotationSetupManager;
	private List<string> actionList;
	private Dictionary<string, string> spriteTranslationTable;
	private GameObject spriteNameGO;
	private GameObject currentButtonGO;
	private GameObject currentItemGO;
	private Dictionary<string, List<string>> tiersByAction;
	private List<string> actions;
	private string currentAction = "";


	// Use this for initialization
	void Start () {
		NewSpriteTranslationTableByAction = new Dictionary<string, Dictionary<string,  string>> ();

		contentSpriteNamePosition = contentSpriteName.position;
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		actionList = annotationSetupManager.ActionList;
		tiersByAction = annotationSetupManager.TiersByAction;
		actions = new List<string>(tiersByAction.Keys);

		if (actions.Count > 0) {
			currentAction = actions [0];
			NewSpriteTranslationTableByAction.Add (currentAction, new Dictionary<string, string> ());
			List<string> tiers = tiersByAction [currentAction];
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
	}

	private void LoadContent(){
		contentSprites.DetachChildren();
		contentSpriteName.DetachChildren();
		contentSpriteName.position = contentSpriteNamePosition;
			
		GameObject[] contentListDetached = GameObject.FindGameObjectsWithTag ("SampleListItem") as GameObject[];
		for (int i = 0; i < contentListDetached.Length; i++) {
			Destroy(contentListDetached[i]);
		}

		if (actions.Count <= 0)
			return;

		currentAction = actions [0];
		NewSpriteTranslationTableByAction.Add (currentAction, new Dictionary<string, string> ());
		List<string> tiers = tiersByAction [currentAction];
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

	private void EditedInputField(){
		InputField currentInputField = currentItemGO.GetComponentInChildren<InputField> ();
		Button button = currentItemGO.GetComponentInChildren<Button> ();

		Debug.Log("EditedInputField = " + currentItemGO.GetComponentInChildren<Text>().text.ToString ());

		NewSpriteTranslationTableByAction[currentAction].Add (button.GetComponent<SampleItemButton>().GetItemListText(), currentInputField.text.ToString ());
	}
		
	public void OnPointerClickButton(PointerEventData data)
	{
		currentButtonGO = data.selectedObject;
		Debug.Log("OnPointerClickButton called = " + currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());

		currentItemGO = GameObject.Instantiate (inputFieldPrefab);
		InputField currentInputField = currentItemGO.GetComponentInChildren<InputField> ();
		Button currentInputButton = currentItemGO.GetComponentInChildren<Button> ();
		currentInputButton.GetComponent<SampleItemButton>().SetItemListText(currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());
		currentInputField.onEndEdit.AddListener (delegate {EditedInputField (); });
		currentItemGO.transform.SetParent (contentSpriteName);

		Destroy(currentButtonGO);
	}

	public void DeleteNewSpriteName(){
		Debug.Log ("Delete new sprite name");
	}

	public void OnClickNextAction(){

		LoadContent ();
		if (actions.Count == 0) {
			buttonNext.GetComponentInChildren<Text> ().text = "Next";
			buttonNext.onClick.AddListener(OnClickNext);
		}
	}

	public void OnClickNext(){

		GameObject[] contentItemDetached = GameObject.FindGameObjectsWithTag ("SampleItem") as GameObject[];
		for (int i = 0; i < contentItemDetached.Length; i++) {
			Destroy(contentItemDetached[i]);
		}

		annotationSetupManager.SpriteSetupPanel.SetActive (false);
		annotationSetupManager.MappingsSetupPanel.SetActive (true);
	}
}
