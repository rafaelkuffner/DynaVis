using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour {

	Button players;
	Button tables;
	Button chairs;

	bool isPlayersButtonActive;
	bool isTablesButtonActive;
	bool isChairsButtonActive;

	Canvas canvas;

	GameObject playerGO;
	GameObject chairGO;

	private float startTime;

	// Use this for initialization
	void Start () {
		players = GameObject.Find ("Players").GetComponent<Button> ();
		players.onClick.AddListener (PlayersButtonClick);

		tables = GameObject.Find("Tables").GetComponent<Button> ();
		tables.onClick.AddListener (TablesButtonClick);

		chairs = GameObject.Find("Chairs").GetComponent<Button> ();
		chairs.onClick.AddListener (ChairsButtonClick);

		isPlayersButtonActive = false;
		isTablesButtonActive = false;
		isChairsButtonActive = false;

		canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();

		playerGO = Resources.Load ("Prefabs/player") as GameObject;
		chairGO = Resources.Load ("Prefabs/chair") as GameObject;

		startTime = Time.time;
	}

	void OnGUI(){

	}

	void PlayersButtonClick(){
		GameObject player = Instantiate (playerGO) as GameObject;
		Debug.Log ("players button click");

	}

	void ChairsButtonClick(){
		GameObject chair = Instantiate (chairGO) as GameObject;
		Debug.Log ("chairs button click");
	
	}

	void TablesButtonClick(){
		Debug.Log ("tables button click");

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			// Casts the ray and get the first game object hit
			if (hit) {
				GameObject hitGO = hit.transform.gameObject;
				Debug.Log (hit.transform.name);
				Vector3 clickedPosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3.Lerp(hitGO.transform.position, clickedPosition, (Time.time - startTime) / 1.0f);
				hitGO.transform.position = Input.mousePosition;
			}
			else
				Debug.Log ("no hit");
		}
	}
}
