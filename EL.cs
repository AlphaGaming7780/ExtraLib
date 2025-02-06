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
using System.IO;
using Extra.Lib.Localization;
using System.Reflection;
using Extra.Lib.mod.Systems;

namespace Extra
{
	public class EL : IMod
	{
		
		public static ILog log = LogManager.GetLogger($"{nameof(ExtraLib)}").SetShowsErrorsInUI(false); //.{nameof(ELT)}
#if DEBUG
		internal static Logger Logger = new(log, true);
#else
		internal static Logger Logger = new(log, false);
#endif
		// private GameSetting m_Setting;

		private Harmony harmony;
		internal static string ResourcesIcons { get; private set; }

		public void OnLoad(UpdateSystem updateSystem)
		{
			Logger.Info(nameof(OnLoad));

			if (!GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
			{
				Logger.Fatal("Failed to get the ExecutableAsset. The mod isn't loaded.");
				return;
			}

			Logger.Info($"Current ExtraLib asset at {asset.path}");

			FileInfo fileInfo = new(asset.path);
			Icons.LoadIconsFolder(Icons.IconsResourceKey, fileInfo.Directory.FullName);
			ResourcesIcons = Path.Combine(fileInfo.DirectoryName, "Icons");

			ExtraLocalization.LoadLocalization(Logger, Assembly.GetExecutingAssembly(), false);

			// m_Setting = new GameSetting(this);
			// m_Setting.RegisterInOptionsUI();
			// GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

			// AssetDatabase.global.LoadSettings(nameof(ExtraDetailingTools), m_Setting, new GameSetting(this));

			ExtraLib.extraLibMonoScript = ExtraLib.ExtraLibMonoObject.AddComponent<ExtraLibMonoScript>();

			updateSystem.UpdateAt<ExtraLibUI>(SystemUpdatePhase.UIUpdate);
			updateSystem.UpdateAt<AssetMultiCategory>(SystemUpdatePhase.UIUpdate);
			updateSystem.UpdateAt<ExtraAssetsMenu>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<MainSystem>(SystemUpdatePhase.LateUpdate);

            harmony = new($"{nameof(ExtraLib)}.{nameof(ExtraLib)}");
			harmony.PatchAll(typeof(ExtraLib).Assembly);
			var patchedMethods = harmony.GetPatchedMethods().ToArray();
			Logger.Info($"Plugin ExtraLib made patches! Patched methods: " + patchedMethods.Length);
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