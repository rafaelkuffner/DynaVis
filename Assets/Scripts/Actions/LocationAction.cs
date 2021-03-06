using UnityEngine;
using System.Collections;
using System;

public class LocationAction : Action {

	public enum ActionType{
		goTo,sit,stand,settling
	}

	ActionType t;

	Vector2 startLocation2D;
	Vector3 startLocation3D;
	GameObject target;

	public LocationAction():base(0,0,null,""){

	}
	public LocationAction(int start, int end, Player subject, string param):base(start,end,subject,param){
       
        char[] delim = { '_' };
        string[] parts = param.Split(delim);
        this.t= (ActionType)Enum.Parse(typeof(ActionType), parts[0]);
        this.param = parts[parts.Length - 1];
        this.target = GameObject.Find(this.param);
		startLocation2D = new Vector2(int.MaxValue,int.MaxValue);
		startLocation3D = new Vector3 (int.MaxValue, int.MaxValue, int.MaxValue);
	}

	


	public override void execute (int current)
    {
       // if (!shouldExecute()) return;
		switch (t) 
		{
		case ActionType.goTo:
			executeGoto (current);
			break;
		case ActionType.settling:
			executeSettling(current);
			break;
		case ActionType.sit:
			executeSit(current);
			break;
		case ActionType.stand:
			executeStand(current);
			break;
			
		}
	}

	void executeGoto(int current){
		Subject.changeStance ("stand");
		if (startLocation2D == new Vector2 (int.MaxValue, int.MaxValue))
			startLocation2D = Subject.getDude2D ().position;
		if (target != null) {
			string targetName = target.name;
			Transform child = target.transform.Find (targetName + "2D"); // get the 2D representation of the TARGET!!!
			if (child == null)
				Debug.Log ("target name = " + targetName);

			Vector2 targLoc = child.position; 
			float lerp = ((float)current - start) / ((float)end - start);
			Vector2 newLoc = Vector2.Lerp (startLocation2D, targLoc, lerp);
			Subject.getDude2D ().position = newLoc;
		} else {
			if (current < end ) {
				Debug.Log("Goto target failed: " + param);
				Subject.setError (true);
			} else {
				Subject.setError (false);
			}
		}
	}

	void executeSettling(int current){
		if (current < this.end)
			Subject.settle (true);
		else
			Subject.settle (false);
	}

	void executeSit(int current){
		if (startLocation2D == new Vector2(int.MaxValue,int.MaxValue))
			startLocation2D = Subject.getDude2D ().position;
		Subject.changeStance ("sit");
		if (param == "part") {
			Subject.getDude2D ().position = startLocation2D + new Vector2(0.1f,-0.1f);
		}

	}
	void executeStand(int current){
		Subject.changeStance ("stand");
	}


	public override void execute3D(int current)
	{
		switch (t) 
		{
		case ActionType.goTo:
			executeGoto3D (current);
			break;
		case ActionType.settling:
			executeSettling(current);
			break;
		case ActionType.sit:
			executeSit(current);
			break;
		case ActionType.stand:
			executeStand(current);
			break;

		}
	}

	void executeGoto3D(int current){
		Subject.changeStance ("stand");
		if (startLocation3D == new Vector3 (int.MaxValue, int.MaxValue, int.MaxValue))
			startLocation3D = Subject.getDude3D ().position;
		if (target != null) {
			string targetName = target.name;
			Transform child = target.transform.Find (targetName + "3D"); // get the 3D representation of the TARGET!!!
			if (child == null)
				Debug.Log ("target name = " + targetName);

			Vector3 targLoc = child.position; 
			float lerp = ((float)current - start) / ((float)end - start);
			Vector3 tmp = Vector3.Lerp (startLocation3D, targLoc, lerp * 0.7f);
			Vector3 newLoc = tmp - new Vector3 (0.0f, -7.0f, 0.0f);
			Subject.getDude3D ().position = newLoc;

		} else {
			if (current < end ) {
				Debug.Log("Goto target failed: " + param);
				Subject.setError (true);
			} else {
				Subject.setError (false);
			}
		}
	}

	public override string ToString ()
	{
		return string.Format ("Location");
	}
}
