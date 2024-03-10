
using Colossal.Serialization.Entities;
using Game;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Collections;
using Game.UI.Menu;
using Game.Rendering;
using Game.Tools;
using Game.UI.InGame;
using Game.Prefabs;
using Colossal.PSI.Common;
using static Extra.Lib.ExtraLib;
using Extra.Lib.Debugger;
using System;
using System.Threading;
using System.Collections;
using UnityEngine;

namespace Extra.Lib.Systems;

public partial class MainSystem : GameSystemBase
{

	public delegate void OnEditEnities(NativeArray<Entity> entities);
	// private static OnEditEnity onEditEnity;

	static internal List<EntityRequester> entityRequesters = [];

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

		if(mode == GameMode.MainMenu && canEditEnties) ExtraLibUI.extraLibMonoScript.FStartCoroutine(EditEntities());

	}

	private IEnumerator EditEntities () {

		canEditEnties = false;
		int curentIndex = 0;

		var notificationInfo = m_NotificationUISystem.AddOrUpdateNotification(
			$"{nameof(ExtraLib)}.{nameof(MainSystem)}.{nameof(EditEntities)}", 
			title: "ExtraLib, Editing Entities",
			progressState: ProgressState.Indeterminate, 
			progress: 0
		);

		yield return null;

		foreach(EntityRequester entityRequester in entityRequesters) {
			
			notificationInfo.progressState = ProgressState.Progressing;
			notificationInfo.progress = curentIndex/entityRequesters.Count*100;
			notificationInfo.text = entityRequester.onEditEnities.Method.DeclaringType.ToString();			
			yield return null;
			
			EntityQuery entityQuery = GetEntityQuery(entityRequester.entityQueryDesc);
			try{
				entityRequester.onEditEnities.Invoke(entityQuery.ToEntityArray(AllocatorManager.Temp));
			} catch (Exception e) {Print.Error(e);}
			curentIndex++;

			notificationInfo.progressState = ProgressState.Progressing;
			notificationInfo.progress = curentIndex/entityRequesters.Count*100;
			notificationInfo.text = entityRequester.onEditEnities.Method.DeclaringType.ToString();			
			yield return null;
		}

		m_NotificationUISystem.RemoveNotification(
			identifier: notificationInfo.id, 
			delay: 5f, 
			text: "Complete",
			progressState: ProgressState.Complete, 
			progress: 100
		);
	}
}