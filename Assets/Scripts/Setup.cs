using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour {

	Button players;
	Button tables;
	Button chairs;

//	bool isPlayersButtonActive;
//	bool isTablesButtonActive;
//	bool isChairsButtonActive;

	Canvas canvas;

	GameObject playerGO;
	GameObject chairGO;
	GameObject tableGO;
	GameObject hitGO;

	private float startTime;

	bool lmbclicked;
	float deltaLastClick;
	float deltaHoldTime;
	private float doubleClickTimeLimit = 0.25f;

	// Use this for initialization
	void Start () {
		players = GameObject.Find ("Players").GetComponent<Button> ();
		players.onClick.AddListener (PlayersButtonClick);

		tables = GameObject.Find("Tables").GetComponent<Button> ();
		tables.onClick.AddListener (TablesButtonClick);

		chairs = GameObject.Find("Chairs").GetComponent<Button> ();
		chairs.onClick.AddListener (ChairsButtonClick);

//		isPlayersButtonActive = false;
//		isTablesButtonActive = false;
//		isChairsButtonActive = false;

		canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();

		playerGO = Resources.Load ("Prefabs/player") as GameObject;
		chairGO = Resources.Load ("Prefabs/chair") as GameObject;
		tableGO = Resources.Load ("Prefabs/table") as GameObject;


		startTime = Time.time;
	}

	private void handleMouseInput()
	{

		deltaLastClick += Time.deltaTime;
		if (lmbclicked) 
			deltaHoldTime += Time.deltaTime;


		if (lmbclicked && deltaHoldTime > doubleClickTimeLimit) 
			LeftMouseButtonHoldDown();

		if (Input.GetMouseButtonDown(0))
			LeftMouseClickEvent();

		if (Input.GetMouseButtonUp (0)) 
			LeftMouseReleaseEvent ();

	}

	private void LeftMouseClickEvent()
	{
		//pause a frame so you don't pick up the same mouse down event.
		//  yield return new WaitForEndOfFrame();

		lmbclicked = true;

		if (deltaLastClick <= doubleClickTimeLimit)
		{
			//TODO: LeftMouseButtonDoubleClick();
		}

		deltaLastClick = 0;

		LeftMouseButtonSingleClick();

	}

	private void LeftMouseButtonSingleClick()
	{
		Debug.Log ("left mouse single click");
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

		if (hit) {
			hitGO = hit.transform.gameObject;
			Debug.Log (hit.transform.name);
		}
		else
			Debug.Log ("no hit");
	}

	private void LeftMouseButtonHoldDown()
	{
		Debug.Log ("left mouse hold down");
		Vector3 clickedPosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 displacement = Vector3.Lerp(hitGO.transform.position, clickedPosition, (Time.time - startTime) / 10.0f);
		hitGO.transform.position = new Vector3 (displacement.x, displacement.y, hitGO.transform.position.z);
	}


	private void LeftMouseReleaseEvent()
	{
		Debug.Log ("left mouse release");
		//pause a frame so you don't pick up the same mouse down event.
		//yield return new WaitForEndOfFrame();
		lmbclicked = false;
		deltaHoldTime = 0;
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
		GameObject table = Instantiate (tableGO) as GameObject;
		Debug.Log ("tables button click");

	}

	// Update is called once per frame
	void Update () {

		handleMouseInput ();
	/*	if (Input.GetMouseButtonDown (0)) {
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
		} */
	}
}
