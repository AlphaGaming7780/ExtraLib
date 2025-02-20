using Colossal.Logging;
using Game;
using Game.Modding;
using HarmonyLib;
using System.Linq;
using Game.SceneFlow;
using System.IO;
using System.Reflection;
using Game.Prefabs;
using Game.UI.InGame;
using Game.UI.Menu;
using Unity.Entities;
using UnityEngine;

using ExtraLib.Systems;
using ExtraLib.Systems.UI;
using ExtraLib.Helpers;
using ExtraLib.Mono;
using Logger = ExtraLib.Debugger.Logger;
using ExtraLib.Systems.UI.ExtraPanels;


namespace ExtraLib
{
    public class EL : IMod
	{
		
		public static ILog log = LogManager.GetLogger($"{nameof(ExtraLib)}").SetShowsErrorsInUI(false);
#if DEBUG
		internal static Logger Logger = new(log, true);
#else
		internal static Logger Logger = new(log, false);
#endif
		// private GameSetting m_Setting;

		private Harmony harmony;

		internal static string ResourcesIcons { get; private set; }

        internal static readonly GameObject ExtraLibMonoObject = new();
        public static ExtraLibMonoScript extraLibMonoScript;

        public static PrefabSystem m_PrefabSystem;
        public static EntityManager m_EntityManager;
        public static ToolbarUISystem m_ToolbarUISystem;
        public static NotificationUISystem m_NotificationUISystem;


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

            extraLibMonoScript = EL.ExtraLibMonoObject.AddComponent<ExtraLibMonoScript>();

			updateSystem.UpdateAt<AssetMultiCategory>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<ExtraPanelsUISystem>(SystemUpdatePhase.UIUpdate);
			//updateSystem.UpdateAt<ExtraAssetsMenu>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<MainSystem>(SystemUpdatePhase.LateUpdate);

            ExtraPanelsUISystem extraPanelsUISystem = updateSystem.World.GetOrCreateSystemManaged<ExtraPanelsUISystem>();
            extraPanelsUISystem.AddExtraPanel<TestExtraPanel>();

            harmony = new($"{nameof(ExtraLib)}.{nameof(EL)}");
            harmony.PatchAll(typeof(EL).Assembly);
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
            harmony.UnpatchAll($"{nameof(ExtraLib)}.{nameof(EL)}");
			Icons.UnLoadAllIconsFolder();
		}

        internal static Stream GetEmbedded(string embeddedPath)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtraLib.embedded." + embeddedPath);
        }

        public static void AddOnEditEnities(MainSystem.OnEditEnities onEditEnities, EntityQueryDesc entityQueryDesc)
        {
            AddOnEditEnities(new(onEditEnities, entityQueryDesc));
        }

        public static void AddOnEditEnities(MainSystem.EntityRequester entityRequester)
        {
            MainSystem.entityRequesters.Add(entityRequester);
        }

        public static void AddOnMainMenu(MainSystem.OnMainMenu OnMainMenu)
        {
            MainSystem.onMainMenu += OnMainMenu;
        }

        public static void AddOnInitialize(MainSystem.OnInitialize OnInitialize)
        {
            MainSystem.onInitialize += OnInitialize;
        }

    }
}