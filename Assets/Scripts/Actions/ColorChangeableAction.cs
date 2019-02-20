using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorChangeableAction : Action {

    protected Color actionColor;

    public ColorChangeableAction():base("",0,0,null,""){

    }
    public ColorChangeableAction(string tier, int start, int end, Player subject, string param)
        : base(tier,start, end, subject, param)
    {
        ActionColor = Color.gray;
    }

    public Color ActionColor
    {
        get
        {
            return actionColor;
        }

        set
        {
            actionColor = value;
        }
    }

    public override void execute(int current)
    {
        ActionColor = Color.gray;
    }
}
