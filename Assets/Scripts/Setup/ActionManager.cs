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

	private List<Action> actionList;
	private SetupButton setup;
	private Dictionary<string, List<string>> newTiersConfig;
	private GameObject currentTierGO;
	private GameObject currentActionGO;
	private int currentActionIndex;


	// Use this for initialization
	void Start () {
		setup = GameObject.Find ("Button Setup").GetComponent<SetupButton> ();
		tiersByAction = new Dictionary<string, List<string>> ();
		actionList = setup.ActionList;

		newTiersConfig = setup.GetNewTiersConfig ();

		foreach (Action action in actionList) {
			GameObject actionStringGO = GameObject.Instantiate (itemPrefab);
			Button actionStringButton = actionStringGO.GetComponent<Button> ();
			actionStringGO.GetComponent<SampleItemButton> ().SetItemListText (action.ToString());
			actionStringGO.transform.SetParent (contentActions);
            actionStringGO.transform.localScale = Vector3.one;
            actionStringGO.GetComponent<RectTransform>().sizeDelta =new Vector2(200,25);
            Text t = actionStringGO.GetComponentInChildren<Text>();
            t.fontSize = 12;
            t.fontStyle = FontStyle.Bold;
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
            tierGO.GetComponent<SampleItemButton>().myPanel = SampleItemButton.Panel.Left;
			tierGO.transform.SetParent (contentTiers);
            tierGO.transform.localScale = Vector3.one;

			EventTrigger trigger = tierButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickTierButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}

		currentActionIndex = 0;
	}


    Color normalButtonColor;
    Color highlightButtonColor;

	public void OnPointerClickActionButton(PointerEventData data){
        if (currentActionGO == null)
        {
            normalButtonColor = data.selectedObject.GetComponent<Button>().colors.normalColor;
            highlightButtonColor = data.selectedObject.GetComponent<Button>().colors.highlightedColor;
        }
        else
        {
            ColorBlock cb = currentActionGO.GetComponent<Button>().colors;
            cb.normalColor = normalButtonColor;
            cb.highlightedColor = highlightButtonColor;
            currentActionGO.GetComponent<Button>().colors = cb;
        }

		currentActionGO = data.selectedObject;

        ColorBlock cb1 = currentActionGO.GetComponent<Button>().colors;
        cb1.normalColor = cb1.pressedColor;
        cb1.highlightedColor = cb1.pressedColor;
        currentActionGO.GetComponent<Button>().colors = cb1;
    }

	public void OnPointerClickTierButton(PointerEventData data){
		currentTierGO = data.selectedObject;

        SampleItemButton sib = currentTierGO.GetComponent<SampleItemButton>();

		string currentTierName = currentTierGO.transform.GetComponent<SampleItemButton> ().GetItemListText ();
        if (currentActionGO == null) return;

		string currentActionName = currentActionGO.transform.GetComponent<SampleItemButton> ().GetItemListText ();
        if (sib.myPanel == SampleItemButton.Panel.Left) { 
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
            sib.myPanel = SampleItemButton.Panel.Right;
            currentTierGO.transform.SetParent(contentActions);
            currentTierGO.transform.SetSiblingIndex(currentActionGO.transform.GetSiblingIndex() + 1);
        }
        else
        {
            string tier = "";
            foreach (KeyValuePair<string, List<string>> p in tiersByAction)
            {
                if (p.Value.Contains(currentTierName))
                {
                    tier = p.Key;
                }
            }
            tiersByAction[tier].Remove(currentTierName);
            if (tiersByAction[tier].Count == 0)
            {
                tiersByAction.Remove(tier);
            }

            currentTierGO.transform.SetParent(contentTiers);
            sib.myPanel = SampleItemButton.Panel.Left;

        }

	}

	public void OnClickNext(){
		setup.TiersByAction = tiersByAction;
		setup.ActionSetupPanel.SetActive (false);
		setup.SpriteSetupPanel.SetActive (true);
	}
}
