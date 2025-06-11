using Colossal.Entities;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace ExtraLib.Prefabs
{

    [ComponentMenu("Extra/", new Type[]
    {

    })]
    public class UIAssetParentCategoryPrefab : UIAssetMenuPrefab
    {
        //public UIAssetMultiCategoryPrefab parentCategory;
        public UIAssetMenuPrefab parentCategoryOrMenu;

        public override void GetPrefabComponents(HashSet<ComponentType> components)
        {
            base.GetPrefabComponents(components);
            components.Remove(ComponentType.ReadWrite<UIAssetMenuData>());

            //if (this.parentCategory != null || this.m_Menu != null)
            if (this.parentCategoryOrMenu != null)
                {
                components.Add(ComponentType.ReadWrite<UIAssetParentCategoryData>());
                components.Add(ComponentType.ReadWrite<UIAssetCategoryData>());
            }
        }

        public override void GetDependencies(List<PrefabBase> prefabs)
        {
            base.GetDependencies(prefabs);
            if (this.parentCategoryOrMenu != null)
            {
                prefabs.Add(this.parentCategoryOrMenu);
            }
        }

        public override void Initialize(EntityManager entityManager, Entity entity)
        {
            base.LateInitialize(entityManager, entity);
            if (this.parentCategoryOrMenu != null)
            {
                PrefabSystem prefabSystem = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
                Entity parentCategoryOrMenuEntity = prefabSystem.GetEntity(this.parentCategoryOrMenu);
                entityManager.SetComponentData<UIAssetParentCategoryData>(entity, new UIAssetParentCategoryData(parentCategoryOrMenuEntity));

                if (entityManager.TryGetComponent<UIAssetCategoryData>(parentCategoryOrMenuEntity, out UIAssetCategoryData uIAssetCategoryData))
                {
                    entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(uIAssetCategoryData.m_Menu));
                    //EL.Logger.Info($"prefab : {prefabSystem.GetPrefabName(entity)} -> m_Menu {prefabSystem.GetPrefabName(uIAssetCategoryData.m_Menu)}");
                } else
                {
                    entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(parentCategoryOrMenuEntity));
                    //EL.Logger.Info($"prefab : {prefabSystem.GetPrefabName(entity)} -> m_Menu {prefabSystem.GetPrefabName(parentCategoryOrMenuEntity)}");
                }

                this.parentCategoryOrMenu.AddElement(entityManager, entity);
            } 
            //else if (this.m_Menu != null)
            //{
            //    PrefabSystem prefabSystem = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
            //    Entity menuEntity = prefabSystem.GetEntity(this.m_Menu);
            //    //entityManager.SetComponentData<UIAssetLevelCategoryData>(entity, new UIAssetLevelCategoryData(menuEntity));
            //    entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(menuEntity));
            //    this.m_Menu.AddElement(entityManager, entity);
            //}
        }
    }
}
