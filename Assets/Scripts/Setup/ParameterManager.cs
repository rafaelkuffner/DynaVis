using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterManager : MonoBehaviour {

	private AnnotationSetupManager annotationSetupManager;

	// Use this for initialization
	void Start () {
		annotationSetupManager = GameObject.Find ("Boss Object").GetComponent<AnnotationSetupManager> ();
	}
	
	public void OnClickExit(){
		// TODO: this showed be done by the EventSystem
		annotationSetupManager.OutputSetupPanel.SetActive (false);
		//annotationSetupManager.ModifiersSetupPanel.SetActive(true);
	}
}
