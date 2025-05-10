using System;
using Colossal.IO.AssetDatabase;
using System.IO;
using ExtraLib.Prefabs;
using ExtraLib.Systems.UI;
using Game.Prefabs;
using UnityEngine;
using static Game.Rendering.NotificationIconBufferSystem;
using static Colossal.AssetPipeline.Diagnostic.Report;

namespace ExtraLib.Helpers
{
    public static class PrefabsHelper
    {
        internal const string CatTabName = "ExtraAssetsMenu";
        public static void LoadPrefabsInDirectory(string directoryPath, bool recursive = true)
        {
            if (!Directory.Exists(directoryPath)) return;

            LoadPrefabInDirectory(directoryPath);

            if (recursive) foreach (string dir in Directory.EnumerateDirectories(directoryPath))
                {
                    LoadPrefabsInDirectory(dir);
                }
        }

        public static void LoadPrefabInDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return;

            foreach (string s in DefaultAssetFactory.instance.GetSupportedExtensions())
            {
                foreach (string file in Directory.GetFiles(directoryPath, $"*{s}"))
                {
                    AssetDataPath assetDataPath = AssetDataPath.Create(Path.GetDirectoryName(file), Path.GetFileName(file), EscapeStrategy.None);
                    try
                    {
                        IAssetData assetData = AssetDatabase.user.AddAsset(assetDataPath);
                        if (assetData is PrefabAsset prefabAsset)
                        {
                            PrefabBase prefabBase = prefabAsset.Load<PrefabBase>();
                            EL.m_PrefabSystem.AddPrefab(prefabBase);
                        }
                    }
                    catch (Exception e)
                    {
                        EL.Logger.Warn(e);
                    }
                }
            }
        }

        public static UIAssetCategoryPrefab GetUIAssetCategoryPrefab(string cat)
        {

            if (!EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetCategoryPrefab), cat), out var p1)
                || p1 is not UIAssetCategoryPrefab Category)
            {
                EL.Logger.Error($"Failed to get the UIAssetCategoryPrefab with this name : {cat}");
                return null;
            }

            return Category;

        }

        public static UIAssetChildCategoryPrefab GetUIAssetChildCategoryPrefab(string cat)
        {

            if (!EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetChildCategoryPrefab), cat), out var p1)
                || p1 is not UIAssetChildCategoryPrefab Category)
            {
                EL.Logger.Error($"Failed to get the UIAssetChildCategoryPrefab with this name : {cat}");
                return null;
            }

            return Category;

        }

        public static UIAssetChildCategoryPrefab GetOrCreateUIAssetChildCategoryPrefab(string parentCategoryName, string catName, string iconPath = null, string behindcat = null)
        {
            return GetOrCreateUIAssetChildCategoryPrefab(GetOrCreateUIAssetParentCategoryPrefab(parentCategoryName), catName, iconPath, behindcat);
        }

        public static UIAssetChildCategoryPrefab GetOrCreateUIAssetChildCategoryPrefab(UIAssetParentCategoryPrefab parentCategory, string catName, string iconPath = null, string behindcat = null)
        {

            if (EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetChildCategoryPrefab), catName), out var p1)
                && p1 is UIAssetChildCategoryPrefab newCategory)
            {
                return newCategory;
            }

            UIAssetChildCategoryPrefab behindCategory = null;

            if (behindcat != null)
            {
                if (!EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetChildCategoryPrefab), behindcat), out var p3)
                    || p3 is not UIAssetChildCategoryPrefab behindCategory2)
                {
                    EL.Logger.Error($"Failed to get the UIAssetChildCategoryPrefab with this name : {behindcat}");
                    return null;
                }
                else
                {
                    behindCategory = behindCategory2;
                }
            }

            newCategory = ScriptableObject.CreateInstance<UIAssetChildCategoryPrefab>();
            newCategory.name = catName;
            newCategory.parentCategory = parentCategory;
            var newCategoryUI = newCategory.AddComponent<UIObject>();
            newCategoryUI.m_Icon = iconPath ?? Icons.GetIcon(newCategory);
            if (behindCategory != null) newCategoryUI.m_Priority = behindCategory.GetComponent<UIObject>().m_Priority + 1;
            newCategoryUI.active = true;
            newCategoryUI.m_IsDebugObject = false;

            EL.m_PrefabSystem.AddPrefab(newCategory);

            return newCategory;
        }

        public static UIAssetParentCategoryPrefab GetOrCreateUIAssetParentCategoryPrefab(string parentCategoryName, string iconPath = null)
        {
            if (EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetParentCategoryPrefab), parentCategoryName), out var p1)
                && p1 is UIAssetParentCategoryPrefab parentCategory)
            {
                return parentCategory;
            }

            parentCategory = ScriptableObject.CreateInstance<UIAssetParentCategoryPrefab>();
            parentCategory.name = parentCategoryName;
            parentCategory.parentCategoryOrMenu = GetOrCreateNewUIAssetMenuPrefab(CatTabName, Icons.GetIcon);
            var parentCategoryUI = parentCategory.AddComponent<UIObject>();
            parentCategoryUI.m_Icon = iconPath ?? Icons.GetIcon(parentCategory);
            parentCategoryUI.active = true;

            EL.m_PrefabSystem.AddPrefab(parentCategory);

            return parentCategory;
        }

        /// <summary>
        /// Create a new menu in the ToolBar.
        /// </summary>
        /// <param name="menuName">The name of the menu</param>
        /// <param name="getIcons">the function called that return the icon path.</param>
        /// <param name="toolBarGroup">The Tool bar group, can be : `Services Toolbar Group`, `Zones Toolbar Group` and `Tools Toolbar Group` or your custom tool bar group.</param>
        /// <param name="offset">The position of the menu in is group.</param>
        public static void CreateUIAssetMenuPrefab(string menuName, Func<PrefabBase, string> getIcons, string toolBarGroup = "Services Toolbar Group", int offset = 999)
        {
            if (!EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menuName), out var p1)
                || p1 is not UIAssetMenuPrefab menuPrefab)
            {

                if (!EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIToolbarGroupPrefab), toolBarGroup), out PrefabBase prefab) || prefab is not UIToolbarGroupPrefab uIToolbarGroupPrefab)
                {
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

                EL.m_PrefabSystem.AddPrefab(menuPrefab);
            }
        }


        /// <summary>
        /// Get or Create a menu in the ToolBar.
        /// </summary>
        /// <param name="menuName">The name of the menu</param>
        /// <param name="getIcons">the function called that return the icon path.</param>
        /// <param name="toolBarGroup">The Tool bar group, can be : `Services Toolbar Group`, `Zones Toolbar Group` and `Tools Toolbar Group` or your custom tool bar group.</param>
        /// <param name="offset">The position of the menu in is group.</param>
        public static UIAssetMenuPrefab GetOrCreateNewUIAssetMenuPrefab(string menuName, Func<PrefabBase, string> getIcons, string toolBarGroup = "Services Toolbar Group", int offset = 999)
        {

            if (EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIAssetMenuPrefab), menuName), out var p1) && p1 is UIAssetMenuPrefab menuPrefab)
            {
                return menuPrefab;
            }

            if (!EL.m_PrefabSystem.TryGetPrefab(new PrefabID(nameof(UIToolbarGroupPrefab), toolBarGroup), out PrefabBase prefab) || prefab is not UIToolbarGroupPrefab uIToolbarGroupPrefab)
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

            EL.m_PrefabSystem.AddPrefab(menuPrefab);

            return menuPrefab;
        }
    }
}