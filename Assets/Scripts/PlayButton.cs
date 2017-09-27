using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

	public bool play;
	public Sprite imagePlay;
	public Sprite imagePause;
	Image i;
	// Use this for initialization
	void Start () {
		i = transform.GetComponent<Image>();
		play = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void stop(){
		i.sprite = imagePlay;
		play = false;
	}

	public void toggle(){
		if (play) {
			i.sprite = imagePlay;
		} else {
			i.sprite = imagePause;
		}
		play = !play;
	}
}
