using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleItemButton : MonoBehaviour {

	public void SetItemListText(string text){
		gameObject.GetComponentInChildren<Text> ().text = text;
	}

	public string GetItemListText(){
		return gameObject.GetComponentInChildren<Text> ().text.ToString ();
	}
}
