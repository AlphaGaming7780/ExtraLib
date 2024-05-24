
using Colossal.Serialization.Entities;
using Game;
using Unity.Entities;
using Unity.Collections;
using Game.UI.Menu;
using Game.UI.InGame;
using Game.Prefabs;
using Colossal.PSI.Common;
using System;
using System.Collections;
using Extra.Lib.UI;
using Extra.Lib.mod.ClassExtension;

namespace Extra.Lib.Systems;

public partial class MainSystem : GameSystemBase
{
	private bool canEditEnties = true;
 
	protected override void OnCreate()
	{
		base.OnCreate();
		Enabled = false;
		ExtraLib.m_PrefabSystem = base.World.GetOrCreateSystemManaged<PrefabSystem>();
        //m_RenderingSystem = base.World.GetOrCreateSystemManaged<RenderingSystem>();
        //m_ToolSystem = base.World.GetOrCreateSystemManaged<ToolSystem>();
        ExtraLib.m_ToolbarUISystem = base.World.GetOrCreateSystemManaged<ToolbarUISystem>();
        ExtraLib.m_NotificationUISystem = base.World.GetOrCreateSystemManaged<NotificationUISystem>();
        ExtraLib.m_EntityManager = EntityManager;
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
			if(canEditEnties) ExtraLib.extraLibMonoScript.StartCoroutine(EditEntities());
            ExtraLib.onMainMenu?.Invoke();
		}
	}

	private IEnumerator EditEntities () {

		canEditEnties = false;
		int curentIndex = 0;

		var notificationInfo = ExtraLib.m_NotificationUISystem.AddOrUpdateNotification(
			$"{nameof(ExtraLib)}.{nameof(MainSystem)}.{nameof(EditEntities)}", 
			title: "ExtraLib, Editing Entities",
			progressState: ProgressState.Indeterminate, 
			progress: 0,
			thumbnail: $"{Icons.COUIBaseLocation}/Icons/Icon.svg"
		);

		foreach(ExtraLib.EntityRequester entityRequester in ExtraLib.entityRequesters) {
			
			notificationInfo.progressState = ProgressState.Progressing;
			notificationInfo.progress = (int)(curentIndex / (float)ExtraLib.entityRequesters.Count * 100);
            notificationInfo.text = entityRequester.onEditEnities.Method.DeclaringType.ToString();
			ExtraLib.m_NotificationUISystem.AddOrUpdateNotification(ref notificationInfo);

			EntityQuery entityQuery = GetEntityQuery(entityRequester.entityQueryDesc);
			try{
				entityRequester.onEditEnities.Invoke(entityQuery.ToEntityArray(AllocatorManager.Temp));
			} catch (Exception e) {EL.Logger.Error(e);}
			curentIndex++;		
			yield return null;
		}

		ExtraLib.m_NotificationUISystem.RemoveNotification(
			identifier: notificationInfo.id, 
			delay: 5f, 
			text: "Complete",
			progressState: ProgressState.Complete, 
			progress: 100
		);
	}
}