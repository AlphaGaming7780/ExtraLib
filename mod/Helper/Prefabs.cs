using System;
using Game.Prefabs;
using UnityEngine;

namespace Extra.Lib.Helper;

public static class PrefabsHelper
{
	public static UIAssetCategoryPrefab GetUIAssetCategoryPrefab(string cat)
	{

		if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), cat), out var p1)
			|| p1 is not UIAssetCategoryPrefab Category)
		{
            EL.Logger.Error($"Failed to get the UIAssetCategoryPrefab with this name : {cat}");
			return null;
		}

		return Category;

	}

    public static UIAssetCategoryPrefab GetOrCreateUIAssetCategoryPrefab(UIAssetMenuPrefab menuPrefab, string catName, Func<PrefabBase, string> getIcons, string behindcat = null)
    {
        UIAssetCategoryPrefab uIAssetMenuPrefab = GetOrCreateUIAssetCategoryPrefab(menuPrefab, catName, "", behindcat);
        uIAssetMenuPrefab.GetComponent<UIObject>().m_Icon = getIcons(uIAssetMenuPrefab);
        return uIAssetMenuPrefab;
    }

    public static UIAssetCategoryPrefab GetOrCreateUIAssetCategoryPrefab(string menuName, string catName, Func<PrefabBase, string> getIcons, string behindcat = null)
    {
        return GetOrCreateUIAssetCategoryPrefab(GetUIAssetMenuPrefab(menuName), catName, getIcons, behindcat);
    }

    public static UIAssetCategoryPrefab GetOrCreateUIAssetCategoryPrefab(string menuName, string catName, string iconPath, string behindcat = null)
    {
        return GetOrCreateUIAssetCategoryPrefab(GetUIAssetMenuPrefab(menuName), catName, iconPath, behindcat);
    }

    public static UIAssetCategoryPrefab GetOrCreateUIAssetCategoryPrefab(UIAssetMenuPrefab menuPrefab, string catName, string iconPath, string behindcat = null)
	{

		if (ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), catName), out var p1)
			&& p1 is UIAssetCategoryPrefab newCategory)
		{
			return newCategory;
		}

		UIAssetCategoryPrefab behindCategory = null;

		if(behindcat != null) {
			if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), behindcat), out var p3)
				|| p3 is not UIAssetCategoryPrefab behindCategory2)
			{
				EL.Logger.Error($"Failed to get the UIAssetCategoryPrefab with this name : {behindcat}");
				return null;
			} else {
				behindCategory = behindCategory2;
			}
		}

		newCategory = ScriptableObject.CreateInstance<UIAssetCategoryPrefab>();
		newCategory.name = catName;
		newCategory.m_Menu = menuPrefab;
		var newCategoryUI = newCategory.AddComponent<UIObject>();
        newCategoryUI.m_Icon = iconPath;
		if(behindCategory != null) newCategoryUI.m_Priority = behindCategory.GetComponent<UIObject>().m_Priority+1;
		newCategoryUI.active = true;
		newCategoryUI.m_IsDebugObject = false;

		ExtraLib.m_PrefabSystem.AddPrefab(newCategory);

		return newCategory;
	}

    public static UIAssetMenuPrefab GetUIAssetMenuPrefab(string menuName)
    {
        if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menuName), out var p1)
            || p1 is not UIAssetMenuPrefab menuPrefab)
        {
            EL.Logger.Error($"Failed to get the UIAssetMenuPrefab with this name : {menuName}");
            return null;
        }

        return menuPrefab;
    }

    /// <summary>
    /// Create a new menu in the ToolBar.
    /// </summary>
    /// <param name="menuName">The name of the menu</param>
    /// <param name="getIcons">the function called that return the icon path.</param>
    /// <param name="toolBarGroup">The Tool bar group, can be : `Services Toolbar Group`, `Zones Toolbar Group` and `Tools Toolbar Group` or your custom tool bar group.</param>
    /// <param name="offset">The position of the menu in is group.</param>
    public static void CreateUIAssetMenuPrefab(string menuName, Func<PrefabBase, string> getIcons, string toolBarGroup = "Services Toolbar Group", int offset = 1)
    {
        if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menuName), out var p1)
            || p1 is not UIAssetMenuPrefab menuPrefab)
        {

			if(!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIToolbarGroupPrefab), toolBarGroup), out PrefabBase prefab) || prefab is not UIToolbarGroupPrefab uIToolbarGroupPrefab) {
                EL.Logger.Error($"Failed to get the UIToolbarGroupPrefab with this name : {toolBarGroup}");
				return;
            }

            menuPrefab = ScriptableObject.CreateInstance<UIAssetMenuPrefab>();
            menuPrefab.name = menuName;
            var MenuUI = menuPrefab.AddComponent<UIObject>();
            MenuUI.m_Icon = getIcons(menuPrefab);
            MenuUI.m_Priority = offset;
            MenuUI.active = true;
            MenuUI.m_IsDebugObject = false;
            MenuUI.m_Group = uIToolbarGroupPrefab;

            ExtraLib.m_PrefabSystem.AddPrefab(menuPrefab);
        }
    }


    /// <summary>
    /// Get or Create a menu in the ToolBar.
    /// </summary>
    /// <param name="menuName">The name of the menu</param>
    /// <param name="getIcons">the function called that return the icon path.</param>
    /// <param name="toolBarGroup">The Tool bar group, can be : `Services Toolbar Group`, `Zones Toolbar Group` and `Tools Toolbar Group` or your custom tool bar group.</param>
    /// <param name="offset">The position of the menu in is group.</param>
    public static UIAssetMenuPrefab GetOrCreateNewUIAssetMenuPrefab(string menuName, Func<PrefabBase, string> getIcons, string toolBarGroup = "Services Toolbar Group", int offset = 1)
    {

        if (ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menuName), out var p1) && p1 is UIAssetMenuPrefab menuPrefab)
        {
            return menuPrefab;
        }

        if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIToolbarGroupPrefab), toolBarGroup), out PrefabBase prefab) || prefab is not UIToolbarGroupPrefab uIToolbarGroupPrefab)
        {
            EL.Logger.Error($"Failed to get the UIToolbarGroupPrefab with this name : {toolBarGroup}");
            return null;
        }

        menuPrefab = ScriptableObject.CreateInstance<UIAssetMenuPrefab>();
        menuPrefab.name = menuName;
        var MenuUI = menuPrefab.AddComponent<UIObject>();
        MenuUI.m_Icon = getIcons(menuPrefab);
        MenuUI.m_Priority = offset;
        MenuUI.active = true;
        MenuUI.m_IsDebugObject = false;
        MenuUI.m_Group = uIToolbarGroupPrefab;

        ExtraLib.m_PrefabSystem.AddPrefab(menuPrefab);

        return menuPrefab;
	}
}