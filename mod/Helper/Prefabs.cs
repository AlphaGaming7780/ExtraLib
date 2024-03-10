using System;
using System.Collections.Generic;
using Colossal.Entities;
using Extra.Lib.Debugger;
using Game.Common;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Extra.Lib.Helper;

public static class PrefabsHelper
{
	public static UIAssetCategoryPrefab GetUIAssetCategoryPrefab(string cat)
	{

		if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), cat), out var p1)
			|| p1 is not UIAssetCategoryPrefab Category)
		{	
			Print.Error($"Failed to get the UIAssetCategoryPrefab with this name : {cat}");
			return null;
		}

		return Category;

	}

	public static UIAssetCategoryPrefab GetOrCreateUIAssetCategoryPrefab(string menu, string cat, string iconPath, string behindcat = null)
	{

		if (ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), cat), out var p1)
			&& p1 is UIAssetCategoryPrefab newCategory)
		{
			return newCategory;
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

		newCategory = ScriptableObject.CreateInstance<UIAssetCategoryPrefab>();
		newCategory.name = cat;
		newCategory.m_Menu = landscapingMenu;
		var newCategoryUI = newCategory.AddComponent<UIObject>();
		newCategoryUI.m_Icon = iconPath; //?? ExtraLib.GetIcon(surfaceCategory);
		if(behindCategory != null) newCategoryUI.m_Priority = behindCategory.GetComponent<UIObject>().m_Priority+1;
		newCategoryUI.active = true;
		newCategoryUI.m_IsDebugObject = false;

		ExtraLib.m_PrefabSystem.AddPrefab(newCategory);

		return newCategory;
	}

	public static void CreateNewUIAssetMenuPrefab(PrefabBase prefab, string menu, string iconPath, int offset = 1) {
	if (!ExtraLib.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menu), out var p2)
		|| p2 is not UIAssetMenuPrefab Menu)
		{
			Menu = ScriptableObject.CreateInstance<UIAssetMenuPrefab>();
			Menu.name = menu;
			var MenuUI = Menu.AddComponent<UIObject>();
			MenuUI.m_Icon = iconPath; //ExtraLib.GetIcon(SurfaceMenu);
			MenuUI.m_Priority = prefab.GetComponent<UIObject>().m_Priority + offset;
			MenuUI.active = true;
			MenuUI.m_IsDebugObject = false;
			MenuUI.m_Group = prefab.GetComponent<UIObject>().m_Group;

			// ELT_UI.validMenuForELTSettings.Add(menu);

			ExtraLib.m_PrefabSystem.AddPrefab(Menu);
		}
	}
}