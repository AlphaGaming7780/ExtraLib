
using Colossal.Serialization.Entities;
using Game;
using Unity.Entities;
using Unity.Collections;
using Game.UI.Menu;
using Game.UI.InGame;
using Game.Prefabs;
using Colossal.PSI.Common;
using Colossal.Core;
using System;
using System.Collections;
using ExtraLib.ClassExtension;
using Game.SceneFlow;
using UnityEngine;
using ExtraLib.Prefabs;
using System.Collections.Generic;
using ExtraLib.Helpers;

namespace ExtraLib.Systems
{
    public partial class MainSystem : GameSystemBase
    {

        public delegate void OnEditEnities(NativeArray<Entity> entities);
        public delegate void OnMainMenu();
        internal static OnMainMenu onMainMenu;

        public delegate void OnInitialize();
        internal static OnInitialize onInitialize;

        static internal List<EntityRequester> entityRequesters = new List<EntityRequester>();

        public struct EntityRequester
        {
            public OnEditEnities onEditEnities;
            public EntityQueryDesc entityQueryDesc;

            public EntityRequester(OnEditEnities onEditEnities, EntityQueryDesc entityQueryDesc)
            {
                this.onEditEnities = onEditEnities;
                this.entityQueryDesc = entityQueryDesc;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;
            EL.m_PrefabSystem = base.World.GetOrCreateSystemManaged<PrefabSystem>();
            //m_RenderingSystem = base.World.GetOrCreateSystemManaged<RenderingSystem>();
            //m_ToolSystem = base.World.GetOrCreateSystemManaged<ToolSystem>();
            EL.m_ToolbarUISystem = base.World.GetOrCreateSystemManaged<ToolbarUISystem>();
            EL.m_NotificationUISystem = base.World.GetOrCreateSystemManaged<NotificationUISystem>();
            EL.m_EntityManager = EntityManager;

            MainThreadDispatcher.RegisterUpdater(Initialize);

        }

        protected override void OnUpdate()
        {
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

            if (mode == GameMode.MainMenu)
            {
                //_mainMenuInitialized = true;
                onMainMenu?.Invoke();
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

            EL.extraLibMonoScript.StartCoroutine(EditEntities());

            onInitialize?.Invoke();

            EL.Logger.Info("ExtraLib Initialized.");

            return true;
        }

        internal IEnumerator EditEntities()
        {
            int curentIndex = 0;

            var notificationInfo = EL.m_NotificationUISystem.AddOrUpdateNotification(
                $"{nameof(EL)}.{nameof(MainSystem)}.{nameof(EditEntities)}",
                title: "ExtraLib, Editing Entities",
                progressState: ProgressState.Indeterminate,
                progress: 0,
                thumbnail: $"{Icons.COUIBaseLocation}/Icons/Icon.svg"
            );

            foreach (EntityRequester entityRequester in entityRequesters)
            {

                notificationInfo.progressState = ProgressState.Progressing;
                notificationInfo.progress = (int)(curentIndex / (float)entityRequesters.Count * 100);
                notificationInfo.text = entityRequester.onEditEnities.Method.DeclaringType.ToString();
                EL.m_NotificationUISystem.AddOrUpdateNotification(ref notificationInfo);

                EntityQuery entityQuery = GetEntityQuery(entityRequester.entityQueryDesc);
                try
                {
                    entityRequester.onEditEnities.Invoke(entityQuery.ToEntityArray(AllocatorManager.Temp));
                }
                catch (Exception e) { EL.Logger.Error(e); }
                curentIndex++;
                yield return null;
            }

            EL.m_NotificationUISystem.RemoveNotification(
                identifier: notificationInfo.id,
                delay: 5f,
                text: "Complete",
                progressState: ProgressState.Complete,
                progress: 100
            );
        }

        private void CreateCustomPrefab()
        {

            if (!EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), "Landscaping"), out var p1) || p1 is not UIAssetMenuPrefab newCategory)
            {
                return;
            }

            UIAssetParentCategoryPrefab uIAssetParentCategoryPrefab = CreateDummyUIAssetParentCategoryPrefab("Test Parent Cat 1", newCategory);
            UIAssetParentCategoryPrefab uIAssetCategoryPrefab = CreateDummyUIAssetParentCategoryPrefab("Test Parent Cat 2", uIAssetParentCategoryPrefab);
            CreateDummyPrefab("test object 1", uIAssetCategoryPrefab);
            CreateDummyPrefab("test object 2", uIAssetCategoryPrefab);
        }

        private UIAssetParentCategoryPrefab CreateDummyUIAssetParentCategoryPrefab(string name, UIAssetMenuPrefab parentCategoryOrMenu = null)
        {
            UIAssetParentCategoryPrefab uIAssetMultiCategoryPrefab = ScriptableObject.CreateInstance<UIAssetParentCategoryPrefab>();
            uIAssetMultiCategoryPrefab.name = name;
            uIAssetMultiCategoryPrefab.parentCategoryOrMenu = parentCategoryOrMenu;
            UIObject uIObject = uIAssetMultiCategoryPrefab.AddComponent<UIObject>();
            EL.m_PrefabSystem.AddPrefab(uIAssetMultiCategoryPrefab);
            return uIAssetMultiCategoryPrefab;
        }

        private UIAssetCategoryPrefab CreateDummyAssetCategory(string name, UIAssetMenuPrefab uIAssetMenuPrefab)
        {
            UIAssetCategoryPrefab uIAssetCategoryPrefab = ScriptableObject.CreateInstance<UIAssetCategoryPrefab>();
            uIAssetCategoryPrefab.name = name;
            uIAssetCategoryPrefab.m_Menu = uIAssetMenuPrefab;
            UIObject uIObject = uIAssetCategoryPrefab.AddComponent<UIObject>();
            EL.m_PrefabSystem.AddPrefab(uIAssetCategoryPrefab);
            return uIAssetCategoryPrefab;
        }

        private void CreateDummyPrefab(string name, UIGroupPrefab uIGroupPrefab)
        {
            StaticObjectPrefab prefabBase = ScriptableObject.CreateInstance<StaticObjectPrefab>();
            prefabBase.name = name;
            UIObject uIObject = prefabBase.AddComponent<UIObject>();
            uIObject.m_Group = uIGroupPrefab;
            EL.m_PrefabSystem.AddPrefab(prefabBase);
        }

    }
}