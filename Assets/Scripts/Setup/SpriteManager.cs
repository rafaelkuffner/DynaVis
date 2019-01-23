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
	public GameObject textDescriptionObject;
	private SetupButton setup;
	private List<Action> actionList;
	private Dictionary<string, string> spriteTranslationTable;
	private GameObject spriteNameGO;
	private GameObject currentButtonGO;
	private GameObject currentItemGO;
	private Dictionary<string, List<string>> tiersByAction;
    private Action currentAction;
	private int processedActions;

	// Use this for initialization
	void Start () {
		NewSpriteTranslationTableByAction = new Dictionary<string, Dictionary<string,  string>> ();

		contentSpriteNamePosition = contentSpriteName.position;
        setup = GameObject.Find("Button Setup").GetComponent<SetupButton>();
		actionList = new List<Action>(setup.ActionList);
		tiersByAction = setup.TiersByAction;

		if (tiersByAction.Count > 0)
        {
			while(!tiersByAction.ContainsKey(actionList[0].GetClassName()))
			{
				actionList.RemoveAt(0);
			}
			currentAction = actionList[0];
			processedActions = 1;
            NewSpriteTranslationTableByAction.Add(currentAction.GetClassName(), new Dictionary<string, string>());
            List<string> tiers = tiersByAction[currentAction.GetClassName()];
			List<string> parameterList = setup.getParametersByTierString (tiers);

			foreach (string param in parameterList) {
				GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
				Button currentButton = spriteNameGO.GetComponent<Button> ();
				spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (param);
				spriteNameGO.transform.SetParent (contentSprites);
                spriteNameGO.transform.localScale = Vector3.one;

				EventTrigger trigger = currentButton.GetComponent<EventTrigger> ();
				EventTrigger.Entry entry = new EventTrigger.Entry ();
				entry.eventID = EventTriggerType.PointerDown;
				entry.callback.AddListener ((data) => {
					OnPointerClickButton ((PointerEventData)data);
				});
				trigger.triggers.Add (entry);
			}
            actionList.RemoveAt(0);

		}
		if (processedActions ==tiersByAction.Count)
        {
            buttonNext.GetComponentInChildren<Text>().text = "Next";
			buttonNext.onClick.RemoveAllListeners ();
            buttonNext.onClick.AddListener(OnClickNext);
        }
		textDescriptionObject.GetComponent<Text>().text = currentAction.GetDescriptionString();
	}

	private void LoadContent(){
		contentSprites.DetachChildren();
		contentSpriteName.DetachChildren();
		contentSpriteName.position = contentSpriteNamePosition;
			
		GameObject[] contentListDetached = GameObject.FindGameObjectsWithTag ("SampleListItem") as GameObject[];
		for (int i = 0; i < contentListDetached.Length; i++) {
			Destroy(contentListDetached[i]);
		}
			
		while( actionList.Count >0  && !tiersByAction.ContainsKey(actionList[0].GetClassName()) )
		{
			actionList.RemoveAt(0);
		}
		if (actionList.Count <= 0)
			return;
		processedActions++;
		currentAction = actionList[0];
        NewSpriteTranslationTableByAction.Add(currentAction.GetClassName(), new Dictionary<string, string>());
        List<string> tiers = tiersByAction[currentAction.GetClassName()];
		List<string> parameterList = setup.getParametersByTierString (tiers);

		foreach (string param in parameterList) {
			GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
			Button currentButton = spriteNameGO.GetComponent<Button> ();
			spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (param);
			spriteNameGO.transform.SetParent (contentSprites);
			spriteNameGO.transform.localScale = Vector3.one;

			EventTrigger trigger = currentButton.GetComponent<EventTrigger> ();
			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener ((data) => {
				OnPointerClickButton ((PointerEventData)data);
			});
			trigger.triggers.Add (entry);
		}
        actionList.RemoveAt(0);
		textDescriptionObject.GetComponent<Text>().text = currentAction.GetDescriptionString();

	}

	private void EditedInputField(){
		InputField currentInputField = currentItemGO.GetComponentInChildren<InputField> ();
		Button button = currentItemGO.GetComponentInChildren<Button> ();

		Debug.Log("EditedInputField = " + currentItemGO.GetComponentInChildren<Text>().text.ToString ());

        NewSpriteTranslationTableByAction[currentAction.GetClassName()].Add(button.GetComponent<SampleItemButton>().GetItemListText(), currentInputField.text.ToString());
	}
		
	public void OnPointerClickButton(PointerEventData data)
	{
		currentButtonGO = data.selectedObject;
		Debug.Log("OnPointerClickButton called = " + currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());

		GameObject ob = GameObject.Instantiate (inputFieldPrefab);
        InputField currentInputField = ob.GetComponentInChildren<InputField>();
        Button currentInputButton = ob.GetComponentInChildren<Button>();

        EventTrigger trigger = currentInputButton.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((datac) => { OnPointerClickButtonRight((PointerEventData)datac); });
        trigger.triggers.Add(entry);

		currentInputButton.GetComponent<SampleItemButton>().SetItemListText(currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());
		currentInputField.onEndEdit.AddListener (delegate {EditedInputField (); });
        
		trigger = currentInputField.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((datad) => { OnPointerClickButtonRight((PointerEventData)datad); });
        trigger.triggers.Add(entry);

        ob.transform.SetParent(contentSpriteName);
        ob.transform.localScale = Vector3.one;
		Destroy(currentButtonGO);
	}

    Color normalButtonColor;
    Color highlightButtonColor;
    public void OnPointerClickButtonRight(PointerEventData data)
    {

        if (currentItemGO == null)
        {
			GameObject buttonGO = transform.parent.GetChild (0).gameObject;
			normalButtonColor = buttonGO.GetComponentInChildren<Button>().colors.normalColor;
			highlightButtonColor = buttonGO.GetComponentInChildren<Button>().colors.highlightedColor;
        }
        else
        {
            ColorBlock cb = currentItemGO.GetComponentInChildren<Button>().colors;
            cb.normalColor = normalButtonColor;
            cb.highlightedColor = highlightButtonColor;
            currentItemGO.GetComponentInChildren<Button>().colors = cb;
        }

        currentItemGO = data.selectedObject.transform.parent.gameObject;
        Debug.Log("Selected item " + currentItemGO);

        ColorBlock cb1 = currentItemGO.GetComponentInChildren<Button>().colors;
        cb1.normalColor = cb1.pressedColor;
        cb1.highlightedColor = cb1.pressedColor;
        currentItemGO.GetComponentInChildren<Button>().colors = cb1;

    }

    //public void OnPointerClickButton(PointerEventData data)
    //{
    //    currentButtonGO = data.selectedObject;
    //    Debug.Log("OnPointerClickButton called = " + currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());

    //    currentItemGO = GameObject.Instantiate(inputFieldPrefab);
    //    InputField currentInputField = currentItemGO.GetComponentInChildren<InputField>();
    //    Button currentInputButton = currentItemGO.GetComponentInChildren<Button>();
    //    currentInputButton.GetComponent<SampleItemButton>().SetItemListText(currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());
    //    currentInputField.onEndEdit.AddListener(delegate { EditedInputField(); });


    //    currentItemGO.transform.SetParent(contentSpriteName);
    //    currentItemGO.transform.localScale = Vector3.one;
    //    Destroy(currentButtonGO);
    //}


	public void DeleteNewSpriteName(){
        string spritename = currentItemGO.GetComponentInChildren<SampleItemButton>().GetItemListText();
        GameObject.Destroy(currentItemGO);
        GameObject spriteNameGO = GameObject.Instantiate (itemPrefab);
		Button currentButton = spriteNameGO.GetComponent<Button> ();
		spriteNameGO.GetComponent<SampleItemButton> ().SetItemListText (spritename);
		spriteNameGO.transform.SetParent (contentSprites);
        spriteNameGO.transform.localScale = Vector3.one;

		EventTrigger trigger = currentButton.GetComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener ((data) => {
			OnPointerClickButton ((PointerEventData)data);
		});
		trigger.triggers.Add (entry);
	}




	public void OnClickNextAction(){

		LoadContent ();
		if (processedActions == tiersByAction.Count)
        {
			buttonNext.GetComponentInChildren<Text> ().text = "Next";
			buttonNext.onClick.RemoveAllListeners ();
			buttonNext.onClick.AddListener(OnClickNext);
		}
	}

	public void OnClickNext(){

		GameObject[] contentItemDetached = GameObject.FindGameObjectsWithTag ("SampleItem") as GameObject[];
		for (int i = 0; i < contentItemDetached.Length; i++) {
			Destroy(contentItemDetached[i]);
		}

		setup.SpriteSetupPanel.SetActive (false);
		setup.MappingsSetupPanel.SetActive (true);
	}
}
