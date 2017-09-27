using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorModifier : Modifier
{
    public ColorModifier(string tierName, int start, int end, Player subject, string value) : base(tierName, start, end, subject, value)
    {
    }

    public ColorModifier():base()
    {
        
    }
    public override void modify(Action a)
    {
        ColorChangeableAction cca = (ColorChangeableAction)a;
        string sCol = subject.Boss.modifiersMapping["ColorModifier"][value];
        char[] sep = { ' ' };
        string[] colparts = sCol.Split(sep);
        Color c = new Color(float.Parse(colparts[0]), float.Parse(colparts[1]), float.Parse(colparts[2]));
        cca.ActionColor = c;  
    }

    public override void drawGUI(Simulation boss, RectTransform mypos, Texture2D whiteTile, GUIStyle boxStyle2, GUIStyle labStyle2, float sx, float sy)
    {
        float ypos = Screen.height - mypos.position.y + (85 * sy);
        GUI.Label(new Rect(5 * sx, ypos, 200 * sx, 20 * sy), "Functions color codes:", labStyle2);
        ypos += 12 * sy;
        whiteTile.SetPixel(0, 0, Color.gray);
        whiteTile.Apply();
        boxStyle2.normal.background = whiteTile;
        GUI.Box(new Rect(5 * sx, ypos, 8 * sx, 8 * sy), "", boxStyle2);
        GUI.Label(new Rect(15 * sx, ypos, 200 * sx, 20 * sy), "(no function)", labStyle2);
        ypos += 12 * sy;
        foreach (KeyValuePair<string, string> s in boss.modifiersMapping["ColorModifier"])
        {
            string[] color = s.Value.Split(null);
            Color c = new Color(float.Parse(color[0]),float.Parse(color[1]),float.Parse(color[2]));
            whiteTile.SetPixel(0, 0, c);
            whiteTile.Apply();
            boxStyle2.normal.background = whiteTile;
            //string s1 = s.Key.Replace("_focused", "");
            GUI.Box(new Rect(5 * sx, ypos, 8 * sx, 8 * sy), "", boxStyle2);
            GUI.Label(new Rect(15 * sx, ypos, 200 * sx, 20 * sy), s.Key, labStyle2);
            ypos += 12 * sy;
        }
    }


}
