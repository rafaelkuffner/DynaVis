using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

	private SpriteHelper sh;
	private SpriteCollection dudeSprite;
	private SpriteCollection faceMUSprite;
	private SpriteCollection upperBodySprite;
	private SpriteCollection lowerBodySprite;
	private SpriteCollection settlingSprite;
	private SpriteCollection errorSprite;

	// Use this for initialization
	void Start () {
		sh = GameObject.Find ("Sprites").GetComponent<SpriteHelper> ();
		dudeSprite = sh.dude;
		faceMUSprite = sh.faceMU;
		upperBodySprite = sh.upperBody;
		lowerBodySprite = sh.lowerBody;
		settlingSprite = sh.settling;
		errorSprite = sh.error;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
