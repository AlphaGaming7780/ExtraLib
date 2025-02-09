using Game.Prefabs;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace ExtraLib.Prefabs
{

    [ComponentMenu("Extra/", new Type[]
    {

    })]
    public class UIAssetChildCategoryPrefab : UIGroupPrefab
    {
        public UIAssetParentCategoryPrefab parentCategory;

        public override void GetPrefabComponents(HashSet<ComponentType> components)
        {
            base.GetPrefabComponents(components);
            //if (this.parentCategory != null || this.m_Menu != null)
            if (this.parentCategory != null)
                {
                components.Add(ComponentType.ReadWrite<UIAssetChildCategoryData>());
                components.Add(ComponentType.ReadWrite<UIAssetCategoryData>());
            }
        }

        public override void GetDependencies(List<PrefabBase> prefabs)
        {
            base.GetDependencies(prefabs);
            if (this.parentCategory != null)
            {
                prefabs.Add(this.parentCategory);
            }
        }

        public override void LateInitialize(EntityManager entityManager, Entity entity)
        {
            base.LateInitialize(entityManager, entity);
            if (this.parentCategory != null)
            {
                PrefabSystem prefabSystem = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
                Entity parentCategoryEntity = prefabSystem.GetEntity(this.parentCategory);
                entityManager.SetComponentData<UIAssetChildCategoryData>(entity, new UIAssetChildCategoryData(parentCategoryEntity));

                if (prefabSystem.TryGetComponentData<UIAssetCategoryData>(parentCategory, out UIAssetCategoryData uIAssetCategoryData))
                {
                    entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(uIAssetCategoryData.m_Menu));
                } else
                {
                    entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(parentCategoryEntity));
                }

                this.parentCategory.AddElement(entityManager, entity);
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
