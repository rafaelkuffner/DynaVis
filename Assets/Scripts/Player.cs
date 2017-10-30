using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	// 3D Vars
	bool prefabLoaded = false;
	Transform dude3D; // current dude3D prefab
	Transform head; // current head
	Transform headTexture; // current face texture
	Transform headTextureSitting;
	Transform headTextureStand;
	Transform dude3DStand;
	Transform dude3DSit;
	Transform headSitting;
	Transform headStand;
	SpriteRenderer Sface3D;
	Transform chair;
	Material dude3DDefaultMaterial;
	LineRenderer lineRenderer3D;

	Dictionary<string, List<Transform>> prefabListByTagSitting;
	Dictionary<string, List<Transform>> prefabListByTagStand;
	List<Transform> prefabListDudeSitting;
	List<Transform> prefabListDudeStand;

	Vector3 startPosition;

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
        whatToPlay = new IndividualSettings();

		dude2D = GetComponent<Transform> ().Find(this.name + "2D").transform;

		prefabListByTagSitting = new Dictionary<string, List<Transform>> ();
		prefabListByTagStand = new Dictionary<string, List<Transform>> ();
		prefabListDudeSitting = new List<Transform> ();
		prefabListDudeStand = new List<Transform> ();

		dude3D = dude3DSit = GetComponent<Transform> ().Find(this.name + "3D").transform;
		dude3DStand = GetComponent<Transform> ().Find(this.name + "3DStand").transform;

		Debug.Log("player prefab 3D found - " + this.name);
		dude3DDefaultMaterial = Resources.Load("Materials/mannikin body", typeof(Material)) as Material;

        Reset();
	}

	public Vector3 getStartPosition(){
		return startPosition;
	}
    
    #region 3D
    	
    public Vector3 getHeadPosition(){
		return head.transform.position;
	}

	public Transform getFaceMU3D() {
		return headTexture;
	} 

	public Vector3 getStartPosition3D(){
		return dude3D.position;
	}

	public void setupPrefabs(){
		setupPrefabsSitUpperBody ();
		setupPrefabsSitLowerBody ();
		setupPrefabsStandUpperBody ();
		setupPrefabsStandLowerBody ();
		prefabLoaded = true;
	}

	public void setupPrefabsSitUpperBody() {

		Transform upperBodyPrefab = dude3DSit.Find ("upper_body").transform;
		Transform upperTorsoPrefab = upperBodyPrefab.Find ("upper_torso").transform;
		// limb left
		Transform leftArmPrefab = upperBodyPrefab.Find ("left_arm").transform;
		Transform leftHandPrefab = leftArmPrefab.Find ("hand").transform;
		// arm left
		Transform leftUpperArm = leftArmPrefab.Find("upper_arm").transform;
		Transform leftLowerArm = leftArmPrefab.Find("lower_arm").transform;

		//limb right
		Transform rightArmPrefab = upperBodyPrefab.Find ("right_arm").transform;
		Transform rightHandPrefab = rightArmPrefab.Find ("hand").transform;
		//arm right
		Transform rightUpperArm = rightArmPrefab.Find("upper_arm").transform;
		Transform rightLowerArm = rightArmPrefab.Find("lower_arm").transform;

		Transform leftShoulderPrefab = upperBodyPrefab.Find ("left_shoulder").transform;
		Transform rightShoulderPrefab = upperBodyPrefab.Find ("right_shoulder").transform;

		prefabListDudeSitting.Add (upperTorsoPrefab);
		prefabListDudeSitting.Add (leftHandPrefab);
		prefabListDudeSitting.Add (leftUpperArm);
		prefabListDudeSitting.Add (leftLowerArm);
		prefabListDudeSitting.Add (rightHandPrefab);
		prefabListDudeSitting.Add (rightUpperArm);
		prefabListDudeSitting.Add (rightLowerArm);
		prefabListDudeSitting.Add (leftShoulderPrefab);
		prefabListDudeSitting.Add (rightShoulderPrefab);

		prefabListByTagSitting.Add ("sit_arm_left", new List<Transform>());
		prefabListByTagSitting ["sit_arm_left"].Add (leftUpperArm);
		prefabListByTagSitting ["sit_arm_left"].Add (leftLowerArm);
		prefabListByTagSitting.Add ("sit_arm_right", new List<Transform>());
		prefabListByTagSitting ["sit_arm_right"].Add (rightUpperArm);
		prefabListByTagSitting ["sit_arm_right"].Add (rightLowerArm);
		prefabListByTagSitting.Add ("sit_arms_both", new List<Transform> ());
		prefabListByTagSitting ["sit_arms_both"].Add (leftUpperArm);
		prefabListByTagSitting ["sit_arms_both"].Add (leftLowerArm);
		prefabListByTagSitting ["sit_arms_both"].Add (rightUpperArm);
		prefabListByTagSitting ["sit_arms_both"].Add (rightLowerArm);
		prefabListByTagSitting.Add ("sit_hand_left", new List<Transform> ());
		prefabListByTagSitting ["sit_hand_left"].Add (leftHandPrefab);
		prefabListByTagSitting.Add ("sit_hand_right", new List<Transform> ());
		prefabListByTagSitting ["sit_hand_right"].Add (rightHandPrefab);
		prefabListByTagSitting.Add ("sit_hands_both", new List<Transform> ());
		prefabListByTagSitting ["sit_hands_both"].Add (leftHandPrefab);
		prefabListByTagSitting ["sit_hands_both"].Add (rightHandPrefab);
		prefabListByTagSitting.Add ("sit_none", new List<Transform> ()); // if list is empty the dummy should be all the same color
		prefabListByTagSitting.Add("sit_shoulder_left", new List<Transform>());
		prefabListByTagSitting ["sit_shoulder_left"].Add (leftShoulderPrefab);
		prefabListByTagSitting.Add("sit_shoulder_right", new List<Transform>());
		prefabListByTagSitting ["sit_shoulder_right"].Add (rightShoulderPrefab);
		prefabListByTagSitting.Add ("sit_shoulders_both", new List<Transform> ());
		prefabListByTagSitting ["sit_shoulders_both"].Add (leftShoulderPrefab);
		prefabListByTagSitting ["sit_shoulders_both"].Add (rightShoulderPrefab);
		prefabListByTagSitting.Add ("sit_ub_all", new List<Transform> ());
		prefabListByTagSitting ["sit_ub_all"].Add (leftUpperArm);
		prefabListByTagSitting ["sit_ub_all"].Add (leftLowerArm);
		prefabListByTagSitting ["sit_ub_all"].Add (leftHandPrefab);
		prefabListByTagSitting ["sit_ub_all"].Add (rightUpperArm);
		prefabListByTagSitting ["sit_ub_all"].Add (rightLowerArm);
		prefabListByTagSitting ["sit_ub_all"].Add (rightHandPrefab);
		prefabListByTagSitting ["sit_ub_all"].Add (upperTorsoPrefab);
		prefabListByTagSitting.Add ("sit_ub_limb_both", new List<Transform> ());
		prefabListByTagSitting ["sit_ub_limb_both"].Add (leftUpperArm);
		prefabListByTagSitting ["sit_ub_limb_both"].Add (leftLowerArm);
		prefabListByTagSitting ["sit_ub_limb_both"].Add (leftHandPrefab);
		prefabListByTagSitting ["sit_ub_limb_both"].Add (rightUpperArm);
		prefabListByTagSitting ["sit_ub_limb_both"].Add (rightLowerArm);
		prefabListByTagSitting ["sit_ub_limb_both"].Add (rightHandPrefab);
		prefabListByTagSitting.Add ("sit_ub_limb_left", new List<Transform> ());
		prefabListByTagSitting ["sit_ub_limb_left"].Add (leftUpperArm);
		prefabListByTagSitting ["sit_ub_limb_left"].Add (leftLowerArm);
		prefabListByTagSitting ["sit_ub_limb_left"].Add (leftHandPrefab);
		prefabListByTagSitting.Add ("sit_ub_limb_right", new List<Transform> ());
		prefabListByTagSitting ["sit_ub_limb_right"].Add (rightUpperArm);
		prefabListByTagSitting ["sit_ub_limb_right"].Add (rightLowerArm);
		prefabListByTagSitting ["sit_ub_limb_right"].Add (rightHandPrefab);

		//SETUP SPRITE FACE
		headSitting = upperBodyPrefab.Find("head");  
		head = headSitting;
		headTexture = headTextureSitting = head.Find ("faceMU3D").transform;


	}

	public void setupPrefabsStandUpperBody() {

		Transform upperBodyPrefab = dude3DStand.Find ("upper_body").transform;
		Transform upperTorsoPrefab = upperBodyPrefab.Find ("upper_torso").transform;
		// limb left
		Transform leftArmPrefab = upperBodyPrefab.Find ("left_arm").transform;
		Transform leftHandPrefab = leftArmPrefab.Find ("hand").transform;
		// arm left
		Transform leftUpperArm = leftArmPrefab.Find("upper_arm").transform;
		Transform leftLowerArm = leftArmPrefab.Find("lower_arm").transform;

		//limb right
		Transform rightArmPrefab = upperBodyPrefab.Find ("right_arm").transform;
		Transform rightHandPrefab = rightArmPrefab.Find ("hand").transform;
		//arm right
		Transform rightUpperArm = rightArmPrefab.Find("upper_arm").transform;
		Transform rightLowerArm = rightArmPrefab.Find("lower_arm").transform;

		Transform leftShoulderPrefab = upperBodyPrefab.Find ("left_shoulder").transform;
		Transform rightShoulderPrefab = upperBodyPrefab.Find ("right_shoulder").transform;

		prefabListDudeStand.Add (upperTorsoPrefab);
		prefabListDudeStand.Add (leftHandPrefab);
		prefabListDudeStand.Add (leftUpperArm);
		prefabListDudeStand.Add (leftLowerArm);
		prefabListDudeStand.Add (rightHandPrefab);
		prefabListDudeStand.Add (rightUpperArm);
		prefabListDudeStand.Add (rightLowerArm);
		prefabListDudeStand.Add (leftShoulderPrefab);
		prefabListDudeStand.Add (rightShoulderPrefab);

		prefabListByTagStand.Add ("sit_arm_left", new List<Transform>());
		prefabListByTagStand ["sit_arm_left"].Add (leftUpperArm);
		prefabListByTagStand ["sit_arm_left"].Add (leftLowerArm);
		prefabListByTagStand.Add ("sit_arm_right", new List<Transform>());
		prefabListByTagStand ["sit_arm_right"].Add (rightUpperArm);
		prefabListByTagStand ["sit_arm_right"].Add (rightLowerArm);
		prefabListByTagStand.Add ("sit_arms_both", new List<Transform> ());
		prefabListByTagStand ["sit_arms_both"].Add (leftUpperArm);
		prefabListByTagStand ["sit_arms_both"].Add (leftLowerArm);
		prefabListByTagStand ["sit_arms_both"].Add (rightUpperArm);
		prefabListByTagStand ["sit_arms_both"].Add (rightLowerArm);
		prefabListByTagStand.Add ("sit_hand_left", new List<Transform> ());
		prefabListByTagStand ["sit_hand_left"].Add (leftHandPrefab);
		prefabListByTagStand.Add ("sit_hand_right", new List<Transform> ());
		prefabListByTagStand ["sit_hand_right"].Add (rightHandPrefab);
		prefabListByTagStand.Add ("sit_hands_both", new List<Transform> ());
		prefabListByTagStand ["sit_hands_both"].Add (leftHandPrefab);
		prefabListByTagStand ["sit_hands_both"].Add (rightHandPrefab);
		prefabListByTagStand.Add ("sit_none", new List<Transform> ()); // if list is empty the dummy should be all the same color
		prefabListByTagStand.Add("sit_shoulder_left", new List<Transform>());
		prefabListByTagStand ["sit_shoulder_left"].Add (leftShoulderPrefab);
		prefabListByTagStand.Add("sit_shoulder_right", new List<Transform>());
		prefabListByTagStand ["sit_shoulder_right"].Add (rightShoulderPrefab);
		prefabListByTagStand.Add ("sit_shoulders_both", new List<Transform> ());
		prefabListByTagStand ["sit_shoulders_both"].Add (leftShoulderPrefab);
		prefabListByTagStand ["sit_shoulders_both"].Add (rightShoulderPrefab);
		prefabListByTagStand.Add ("sit_ub_all", new List<Transform> ());
		prefabListByTagStand ["sit_ub_all"].Add (leftUpperArm);
		prefabListByTagStand ["sit_ub_all"].Add (leftLowerArm);
		prefabListByTagStand ["sit_ub_all"].Add (leftHandPrefab);
		prefabListByTagStand ["sit_ub_all"].Add (rightUpperArm);
		prefabListByTagStand ["sit_ub_all"].Add (rightLowerArm);
		prefabListByTagStand ["sit_ub_all"].Add (rightHandPrefab);
		prefabListByTagStand ["sit_ub_all"].Add (upperTorsoPrefab);
		prefabListByTagStand.Add ("sit_ub_limb_both", new List<Transform> ());
		prefabListByTagStand ["sit_ub_limb_both"].Add (leftUpperArm);
		prefabListByTagStand ["sit_ub_limb_both"].Add (leftLowerArm);
		prefabListByTagStand ["sit_ub_limb_both"].Add (leftHandPrefab);
		prefabListByTagStand ["sit_ub_limb_both"].Add (rightUpperArm);
		prefabListByTagStand ["sit_ub_limb_both"].Add (rightLowerArm);
		prefabListByTagStand ["sit_ub_limb_both"].Add (rightHandPrefab);
		prefabListByTagStand.Add ("sit_ub_limb_left", new List<Transform> ());
		prefabListByTagStand ["sit_ub_limb_left"].Add (leftUpperArm);
		prefabListByTagStand ["sit_ub_limb_left"].Add (leftLowerArm);
		prefabListByTagStand ["sit_ub_limb_left"].Add (leftHandPrefab);
		prefabListByTagStand.Add ("sit_ub_limb_right", new List<Transform> ());
		prefabListByTagStand ["sit_ub_limb_right"].Add (rightUpperArm);
		prefabListByTagStand ["sit_ub_limb_right"].Add (rightLowerArm);
		prefabListByTagStand ["sit_ub_limb_right"].Add (rightHandPrefab);

		//SETUP SPRITE FACE
		headStand = upperBodyPrefab.Find("head");  
		headTextureStand = headStand.Find ("faceMU3D").transform;
	}

	public void setupPrefabsSitLowerBody(){
	
		Transform lowerBodyPrefab = dude3D.Find ("lower_body").transform;
		Transform upperBodyPrefab = dude3D.Find ("upper_body").transform;
		Transform waistPrefab = upperBodyPrefab.Find ("lower_torso").transform;
		Transform leftLegPrefab = lowerBodyPrefab.Find ("left_leg").transform;
		Transform upperLeftLegPrefab = leftLegPrefab.Find ("upper_leg").transform;
		Transform lowerLeftLegPrefab = leftLegPrefab.Find ("lower_leg").transform;
		Transform rightLegPrefab = lowerBodyPrefab.Find ("right_leg").transform;
		Transform upperRightLegPrefab = rightLegPrefab.Find ("upper_leg").transform;
		Transform lowerRightLegPrefab = rightLegPrefab.Find ("lower_leg").transform;
		Transform leftFootPrefab = lowerBodyPrefab.Find ("left_foot").transform;
		Transform rightFootPrefab = lowerBodyPrefab.Find ("right_foot").transform;

		prefabListDudeSitting.Add (waistPrefab);
		prefabListDudeSitting.Add (upperLeftLegPrefab);
		prefabListDudeSitting.Add (lowerLeftLegPrefab);
		prefabListDudeSitting.Add (upperRightLegPrefab);
		prefabListDudeSitting.Add (lowerRightLegPrefab);
		prefabListDudeSitting.Add (leftFootPrefab);
		prefabListDudeSitting.Add (rightFootPrefab);

		prefabListByTagSitting.Add ("sit_feet_both", new List<Transform> ());
		prefabListByTagSitting ["sit_feet_both"].Add (leftFootPrefab);
		prefabListByTagSitting ["sit_feet_both"].Add (rightFootPrefab);

		prefabListByTagSitting.Add ("sit_foot_left", new List<Transform> ());
		prefabListByTagSitting ["sit_foot_left"].Add (leftFootPrefab);

		prefabListByTagSitting.Add ("sit_foot_right", new List<Transform> ());
		prefabListByTagSitting ["sit_foot_right"].Add (rightFootPrefab);

		prefabListByTagSitting.Add ("sit_hip_s", new List<Transform> ());
		prefabListByTagSitting ["sit_hip_s"].Add (waistPrefab);

		prefabListByTagSitting.Add ("sit_lb_all", new List<Transform> ());
		prefabListByTagSitting ["sit_lb_all"].Add (waistPrefab);
		prefabListByTagSitting ["sit_lb_all"].Add (upperLeftLegPrefab);
		prefabListByTagSitting ["sit_lb_all"].Add (lowerLeftLegPrefab);
		prefabListByTagSitting ["sit_lb_all"].Add (leftFootPrefab);
		prefabListByTagSitting ["sit_lb_all"].Add (upperRightLegPrefab);
		prefabListByTagSitting ["sit_lb_all"].Add (lowerRightLegPrefab);
		prefabListByTagSitting ["sit_lb_all"].Add (rightFootPrefab);

		prefabListByTagSitting.Add ("sit_lb_limb_left", new List<Transform> ());
		prefabListByTagSitting ["sit_lb_limb_left"].Add (upperLeftLegPrefab);
		prefabListByTagSitting ["sit_lb_limb_left"].Add (lowerLeftLegPrefab);
		prefabListByTagSitting ["sit_lb_limb_left"].Add (leftFootPrefab);

		prefabListByTagSitting.Add ("sit_lb_limb_right", new List<Transform> ());
		prefabListByTagSitting ["sit_lb_limb_right"].Add (upperRightLegPrefab);
		prefabListByTagSitting ["sit_lb_limb_right"].Add (lowerRightLegPrefab);
		prefabListByTagSitting ["sit_lb_limb_right"].Add (rightFootPrefab);

		prefabListByTagSitting.Add ("sit_lb_limbs_both", new List<Transform> ());
		prefabListByTagSitting ["sit_lb_limbs_both"].Add (upperLeftLegPrefab);
		prefabListByTagSitting ["sit_lb_limbs_both"].Add (lowerLeftLegPrefab);
		prefabListByTagSitting ["sit_lb_limbs_both"].Add (leftFootPrefab);
		prefabListByTagSitting ["sit_lb_limbs_both"].Add (upperRightLegPrefab);
		prefabListByTagSitting ["sit_lb_limbs_both"].Add (lowerRightLegPrefab);
		prefabListByTagSitting ["sit_lb_limbs_both"].Add (rightFootPrefab);

		prefabListByTagSitting.Add ("sit_leg_left", new List<Transform> ());
		prefabListByTagSitting ["sit_leg_left"].Add (upperLeftLegPrefab);
		prefabListByTagSitting ["sit_leg_left"].Add (lowerLeftLegPrefab);

		prefabListByTagSitting.Add ("sit_leg_right", new List<Transform> ());
		prefabListByTagSitting ["sit_leg_right"].Add (upperRightLegPrefab);
		prefabListByTagSitting ["sit_leg_right"].Add (lowerRightLegPrefab);

		prefabListByTagSitting.Add ("sit_legs_both", new List<Transform> ());
		prefabListByTagSitting ["sit_legs_both"].Add (upperLeftLegPrefab);
		prefabListByTagSitting ["sit_legs_both"].Add (lowerLeftLegPrefab);
		prefabListByTagSitting ["sit_legs_both"].Add (upperRightLegPrefab);
		prefabListByTagSitting ["sit_legs_both"].Add (lowerRightLegPrefab);

		//prefabListByTag.Add ("sit_none", new List<Transform> ()); //empty list, dummy default color

	}

    public Transform getDude3D()
    {
        return dude3D;
    }

    public void setupPrefabsStandLowerBody(){

		Transform lowerBodyPrefab = dude3DStand.Find ("lower_body").transform;
		Transform upperBodyPrefab = dude3DStand.Find ("upper_body").transform;
		Transform waistPrefab = upperBodyPrefab.Find ("lower_torso").transform;
		Transform leftLegPrefab = lowerBodyPrefab.Find ("left_leg").transform;
		Transform upperLeftLegPrefab = leftLegPrefab.Find ("upper_leg").transform;
		Transform lowerLeftLegPrefab = leftLegPrefab.Find ("lower_leg").transform;
		Transform rightLegPrefab = lowerBodyPrefab.Find ("right_leg").transform;
		Transform upperRightLegPrefab = rightLegPrefab.Find ("upper_leg").transform;
		Transform lowerRightLegPrefab = rightLegPrefab.Find ("lower_leg").transform;
		Transform leftFootPrefab = lowerBodyPrefab.Find ("left_foot").transform;
		Transform rightFootPrefab = lowerBodyPrefab.Find ("right_foot").transform;

		prefabListDudeStand.Add (waistPrefab);
		prefabListDudeStand.Add (upperLeftLegPrefab);
		prefabListDudeStand.Add (lowerLeftLegPrefab);
		prefabListDudeStand.Add (upperRightLegPrefab);
		prefabListDudeStand.Add (lowerRightLegPrefab);
		prefabListDudeStand.Add (leftFootPrefab);
		prefabListDudeStand.Add (rightFootPrefab);

		prefabListByTagStand.Add ("sit_feet_both", new List<Transform> ());
		prefabListByTagStand ["sit_feet_both"].Add (leftFootPrefab);
		prefabListByTagStand ["sit_feet_both"].Add (rightFootPrefab);

		prefabListByTagStand.Add ("sit_foot_left", new List<Transform> ());
		prefabListByTagStand ["sit_foot_left"].Add (leftFootPrefab);

		prefabListByTagStand.Add ("sit_foot_right", new List<Transform> ());
		prefabListByTagStand ["sit_foot_right"].Add (rightFootPrefab);

		prefabListByTagStand.Add ("sit_hip_s", new List<Transform> ());
		prefabListByTagStand ["sit_hip_s"].Add (waistPrefab);

		prefabListByTagStand.Add ("sit_lb_all", new List<Transform> ());
		prefabListByTagStand ["sit_lb_all"].Add (waistPrefab);
		prefabListByTagStand ["sit_lb_all"].Add (upperLeftLegPrefab);
		prefabListByTagStand ["sit_lb_all"].Add (lowerLeftLegPrefab);
		prefabListByTagStand ["sit_lb_all"].Add (leftFootPrefab);
		prefabListByTagStand ["sit_lb_all"].Add (upperRightLegPrefab);
		prefabListByTagStand ["sit_lb_all"].Add (lowerRightLegPrefab);
		prefabListByTagStand ["sit_lb_all"].Add (rightFootPrefab);

		prefabListByTagStand.Add ("sit_lb_limb_left", new List<Transform> ());
		prefabListByTagStand ["sit_lb_limb_left"].Add (upperLeftLegPrefab);
		prefabListByTagStand ["sit_lb_limb_left"].Add (lowerLeftLegPrefab);
		prefabListByTagStand ["sit_lb_limb_left"].Add (leftFootPrefab);

		prefabListByTagStand.Add ("sit_lb_limb_right", new List<Transform> ());
		prefabListByTagStand ["sit_lb_limb_right"].Add (upperRightLegPrefab);
		prefabListByTagStand ["sit_lb_limb_right"].Add (lowerRightLegPrefab);
		prefabListByTagStand ["sit_lb_limb_right"].Add (rightFootPrefab);

		prefabListByTagStand.Add ("sit_lb_limbs_both", new List<Transform> ());
		prefabListByTagStand ["sit_lb_limbs_both"].Add (upperLeftLegPrefab);
		prefabListByTagStand ["sit_lb_limbs_both"].Add (lowerLeftLegPrefab);
		prefabListByTagStand ["sit_lb_limbs_both"].Add (leftFootPrefab);
		prefabListByTagStand ["sit_lb_limbs_both"].Add (upperRightLegPrefab);
		prefabListByTagStand ["sit_lb_limbs_both"].Add (lowerRightLegPrefab);
		prefabListByTagStand ["sit_lb_limbs_both"].Add (rightFootPrefab);

		prefabListByTagStand.Add ("sit_leg_left", new List<Transform> ());
		prefabListByTagStand ["sit_leg_left"].Add (upperLeftLegPrefab);
		prefabListByTagStand ["sit_leg_left"].Add (lowerLeftLegPrefab);

		prefabListByTagStand.Add ("sit_leg_right", new List<Transform> ());
		prefabListByTagStand ["sit_leg_right"].Add (upperRightLegPrefab);
		prefabListByTagStand ["sit_leg_right"].Add (lowerRightLegPrefab);

		prefabListByTagStand.Add ("sit_legs_both", new List<Transform> ());
		prefabListByTagStand ["sit_legs_both"].Add (upperLeftLegPrefab);
		prefabListByTagStand ["sit_legs_both"].Add (lowerLeftLegPrefab);
		prefabListByTagStand ["sit_legs_both"].Add (upperRightLegPrefab);
		prefabListByTagStand ["sit_legs_both"].Add (lowerRightLegPrefab);

		//prefabListByTag.Add ("sit_none", new List<Transform> ()); //empty list, dummy default color

	}
    #endregion

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
		updatePrefabs (); // TO DO! CHECK IF THIS IS OKAY! 
	}

	public Transform getDude2D(){
		return dude2D;
	}

	public void setError(bool value){
		error = value;
	}

	public void changeStance(string dude){
		this.dude = dude;
		updateSprites ();

		if (dude.Equals ("sit")) {
			dude3D = dude3DSit;
			dude3DStand.gameObject.SetActive (false);
			dude3DSit.gameObject.SetActive (true);
			head = headSitting;
			headTexture = headTextureSitting;
		} else {
			dude3D = dude3DStand;
			dude3DStand.gameObject.SetActive (true);
			dude3DSit.gameObject.SetActive (false);
			head = headStand;
			headTexture = headTextureStand;
		}
	}

	public void settle(bool state){
		this.settling = state;
		updateSprites ();
		updatePrefabs ();
	}

	public void faceExpression(string expr,Color color){
		this.face = expr;
		this.faceColor = color;
		updateSprites ();
		updatePrefabs ();

	}

	public void highlightUpper(string param,Color color){
		this.upper = param;
		this.upperColor = color;
		updateSprites ();
		updatePrefabs ();
	}

	public void highlightLower(string param,Color color){
		this.lower = param;
		this.lowerColor = color;
		updateSprites ();
		updatePrefabs ();
	}

	public void lookat (Vector3 end){
		if(lineRenderer2D == null) 
			lineRenderer2D = dude2D.GetComponent<LineRenderer>();
		if (end == dude2D.position) {
			lineRenderer2D.enabled = false;
			return;
		}
		lineRenderer2D.enabled = true;
        //lineRenderer2D.positionCount = 3;
		lineRenderer2D.numPositions = 3;
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

	public void lookat3D (Vector3 end){
		if(lineRenderer3D == null) 
			lineRenderer3D = dude3D.GetComponent<LineRenderer>();
		if (end == dude3D.position) {
			lineRenderer3D.enabled = false;
			return;
		}
		lineRenderer3D.enabled = true;
		//lineRenderer3D.positionCount = 3;
		lineRenderer2D.numPositions = 3;
		lineRenderer3D.SetPosition (0, head.transform.position);
		Vector3 diff = end-(head.transform.position);
		diff.x *= 0.01f;diff.y *= 0.01f;diff.z *= 0.01f;
		lineRenderer3D.SetPosition (1, end-diff);
		lineRenderer3D.SetPosition (2, end+diff);
        lineRenderer3D.startWidth = 0.525f;
        lineRenderer3D.endWidth =  0.525f;
        lineRenderer3D.startColor = Color.cyan;
        lineRenderer3D.endColor = Color.blue;
		lineRenderer3D.sortingOrder = 4;
	}

	void updateSprites(){
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

	public void updateFaceExpression3D(Sprite expr, Color color){
		Sface3D = headTexture.GetComponentInChildren<SpriteRenderer>();
		Sface3D.sprite = expr;
		Sface3D.color = color;
	}

	public void highlightPrefabs(string param, Color color)
	{		
		if (dude.Equals ("sit")) {
			//update sit
			if (!prefabListByTagSitting.ContainsKey (param))
				Debug.Log ("ERROR: the key - " + param + " doesnt exist in prefabListByTag");
			else {
				List<Transform> prefabListByTagToBeUpdated = prefabListByTagSitting [param];

				if (prefabListByTagToBeUpdated.Count == 0) {
					foreach (Transform transform in prefabListDudeSitting) {
						transform.GetComponent<Renderer> ().material.color = dude3DDefaultMaterial.color;
						transform.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", dude3DDefaultMaterial.color);
						transform.GetComponent<Renderer> ().material.color = dude3DDefaultMaterial.color;
					}
				}

				foreach (Transform transform in prefabListByTagToBeUpdated) {
					transform.GetComponent<Renderer> ().material.color = color;
					transform.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", color);
					transform.GetComponent<Renderer> ().material.color = color;

				}
			}
		}  else {
			// update stand
			if (!prefabListByTagStand.ContainsKey (param))
				Debug.Log ("ERROR: the key - " + param + " doesnt exist in prefabListByTag");
			else {
				List<Transform> prefabListByTagToBeUpdated = prefabListByTagStand [param];

				if (prefabListByTagToBeUpdated.Count == 0) {
					foreach (Transform transform in prefabListDudeStand) {
						transform.GetComponent<Renderer> ().material.color = dude3DDefaultMaterial.color;
						transform.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", dude3DDefaultMaterial.color);
						transform.GetComponent<Renderer> ().material.color = dude3DDefaultMaterial.color;
					}
				}

				foreach (Transform transform in prefabListByTagToBeUpdated) {
					transform.GetComponent<Renderer> ().material.color = color;
					transform.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", color);
					transform.GetComponent<Renderer> ().material.color = color;

				}
			}

		}
	}



	void updatePrefabs() {

		if (!prefabLoaded)
			setupPrefabs ();
	
		highlightPrefabs (dude + "_" + upper, upperColor);
		highlightPrefabs (dude + "_" + lower,lowerColor);

		updateFaceExpression3D (sh.faceMU.GetSprite (face),faceColor);

	
	}


}
