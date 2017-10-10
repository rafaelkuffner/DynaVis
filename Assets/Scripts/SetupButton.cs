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

	public bool isSetupTiersActive = false;
	public bool isSetupActionActive = false;


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

		GUIStyle butStyle =new GUIStyle(GUI.skin.button);
		butStyle.fontSize = Mathf.RoundToInt((GUI.skin.font.fontSize - 2) * 3*scale/4);

		if (open) {

			// Main Window
			int sizex = Mathf.RoundToInt(canvasTransform.rect.width * 0.8f);
			int sizey = Mathf.RoundToInt(canvasTransform.rect.height * 0.8f);
			int posx = Mathf.RoundToInt((canvasTransform.rect.width / 2) - (sizex / 2));
			int posy = Mathf.RoundToInt((canvasTransform.rect.height / 2) - (sizey / 2));
			Rect boxRect = new Rect (posx, posy, sizex, sizey);

			GUI.Box (boxRect, "Setup Tiers", boxStyle);

			int borderx = Mathf.RoundToInt((sizex - (sizex * 0.4f) * 2) / 3);
			int bordery = Mathf.RoundToInt((sizey - (sizey * 0.8f)) / 2);

			// List Window
			int sListx = Mathf.RoundToInt(sizex * 0.4f);
			int sListy = Mathf.RoundToInt(sizey * 0.8f);
			int pListx = Mathf.RoundToInt(posx + borderx);
			int pListy = Mathf.RoundToInt(posy + bordery);
			Rect listBox = new Rect (pListx, pListy, sListx, sListy);
		
			GUI.Box (listBox, "Tiers", boxStyle);

			// Edit Window
			int sEditx = Mathf.RoundToInt(sizex * 0.4f);
			int sEdity = Mathf.RoundToInt(sizey * 0.8f);
			int pEditx = posx + borderx * 2 + sListx;
			int pEdity = Mathf.RoundToInt(posy + bordery);
			Rect editBox = new Rect (pEditx, pEdity, sEditx, sEdity);

			GUI.Box (editBox, "New Tiers", boxStyle);

			// Setup Action - Tiers
			boxRect.x += sizex*0.85f;
			boxRect.y += sizey*0.93f;
			boxRect.width = 0.075f*sizex;
			boxRect.height = 0.035f*sizey;
			if (GUI.Button (boxRect, "Next", butStyle)) {
				Debug.Log ("Button Next");
				isSetupActionActive = true;
				isSetupTiersActive = false;
			}
		}
	}


	public void drawSetupActions(){
	
		float sx = canvasTransform.localScale.x;
		float sy = canvasTransform.localScale.y;
		float scale = sx > sy ? sy : sx;

		GUIStyle boxStyle =new GUIStyle(GUI.skin.box);
		boxStyle.fontSize =Mathf.RoundToInt((GUI.skin.font.fontSize+6)*3*scale/4);

		GUIStyle butStyle =new GUIStyle(GUI.skin.button);
		butStyle.fontSize = Mathf.RoundToInt((GUI.skin.font.fontSize - 2) * 3*scale/4);

		if (open) {

			// Main Window
			int sizex = Mathf.RoundToInt(canvasTransform.rect.width * 0.8f);
			int sizey = Mathf.RoundToInt(canvasTransform.rect.height * 0.8f);
			int posx = Mathf.RoundToInt((canvasTransform.rect.width / 2) - (sizex / 2));
			int posy = Mathf.RoundToInt((canvasTransform.rect.height / 2) - (sizey / 2));
			Rect boxRect = new Rect (posx, posy, sizex, sizey);

			GUI.Box (boxRect, "Setup Actions", boxStyle);

			int borderx = Mathf.RoundToInt((sizex - (sizex * 0.4f) * 2) / 3);
			int bordery = Mathf.RoundToInt((sizey - (sizey * 0.8f)) / 2);

			// List Window
			int sListx = Mathf.RoundToInt(sizex * 0.4f);
			int sListy = Mathf.RoundToInt(sizey * 0.8f);
			int pListx = Mathf.RoundToInt(posx + borderx);
			int pListy = Mathf.RoundToInt(posy + bordery);
			Rect listBox = new Rect (pListx, pListy, sListx, sListy);

			GUI.Box (listBox, "Actions", boxStyle);

			// Edit Window
			int sEditx = Mathf.RoundToInt(sizex * 0.4f);
			int sEdity = Mathf.RoundToInt(sizey * 0.8f);
			int pEditx = posx + borderx * 2 + sListx;
			int pEdity = Mathf.RoundToInt(posy + bordery);
			Rect editBox = new Rect (pEditx, pEdity, sEditx, sEdity);

			GUI.Box (editBox, "Tiers", boxStyle);

			// Setup Action - Tiers
			boxRect.x += sizex*0.85f;
			boxRect.y += sizey*0.93f;
			boxRect.width = 0.075f * sizex;
			boxRect.height = 0.035f * sizey;
			if (GUI.Button (boxRect, "Next", butStyle)) {
				Debug.Log ("Button Next");
				isSetupActionActive = true;
				isSetupTiersActive = false;
			}

			// Setup Tiers - New Tiers
			Debug.Log ("drawing button back 2");
			Rect backButton = new Rect (sizex * 0.15f, boxRect.y, 0.075f * sizex, 0.035f * sizey);
			if (GUI.Button (backButton, "Back", butStyle)) {
				Debug.Log ("Button Back");
				isSetupActionActive = false;
				isSetupTiersActive = true;
			}
		}
	}

	public void startSetup(){
		open = !open;
		if (open) {
			ColorBlock cb = theButton.colors;
			cb.normalColor = pressedColor;
			cb.highlightedColor = pressedColor;
			theButton.colors = cb;
			isSetupTiersActive = true;
		} else {
			ColorBlock cb = theButton.colors;
			cb.normalColor = normalColor;
			cb.highlightedColor = highlightedColor;
			theButton.colors = cb;
			isSetupActionActive = false;

		}
	}
}
