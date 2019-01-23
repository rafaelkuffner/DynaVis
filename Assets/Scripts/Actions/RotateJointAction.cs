using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateJointAction : Action {

	GameObject bodyPart;
	float degrees; 
	float startRotation;
	Vector3 axis;
	string axisstring;

	public RotateJointAction():base(0,0,null,""){

	}
	public RotateJointAction(int start, int end, Player subject, string param):base(start,end,subject,param){
		
		char[] delim = { '_' };
		string[] parts = param.Split(delim);
		if (parts.Length < 4)
			return;
		bodyPart = GameObject.Find(parts [0]);
		axisstring = parts [1];
		if(parts [1] == "x") axis = new Vector3(1,0,0);
		if(parts [1] == "y") axis = new Vector3(0,1,0);
		if(parts [1] == "z") axis = new Vector3(0,0,1);

		if (parts [3].Contains (":")) 
		{
			char [] delim2 = {':'};
			string[] angles = parts [3].Split (delim2);
			float a = float.Parse(angles[0]);
			float b = float.Parse(angles[1]);
			degrees = Random.Range (a, b);

		}else{
			degrees = float.Parse(parts [3]);
		}
		if (parts [2] == "-")
			degrees *= -1;

		startRotation = float.MaxValue;
	}




	public override void execute (int current)
	{
		if (startRotation == float.MaxValue) 
		{
			if (axisstring == "x")
				startRotation = bodyPart.transform.localEulerAngles.x;
			if (axisstring == "y")
				startRotation = bodyPart.transform.localEulerAngles.y;
			if (axisstring == "z")
				startRotation = bodyPart.transform.localEulerAngles.z;
		}
			
		float lerp = ((float)current - start) / ((float)end - start);
		float currentRot = Mathf.Lerp (startRotation, degrees, lerp);

		bodyPart.transform.Rotate (axis, currentRot);
	}
		

	public override string GetClassName()
	{
		return "RotateJointAction";
	}

	public override string ToString ()
	{
		return string.Format ("Rotate");
	}

	public override string GetDescriptionString()
	{
		return "RotateJointAction: Use this format bodyPart_axis_+or-_degrees. Degrees can have A:B format to be in the range of these values.";
	}
}
