using Game.UI.Editor;
using HarmonyLib;
using System;
using System.Linq;

namespace ExtraLib.ClassExtension
{
    public static class EditorAssetCategorySystemExtension
    {
        public static EditorAssetCategory GetCategory(this EditorAssetCategorySystem system, string categoryID)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            if (string.IsNullOrEmpty(categoryID)) throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryID));
            return system.GetCategories().FirstOrDefault(c => c.id == categoryID);
        }

        public static bool TryGetCategory(this EditorAssetCategorySystem system, string categoryID, out EditorAssetCategory category)
        {
            category = system.GetCategory(categoryID);
            if( category == null || category == default ) return false;
            return true;
        }

        public static void AddCategory(this EditorAssetCategorySystem system, EditorAssetCategory category, EditorAssetCategory parent = null)
        {
            Traverse.Create(system)
                .Method("AddCategory", new Type[] { typeof(EditorAssetCategory), typeof(EditorAssetCategory) })
                .GetValue(new object[] { category, parent });
        }

    }
}
