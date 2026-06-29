using ExtraLib.ClassExtension;
using ExtraLib.Helpers;
using ExtraLib.Prefabs;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace ExtraLib
{
    internal static class EAMSupport
    {
        internal static void SetupEditEntities()
        {
            EntityQueryDesc entityQueryDesc = new()
            {
                All = new[] { ComponentType.ReadOnly<EditorAssetCategoryOverrideData>() }
            };

            EL.AddOnEditEnities(new(OnEditEntities, entityQueryDesc));
        }

        private static void OnEditEntities(NativeArray<Entity> entities)
        {
            if (entities.Length == 0)
                return;

            foreach (Entity entity in entities)
            {
                if (!EL.m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefab))
                    continue;

                EditorAssetCategoryOverride eaco = prefab.GetComponent<EditorAssetCategoryOverride>();
                if (eaco == null || eaco.m_IncludeCategories == null)
                    continue;

                foreach (string s in eaco.m_IncludeCategories)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    if (!s.StartsWith("EAM"))
                        continue;

                    string[] cats = s.Split('/', '\\');
                    if (cats.Length < 3)
                        continue;

                    UIAssetParentCategoryPrefab assetCat = PrefabsHelper.GetOrCreateUIAssetParentCategoryPrefab(cats[1]);

                    for (int i = 2; i < cats.Length - 1; i++)
                    {
                        assetCat = PrefabsHelper.GetOrCreateUIAssetParentCategoryPrefab(cats[i], assetCat);
                    }

                    string[] last = cats[cats.Length - 1].Split(':');
                    string catName = last[0];
                    string uiPriority = last.Length > 1 ? last[1] : string.Empty;

                    int priorityValue = int.MaxValue;
                    if (!string.IsNullOrEmpty(uiPriority))
                        int.TryParse(uiPriority, NumberStyles.Integer, CultureInfo.InvariantCulture, out priorityValue);

                    var prefabUI = prefab.GetComponent<UIObject>();
                    if (prefabUI == null)
                    {
                        prefabUI = prefab.AddComponent<UIObject>();
                        prefabUI.active = true;
                        prefabUI.m_IsDebugObject = false;
                        prefabUI.m_Icon = Icons.GetIcon(prefab);
                        prefabUI.m_Priority = priorityValue;
                    }
                    else
                    {
                        prefabUI.m_Priority = priorityValue;
                    }

                    prefabUI.m_Group?.RemoveElement(entity);
                    prefabUI.m_Group = PrefabsHelper.GetOrCreateUIAssetChildCategoryPrefab(
                        assetCat,
                        $"{catName} {assetCat.name}"
                    );
                    prefabUI.m_Group.AddElement(entity);

                    EL.m_EntityManager.AddOrSetComponentData(entity, prefabUI.ToComponentData());

                    break;
                }
            }
        }
    }
}
