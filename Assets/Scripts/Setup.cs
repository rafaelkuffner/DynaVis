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
	GameObject hitLeftButtonGO;
	GameObject hitRightButtonGO;

	bool rightButtonClicked;
	bool popupMenuDrawn;

	private float startTime;

	RectTransform canvasTransform;
	float scale;

	bool lmbclicked;
	float deltaLastClick;
	float deltaHoldTime;
	private float doubleClickTimeLimit = 0.25f;

	List<GameObject> playersList;
	List<GameObject> chairsList;
	List<GameObject> tablesList;

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
		canvasTransform = GameObject.Find ("Canvas").GetComponent<RectTransform> ();

		float sx = canvasTransform.localScale.x;
		float sy = canvasTransform.localScale.y;
		scale = sx > sy ? sy : sx;

		rightButtonClicked = false;
		popupMenuDrawn = false;

		playersList = new List<GameObject>();
		chairsList = new List<GameObject>();
		tablesList = new List<GameObject>();
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

		if (Input.GetMouseButtonDown(1))
			RightMouseClickEvent();

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
			hitLeftButtonGO = hit.transform.gameObject;
			Debug.Log (hit.transform.name);
		}
		else
			Debug.Log ("no hit");
	}

	private void LeftMouseButtonHoldDown()
	{
		Debug.Log ("left mouse hold down");
		Vector3 clickedPosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 displacement = Vector3.Lerp(hitLeftButtonGO.transform.position, clickedPosition, (Time.time - startTime) / 10.0f);
		hitLeftButtonGO.transform.position = new Vector3 (displacement.x, displacement.y, hitLeftButtonGO.transform.position.z);
	}


	private void LeftMouseReleaseEvent()
	{
		Debug.Log ("left mouse release");

		lmbclicked = false;
		deltaHoldTime = 0;
	}

	private void RightMouseClickEvent()
	{
		Debug.Log("right mouse single click");
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

		if (hit) {
			hitRightButtonGO = hit.transform.gameObject;
			rightButtonClicked = true;
		} else {
			Debug.Log ("no hit");
			rightButtonClicked = false;
			hitRightButtonGO = null;
		}
	}

	void OnGUI(){

		if (rightButtonClicked && hitRightButtonGO != null) {
			// void draw right click contextual menu
			GUIStyle boxStyle = new GUIStyle (GUI.skin.button);
			boxStyle.fontSize = Mathf.RoundToInt ((GUI.skin.font.fontSize - 2) * 3 * scale / 2);

			int sizex = Mathf.RoundToInt (canvasTransform.rect.width * 0.1f);
			int sizey = Mathf.RoundToInt (canvasTransform.rect.height * 0.05f);

			Vector3 pos = Camera.main.WorldToScreenPoint (hitRightButtonGO.transform.position);

			Debug.Log ("hit = " + hitRightButtonGO.name);
			Debug.Log ("pos = " + pos.ToString ());

			Rect boxRect = new Rect (pos.x, pos.y, sizex, sizey);

			if (GUI.Button (boxRect, "delete", boxStyle)) {
				DeletePlayerFromList (hitRightButtonGO);
				Destroy (hitRightButtonGO);
			}

			popupMenuDrawn = true;
		}
	}

	// TODO FIX THIS!
	void DeletePlayerFromList(GameObject player){

		Debug.Log ("Count Before = " + playersList.Count);
		foreach (GameObject playerGO in playersList) {
			if (playerGO.GetInstanceID () == player.GetInstanceID ())
				playersList.Remove (player);
		}
		Debug.Log ("Count after = " + playersList.Count);

	}

	void PlayersButtonClick(){
		GameObject player = Instantiate (playerGO) as GameObject;
		playersList.Add (player);
		Debug.Log ("players button click");

	}

	void ChairsButtonClick(){
		GameObject chair = Instantiate (chairGO) as GameObject;
		chairsList.Add (chair);
		Debug.Log ("chairs button click");
	
	}

	void TablesButtonClick(){
		GameObject table = Instantiate (tableGO) as GameObject;
		tablesList.Add (table);
		Debug.Log ("tables button click");

	}

	// Update is called once per frame
	void Update () {

		handleMouseInput ();
	}
}
