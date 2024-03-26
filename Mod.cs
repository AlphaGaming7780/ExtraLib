using Colossal.Logging;
using Game;
using Game.Modding;
using HarmonyLib;
using System.Linq;
using Extra.Lib.Systems;
using Extra.Lib;
using Extra.Lib.Debugger;
using Extra.Lib.UI;
using Game.SceneFlow;
using System.Drawing;
using System.IO;

namespace Extra
{
	public class Mod : IMod
	{
		
		public static ILog log = LogManager.GetLogger($"{nameof(ExtraLib)}").SetShowsErrorsInUI(false); //.{nameof(ELT)}
		internal static Logger Logger = new(log);
		// private GameSetting m_Setting;

		private Harmony harmony;

		public void OnLoad(UpdateSystem updateSystem)
		{
			Logger.Info(nameof(OnLoad));

			if (!GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset)) return;

			Logger.Info($"Current ExtraLib asset at {asset.path}");

			Icons.LoadIconsFolder(Icons.IconsResourceKey, new FileInfo(asset.path).Directory.FullName);

			// m_Setting = new GameSetting(this);
			// m_Setting.RegisterInOptionsUI();
			// GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

			// AssetDatabase.global.LoadSettings(nameof(ExtraDetailingTools), m_Setting, new GameSetting(this));

			ExtraLib.extraLibMonoScript = ExtraLib.ExtraLibMonoObject.AddComponent<ExtraLibMonoScript>();

			updateSystem.UpdateAt<MainSystem>(SystemUpdatePhase.LateUpdate);
			updateSystem.UpdateAt<ExtraLibUI>(SystemUpdatePhase.UIUpdate);

			harmony = new($"{nameof(ExtraLib)}.{nameof(ExtraLib)}");
			harmony.PatchAll(typeof(ExtraLib).Assembly);
			var patchedMethods = harmony.GetPatchedMethods().ToArray();
			Logger.Info($"Plugin ExtraDetailingTools made patches! Patched methods: " + patchedMethods.Length);
			foreach (var patchedMethod in patchedMethods)
			{
				Logger.Info($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
			}
		}

		public void OnDispose()
		{
			Logger.Info(nameof(OnDispose));
			harmony.UnpatchAll($"{nameof(ExtraLib)}.{nameof(ExtraLib)}");
			Icons.UnLoadAllIconsFolder();
			//if (m_Setting != null)
			//{
			//    m_Setting.UnregisterInOptionsUI();
			//    m_Setting = null;
			//}
		}
	}
}