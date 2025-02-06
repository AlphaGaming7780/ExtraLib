using Game.Prefabs;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Extra.Lib.Prefabs
{

    [ComponentMenu("Extra/", new Type[]
    {

    })]
    public class UIAssetMultiCategoryPrefab : UIGroupPrefab
    {
        public UIAssetMultiCategoryPrefab parentCategory;
        public UIAssetMenuPrefab m_Menu;

        public override void GetPrefabComponents(HashSet<ComponentType> components)
        {
            base.GetPrefabComponents(components);
            //components.Remove(ComponentType.ReadWrite<UIAssetMenuData>());

            if (this.parentCategory != null || this.m_Menu != null)
            {
                components.Add(ComponentType.ReadWrite<UIAssetMultiCategoryData>());
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

            if(this.m_Menu != null)
            {
                prefabs.Add(this.m_Menu);
            }

        }

        public override void LateInitialize(EntityManager entityManager, Entity entity)
        {
            base.LateInitialize(entityManager, entity);
            if (this.parentCategory != null)
            {
                PrefabSystem prefabSystem = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
                Entity parentCategoryEntity = prefabSystem.GetEntity(this.parentCategory);
                entityManager.SetComponentData<UIAssetMultiCategoryData>(entity, new UIAssetMultiCategoryData(parentCategoryEntity));

                if (prefabSystem.TryGetComponentData<UIAssetCategoryData>(parentCategory, out UIAssetCategoryData uIAssetCategoryData))
                {
                    entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(uIAssetCategoryData.m_Menu));
                }

                this.parentCategory.AddElement(entityManager, entity);
            } else if (this.m_Menu != null)
            {
                PrefabSystem prefabSystem = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
                Entity menuEntity = prefabSystem.GetEntity(this.m_Menu);
                //entityManager.SetComponentData<UIAssetLevelCategoryData>(entity, new UIAssetLevelCategoryData(menuEntity));
                entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(menuEntity));
                this.m_Menu.AddElement(entityManager, entity);
            }
        }
    }
}
