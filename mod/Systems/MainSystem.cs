
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
using Game.SceneFlow;
using UnityEngine;
using Extra.Lib.Prefabs;
using Extra.Lib.mod.Prefabs;
using Colossal.IO.AssetDatabase;

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

        GameManager.instance.RegisterUpdater(Initialize);

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

    public bool Initialize()
    {
        if (!GameManager.instance.modManager.isInitialized ||
            GameManager.instance.gameMode != GameMode.MainMenu ||
            GameManager.instance.state == GameManager.State.Loading ||
            GameManager.instance.state == GameManager.State.Booting
            ) return false;

        //CreateCustomPrefab();

        ExtraLib.extraLibMonoScript.StartCoroutine(EditEntities());

        ExtraLib.onInitialize?.Invoke();

        return true;
    }

    internal IEnumerator EditEntities () {
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

    private void CreateCustomPrefab()
    {

        if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), "Landscaping"), out var p1) || p1 is not UIAssetMenuPrefab newCategory)
        {
            return;
        }

        UIAssetMultiCategoryPrefab uIAssetParentCategoryPrefab = CreateDummyUIAssetMultiCategoryPrefab("Test Parent Cat 1", null, newCategory);
        UIAssetMultiCategoryPrefab uIAssetCategoryPrefab = CreateDummyUIAssetMultiCategoryPrefab("Test Parent Cat 2", uIAssetParentCategoryPrefab);
        CreateDummyPrefab("test object 1", uIAssetCategoryPrefab);
        CreateDummyTestPrefab("test object 2", uIAssetCategoryPrefab);
    }

    private UIAssetMultiCategoryPrefab CreateDummyUIAssetMultiCategoryPrefab(string name, UIAssetMultiCategoryPrefab parentCategory = null, UIAssetMenuPrefab m_Menu = null)
    {
        UIAssetMultiCategoryPrefab uIAssetMultiCategoryPrefab = ScriptableObject.CreateInstance<UIAssetMultiCategoryPrefab>();
        uIAssetMultiCategoryPrefab.name = name;
        uIAssetMultiCategoryPrefab.parentCategory = parentCategory;
        uIAssetMultiCategoryPrefab.m_Menu = m_Menu;
        UIObject uIObject = uIAssetMultiCategoryPrefab.AddComponent<UIObject>();
        ExtraLib.m_PrefabSystem.AddPrefab(uIAssetMultiCategoryPrefab);
        return uIAssetMultiCategoryPrefab;
    }

    private UIAssetCategoryPrefab CreateDummyAssetCategory(string name, UIAssetMenuPrefab uIAssetMenuPrefab)
    {
        UIAssetCategoryPrefab uIAssetCategoryPrefab = ScriptableObject.CreateInstance<UIAssetCategoryPrefab>();
        uIAssetCategoryPrefab.name = name;
        uIAssetCategoryPrefab.m_Menu = uIAssetMenuPrefab;
        UIObject uIObject = uIAssetCategoryPrefab.AddComponent<UIObject>();
        ExtraLib.m_PrefabSystem.AddPrefab(uIAssetCategoryPrefab);
        return uIAssetCategoryPrefab;
    }

    private void CreateDummyPrefab(string name, UIGroupPrefab uIGroupPrefab)
    {
        StaticObjectPrefab prefabBase = ScriptableObject.CreateInstance<StaticObjectPrefab>();
        prefabBase.name = name;
        UIObject uIObject = prefabBase.AddComponent<UIObject>();
        uIObject.m_Group = uIGroupPrefab;
        ExtraLib.m_PrefabSystem.AddPrefab(prefabBase);
    }

    private void CreateDummyTestPrefab(string name, UIGroupPrefab uIGroupPrefab)
    {
        TestPrefabCustomPrefab prefabBase = ScriptableObject.CreateInstance<TestPrefabCustomPrefab>();
        prefabBase.name = name;
        UIObject uIObject = prefabBase.AddComponent<UIObject>();
        uIObject.m_Group = uIGroupPrefab;
        ExtraLib.m_PrefabSystem.AddPrefab(prefabBase);
    }

}