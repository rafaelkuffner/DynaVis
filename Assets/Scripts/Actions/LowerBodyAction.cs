using System.Collections.Generic;

public class LowerBodyAction : ColorChangeableAction
{

	public LowerBodyAction():base(0,0,null,""){
		
	}
	
	public LowerBodyAction(int start, int end, Player subject, string param):base(start,end,subject,param){
	
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
            Subject.highlightLower(param, ActionColor);
        }
        else
        {
            Subject.highlightLower("none", ActionColor);
        }
    }

    public override string GetClassName()
    {
        return "LowerBodyAction";
    }

    public override string ToString ()
	{
		return string.Format ("Lower Body MU");
	}
}
