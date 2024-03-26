
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
using Extra.Lib.UI;

namespace Extra.Lib.Systems;

public partial class MainSystem : GameSystemBase
{
	private bool canEditEnties = true;
 
	protected override void OnCreate()
	{
		base.OnCreate();
		Enabled = false;
		m_PrefabSystem = base.World.GetOrCreateSystemManaged<PrefabSystem>();
		m_RenderingSystem = base.World.GetOrCreateSystemManaged<RenderingSystem>();
		m_ToolSystem = base.World.GetOrCreateSystemManaged<ToolSystem>();
		m_ToolbarUISystem = base.World.GetOrCreateSystemManaged<ToolbarUISystem>();
		m_NotificationUISystem = base.World.GetOrCreateSystemManaged<NotificationUISystem>();
		m_EntityManager = EntityManager;
	}

	protected override void OnUpdate() {}

	protected override void OnGamePreload(Purpose purpose, GameMode mode)
	{
		base.OnGamePreload(purpose, mode);
		// Print.Info($"OnGamePreload {purpose} | {mode}");
	}

	protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
	{
		base.OnGameLoadingComplete(purpose, mode);
		// Print.Info($"OnGameLoadingComplete {purpose} | {mode}");

		if(mode == GameMode.MainMenu) {
			if(canEditEnties) extraLibMonoScript.StartCoroutine(EditEntities());
			onMainMenu.Invoke();
		}
		
	}

	private IEnumerator EditEntities () {

		canEditEnties = false;
		int curentIndex = 0;

		var notificationInfo = m_NotificationUISystem.AddOrUpdateNotification(
			$"{nameof(ExtraLib)}.{nameof(MainSystem)}.{nameof(EditEntities)}", 
			title: "ExtraLib, Editing Entities",
			progressState: ProgressState.Indeterminate, 
			progress: 0,
			thumbnail: $"{Icons.COUIBaseLocation}/Icon.svg"
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
			} catch (Exception e) {Mod.Logger.Error(e);}
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