using Colossal.UI.Binding;
using Game.Prefabs;
using Game.UI.InGame;
using HarmonyLib;
using Unity.Entities;
using ExtraLib.Systems.UI;
using System.Collections.Generic;

namespace ExtraLib.Patches;

public class ToolbarUISystemPatch
{
    [HarmonyPatch(typeof(ToolbarUISystem), "SelectAssetMenu")]
    class SelectAssetMenu
    {
        static void Postfix(Entity assetMenu)
        {

            if (assetMenu != Entity.Null && EL.m_EntityManager.HasComponent<UIAssetMenuData>(assetMenu))
            {
                AssetMultiCategory.instance.OnSelectAssetMenu(assetMenu);

                //EL.m_PrefabSystem.TryGetPrefab(assetMenu, out PrefabBase prefabBase);

                //ExtraAssetsMenu.ShowCatsTab(prefabBase is UIAssetMenuPrefab && prefabBase.name == ExtraAssetsMenu.CatTabName);

            }
        }
    }

    [HarmonyPatch(typeof(ToolbarUISystem), "SelectAssetCategory")]
    class SelectAssetCategory
    {
        static void Postfix(Entity assetCategory)
        {
            if (assetCategory != Entity.Null && EL.m_EntityManager.HasComponent<UIAssetCategoryData>(assetCategory))
            {
                AssetMultiCategory.instance.OnSelectAssetCategory(assetCategory);
            }
        }
    }

    //[HarmonyPatch(typeof(ToolbarUISystem), "Apply")]
    //class Apply
    //{
    //    static void Postfix(List<Entity> themes, List<Entity> packs, Entity assetMenuEntity, Entity assetCategoryEntity, Entity assetEntity, bool updateTool = false)
    //    {

    //        string menuPrefabName = EL.m_PrefabSystem.GetPrefabName(assetMenuEntity);
    //        string assetCatPrefabName = EL.m_PrefabSystem.GetPrefabName(assetCategoryEntity);
    //        string assetEntityName = EL.m_PrefabSystem.GetPrefabName(assetEntity);
    //        EL.Logger.Info($" {menuPrefabName} -> {assetCatPrefabName} -> {assetEntityName}");

    //    }
    //}


    //public static void UpdateCatUI()
    //{
    //    Traverse.Create(EL.m_ToolbarUISystem).Field("m_AssetMenuCategoriesBinding").GetValue<RawMapBinding<Entity>>().UpdateAll();
    //}

    //public static void UpdateMenuUI()
    //{
    //    Traverse.Create(EL.m_ToolbarUISystem).Field("m_AssetMenuCategoriesBinding").GetValue<RawMapBinding<Entity>>().UpdateAll();
    //}

    //public static void SelectCatUI(Entity entity)
    //{
    //    Traverse.Create(EL.m_ToolbarUISystem).Method("SelectAssetCategory", [typeof(Entity)]).GetValue(entity);
    //}

    //public static void SelectMenuUI(Entity entity)
    //{
    //    Traverse.Create(EL.m_ToolbarUISystem).Method("SelectAssetMenu", [typeof(Entity)]).GetValue(entity);
    //}

}
