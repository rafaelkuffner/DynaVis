﻿
using System.Collections.Generic;

public class UpperBodyAction : ColorChangeableAction {


	public UpperBodyAction():base(0,0,null,""){
		
	}
	
	public UpperBodyAction(int start, int end, Player subject, string param):base(start,end,subject,param){
		
	}

	
	public override void execute (int current)
    {
        base.execute(current);
        List<Modifier> mods = modifiersOn();
		if (current < this.end && shouldExecute())
        {
            foreach (Modifier m in mods)
            {
                m.modify(this);
            }
			Subject.highlightUpper(param,ActionColor);
        }
        else
        {
			Subject.highlightUpper ("none",ActionColor);
        }
	}

    public override void execute3D(int current)
    {
        execute(current);
    }

 
	public override string ToString ()
	{
		return string.Format ("Upper Body MU");
	}
}
