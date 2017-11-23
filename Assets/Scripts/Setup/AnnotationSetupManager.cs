using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationSetupManager : MonoBehaviour {

	Simulation boss;

	public Simulation GetBoss(){
		return boss;
	}

	// Use this for initialization
	void Awake () {
		boss = GameObject.Find ("Boss Object").GetComponent<Simulation> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
