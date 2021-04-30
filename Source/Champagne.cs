/*
 * SimplePartlessPlugin.cs
 * 
 * Part of the KSP modding examples from Thunder Aerospace Corporation.
 * 
 * (C) Copyright 2013, Taranis Elsu
 * 
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 * 
 * This code is licensed under the Apache License Version 2.0. See the LICENSE.txt and NOTICE.txt
 * files for more information.
 * 
 * Note that Thunder Aerospace Corporation is a ficticious entity created for entertainment
 * purposes. It is in no way meant to represent a real entity. Any similarity to a real entity
 * is purely coincidental.
 */

using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
//using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;
using KSP.UI.Screens;

using ClickThroughFix;
using ToolbarControl_NS;
using System.Linq;
using System.Text;

namespace ChampagneBottle
{
    /*
     * This project is meant to be a "Hello World" example showing how to make a part-less plugin.
     * 
     * The main class for your plug-in should implement MonoBehavior, which is a Unity class.
     * http://docs.unity3d.com/Documentation/ScriptReference/MonoBehaviour.html
     * 
     * KSPAddon is an "attribute" that lets KSP know that this class is a plug-in. KSP will create
     * an instance of the class whenever the game loads the specified scene. The second parameter
     * tells KSP whether to create an instance once, or every time it loads the scene.
     * 
     * Do not use true for the second parameter until this bug gets fixed:
     * http://forum.kerbalspaceprogram.com/threads/45107-KSPAddon-bug-causes-mod-incompatibilities
     * That link has a workaround, but prefer to use false unless your needs dictate otherwise.
     * 
     * The lifecycle for KSP comes from Unity with a few differences:
     *    
     *    Constructor -> Awake() -> Start() -> Update/FixedUpdate() [repeats] -> OnDestroy()
     * 
     * When the specified game scene loads, first KSP will construct your MonoBehaviour class and
     * call Awake(). When it finishes doing that for all the mods, then it calls Start(). After
     * that, it will call Update() every frame and FixedUpdate() every physics time step. Just
     * before exiting the scene, the game will call OnDestroy() which gives you the opportunity to
     * save any settings.
     * 
     * Unity uses Serialization a lot, so use the Awake() method to initialize your fields rather
     * than the constructor. And you can use OnDestroy() to do some of the things you would do in
     * a destructor.
     * 
     * Also see http://www.richardfine.co.uk/2012/10/unity3d-monobehaviour-lifecycle/ for more
     * information about the Unity lifecycle.
     * 
     * This plugin does not actually do anything beyond logging to the Debug console, which you
     * can access by pressing Alt+F2 or Alt+F12 (on Windows, for OSX use Opt and for Linux use
     * Right Shift). You can also look at the debug file, which you can find at
     * {KSP}/KSP_Data/output_log.txt.
     */

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(ChampagneBottle.MODID, ChampagneBottle.MODNAME);
        }
    }

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class ChampagneBottle : MonoBehaviour
	{
		private static List<String> _patterns;
		private static Dictionary<String, List<String>> _wordLists;

		private static readonly Random Ran = new Random();

		private static int _lang;

		private static VesselType _shipType;

		private static bool activated = false;

		private static String _path = "/GameData/Champagne/PluginData/WordLists/";
		private readonly String[] _generatedNames = new String[5];

		//private bool _generated = false;
        private static bool languageSelected = false;
        
		private const float LogInterval = 5.0f;
        
		protected Rect WindowPos = new Rect(Screen.width/2, Screen.height/2, 320, 0);
        ToolbarControl toolbarControl = null;
        

		ChampagneSettings cfg;

        #region Unity Stuff

        // Called after the scene is loaded.
        private void Awake()
        {
            cfg = new ChampagneSettings();
            cfg.LoadSettings();

            _wordLists = new Dictionary<string, List<string>>();

            Debug.Log("ChampagneBottle [" + GetInstanceID().ToString("X")
                      + "][" + Time.time.ToString("0.0000") + "]: Awake: " + name);

            InitAppLauncherButton();

        }

        // Called next.
        private void Start()
		{
			try
			{
				Debug.Log("ChampagneBottle [" + GetInstanceID().ToString("X")
				          + "][" + Time.time.ToString("0.0000") + "]: Start");

				_patterns = LoadList(KSPUtil.ApplicationRootPath + _path + "patterns.txt");


            }
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
			}
 ;
		}
		private void OnGUI()
		{
            DrawGui();
		}
		/*
         * Called when the game is leaving the scene (or exiting). Perform any clean up work here.
         */
		private void OnDestroy()
		{
            //Debug.Log("ChampagneBottle [" + GetInstanceID().ToString("X")
            //          + "][" + Time.time.ToString("0.0000") + "]: OnDestroy");

            toolbarControl.OnDestroy();
            Destroy(toolbarControl);
        }

#endregion Unity Stuff


#region Name Stuff

		private static String GenerateName()
		{
			int namingPriority = 0;
			_shipType = VesselType.Unknown;

			Part p = VesselNaming.FindPriorityNamePart(EditorLogic.fetch.ship);
			_shipType = p.vesselType;


#if false
			foreach (var part in EditorLogic.fetch.ship.Parts)
			{
				var m = part.FindModuleImplementing<VesselNaming>();
				if (m != null)
				{
					if (m.namingPriority > namingPriority)
					{
						namingPriority = m.namingPriority;
						_shipType = m.vesselType;
					}
				}
				else
				{
					if (namingPriority == 0 && part.vesselType == VesselType.Unknown) continue;
					_shipType = part.vesselType;
				}
			}
#endif
			Debug.Log("ChampagneBottle: Ship type: " + _shipType);
				
			bool scr = false;
			switch (_lang)
			{
				case 1:
					scr = true;
					break;
				case 2:
					scr = (Ran.Next(2) == 1);
					break;
			}
			return ParsePattern( Choose(_patterns), scr );
		}

		private static String ParsePattern(String pattern, bool scr)
		{
			Debug.Log("Parsing pattern " + pattern);
			String output = pattern;
			while (Regex.IsMatch(output, "<'?\\w+>"))
			{
				Match i = Regex.Match(output, "<'?\\w+>");
				bool pos = (i.Value[1] == '\'');
				String listName = Regex.Match(i.Value, "\\w+").Value;
				String wordChosen;
				if (listName != "C")
				{
					Debug.Log("\tTag is " + i.Value + ", list name = " + listName);
					if (!_wordLists.ContainsKey(listName))
					{
						Debug.Log("\tWord list " + listName + " referenced for the first time, loading...");
						_wordLists[listName] = LoadList(KSPUtil.ApplicationRootPath + _path + listName + ".txt");
					}
					wordChosen = Capitalise(Choose(_wordLists[listName], scr));
					if (pos) wordChosen = Possess(wordChosen);
					Debug.Log("\tChosen word: " + wordChosen);
				}
				else
				{
					wordChosen = GenerateShipClass();
				}
				output = output.Remove(i.Index, i.Length).Insert(i.Index, wordChosen);
				Debug.Log("\tString is now " + output);
			}
			return output;
		}

		private static String Choose(List<String> fromList, bool scramble = false)
		{
			if (!scramble)
			{
				return fromList[Ran.Next(fromList.Count)];
			}
			String a = fromList[Ran.Next(fromList.Count)];
			String b = fromList[Ran.Next(fromList.Count)];
			return a.Substring(0, a.Length/2) + b.Substring(b.Length/2);
		}

		private static String GenerateShipClass()
		{
			if (!_wordLists.ContainsKey("prefixes"))
			{
				Debug.Log("\tWord list prefixes referenced for the first time, loading...");
				_wordLists["prefixes"] = LoadList(KSPUtil.ApplicationRootPath + _path + "prefixes" + ".txt");
			}
			if (!_wordLists.ContainsKey("ShipTypes"))
			{
				Debug.Log("\tWord list ShipTypes referenced for the first time, loading...");
				_wordLists["ShipTypes"] = LoadList(KSPUtil.ApplicationRootPath + _path + "ShipTypes" + ".txt");
			}

			var output = new StringBuilder();

			var pfx = _wordLists["prefixes"];
			if (pfx.Count > 0)
				output.Append(pfx[Ran.Next(0, pfx.Count)]);
			else
				output.Append("K");
			
			var st = _wordLists["ShipTypes"];
			if (st.Count > (int)_shipType)
				output.Append(st[(int)_shipType]);
			else if (st.Count > 0)
				output.Append(st[Ran.Next(0, st.Count)]);
			else
				output.Append("SS");

			return output.ToString();
		}

		private static List<String> LoadList(String filePath)
		{
			Debug.Log("Loading list from " + filePath);
			return new List<string>( File.ReadAllLines(filePath).Where(l=>!l.StartsWith("#")) );
		}

		private static String Possess(String pronoun)
		{
			return pronoun.EndsWith("s") ? pronoun + "'" : pronoun + "'s";
		}

		private static String Capitalise(String str)
		{
			String[] split = str.Split(' ');
			String output = "";
			foreach (String i in split)
			{
				output += i.Substring(0, 1).ToUpper() + i.Substring(1).ToLower();
			}
			return output;
		}

#endregion Name Stuff


#region Gooey Stuff

        internal const string MODID = "ChampagneBottle_NS";
        internal const string MODNAME = "Champagne Bottle";

        public void InitAppLauncherButton()
		{

            toolbarControl = gameObject.AddComponent<ToolbarControl>();
            toolbarControl.AddToAllToolbars(OnAppLauncherTrue, OnAppLauncherFalse,
                ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
                MODID,
                "champagneBottleButton",
                "Champagne/PluginData/Textures/icon_button_active",
                "Champagne/PluginData/Textures/icon_button",
                "Champagne/PluginData/Textures/icon_button_active_24",
                "Champagne/PluginData/Textures/icon_button_24",
                MODNAME
            );
        }

        public void RemoveAppLauncherButton()
		{
            //ApplicationLauncher.Instance.RemoveModApplication (button);
            toolbarControl.OnDestroy();
            Destroy(toolbarControl);
        }

		public void OnAppLauncherTrue()
		{
            //EditorLogic.fetch.Lock(true, true, true, "ChampagneLocked");
            //_editorLocked = true;
            Debug.Log("OnAppLauncherTrue");
			activated = true;
			if (HighLogic.CurrentGame.Parameters.CustomParams<CB>().selectLanguageEveryTime)
                languageSelected = false;
            for (int i = 0; i < _generatedNames.Length; i++)
                _generatedNames[i] = GenerateName();
        }

		public void OnAppLauncherFalse()
		{
            Debug.Log("OnAppLauncherFalse");

            activated = false;
		}


		private void DrawGui()
		{
			if (!activated) return;

			GUI.skin = HighLogic.Skin;
			WindowPos = ClickThruBlocker.GUILayoutWindow(246243, WindowPos, WindowGui, "Champagne Bottle", GUILayout.MinWidth(100));


		}

		private void WindowGui(int windowId)
		{
			try
			{
				var mySty = new GUIStyle(GUI.skin.button);
				mySty.normal.textColor = mySty.focused.textColor = Color.white;
				mySty.hover.textColor = mySty.active.textColor = Color.yellow;
				mySty.onNormal.textColor =
					mySty.onFocused.textColor = mySty.onHover.textColor = mySty.onActive.textColor = Color.green;
				mySty.padding = new RectOffset(8, 8, 8, 8);
				
				GUILayout.BeginVertical();
				String generateButtonText = "";
				if (languageSelected)
				{
					GUILayout.Label("Here are the KSC staff members' suggestions:", GUILayout.ExpandWidth(true));
					foreach (String s in _generatedNames)
					{
						if (GUILayout.Button(s, mySty, GUILayout.ExpandWidth(true)))
						{
							EditorLogic.fetch.shipNameField.text = s;
                            toolbarControl.SetFalse(true);
                        }
					}
					generateButtonText = "Fire those guys!";
					GUILayout.Space(16);
				}
				else
				{
					generateButtonText = "What should we call this?";
					GUILayout.Label("Language: ", GUILayout.ExpandWidth(true));
					String[] langOpts = {"English", "Kerbal", "Both"};
					_lang = GUILayout.SelectionGrid(_lang, langOpts, 1, "toggle", GUILayout.ExpandWidth(true));
 
					GUILayout.Space(16);
				}

				//GUILayout.Button is "true" when clicked
				if (GUILayout.Button(generateButtonText, mySty, GUILayout.ExpandWidth(true)))
				{
					for (int i = 0; i < _generatedNames.Length; i++)
						_generatedNames[i] = GenerateName();
                    if (!languageSelected)
                        languageSelected = true;
                    //else
					//    _generated = true;
				}
				GUILayout.EndVertical();

				GUI.DragWindow();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
			}
		}

#endregion Gooey Stuff
	}
}