using System;
using UnityEngine;

public abstract class Modifier : IComparable
{

    public int start;
    public int end;
    protected string tierName;
    protected Player subject;
    protected string value;

    public string TierName
    {
        get
        {
            return tierName;
        }

        set
        {
            tierName = value;
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

    public string Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
        }
    }

    public Modifier()
    {
        tierName = "";
        start = -1;
        end = -1;
        subject = null;
        value = "";
    }
    public Modifier(string tierName, int start, int end, Player subject, string value)
    {
        this.tierName = tierName;
        this.start = start;
        this.end = end;
        this.Subject = subject;
        this.Value = value;
    }

    public abstract void modify(Action a);

    public abstract void drawGUI(Simulation boss,RectTransform mypos, Texture2D whiteTile, GUIStyle boxStyle2, GUIStyle labStyle2, float sx, float sy);

    public int CompareTo(object comparePart)
    {
        // A null value means that this object is greater.
        if (comparePart == null)
            return 1;

        else
            return this.start.CompareTo(((Modifier)comparePart).start);
    }
}
