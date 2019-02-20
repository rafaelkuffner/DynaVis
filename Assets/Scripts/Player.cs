using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {
	
	string dude;
	string upper;
	string lower;
	string face;
	Color upperColor;
	Color lowerColor;
	Color faceColor;
	bool settling;
	bool error;
	SpriteHelper sh;
	SpriteRenderer Sdude;
	SpriteRenderer Slower;
	SpriteRenderer Supper;
	SpriteRenderer Sface;
	SpriteRenderer SSettle;
	SpriteRenderer SError;
	LineRenderer lineRenderer2D;
	public IndividualSettings whatToPlay;
	Simulation boss;
	Transform dude2D;

	Dictionary<string, List<Transform>> prefabListByTagSitting;
	Dictionary<string, List<Transform>> prefabListByTagStand;
	List<Transform> prefabListDudeSitting;
	List<Transform> prefabListDudeStand;
	Vector3 startPosition;

    public Dictionary<GameObject, Vector3> objectRotation;

    public Simulation Boss
    {
        get
        {
            return boss;
        }

        set
        {
            boss = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Boss = GameObject.Find("Boss Object").GetComponent<Simulation>();

		TextMesh tm = GetComponentInChildren<TextMesh> ();
		tm.text = gameObject.name;

		dude2D =this.transform;

		prefabListByTagSitting = new Dictionary<string, List<Transform>> ();
		prefabListByTagStand = new Dictionary<string, List<Transform>> ();
		prefabListDudeSitting = new List<Transform> ();
		prefabListDudeStand = new List<Transform> ();
        objectRotation = new Dictionary<GameObject, Vector3>();

        GameObject[] joints = GameObject.FindGameObjectsWithTag("playerjoints");
        foreach (GameObject joint in joints)
        {
            objectRotation[joint] = Vector3.zero;
        }

        Reset();
	}

	public Vector3 getStartPosition(){
		return startPosition;
	}

    public void loadSettings()
    {
        whatToPlay = new IndividualSettings();
    }
 
    public void Reset(){
        upper = "none";
        lower = "none";
        face = "none";
        upperColor = Color.white;
        lowerColor = Color.white;
        faceColor = Color.white;
        settling = false;
        error = false;
        if (dude2D == null)
            Start();
		startPosition = dude2D.position;
        changeStance("sit");
		lookat(dude2D.position);
        updateSprites();
        ResetRotations();
	}

	public Transform getDude2D(){
		return dude2D;
	}

	public void setError(bool value){
		error = value;
        updateSprites();
	}

	public void changeStance(string dude){
		this.dude = dude;
		updateSprites ();
	}

	public void settle(bool state){
		this.settling = state;
		updateSprites ();
	}

	public void faceExpression(string expr,Color color){
		this.face = expr;
		this.faceColor = color;
		updateSprites ();

	}

	public void highlightUpper(string param,Color color){
		this.upper = param;
		this.upperColor = color;
		updateSprites ();
	}

	public void highlightLower(string param,Color color){
		this.lower = param;
		this.lowerColor = color;
		updateSprites ();
	}


	public void lookat (Vector3 end){
		if(lineRenderer2D == null) 
			lineRenderer2D = dude2D.GetComponent<LineRenderer>();
        if (lineRenderer2D == null) return; //player has no lr
		if (end == dude2D.position) {
			lineRenderer2D.enabled = false;
			return;
		}
		lineRenderer2D.enabled = true;
        //lineRenderer2D.positionCount = 3;
		lineRenderer2D.positionCount = 3;
        Vector3 EyesPos = new Vector3 (0, 0.9f, 0);
		lineRenderer2D.SetPosition (0, dude2D.position + EyesPos);
		Vector3 diff = end-(dude2D.position + EyesPos);
		diff.x *= 0.01f;diff.y *= 0.01f;diff.z *= 0.01f;
		lineRenderer2D.SetPosition (1, end-diff);
		lineRenderer2D.SetPosition (2, end+diff);
        lineRenderer2D.startWidth = 0.0525f;
        lineRenderer2D.endWidth = 0.0525f;
        lineRenderer2D.startColor = Color.cyan;
        lineRenderer2D.endColor = Color.blue; ;
		lineRenderer2D.sortingOrder = 4;
	}


    public void ResetRotations()
    {
        foreach(KeyValuePair<GameObject,Vector3> kp in objectRotation){
            kp.Key.transform.localRotation = Quaternion.identity;
        }
        List<GameObject> keys = objectRotation.Keys.ToList();
        foreach (GameObject g in keys)
        {
            objectRotation[g] = Vector3.zero;
        }
    }


    //if he has no sprites, this goes right out
	public void updateSprites(){
		Debug.Log ("dude = " + dude);
		if(sh == null)
			sh = GameObject.Find ("Sprites").GetComponent<SpriteHelper> ();
		
		foreach (Transform t in dude2D.transform) {
			if(t.name == "dudeSheet"){
				if(Sdude== null)
					Sdude = t.GetComponent<SpriteRenderer>();
				Sdude.sprite = sh.dude.GetSprite(dude);
			}
			if(t.name == "lowerBody"){
				if(Slower == null)
					Slower = t.GetComponent<SpriteRenderer>();
				Slower.sprite = sh.lowerBody.GetSprite(dude+"_"+lower);
				Slower.color = lowerColor;
			}
			if(t.name == "upperBody"){
				if(Supper == null)
					Supper = t.GetComponent<SpriteRenderer>();
				Supper.sprite = sh.upperBody.GetSprite(dude+"_"+upper);
				Supper.color = upperColor;
			}
			if(t.name == "faceMU"){
				if(Sface == null)
					Sface = t.GetComponent<SpriteRenderer>();
				Sface.sprite = sh.faceMU.GetSprite(face);
				Sface.color = faceColor;
			}
			if(t.name == "settling"){
				if(SSettle == null)
				SSettle = t.GetComponent<SpriteRenderer>();
				string settle = settling? dude:"none";
				SSettle.sprite = sh.settling.GetSprite(settle);
			}
			if(t.name == "error"){
				if(SError == null)
					SError = t.GetComponent<SpriteRenderer>();
				string err = error? "yes":"no";
				SError.sprite = sh.error.GetSprite(err);
			}
		}
	}

	

}
