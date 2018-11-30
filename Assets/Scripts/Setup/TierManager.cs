using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TierManager : MonoBehaviour {

	public GameObject itemList; // SampleListItem Prefab
	public Transform contentPanelTierStrings;
	public GameObject inputField; //SampleInputField Prefab
	public Transform contentPanelNewTiers;

	private Dictionary<string, List<string>> newTiersConfig;
	private SetupButton setup;
	private List<string> setupTiersList;
	private Dictionary<GameObject, List<GameObject>> itemListByInputField;
	private GameObject currentInputFieldGO;
	private GameObject currentButtonGO;


	// Use this for initialization
	void Start () {
		newTiersConfig = new Dictionary<string, List<string>> ();
		itemListByInputField = new Dictionary<GameObject, List<GameObject>> ();
		currentInputFieldGO = null;
		currentButtonGO = null;

        setup = GameObject.Find("Button Setup").GetComponent<SetupButton>();
        //Simulation boss = annotationSetupManager.GetBoss ();
		setupTiersList = setup.SetupTiersList;

		foreach (string t in setupTiersList) {
			GameObject tierStringGO = GameObject.Instantiate (itemList);
			Button currentButton = tierStringGO.GetComponent<Button> ();
            SampleItemButton sib = tierStringGO.GetComponent<SampleItemButton>();
            sib.SetItemListText(t);
            sib.myPanel = SampleItemButton.Panel.Left;
			tierStringGO.transform.SetParent (contentPanelTierStrings);
            tierStringGO.transform.localScale = Vector3.one;

			EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerClickButton((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}

        AddDefaultTiers();

	}

    //Vito Requested this
    public void AddDefaultTiers()
    {
        ClickedAddNewTier("Gaze");
        ClickedAddNewTier("Change Location");
        ClickedAddNewTier("Upper Body Movement");
        ClickedAddNewTier("Lower Body Movement");
        ClickedAddNewTier("Facial expression");
    
    }

	public void ClickedAddNewTier(string title){

		Debug.Log ("Add new tier");
		GameObject newInputFieldGO = GameObject.Instantiate (inputField);
		InputField currentInputField = newInputFieldGO.GetComponent<InputField> ();
		currentInputField.onEndEdit.AddListener (delegate {EditedInputField (); });
        currentInputField.text = title;
		newInputFieldGO.transform.SetParent (contentPanelNewTiers);
        newInputFieldGO.transform.localScale = Vector3.one;
		itemListByInputField.Add (newInputFieldGO, new List<GameObject>());

		EventTrigger trigger = currentInputField.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => { OnPointerClickInputField((PointerEventData)data); });
		trigger.triggers.Add(entry);
	}

    public void SortTiers()
    {
       SampleItemButton[] items = contentPanelTierStrings.GetComponentsInChildren<SampleItemButton>();
       List<string> names = new List<string>(); 
       foreach (SampleItemButton i in items)
       {
           names.Add(i.GetItemListText());
           GameObject.Destroy(i.gameObject);
       }

       names.Sort();
       foreach (string t in names)
       {
           GameObject tierStringGO = GameObject.Instantiate(itemList);
           Button currentButton = tierStringGO.GetComponent<Button>();
           SampleItemButton sib = tierStringGO.GetComponent<SampleItemButton>();
           sib.SetItemListText(t);
           sib.myPanel = SampleItemButton.Panel.Left;
           tierStringGO.transform.SetParent(contentPanelTierStrings);
           tierStringGO.transform.localScale = Vector3.one;

           EventTrigger trigger = currentButton.GetComponent<EventTrigger>();
           EventTrigger.Entry entry = new EventTrigger.Entry();
           entry.eventID = EventTriggerType.PointerDown;
           entry.callback.AddListener((data) => { OnPointerClickButton((PointerEventData)data); });
           trigger.triggers.Add(entry);
       }
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

        SampleItemButton sib = currentButtonGO.GetComponent<SampleItemButton>();
        if (sib.myPanel == SampleItemButton.Panel.Left) { 

            
		    if (currentInputFieldGO == null)
			    return;

            currentButtonGO.GetComponent<SampleItemButton>().myPanel = SampleItemButton.Panel.Right;
            string selectedTier = currentInputFieldGO.GetComponent<InputField>().text.ToString ();
		    foreach (string tier in newTiersConfig.Keys) {
			    if (selectedTier.Equals (tier)) {
				    newTiersConfig [tier].Add (currentButtonGO.GetComponent<SampleItemButton>().GetItemListText());
				    itemListByInputField [currentInputFieldGO].Add (currentButtonGO);
				    currentButtonGO.transform.SetParent (contentPanelNewTiers);
                    currentButtonGO.transform.SetSiblingIndex(currentInputFieldGO.transform.GetSiblingIndex() + 1);
			    }
		    }
        }
        else
        {
            currentButtonGO.transform.SetParent(contentPanelTierStrings);
            currentButtonGO.GetComponent<SampleItemButton>().myPanel = SampleItemButton.Panel.Left;
            string deltier = "";
            string value = currentButtonGO.GetComponent<SampleItemButton>().GetItemListText();
            foreach (KeyValuePair<string,List<string>> pair in newTiersConfig)
            {
                if (pair.Value.Contains(value))
                {
                    deltier = pair.Key;
                    break;
                }
            }
            newTiersConfig[deltier].Remove(value);
            GameObject tierGO = null;
            foreach (KeyValuePair<GameObject, List<GameObject>> objectPair in itemListByInputField)
            {
                if (objectPair.Value.Contains(currentButtonGO))
                {
                    tierGO = objectPair.Key;
                    break;
                }
            }
            itemListByInputField[tierGO].Remove(currentButtonGO);
        }
	}


    Color normalTextFieldColor;
	public void OnPointerClickInputField(PointerEventData data)
	{
        if (currentInputFieldGO == null)
        {
            normalTextFieldColor = data.selectedObject.GetComponent<InputField>().colors.normalColor;
        }
        else { 
            ColorBlock cb = currentInputFieldGO.GetComponent<InputField>().colors;
            cb.normalColor = normalTextFieldColor;
            currentInputFieldGO.GetComponent<InputField>().colors = cb;
        }
        currentInputFieldGO = data.selectedObject;

        ColorBlock cb1 = currentInputFieldGO.GetComponent<InputField>().colors;
        cb1.normalColor = cb1.highlightedColor;
        currentInputFieldGO.GetComponent<InputField>().colors = cb1;

        
		Debug.Log("OnPointerClickInputField called = " + currentInputFieldGO.GetComponent<InputField>().text.ToString ());
	}

	public void OnClickNext(){
		setup.SetNewTierConfig (newTiersConfig);

		// TODO: this showed be done by the EventSystem
		setup.TierSetupPanel.SetActive (false);
		setup.ActionSetupPanel.SetActive(true);
	}

}
