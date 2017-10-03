using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupButton : MonoBehaviour {

	bool open;
	Button theButton;
	Simulation boss;

	Button nextButton;
	public GameObject nextButtonGO;

	Color normalColor;
	Color pressedColor;
	Color highlightedColor;

	RectTransform canvasTransform;

	bool lateInited = false;

	void lateInit(){
		boss = GameObject.Find ("Boss Object").GetComponent<Simulation> ();
		open = false;
		theButton = transform.GetComponent<Button> ();
		ColorBlock cb = theButton.colors;
		normalColor = cb.normalColor;
		pressedColor = cb.pressedColor;
		highlightedColor = cb.highlightedColor;
		canvasTransform = GameObject.Find ("Canvas").GetComponent<RectTransform> ();

		// TODO CREATE BUTTON PROGRAMMATICALLY 
		//nextButton = Instantiate(nextButtonGO, Vector3.zero, Quaternion.identity) as Button;
		//Transform rectTransform = nextButton.GetComponent<RectTransform>();
		//rectTransform.SetParent(canvasTransform);
		//rectTransform.offsetMin = Vector2.zero;
		//rectTransform.offsetMax = Vector2.zero;
		//TODO: button.onClick.AddListener(SpawnPlayer);

		lateInited = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!lateInited)
			lateInit ();
	}

	public void drawSetupTiers(){

		float sx = canvasTransform.localScale.x;
		float sy = canvasTransform.localScale.y;
		float scale = sx > sy ? sy : sx;

		GUIStyle boxStyle =new GUIStyle(GUI.skin.box);
		boxStyle.fontSize =Mathf.RoundToInt((GUI.skin.font.fontSize+6)*3*scale/4);

		if (open) {
			int sizex = Mathf.RoundToInt(canvasTransform.rect.width * 0.8f);
			int sizey = Mathf.RoundToInt(canvasTransform.rect.height * 0.8f);
			Rect boxRect = new Rect ((canvasTransform.rect.width / 2) - (sizex / 2), (canvasTransform.rect.height / 2) - (sizey / 2), sizex, sizey);

			GUI.Box (boxRect, "Setup Tiers", boxStyle);

			nextButton.transform.position = new Vector3 ( Mathf.RoundToInt(canvasTransform.rect.width * 0.7f), canvasTransform.rect.height * 0.7f);
		}
	}

	public void startSetup(){
		open = !open;
		if (open) {
			ColorBlock cb = theButton.colors;
			cb.normalColor = pressedColor;
			cb.highlightedColor = pressedColor;
			theButton.colors = cb;
		} else {
			ColorBlock cb = theButton.colors;
			cb.normalColor = normalColor;
			cb.highlightedColor = highlightedColor;
			theButton.colors = cb;
		}
	}
}
