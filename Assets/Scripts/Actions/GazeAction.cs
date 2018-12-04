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
		Transform playerTransform = Subject.transform.Find (Subject.name).transform;
        if (current >= end || !shouldExecute())
        {
			Subject.lookat (playerTransform.position);
			Subject.setError (false);
	        return;
		}
		if (go != null) {
			Transform child = go.transform.Find (param);
			Subject.lookat (child.position);

		}else if(param == "down"){
			Subject.lookat (playerTransform.position + new Vector3 (0, -0.9f, 0));

        }
        else if (param == "self")
        {
            Subject.lookat(playerTransform.position + new Vector3(0, 0, 0));

        }
        else if(param == "up"){
			Subject.lookat (playerTransform.position + new Vector3 (0, +1.8f, 0)); 
		
		}else if(param == "free"){
			if(Subject.transform.position.x < 0)
				Subject.lookat (playerTransform.position+ new Vector3 (0.3f, 0.9f, 0) );
			else
				Subject.lookat (playerTransform.position+ new Vector3 (-0.3f, 0.9f, 0) );

        }
        else if (param == "home") {
			Subject.lookat (Subject.getStartPosition()); 
		
		}else {
			Debug.Log("Gaze target failed: " + param);
			Subject.setError (true);
		}
	}

    public override string GetClassName()
    {
        return "GazeAction";
    }

	public override string ToString ()
	{
		return string.Format ("Gaze Endpoint");
	}


    public override string GetDescriptionString()
    {
        return "Gaze Action: Assign to each parameter in the .csv file, the name of one element in the unity Scene that you set up. "
        + "This action offers the following alternatives: home for starting position, free for a straight direction in front of you, "
        + "up and down for relative positions to the character, self to look at the own chest.";
    }
}
	