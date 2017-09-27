using UnityEngine;
using System.Collections;

public class GazeAction : Action {

	public GazeAction():base(0,0,null,""){
		
	}

	public GazeAction(int start, int end, Player subject, string param):base(start,end,subject,param){

	}
	
	
	
	public override void execute (int current)
	{
		GameObject go = GameObject.Find (param);
		Transform playerTransform = Subject.transform.Find (Subject.name + "2D").transform;
        if (current >= end || !shouldExecute())
        {
			Subject.lookat (playerTransform.position);
			Subject.setError (false);
	        return;
		}
		if (go != null) {
			Transform child = go.transform.Find (param + "2D");
			Subject.lookat (child.position + new Vector3 (0, 0.5f, 0));

		}else if(param == "down"){
			Subject.lookat (playerTransform.position + new Vector3 (0, -0.9f, 0));

		}else if(param == "up"){
			Subject.lookat (playerTransform.position + new Vector3 (0, +1.8f, 0)); 
		
		}else if(param == "free"){
			if(Subject.transform.position.x < 0)
				Subject.lookat (playerTransform.position+ new Vector3 (0.3f, 0.9f, 0) );
			else
				Subject.lookat (playerTransform.position+ new Vector3 (-0.3f, 0.9f, 0) );

		}else if(param == "home"){
			Subject.lookat (Subject.getStartPosition()); 
		
		}else {
			Debug.Log("Gaze target failed: " + param);
			Subject.setError (true);
		}
	}

	public override void execute3D(int current)
	{
		
		GameObject go = GameObject.Find (param);
		Transform playerTransform = Subject.transform.Find (Subject.name + "3D").transform;
		if (current >= end ||  !shouldExecute()) {
			Subject.lookat3D (playerTransform.position); 
			Subject.setError (false);
			return;
		}
		if (go != null) {
			Transform child = go.transform.Find (param + "3D");
			if (child.name.Contains ("p")) {
				Player player = go.GetComponent<Player> ();
				Subject.lookat3D (player.getHeadPosition() + new Vector3 (0, 0.5f, 0));
			}
			else
				Subject.lookat3D (child.position + new Vector3 (0, 5.5f, 0));

		}else if(param == "down"){
			Subject.lookat3D (playerTransform.position + new Vector3 (0, -0.9f, 0));

		}else if(param == "up"){
			Subject.lookat3D (playerTransform.position + new Vector3 (0, +1.8f, 0)); 

		}else if(param == "free"){
			if(Subject.transform.position.x < 0)
				Subject.lookat3D (playerTransform.position+ new Vector3 (0.3f, 0.9f, 0) );
			else
				Subject.lookat3D (playerTransform.position+ new Vector3 (-0.3f, 0.9f, 0) );

		}else if(param == "home"){
			Subject.lookat3D (Subject.getStartPosition3D()); 

		}else {
			Debug.Log("Gaze target failed: " + param);
			Subject.setError (true);
		} 
	}


	public override string ToString ()
	{
		return string.Format ("Gaze Endpoint");
	}
}
	