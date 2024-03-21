using System;
using System.Collections;
using System.IO;
using Game.SceneFlow;
using Game.UI;
using UnityEngine;

namespace Extra.Lib.UI
{
	
	public partial class ExtraLibUI : UISystemBase
	{	

		protected override void OnCreate() {

			base.OnCreate();
            
		}

		internal static string GetStringFromEmbbededJSFile(string path) {
			return new StreamReader(ExtraLib.GetEmbedded("UI."+path)).ReadToEnd();
		}

        public static void RunUIScript(string JS) {
            ExtraLib.extraLibMonoScript.ChangeUiNextFrame(JS);
        }

	}
}