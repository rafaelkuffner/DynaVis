using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupData {

	public string Tier { get; set; }
	public string Participant { get; set; }
	public int Startime { get; set; } //miliseconds
	public int Endtime { get; set; } //miliseconds
	public string Parameter { get; set; }

	// constructor
	public SetupData (string t, string p, int st, int et, string param) {
		Tier = t;
		Participant = p;
		Startime = st;
		Endtime = et;
		Parameter = param;
	}

	public void DebugSetupData(){

		Debug.Log ("Tier = " + Tier + " | Participant = " + Participant + " | Startime = " +
		Startime + " | Endtime = " + Endtime + " | Parameter = " + Parameter);
	}

}
