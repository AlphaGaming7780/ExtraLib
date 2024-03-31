using System.Collections;
using System.Collections.Generic;
using System.IO;
using Colossal.UI;

namespace Extra.Lib.UI;

public static class Icons
{
	private static readonly Dictionary<string, List<string>> pathToIconLoaded = [];
	internal static readonly string IconsResourceKey = "extralib";
	internal static readonly string COUIBaseLocation = $"coui://{IconsResourceKey}";

	// internal static void AddNewIconsFolder(string pathToFolder) {
	// 	if(!pathToIconToLoad.Contains(pathToFolder)) pathToIconToLoad.Add(pathToFolder);
	// }

	public static void LoadIconsFolder(string uri, string path, bool shouldWatch = false) {

		if(pathToIconLoaded.ContainsKey(uri)) {
			if(pathToIconLoaded[uri].Contains(path)) return;
			pathToIconLoaded[uri].Add(path);
		} else {
			pathToIconLoaded.Add(uri, [path]);
		}

		UIManager.defaultUISystem.AddHostLocation(uri, path, shouldWatch);
	}

	public static void UnLoadIconsFolder(string uri, string path = null) {
		
		if(!pathToIconLoaded.ContainsKey(uri)) return;

		if(path == null) {
			pathToIconLoaded.Remove(uri);
			UIManager.defaultUISystem.RemoveHostLocation(uri);
		} else {
			if(!pathToIconLoaded[uri].Contains(path)) return;
			pathToIconLoaded[uri].Remove(path);

			UIManager.defaultUISystem.RemoveHostLocation(uri, path);
		}
	}

	internal static void UnLoadAllIconsFolder() {
		foreach(string key in pathToIconLoaded.Keys) {
			UIManager.defaultUISystem.RemoveHostLocation(key);
		}
	}
}