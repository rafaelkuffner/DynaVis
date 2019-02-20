using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateJointAction : Action {

	GameObject bodyPart;
    GameObject mirrorBodyPart;
    float degrees; 
	float startRotation;
    float startMirrorRotation;
	Vector3 axis;
	string axisstring;
    bool mirror;
    bool reset;
    bool lerp;
    int mirrorMap;

	public RotateJointAction():base("",0,0,null,""){

	}
    public RotateJointAction(string tier, int start, int end, Player subject, string param)
        : base(tier,start, end, subject, param)
    {
        if (tier == "reset"){
            reset = false;
            return;
        }
		char[] delim = { '_' };
		string[] parts = tier.Split(delim);
		if (parts.Length < 3)
			return;

        if (parts[1] == "both") { 
            mirror = true;
            string mirrorbodyPartName = parts[0] + "_left";
            string bodyPartName = parts[0]+"_right";
            mirrorBodyPart = GameObject.Find(mirrorbodyPartName);
            bodyPart = GameObject.Find(bodyPartName);
        }
        else {
            string bodyPartName = parts[0] + "_" + parts[1];
		    bodyPart = GameObject.Find(bodyPartName);
        }

		axisstring = parts [2];
        if (parts[2] == "x")
        {
            axis = new Vector3(1, 0, 0);
            mirrorMap = 1;
        }
        if (parts[2] == "y")
        {
            axis = new Vector3(0, 1, 0);
            mirrorMap = -1;
        }
        if (parts[2] == "z")
        {
            axis = new Vector3(0, 0, 1);
            mirrorMap = -1;
        }

		if (param.Contains (":")) 
		{
            lerp = true;
			char [] delim2 = {':'};
			string[] angles = param.Split (delim2);
			float a = float.Parse(angles[0]);
			float b = float.Parse(angles[1]);
			degrees = Random.Range (a, b);

		}else{
            lerp = false;
            degrees = float.Parse(param);
		}
		
		startRotation = float.MaxValue;
        startMirrorRotation = float.MaxValue;
	}



    private void doLerp(GameObject bodyPart,int current, int mask, ref float startRotation)
    {
        if (startRotation == float.MaxValue)
        {
            if (axisstring == "x")
                startRotation = subject.objectRotation[bodyPart].x;
            if (axisstring == "y")
                startRotation = subject.objectRotation[bodyPart].y;
            if (axisstring == "z")
                startRotation = subject.objectRotation[bodyPart].z;
        }

        float lerp = ((float)current - start) / ((float)end - start);
        if (lerp > 1) lerp = 1;
        float targetRot = Mathf.Lerp(startRotation, startRotation + degrees*mask, lerp);

        Vector3 currentAngles = subject.objectRotation[bodyPart];
        if (axisstring == "x")
            currentAngles.x = targetRot;
        if (axisstring == "y")
            currentAngles.y = targetRot;
        if (axisstring == "z")
            currentAngles.z = targetRot;

        bodyPart.transform.localEulerAngles = currentAngles;
        subject.objectRotation[bodyPart] = currentAngles;
        //bodyPart.transform.Rotate (axis, Mathf.Deg2Rad* currentRot,Space.Self);
    }

    private void doNoLerp(GameObject bodyPart, int mask)
    {
        Vector3 angles = subject.objectRotation[bodyPart];

        if (axisstring == "x")
            angles.x = degrees * mask;
        if (axisstring == "y")
            angles.y = degrees * mask;
        if (axisstring == "z")
            angles.z = degrees * mask;

        bodyPart.transform.localEulerAngles = angles;
        subject.objectRotation[bodyPart] = angles;

    }

	public override void execute (int current)
	{
        if (tier == "reset")
        {
            if (!reset) { 
                subject.ResetRotations();
                reset = true;
            }
            return;
        }


     
        if (!lerp)
        {
            doNoLerp(bodyPart,1);
            if (mirror) doNoLerp(mirrorBodyPart, mirrorMap);

        }
        else {
            doLerp(bodyPart, current, 1, ref startRotation);
            if (mirror) doLerp(mirrorBodyPart, current,mirrorMap, ref startMirrorRotation);

        }

        if (current >= end) { 
            reset = false;
            startRotation = float.MaxValue;
            startMirrorRotation = float.MaxValue;
        }
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
		return "RotateJointAction: Use this format bodyPart_axis(x y z)_direction(+-)_degrees_mirror(yes no). Degrees can have A:B format to be in the range of these values."
            + " If you want the mirrored body part to execute, use the right part in the \"bodyPart\" component, and choose yes in mirror at the end";
	}
}
