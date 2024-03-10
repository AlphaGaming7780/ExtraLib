using System;
using System.Collections;
using System.IO;
using Game.SceneFlow;
using Game.UI;
using UnityEngine;

namespace Extra.Lib
{
	
	public partial class ExtraLibUI : UISystemBase
	{	

		private static readonly GameObject ExtraLibMonoObject = new();
		internal static ExtraLibMonoScript extraLibMonoScript;

		protected override void OnCreate() {

			base.OnCreate();
			extraLibMonoScript = ExtraLibMonoObject.AddComponent<ExtraLibMonoScript>();
            
		}

		internal static string GetStringFromEmbbededJSFile(string path) {
			return new StreamReader(ExtraLib.GetEmbedded("UI."+path)).ReadToEnd();
		}

        public static void RunUIScript(string JS) {
            extraLibMonoScript.ChangeUiNextFrame(JS);
        }

	}

	internal class ExtraLibMonoScript : MonoBehaviour
	{
        void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

		internal void ChangeUiNextFrame(string js) {
			StartCoroutine(ChangeUI(js));
		}

        internal void FStartCoroutine(IEnumerator routine) {
            StartCoroutine(routine);
        }

		private IEnumerator ChangeUI(string js) {
			yield return new WaitForEndOfFrame();
			GameManager.instance.userInterface.view.View.ExecuteScript(js);
			yield return null;
		}
	}
}