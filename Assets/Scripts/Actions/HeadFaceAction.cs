﻿using System.Collections.Generic;

public class HeadFaceAction : ColorChangeableAction
{


    public HeadFaceAction()
        : base("", 0, 0, null, "")
    {
		
	}

    public HeadFaceAction(string tier, int start, int end, Player subject, string param)
        : base(tier,start, end, subject, param)
    {

	}
	

    public override void execute(int current)
    {
        base.execute(current);
        List<Modifier> mods = modifiersOn();
        if (current < this.end && shouldExecute())
        {
            foreach (Modifier m in mods)
            {
                m.modify(this);
            }
            Subject.faceExpression(param, ActionColor);
        }
        else
        {
            Subject.faceExpression("none", ActionColor);
        }
    }


    public override string GetClassName()
    {
        return "HeadFaceAction";
    }

    public override string ToString ()
	{
		return string.Format ("Head/Face MU");
	}

    public override string GetDescriptionString()
    {
        return "HeadFaceAction: Assign to each parameter in the .csv file to one element in the face expressions spritesheet attached " + 
            "to the character.";
    }
}
