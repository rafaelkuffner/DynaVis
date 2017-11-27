using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {

	public Transform contentTiers;
	public Transform contentActions;
	public GameObject itemPrefab;

	private string[] actions = { "UpperBodyAction", "LowerBodyAction", "HeadFaceAction", "LocationAction", "GazeAction" };
	private AnnotationSetupManager annotationSetupManager;
	private Dictionary<string, List<string>> newTiersConfig;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
		newTiersConfig = annotationSetupManager.GetNewTiersConfig ();

		for (int i = 0; i < actions.Length; i++) {
			GameObject actionStringGO = GameObject.Instantiate (itemPrefab);
			actionStringGO.GetComponent<SampleItemList> ().SetItemListText (actions[i]);
			actionStringGO.transform.SetParent (contentActions);
		}

		foreach (string tier in newTiersConfig.Keys) {
			GameObject tierGO = GameObject.Instantiate (itemPrefab);
			tierGO.GetComponent<SampleItemList> ().SetItemListText (tier);
			tierGO.transform.SetParent (contentTiers);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
