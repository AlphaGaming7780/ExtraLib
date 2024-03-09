using Colossal.Logging;
using Game;
using Game.Modding;
using HarmonyLib;
using System.Linq;
using Extra.Lib.Systems;
using Extra.Lib;

namespace Extra
{
	public class Mod : IMod
	{
		
		public static ILog Logger = LogManager.GetLogger($"{nameof(ExtraLib)}").SetShowsErrorsInUI(false); //.{nameof(ELT)}
		// private GameSetting m_Setting;

		private Harmony harmony;

		public void OnLoad(UpdateSystem updateSystem)
		{
			Logger.Info(nameof(OnLoad));

			// if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
			//    Logger.Info($"Current ELT asset at {asset.path}");

			// m_Setting = new GameSetting(this);
			// m_Setting.RegisterInOptionsUI();
			// GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

			// AssetDatabase.global.LoadSettings(nameof(ExtraDetailingTools), m_Setting, new GameSetting(this));

			updateSystem.UpdateAt<MainSystem>(SystemUpdatePhase.LateUpdate);

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
			//if (m_Setting != null)
			//{
			//    m_Setting.UnregisterInOptionsUI();
			//    m_Setting = null;
			//}
		}
	}
}