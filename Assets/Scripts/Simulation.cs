﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Xml;
using System;

public class Simulation : MonoBehaviour {

    public string configFilename;
	public string filename = "";
	bool browse = false;
	FileBrowser fb;
	Slider timeline;
	Text durationText;
	Text currentText;
	Text loadedFileText;

	public GUISkin sk;
	public Texture2D file,folder,back,drive;
	public List<Action> actions;
    public List<Configuration> actionsConfig;
    public Dictionary<string, string> tiersConfig;
    public Dictionary<string, List<Configuration>> modifiersConfig;
    public Dictionary<string, Dictionary<string,string>> modifiersMapping;

    List<Action> activeActions;
	int lastActionidx;
	int duration;
	int current;
	bool dragging;
	bool fileLoaded;
	SettingsButton settings;
	PlayButton control;
	SetupButton setupAnnotation;
	Dictionary<Transform,Vector2> initialState;
	float playspeed;
	public Text speedtext;
	// Use this for initialization
	void Awake () {
		//setting filebrowser
		fb = new FileBrowser("./Data Files",0);
		fb.setLayout (0);
		fb.guiSkin = sk;
		fb.fileTexture = file; 
		fb.directoryTexture = folder;
		fb.backTexture = back;
		fb.driveTexture = drive;
		fb.showSearch = true;
		fb.searchRecursively = true;
		playspeed = 1.0f;
		speedtext.text = "Play Speed: " + playspeed + "x";


        ////different action types
        //constructorActions = new List<Action> ();
        //constructorActions.Add (new LocationAction ());
        //constructorActions.Add (new GazeAction ());
        //constructorActions.Add (new HeadFaceAction ());
        //constructorActions.Add (new UpperBodyAction ());
        //constructorActions.Add (new LowerBodyAction ());

        ////functions
        //functions = new Dictionary<string, Color> ();
        //functions.Add ("self_focused", Color.red);
        //functions.Add ("context_focused", Color.green);
        //functions.Add ("communication_focused", Color.blue);

       
		actions = new List<Action> ();
		activeActions = new List<Action> ();
		timeline = GameObject.Find ("Timeline").GetComponent<Slider> ();
		durationText = GameObject.Find ("TextDuration").GetComponent<Text> ();
		currentText = GameObject.Find ("CurrentText").GetComponent<Text> ();
		control = GameObject.Find ("ButtonPlay").GetComponentInChildren<PlayButton> ();
		settings = GameObject.Find ("Button Settings").GetComponentInChildren<SettingsButton> ();
		setupAnnotation = GameObject.Find ("Button Setup").GetComponentInChildren<SetupButton> ();
		loadedFileText = GameObject.Find ("LoadedFileText").GetComponent<Text> ();
		timeline.value = 0;
		current = 0;
		lastActionidx = 0;
		fileLoaded = false;
		timeline.interactable = false;
		dragging = false;
		saveInitialSetup ();
	}

	public void faster(){
		playspeed *= 2;
		speedtext.text = "Play Speed: " + playspeed + "x";
	}

	public void slower(){
		playspeed /= 2;
		speedtext.text = "Play Speed: " + playspeed + "x";
	}
	void saveInitialSetup(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		initialState = new Dictionary<Transform, Vector2> ();
		foreach (GameObject o in players) {
			Transform p = o.transform;
			initialState.Add(p,p.position);
			o.GetComponent<Player>().Reset();
		}
	}
    void resetInitialState()
    {
        activeActions.Clear();
        lastActionidx = 0;
		foreach (KeyValuePair<Transform,Vector2> kp in initialState) {
			kp.Key.position = kp.Value;
		}
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject o in players) {
			o.GetComponent<Player>().Reset();
		}
	}
	public void quit(){
		Application.Quit ();
	}


	

	// Update is called once per frame
	void Update () {
	if (!dragging && control.play) 
		{
			float deltams = Time.deltaTime *1000.0f*playspeed;
			current+= (int)Mathf.Round(deltams);
			timeline.value = (float)current/(float)duration;
			UpdateActiveActions ();
			ExecuteActiveActions();
		}

		if(lastActionidx >= actions.Count){
			control.play = false;
		}
	}

	void SuddenChangeActiveActions(){
		activeActions.Clear ();
		resetInitialState ();
		foreach (Action a in actions) {
			if(a.start < current){
				int val = current > a.end? a.end:current;
				a.execute(val);
			}
		}
	}
	void UpdateActiveActions(){
		//check for current actions termination
		List<Action> toDelete = new List<Action> ();
		foreach (Action a in activeActions) {
			if (a.end < current)
				toDelete.Add (a);
		}
		foreach (Action a in toDelete) {
			activeActions.Remove (a);
			//finish the action
			a.execute(current);
		}

		int i = lastActionidx; 
		while (i < actions.Count) {
			if(actions[i].start > current) break;
			if(actions[i].end > current) activeActions.Add(actions[i]);
			i++;
		}
		lastActionidx = i;
	}

	void ExecuteActiveActions(){
		foreach (Action a in activeActions) {
			a.execute(current);
		}
	}

	public void importFile(){
		browse = true;
	}

	public void play(){
		if (duration != 0) {
			control.toggle ();
		}
	}

	public void toggleSettings(){
		settings.toggleOpen();
	}


	public void stop(){
		control.stop ();
		resetInitialState ();
		current = 0;
		timeline.value = 0;
	}

	public void OnBeginDrag(){
		dragging = true;
	}

	public void OnEndDrag(){
		current = (int) Mathf.Round(timeline.value * duration);
		SuddenChangeActiveActions();
		dragging = false;
	}

	void OnGUI(){
		if(browse && fb.draw()){
			if(fb.outputFile == null){
				Debug.Log("Cancel hit");
				browse = false;
			}else{
				Debug.Log("Ouput File = \""+fb.outputFile.ToString()+"\"");
				//filename = 
				browse = false;
                load(fb.outputFile.ToString());
			}
		}
		System.TimeSpan ts = System.TimeSpan.FromMilliseconds (current);
		currentText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}",ts.Hours,ts.Minutes, ts.Seconds,ts.Milliseconds);
	
		settings.drawGUI ();

		if (fb.outputFile != null && fileLoaded) {
			loadedFileText.text ="Loaded File: "+ fb.outputFile.ToString();
		}
	}

    void loadConfigFile()
    {
        tiersConfig = new Dictionary<string, string>();
        actionsConfig = new List<Configuration>();
        modifiersConfig = new Dictionary<string, List<Configuration>>();
        modifiersMapping = new Dictionary<string, Dictionary<string, string>>();
        XmlDocument doc = new XmlDocument();
        doc.Load(configFilename);
        XmlNodeList tiers = doc.SelectNodes("/Definitions/Tiers/Tier");
        foreach(XmlNode n in tiers)
        {
            string name = n.Attributes["name"].Value;
            XmlNodeList associations = n.ChildNodes;
            foreach(XmlNode n2 in associations)
            {
                string trans1 = n2.InnerText;
				tiersConfig.Add(trans1,name);
            }
            
        }


        XmlNodeList list = doc.SelectNodes("/Definitions/Actions/Action");
     
        foreach (XmlNode node in list)
        {
            string className = node.Attributes["name"].Value;
            Configuration c = new Configuration(className);
            
			//XmlNodeList annotations = doc.SelectNodes("/Definitions/Actions/Action/ApplicableTiers/ApplicableTier");
            XmlNodeList annotations = node.ChildNodes.Item(0).ChildNodes;
            foreach(XmlNode node2 in annotations)
            {
                c.tiers.Add(node2.InnerText);
            }
           // XmlNodeList parameters = doc.SelectNodes("/Definitions/Actions/Action/Parameters/Parameter");
            XmlNodeList parameters = node.ChildNodes.Item(1).ChildNodes;
            foreach(XmlNode node3 in parameters)
            {
                string input = node3.Attributes["input"].Value;
                string tier = node3.Attributes["tier"].Value;
                string output = node3.Attributes["output"].Value;
                c.translationTable.Add(new Tuple<string, string>(tier,input), output);
            }
            actionsConfig.Add(c);
        }

        list = doc.SelectNodes("/Definitions/Modifiers/Modifier");
        foreach(XmlNode node in list)
        {
            string className = node.Attributes["name"].Value;
            // XmlNodeList mappings = doc.SelectNodes("/Definitions/Modifiers/Modifier/Mappings/Mapping");
            XmlNodeList mappings = node.ChildNodes.Item(0).ChildNodes;
            Dictionary<string, string> maps = new Dictionary<string, string>();
            foreach (XmlNode node1 in mappings)
            {
                maps.Add(node1.Attributes["input"].Value, node1.Attributes["output"].Value);
            }
            modifiersMapping.Add(className, maps);

            //XmlNodeList modifiableActions = doc.SelectNodes("/Definitions/Modifiers/Modifier/ModifiableActions/ModifiableAction");
            XmlNodeList modifiableActions = node.ChildNodes.Item(1).ChildNodes;
            List<Configuration> configs = new List<Configuration>();
            foreach(XmlNode node2 in modifiableActions)
            {
                string appliableName = node2.Attributes["name"].Value;
                Configuration c = new Configuration(appliableName);
                //XmlNodeList annotations = doc.SelectNodes("/Definitions/Modifiers/Modifier/ModifiableActions/ModifiableAction/Tiers/TierString");
                XmlNodeList annotations = node2.ChildNodes.Item(0).ChildNodes;
                foreach (XmlNode node3 in annotations)
                {
                    c.tiers.Add(node3.InnerText);
                }
                //XmlNodeList parameters = doc.SelectNodes("/Definitions/Modifiers/Modifier/ModifiableActions/ModifiableAction/Parameters/Parameter");
                XmlNodeList parameters = node2.ChildNodes.Item(1).ChildNodes;
                foreach (XmlNode node4 in parameters)
                {
                    string input = node4.Attributes["input"].Value;
                    string tier = node4.Attributes["tier"].Value;
                    string output = node4.Attributes["output"].Value;
                    c.translationTable.Add(new Tuple<string, string>(tier,input), output);
                }
                configs.Add(c);

            }
            modifiersConfig.Add(className, configs);
        }
    }

    void load(string s)
    {
        if (s.Contains("xml"))
        {
            configFilename = s;
            loadConfigFile();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject o in players)
            {
                o.GetComponent<Player>().loadSettings();
            }
            settings.loadModifiers();
        }
        else
        {
            filename = s;
            if (configFilename == "")
            {
                Debug.Log("Load .xml config first, or run setup, loading with no model");
            }
            else
            {
                loadFile();
            }
        }
    }

    void loadFile(){
		actions.Clear ();
		activeActions.Clear ();
        StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8);
		//Skipping header;
		string line = sr.ReadLine ();
		char[] delim = {','};
        List<Modifier> modifs = new List<Modifier>();
		while((line = sr.ReadLine()) != null)
		{
			string[] components = line.Split(delim);
            Player p = GameObject.Find(components[1]).GetComponent<Player>();
          
            if (!tiersConfig.ContainsKey(components[0]))
            {
                Debug.Log("Action not on config: " + components[0]);
                continue;
            }
			string tiername = tiersConfig[components[0]];
			foreach(Configuration c in actionsConfig)
            {
                Tuple<string,string> key = new Tuple<string, string>(tiername, components[4]);
                if (c.tiers.Contains(tiername) && c.translationTable.ContainsKey(key))
                {
                    string value = c.translationTable[key];
                    Type t = Type.GetType(c.className);
                    Action a = Activator.CreateInstance(t,tiername, int.Parse(components[2]), int.Parse(components[3]), p, value) as Action;
                    actions.Add(a);
                    break;
                }
            }

            foreach(KeyValuePair<string,List<Configuration>> kp in modifiersConfig)
            {
                string className = kp.Key;
                foreach(Configuration c in kp.Value)
                {
                    Tuple<string, string> key = new Tuple<string, string>(tiername, components[4]);
                    if (c.tiers.Contains(tiername) && c.translationTable.ContainsKey(key))
                    {
                        Type t = Type.GetType(className);
                        string value = c.translationTable[key];
                        Modifier m = Activator.CreateInstance(t,tiername, int.Parse(components[2]), int.Parse(components[3]), p,value) as Modifier;
                        modifs.Add(m);
                    }
                }
            }
		}
		actions.Sort ();
        modifs.Sort();
        for(int i = 0,j=0; i < modifs.Count; j++)
        {
            Modifier m = modifs[i];
            string modifierName = m.GetType().ToString();
            string appliableClassName = "";
            foreach(Configuration c in modifiersConfig[modifierName])
            {
                if (c.tiers.Contains(m.TierName))
                {
                    appliableClassName = c.className;
                    break;
                }
            }
            if (j >= actions.Count)
            {
                Debug.Log("Did not find action to this modifier: " +m.TierName+" "+ m.start + " , " + m.end + " , ");
                i++; j =0;
            }
            Action a = actions[j];
            string className = a.GetType().ToString();
            if(appliableClassName == className && m.Subject == a.Subject && a.start == m.start && a.end == m.end)
            {
                a.Modifiers.Add(m);
                i++;
                j = 0;
            }
        }
        foreach (Action a in actions) {
			duration = a.end > duration? a.end: duration;
		}
	
		System.TimeSpan ts = System.TimeSpan.FromMilliseconds (duration);
		durationText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}",ts.Hours,ts.Minutes, ts.Seconds,ts.Milliseconds);
		timeline.interactable = true;

		sr.Close ();
		Debug.Log ("Found " + actions.Count + " actions with total duration of " + duration + "ms");
		fileLoaded = true;
	}
}
