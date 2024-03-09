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
using static ExtraLib.Systems.MainSystem;

namespace ExtraLib
{
	public class ExtraLib
	{

		public struct EntityRequester(OnEditEnity onEditEnity, EntityQueryDesc entityQueryDesc) {
			public OnEditEnity onEditEnity = onEditEnity;
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

		internal static Stream GetEmbedded(string embeddedPath)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtraDetailingTools.embedded." + embeddedPath);
		}

		public static void AddOnEditEnity(OnEditEnity onEditEnity, EntityQueryDesc entityQueryDesc) {
			AddOnEditEnity(new(onEditEnity, entityQueryDesc));
		}

		public static void AddOnEditEnity(EntityRequester entityRequester) {
			entityRequesters.Add(entityRequester);
		}

	}
}