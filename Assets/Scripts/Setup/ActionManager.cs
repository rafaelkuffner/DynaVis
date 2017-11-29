using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {

	public Transform contentTiers;
	public Transform contentActions;
	public GameObject itemPrefab;

	private List<Configuration> actions;
	private AnnotationSetupManager annotationSetupManager;
	private Dictionary<string, List<string>> newTiersConfig;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();

		actions = annotationSetupManager.GetBoss ().actionsConfig;

		newTiersConfig = annotationSetupManager.GetNewTiersConfig ();

		foreach (Configuration actionConfiguration in actions) {
			GameObject actionStringGO = GameObject.Instantiate (itemPrefab);
			actionStringGO.GetComponent<SampleItemButton> ().SetItemListText (actionConfiguration.className);
			actionStringGO.transform.SetParent (contentActions);
		}

		foreach (string tier in newTiersConfig.Keys) {
			GameObject tierGO = GameObject.Instantiate (itemPrefab);
			tierGO.GetComponent<SampleItemButton> ().SetItemListText (tier);
			tierGO.transform.SetParent (contentTiers);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
