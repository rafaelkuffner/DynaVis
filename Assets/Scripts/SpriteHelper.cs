using UnityEngine;
using System.Collections;

public class SpriteHelper : MonoBehaviour {
	
	public SpriteCollection dude;
	public SpriteCollection faceMU;
	public SpriteCollection upperBody;
	public SpriteCollection lowerBody;
	public SpriteCollection settling;
	public SpriteCollection error;
	void Awake(){
		
		dude = new SpriteCollection ("Sprites/dudeSheet");
		faceMU = new SpriteCollection ("Sprites/faceMU");
		upperBody = new SpriteCollection ("Sprites/upperBody");
		lowerBody = new SpriteCollection ("Sprites/lowerBody");
		settling = new SpriteCollection ("Sprites/settling");
		error = new SpriteCollection ("Sprites/error");
	}

	// Use this for initialization
	void Start () {
	}

}
