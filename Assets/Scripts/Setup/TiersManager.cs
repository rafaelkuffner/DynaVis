using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiersManager : MonoBehaviour {

	public GameObject itemList; // SampleListItem Prefab
	public Transform contentPanelTierStrings;

	public GameObject inputField; //SampleInputField Prefab
	public Transform contentPanelNewTiers;

	Dictionary<string, List<string>> newTiersList;
	AnnotationSetupManager annotationSetupManager;
	Dictionary<string, string> tiersConfig;

	// Use this for initialization
	void Start () {
		newTiersList = new Dictionary<string, List<string>> ();

		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		Simulation boss = annotationSetupManager.GetBoss ();
		tiersConfig = boss.tiersConfig;

		foreach (string t in tiersConfig.Keys) {
			GameObject tierString = GameObject.Instantiate (itemList);
			tierString.GetComponent<SampleItemList> ().SetItemListText (t);
			tierString.transform.SetParent (contentPanelTierStrings);
		}
	}


	public void ClickedAddNewTier(){

		Debug.Log ("Add new tier");
		GameObject newInputField = GameObject.Instantiate (inputField);
		newInputField.transform.SetParent (contentPanelNewTiers);
	}

}
