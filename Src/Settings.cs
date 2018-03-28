using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

// http://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/#comment-2754813
// search for "Mod integration into Stock Settings

namespace ChampagneBottle
{
    public class CB : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Champagne Bottle"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Champagne Bottle"; } }
        public override string DisplaySection { get { return "Champagne Bottle"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }


        [GameParameters.CustomParameterUI("Use Blizzy Toolbar if available")]
        public bool useBlizzy = false;

        [GameParameters.CustomParameterUI("select Language Every Time")]
        public bool selectLanguageEveryTime = false;

       
        public override void SetDifficultyPreset(GameParameters.Preset preset)
        { }
        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {

            return true;
        }
        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
        }
        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }
}
