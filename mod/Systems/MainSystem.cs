
using Colossal.Serialization.Entities;
using Game;
using ExtraLib.Debugger;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Collections;
using Game.UI.Menu;
using Game.Rendering;
using Game.Tools;
using Game.UI.InGame;
using Game.Prefabs;
using Colossal.PSI.Common;

namespace ExtraLib.Systems;

public partial class MainSystem : GameSystemBase
{

	public struct EntityRequester {
		public OnEditEnity onEditEnity;
		public EntityQueryDesc entityQueryDesc;
	}

	public delegate void OnEditEnity(NativeArray<Entity> entities);
	// private static OnEditEnity onEditEnity;

	static private List<EntityRequester> entityRequesters = [];

	private bool canEditEnties = true;
 
	protected override void OnCreate()
	{
		base.OnCreate();
		Enabled = false;
		ExtraLib.m_PrefabSystem = base.World.GetOrCreateSystemManaged<PrefabSystem>();
		ExtraLib.m_RenderingSystem = base.World.GetOrCreateSystemManaged<RenderingSystem>();
		ExtraLib.m_ToolSystem = base.World.GetOrCreateSystemManaged<ToolSystem>();
		ExtraLib.m_ToolbarUISystem = base.World.GetOrCreateSystemManaged<ToolbarUISystem>();
		ExtraLib.m_NotificationUISystem = base.World.GetOrCreateSystemManaged<NotificationUISystem>();
		ExtraLib.m_EntityManager = EntityManager;
	}

	protected override void OnUpdate() {}

	protected override void OnGamePreload(Purpose purpose, GameMode mode)
	{
		base.OnGamePreload(purpose, mode);
		Print.Info($"OnGamePreload {purpose} | {mode}");
	}

	protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
	{
		base.OnGameLoadingComplete(purpose, mode);
		Print.Info($"OnGameLoadingComplete {purpose} | {mode}");

		if(mode == GameMode.MainMenu && canEditEnties) EditEntities();

	}

	private void EditEntities () {

		canEditEnties = false;
		int curentIndex = 0;

		var notificationInfo = ExtraLib.m_NotificationUISystem.AddOrUpdateNotification(
			$"{nameof(ExtraLib)}.{nameof(MainSystem)}.{nameof(EditEntities)}", 
			title: "ExtraLib, Editing Entities",
			progressState: ProgressState.Indeterminate, 
			progress: 0
		);

		foreach(EntityRequester entityRequester in entityRequesters) {
			notificationInfo.progressState = ProgressState.Progressing;
			notificationInfo.progress = curentIndex/entityRequesters.Count*100;
			notificationInfo.text = entityRequester.onEditEnity.Method.DeclaringType.ToString();
			EntityQuery entityQuery = GetEntityQuery(entityRequester.entityQueryDesc);
			entityRequester.onEditEnity.Invoke(entityQuery.ToEntityArray(AllocatorManager.Temp));
			curentIndex++;
		}

		ExtraLib.m_NotificationUISystem.RemoveNotification(
			identifier: notificationInfo.id, 
			delay: 5f, 
			progressState: ProgressState.Complete, 
			progress: 100
		);

	}


}