using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	public Camera camera3D;
	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {

		if(camera3D != null && camera3D.enabled)
			transform.LookAt(camera3D.transform.position, Vector3.up);
	}
}
