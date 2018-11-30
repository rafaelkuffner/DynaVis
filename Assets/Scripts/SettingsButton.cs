using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class SettingsButton : MonoBehaviour {

	bool open;
	int currentSeeing;
	Button theButton;
	RectTransform mypos;
	Color normalColor;
	Color pressedColor;
	Color highlightedColor;
	Dictionary<string,IndividualSettings> players;
	List<string> playerNames;
	RectTransform canvasTransform;
	Simulation boss;
	Texture2D whiteTile;
    List<Modifier> modifiers;
    int numberModifiers;
	// Use this for initialization
	bool lateInited = false;
    bool modifiersLoaded = false;
	void Start () {
	
	}

	void lateInit(){
		boss = GameObject.Find ("Boss Object").GetComponent<Simulation> ();
		open = false;
		currentSeeing = 0;
		theButton = transform.GetComponent<Button>();
		ColorBlock cb = theButton.colors;
		normalColor = cb.normalColor;
		pressedColor = cb.pressedColor;
		highlightedColor = cb.highlightedColor;
		GameObject [] pl = GameObject.FindGameObjectsWithTag ("Player");
		mypos = this.GetComponent<RectTransform> ();
		players = new Dictionary<string, IndividualSettings> ();
		playerNames = new List<string> ();
		foreach (GameObject o in pl) {
			string name = o.name.ToString();
			IndividualSettings s = o.GetComponent<Player>().whatToPlay;
			players.Add (name,s);
			playerNames.Add(name);
		}
		whiteTile = new Texture2D (1, 1);
		playerNames.Add("All");
		playerNames.Sort ();
		canvasTransform = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		lateInited = true;
   
	}
	// Update is called once per frame
	void Update () {
		if (!lateInited)
			lateInit ();
	}

    public void loadModifiers()
    {
        modifiers = new List<Modifier>();
        List<string> modifsAdded = new List<string>();
        foreach (KeyValuePair<string, Dictionary<string, string>> kp in boss.modifiersMapping)
        {
            Type t = Type.GetType(kp.Key);
            Modifier m = Activator.CreateInstance(t) as Modifier;
            if (!modifsAdded.Contains(kp.Key))
            {
                modifsAdded.Add(kp.Key);
                modifiers.Add(m);
            }
        }
        numberModifiers = 0;
        foreach (KeyValuePair<string, Dictionary<string, string>> kp in boss.modifiersMapping)
        {
            numberModifiers += kp.Value.Keys.Count;
        }
        modifiersLoaded = true;
    }

    public void applyAllSettings(IndividualSettings sets)
    {
        foreach (string s in playerNames)
        {
            if (s == "All") continue;
            IndividualSettings ps = players[s];
            foreach (KeyValuePair<string, bool> ab in sets.actionFunction)
            {
                ps.actionFunction[ab.Key] = ab.Value;
            }
        }
    }
    public IndividualSettings getAllSettings()
    {
        IndividualSettings sets = new IndividualSettings();
        sets.setAll(false);
        //if one is true we show as true
        foreach (KeyValuePair<string, IndividualSettings> ps in players)
        {
            foreach (KeyValuePair<string, bool> ab in ps.Value.actionFunction)
            {
                if (ab.Value)
                {
                    sets.actionFunction[ab.Key] = ab.Value;
                }
            }
        }
        return sets;
    }
	float aux;
	public void drawGUI(){
		
		float sx = canvasTransform.localScale.x;
		float sy = canvasTransform.localScale.y;
		float scale = sx > sy ? sy : sx;

		GUIStyle boxStyle =new GUIStyle(GUI.skin.box);
		boxStyle.fontSize =Mathf.RoundToInt((GUI.skin.font.fontSize+6)*3*scale/4);

		GUIStyle boxStyle2 =new GUIStyle(GUI.skin.box);

		GUIStyle labStyle =new GUIStyle(GUI.skin.label);
		labStyle.fontSize =Mathf.RoundToInt((GUI.skin.font.fontSize-2)*3*scale/4);

		GUIStyle labStyle2 =new GUIStyle(GUI.skin.label);
		labStyle2.normal.textColor = Color.black;
		labStyle2.fontSize =Mathf.RoundToInt((GUI.skin.font.fontSize-4)*3*scale/4);

		GUIStyle butStyle =new GUIStyle(GUI.skin.button);
		butStyle.fontSize = Mathf.RoundToInt((GUI.skin.font.fontSize - 2) * 3*scale/4);
		
		GUIStyle togStyle =new GUIStyle(GUI.skin.toggle);
		togStyle.fontSize = Mathf.RoundToInt((GUI.skin.font.fontSize - 2) * 3*scale/4);

        if (modifiersLoaded) { 
           foreach(Modifier m in modifiers)
            {
                m.drawGUI(boss,mypos,whiteTile,boxStyle2,labStyle2,sx,sy);
            }
     
            if (open) {
			    string currentPlayer = playerNames[currentSeeing];
            
			    IndividualSettings sets;
                if(currentPlayer == "All"){
                    sets = getAllSettings();
                }
                else
                {
                    sets = players[currentPlayer];
                }
			    //Background box scales according to screen and button position

			    //Rect boxRect = new Rect(Screen.width*0.3f,Screen.height*0.3f,Screen.width*0.4f,Screen.height*0.4f);

            
			    Rect boxRect = new Rect(mypos.position.x+(mypos.sizeDelta.x*mypos.localScale.x*canvasTransform.localScale.x/2),
			                            Screen.height - mypos.position.y-(mypos.sizeDelta.y*mypos.localScale.y*canvasTransform.localScale.y/2),
			                            (12* numberModifiers + 130)*sx,
			                            (12*boss.actionsConfig.Count + 120)*sy);

		
		
			    GUI.Box(boxRect,currentPlayer,boxStyle);

			    //Margin
			    boxRect.x+=2*sx;
			    boxRect.y+=2*sy;
			    //Control over what player showing
			    if(GUI.Button(new Rect(boxRect.x,boxRect.y,10*sx,10*sy),"<"))
				    currentSeeing--;
			    if(GUI.Button(new Rect(boxRect.x+boxRect.width-14*sx,boxRect.y,10*sx,10*sy),">"))
				    currentSeeing++;
			    currentSeeing = currentSeeing < 0? playerNames.Count-1:currentSeeing;
			    currentSeeing = currentSeeing == playerNames.Count? 0:currentSeeing;
		
			    Matrix4x4 m1 = Matrix4x4.identity;
			
			    m1.SetTRS(new Vector3(0,0,0), Quaternion.Euler(0,0,90.0f), Vector3.one);	
			    int xpos = 0;
			    GUI.matrix = m1;
			    GUI.Label( new Rect(boxRect.y+20*sy,-boxRect.x-((90-xpos)*sx), 100*sx, 20*sy), "(no function)",labStyle);
			    xpos-=20;
			    foreach(KeyValuePair<string,Dictionary<string,string>> kp  in boss.modifiersMapping){
                    foreach (string s in kp.Value.Keys) { 
				       // string s1 = s.Replace("_focused","");
				        GUI.Label( new Rect(boxRect.y+20*sy,-boxRect.x-((90-xpos)*sx), 160*sx, 20*sy), s,labStyle);;
				        xpos-=20;
                    }
			    }
			    GUI.matrix = Matrix4x4.identity;

			          
			    m1.SetTRS(new Vector3(0,0,0), Quaternion.Euler(0,0,-45f), Vector3.one);	
			    GUI.matrix = m1;

			    GUI.Label(new Rect(boxRect.x-90*sx,boxRect.y+80*sy,140*sx,30*sy),"MU/Function",labStyle);
			    GUI.matrix = Matrix4x4.identity;

			    boxRect.height = 20*sy;
			    boxRect.width = 80*sx;
			    boxRect.y+= 90*sy; 
    //			//Toggles
			    foreach(Configuration c in boss.actionsConfig){
				    GUI.Label(boxRect,c.className,labStyle);
				    Rect toggleRect= new Rect(boxRect.x+boxRect.width,boxRect.y,12*sx,12*sy);
				    sets.actionFunction[c.className] = GUI.Toggle(toggleRect,sets.actionFunction[c.className],"",togStyle);
				    foreach(KeyValuePair <string,Dictionary<string,string>> kp in boss.modifiersMapping){
                        List<Configuration> mappedActions = boss.modifiersConfig[kp.Key];
                        if (mappedActions.Exists(x => x.className == c.className))
                        {
                            foreach (string s in kp.Value.Keys) 
                            { 
                            toggleRect.x += 20 * sx;
                            sets.actionFunction[c.className + s] = GUI.Toggle(toggleRect, sets.actionFunction[c.className + s], "", togStyle);
                            }
                        }
					
				    }
				    boxRect.y+= 12*sy; 
			    }
                float iniwidth = (numberModifiers + 130) * sx * 0.1f;
			    boxRect.x += iniwidth;
			    boxRect.y+=8*sy;
			    boxRect.width = 60*sx;
			    boxRect.height = 12*sy;
			    if(GUI.Button(boxRect,"Check all",butStyle)){
				    sets.setAll(true);
			    }
			    boxRect.x += 65*sx;
			    if(GUI.Button(boxRect,"Uncheck all",butStyle)){
				    sets.setAll(false);
			    }
                if (currentPlayer == "All")
                {
                    applyAllSettings(sets);
                }
            }
            
        }

	}
	public void toggleOpen(){
        if(!modifiersLoaded){
            Debug.Log("No config file loaded!");
            return;
        }
		open = !open;
		if (open) {
			ColorBlock cb = theButton.colors;
			cb.normalColor = pressedColor;
			cb.highlightedColor = pressedColor;
			theButton.colors = cb;
		} else {
			ColorBlock cb = theButton.colors;
			cb.normalColor = normalColor;
			cb.highlightedColor = highlightedColor;
			theButton.colors = cb;
		}
	}
}
