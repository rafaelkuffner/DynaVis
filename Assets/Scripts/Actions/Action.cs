using System.Collections.Generic;
using System;

public abstract class Action :  IComparable{

	public int start;
	public int end;
    protected Player subject;
    protected string param;
    protected List<Modifier> modifiers;

    public List<Modifier> Modifiers
    {
        get
        {
            return modifiers;
        }

        set
        {
            modifiers = value;
        }
    }

    public Player Subject
    {
        get
        {
            return subject;
        }

        set
        {
            subject = value;
        }
    }

    public Action(int start, int end, Player subject,string param){
		this.start = start;
		this.end = end;
		this.Subject = subject;
        this.param = param;
        this.Modifiers = new List<Modifier>();
    }

    public abstract void execute(int current);

	//0: shouldnt, 1 with function, 2 no function
    public bool shouldExecute()
    {
		if (Subject.whatToPlay.actionFunction.ContainsKey (this.GetType().ToString()) &&
            Subject.whatToPlay.actionFunction[this.GetType().ToString()])
			return true;
		return false;

    } 

    public List<Modifier> modifiersOn()
    {
        List<Modifier> res = new List<Modifier>();
        foreach(Modifier m in modifiers)
        {
            if(Subject.whatToPlay.actionFunction.ContainsKey(this.GetType().ToString() + m.Value) && 
                Subject.whatToPlay.actionFunction[this.GetType().ToString() + m.Value])
            {
                res.Add(m);
            }
        }
        return res;
    }
	public int CompareTo(object comparePart)
	{
		// A null value means that this object is greater.
		if (comparePart == null)
			return 1;
		
		else
			return this.start.CompareTo(((Action)comparePart).start);
	}

    public abstract string GetClassName();

    public abstract string GetDescriptionString();
}
