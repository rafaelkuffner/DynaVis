﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleItemList : MonoBehaviour {

	public void SetItemListText(string text){
		gameObject.GetComponentInChildren<Text> ().text = text;
	}
}