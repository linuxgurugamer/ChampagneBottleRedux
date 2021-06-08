using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChampagneBottle
{
	class ChampagneSettings
	{


		//public bool blizzyToolbar = false;
		public string patternfile = "patterns.txt";
		//public bool selectLanguageEveryTime = false;


		public static readonly String ROOT_PATH = KSPUtil.ApplicationRootPath;
		private static readonly String CONFIG_BASE_FOLDER = ROOT_PATH + "GameData/";
		private static String BASE_FOLDER = CONFIG_BASE_FOLDER + "Champagne/";
		private static String NODENAME = "ChampagneBottle";
		private static String CFG_FILE = BASE_FOLDER + "PluginData/ChampagneBottle.cfg";


		public static List<String> ShipTypes = new List<String> ();

		static string SafeLoad (string value, bool oldvalue)
		{
			if (value == null)
				return oldvalue.ToString ();
			return value;
		}

		static string SafeLoad (string value, string oldvalue)
		{
			if (value == null)
				return oldvalue;
			return value;
		}

		public void LoadSettings ()
		{
			try {
				ConfigNode root = ConfigNode.Load (CFG_FILE);
				if (root != null) {
					ConfigNode settings = root.GetNode (NODENAME);
					if (settings != null) {

						patternfile = SafeLoad (settings.GetValue ("patterns"), patternfile);
					} else {
						Debug.Log ("No ChampagneBottle settings found");
					}
				} else {
					Debug.Log ("No ChampagneBottle settings file found");
				}
			} catch (Exception) {
				Debug.Log ("ChampagneBottle: Load Settings: Failed to load settings from game database");
				throw;
			}
		}


	}
}
