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
#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using System.IO;
using UnityEngine;

//using Toolbar;
using System.Reflection;

//using ChampagneBottle;

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
	[KSPAddon (KSPAddon.Startup.EditorAny, false)]
	public class ChampagneBottle2 : MonoBehaviour
	{


		private IButton button1 = null;

		//private static System.Random ran = new System.Random ();

		//String[] generatedNames = new String[5];
		//bool generated = false;
		//static int lang = 0;

		//protected Rect windowPos;

		//String path;

		/*
         * Called after the scene is loaded.
         */
		void Awake ()
		{
			try {
				Debug.Log ("ChampagneBottle [" + this.GetInstanceID ().ToString ("X")
				+ "][" + Time.time.ToString ("0.0000") + "]: Awake: " + this.name);

				showButton ();
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
			}
		}

		void showButton ()
		{
			if (!ToolbarManager.ToolbarAvailable)
				return;
			Debug.Log ("Starting Toolbar button!"); 
			bool state1 = false;
			button1 = ToolbarManager.Instance.add ("Champagne", "toggle");
			button1.TexturePath = "Champagne/Textures/icon_button";
			button1.ToolTip = "Toggle Champagne Bottle window";
			button1.OnClick += (e) => {
				Debug.Log ("button1 clicked, mouseButton: " + e.MouseButton);
				//button1.TexturePath = state1 ? "000_Toolbar/img_buttonTypeMNode" : "000_Toolbar/icon";
				state1 = !state1;
				setGUI (state1);
			};
			button1.Visible = true;
			if (button1.EffectivelyVisible)
				ChampagneBottle.RemoveAppLauncherButton (ChampagneBottle._appLauncherButton);
			Debug.Log ("Done starting Toolbar button!"); 
		}
			

		/*
         * Called when the game is leaving the scene (or exiting). Perform any clean up work here.
         */
		void OnDestroy ()
		{
			Debug.Log ("ChampagneBottle [" + this.GetInstanceID ().ToString ("X")
			+ "][" + Time.time.ToString ("0.0000") + "]: OnDestroy");
			setGUI (false);
			if (button1 != null)
				button1.Destroy ();
		}


		bool display = false;

		private void setGUI (bool show)
		{
			display = show;
			ChampagneBottle.blizzy = true;
			#if true
			if (show) {
				//RenderingManager.AddToPostDrawQueue(3, new Callback(drawGUI));//start the GUI
				ChampagneBottle.OnAppLauncherTrue ();
			} else {
				//RenderingManager.RemoveFromPostDrawQueue(3, new Callback(drawGUI)); //close the GUI
				ChampagneBottle.OnAppLauncherFalse ();
			}
			#endif
		}
	}
}

#endif