
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
using Game.Modding;
using Game.SceneFlow;
using UnityEngine;

namespace Extra.Lib.Systems;

public partial class MainSystem : GameSystemBase
{
	//private bool _canEditEnties = true;
	//private bool _modInitialized = false;
	//private bool _mainMenuInitialized = false;

    //private NotificationUISystem.NotificationInfo _extraLibNotLoadedNotification;
 
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

        GameManager.instance.RegisterUpdater(() => ExtraLib.extraLibMonoScript.StartCoroutine(EditEntities()));

		//_extraLibNotLoadedNotification = ExtraLib.m_NotificationUISystem.AddOrUpdateNotification(
		//	$"{nameof(ExtraLib)}.{nameof(MainSystem)}.{nameof(_extraLibNotLoadedNotification)}",
		//	title: "ExtraLib didn't load !!!",
		//	text: "Click here to load ExtraLib.",
		//	progressState: ProgressState.Indeterminate,
		//	progress: 0,
		//	thumbnail: Icons.GameCrashWarning,
		//	onClicked: new Action(OnMainMenu)
		//);

	}

	protected override void OnUpdate() {
		//Debug.Log(GameManager.instance.isLoading);
		//if (GameManager.instance.modManager.isInitialized)
		//{
		//	Enabled = false;
		//	if (_mainMenuInitialized || (GameManager.instance.gameMode == GameMode.MainMenu && !GameManager.instance.isLoading))
		//	{
		//		OnMainMenu();
		//	}
		//}
	}

	protected override void OnGamePreload(Purpose purpose, GameMode mode)
	{
		base.OnGamePreload(purpose, mode);
		// Print.Info($"OnGamePreload {purpose} | {mode}");
	}

    protected override void OnGameLoaded(Context serializationContext)
    {
        base.OnGameLoaded(serializationContext);
    }

    protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
	{
		base.OnGameLoadingComplete(purpose, mode);
		// Print.Info($"OnGameLoadingComplete {purpose} | {mode}");

		if(mode == GameMode.MainMenu) {
            //_mainMenuInitialized = true;
            ExtraLib.onMainMenu?.Invoke();
            //if (GameManager.instance.modManager.isInitialized)
        }
	}

	private IEnumerator EditEntities () {
		int curentIndex = 0;

        var notificationInfo = ExtraLib.m_NotificationUISystem.AddOrUpdateNotification(
            $"{nameof(ExtraLib)}.{nameof(MainSystem)}.{nameof(EditEntities)}",
            title: "ExtraLib, Editing Entities",
            progressState: ProgressState.Indeterminate,
            progress: 0,
            thumbnail: $"{Icons.COUIBaseLocation}/Icons/Icon.svg"
        );

        foreach (ExtraLib.EntityRequester entityRequester in ExtraLib.entityRequesters)
        {

            notificationInfo.progressState = ProgressState.Progressing;
            notificationInfo.progress = (int)(curentIndex / (float)ExtraLib.entityRequesters.Count * 100);
            notificationInfo.text = entityRequester.onEditEnities.Method.DeclaringType.ToString();
            ExtraLib.m_NotificationUISystem.AddOrUpdateNotification(ref notificationInfo);

            EntityQuery entityQuery = GetEntityQuery(entityRequester.entityQueryDesc);
            try
            {
                entityRequester.onEditEnities.Invoke(entityQuery.ToEntityArray(AllocatorManager.Temp));
            }
            catch (Exception e) { EL.Logger.Error(e); }
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