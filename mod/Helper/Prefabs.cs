using ExtraLib.Debugger;
using Game.Prefabs;
using Unity.Collections;
using UnityEngine;

namespace ExtraLib.Helper;

public static class PrefabsHelper
{
	public static UIAssetCategoryPrefab GetExistingToolCategory(PrefabBase prefabBase ,string cat)
	{

		if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), cat), out var p1)
			|| p1 is not UIAssetCategoryPrefab terraformingCategory)
		{	
			Print.Error($"Failed to get the UIAssetCategoryPrefab with this name : {cat}");
			return null;
		}

		return terraformingCategory;

	}

	public static UIAssetCategoryPrefab GetOrCreateNewToolCategory(PrefabBase prefabBase, string menu, string cat, string iconPath, string behindcat = null)
	{

		if (ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), cat), out var p1)
			&& p1 is UIAssetCategoryPrefab surfaceCategory)
		{
			return surfaceCategory;
		}

		UIAssetCategoryPrefab behindCategory = null;

		if(behindcat != null) {
			if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), behindcat), out var p3)
				|| p3 is not UIAssetCategoryPrefab behindCategory2)
			{
				Print.Error($"Failed to get the UIAssetCategoryPrefab with this name : {behindcat}");
				return null;
			} else {
				behindCategory = behindCategory2;
			}
		}

		if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menu), out var p2)
			|| p2 is not UIAssetMenuPrefab landscapingMenu)
		{	
			Print.Error($"Failed to get the UIAssetMenuPrefab with this name : {menu}");
			return null;
		}

		surfaceCategory = ScriptableObject.CreateInstance<UIAssetCategoryPrefab>();
		surfaceCategory.name = cat;
		surfaceCategory.m_Menu = landscapingMenu;
		var surfaceCategoryUI = surfaceCategory.AddComponent<UIObject>();
		surfaceCategoryUI.m_Icon = iconPath; //?? ExtraLib.GetIcon(surfaceCategory);
		if(behindCategory != null) surfaceCategoryUI.m_Priority = behindCategory.GetComponent<UIObject>().m_Priority+1;
		surfaceCategoryUI.active = true;
		surfaceCategoryUI.m_IsDebugObject = false;

		ExtraLib.m_PrefabSystem.AddPrefab(surfaceCategory);

		return surfaceCategory;
	}

	public static void CreateNewUiToolMenu(PrefabBase prefab, string menu, string iconPath, int offset = 1) {
	if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menu), out var p2) //Landscaping
		|| p2 is not UIAssetMenuPrefab SurfaceMenu)
		{
			SurfaceMenu = ScriptableObject.CreateInstance<UIAssetMenuPrefab>();
			SurfaceMenu.name = menu;
			var SurfaceMenuUI = SurfaceMenu.AddComponent<UIObject>();
			SurfaceMenuUI.m_Icon = iconPath; //ExtraLib.GetIcon(SurfaceMenu);
			SurfaceMenuUI.m_Priority = prefab.GetComponent<UIObject>().m_Priority + offset;
			SurfaceMenuUI.active = true;
			SurfaceMenuUI.m_IsDebugObject = false;
			SurfaceMenuUI.m_Group = prefab.GetComponent<UIObject>().m_Group;

			// ELT_UI.validMenuForELTSettings.Add(menu);

			ExtraLib.m_PrefabSystem.AddPrefab(SurfaceMenu);
		}
	}
}