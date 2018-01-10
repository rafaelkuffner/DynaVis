using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersManager : MonoBehaviour {

	private AnnotationSetupManager annotationSetupManager;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
	}
	
	public void OnClickNext(){
		// TODO: this showed be done by the EventSystem
		annotationSetupManager.ModifiersSetupPanel.SetActive (false);
		annotationSetupManager.OutputSetupPanel.SetActive(true);
	}
}
