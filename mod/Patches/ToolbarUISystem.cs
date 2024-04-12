using Colossal.UI.Binding;
using Extra;
using Extra.Lib;
using Extra.Lib.UI;
using Game.Prefabs;
using Game.UI.InGame;
using HarmonyLib;
using Unity.Entities;

namespace Extra.Lib.Patches;

public class ToolbarUISystemPatch
{

    [HarmonyPatch(typeof(ToolbarUISystem), "SelectAssetMenu")]
    class SelectAssetMenu
    {
        static void Postfix(Entity assetMenu)
        {

            if (assetMenu != Entity.Null && ExtraLib.m_EntityManager.HasComponent<UIAssetMenuData>(assetMenu))
            {
                ExtraLib.m_PrefabSystem.TryGetPrefab(assetMenu, out PrefabBase prefabBase);

                ExtraAssetsMenu.ShowCatsTab(prefabBase is UIAssetMenuPrefab && prefabBase.name == ExtraAssetsMenu.CatTabName);
            }
        }
    }

    public static void UpdateCatUI()
    {
        Traverse.Create(ExtraLib.m_ToolbarUISystem).Field("m_AssetMenuCategoriesBinding").GetValue<RawMapBinding<Entity>>().UpdateAll();
    }

    public static void UpdateMenuUI()
    {
        Traverse.Create(ExtraLib.m_ToolbarUISystem).Field("m_AssetMenuCategoriesBinding").GetValue<RawMapBinding<Entity>>().UpdateAll();
    }

    public static void SelectCatUI(Entity entity)
    {
        Traverse.Create(ExtraLib.m_ToolbarUISystem).Method("SelectAssetCategory", [typeof(Entity)]).GetValue(entity);
    }

    public static void SelectMenuUI(Entity entity)
    {
        Traverse.Create(ExtraLib.m_ToolbarUISystem).Method("SelectAssetMenu", [typeof(Entity)]).GetValue(entity);
    }

}
