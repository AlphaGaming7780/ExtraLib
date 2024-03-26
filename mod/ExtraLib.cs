using System.Reflection;
using System.IO;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using Game.UI.InGame;
using Unity.Entities;
using Game.UI.Menu;
using static Extra.Lib.Systems.MainSystem;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extra.Lib
{
	public class ExtraLib
	{
		internal static readonly GameObject ExtraLibMonoObject = new();
		public static ExtraLibMonoScript extraLibMonoScript;

		public delegate void OnEditEnities(NativeArray<Entity> entities);
		public delegate void OnMainMenu();
		internal static OnMainMenu onMainMenu;

		static internal List<EntityRequester> entityRequesters = [];

		public struct EntityRequester(OnEditEnities onEditEnities, EntityQueryDesc entityQueryDesc) {
			public OnEditEnities onEditEnities = onEditEnities;
			public EntityQueryDesc entityQueryDesc = entityQueryDesc;
		}

		public static PrefabSystem m_PrefabSystem;
		public static RenderingSystem m_RenderingSystem;
		public static EntityManager m_EntityManager;
		public static ToolSystem m_ToolSystem;
		// public static ToolUISystem m_ToolUISystem;s
		public static ToolbarUISystem m_ToolbarUISystem;
		public static NotificationUISystem m_NotificationUISystem;
		public static ILog Logger = LogManager.GetLogger($"{nameof(ExtraLib)}").SetShowsErrorsInUI(false); //.{nameof(ELT)}
																										   // private GameSetting m_Setting;

		public static bool debugMod = false;

		internal static Stream GetEmbedded(string embeddedPath)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtraDetailingTools.embedded." + embeddedPath);
		}

		public static void AddOnEditEnities(OnEditEnities onEditEnities, EntityQueryDesc entityQueryDesc) {
			AddOnEditEnities(new(onEditEnities, entityQueryDesc));
		}

		public static void AddOnEditEnities(EntityRequester entityRequester) {
			entityRequesters.Add(entityRequester);
		}

		public static void AddOnMainMenu(OnMainMenu OnMainMenu) {
			onMainMenu += OnMainMenu;
		}
	}
}