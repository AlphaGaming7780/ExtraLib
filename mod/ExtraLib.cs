using System.Reflection;
using System.IO;
using Game.Prefabs;
using Game.UI.InGame;
using Unity.Entities;
using Game.UI.Menu;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colossal.IO.AssetDatabase;
using System.Linq;
using System;

namespace Extra.Lib
{
	public class ExtraLib
	{
		internal static readonly GameObject ExtraLibMonoObject = new();
		public static ExtraLibMonoScript extraLibMonoScript;

		public delegate void OnEditEnities(NativeArray<Entity> entities);
		public delegate void OnMainMenu();
		internal static OnMainMenu onMainMenu;

		public delegate void OnInitialize();
		internal static OnInitialize onInitialize;

		static internal List<EntityRequester> entityRequesters = [];

		public struct EntityRequester(OnEditEnities onEditEnities, EntityQueryDesc entityQueryDesc) {
			public OnEditEnities onEditEnities = onEditEnities;
			public EntityQueryDesc entityQueryDesc = entityQueryDesc;
		}

		public static PrefabSystem m_PrefabSystem;
		public static EntityManager m_EntityManager;
		public static ToolbarUISystem m_ToolbarUISystem;
		public static NotificationUISystem m_NotificationUISystem;

		internal static Stream GetEmbedded(string embeddedPath)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtraLib.embedded." + embeddedPath);
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

		public static void AddOnInitialize(OnInitialize OnInitialize)
		{
			onInitialize += OnInitialize;
		}
    }
}