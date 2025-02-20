using Colossal.UI.Binding;
using ExtraLib.Helpers;
using ExtraLib.Patches;
using ExtraLib.ClassExtension;
using ExtraLib.Prefabs;
using Game.Prefabs;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace ExtraLib.Systems.UI;

public partial class ExtraAssetsMenu : UISystemBase
{

    //public class AssetCat : IJsonWritable, IJsonReadable
    //{
    //    public string name = "Not Good";
    //    public string icon = Icons.GetIcon(null);

    //    public AssetCat(string name, string icon)
    //    {
    //        this.name = name;
    //        this.icon = icon;
    //    }

    //    public AssetCat()
    //    {
    //    }

    //    public static AssetCat Null = new("null", "null");

    //    public void Write(IJsonWriter writer)
    //    {
    //        writer.TypeBegin("AssetCat");
    //        writer.PropertyName("name");
    //        writer.Write(name);
    //        writer.PropertyName("icon");
    //        writer.Write(icon);
    //        writer.TypeEnd();
    //    }

    //    public void Read(IJsonReader reader)
    //    {
    //        reader.ReadMapBegin();
    //        reader.ReadProperty("name");
    //        reader.Read(out name);
    //        reader.ReadProperty("icon");
    //        reader.Read(out icon);
    //        reader.ReadMapEnd();
    //    }
    //}

    internal const string CatTabName = "ExtraAssetsMenu";

    //private static readonly List<AssetCat> assetsCats = [];
    //private static readonly Dictionary<string, List<UIAssetCategoryPrefab>> categories = [];
    //private static string selectedCat = "";
    //internal static bool showCatTab = false;
    //private AssetCat mouseOverAssetCat = AssetCat.Null;

    //static ValueBinding<AssetCat[]> VB_assetsCats;
    //static GetterValueBinding<bool> GVB_ShowCatTab;
    //static GetterValueBinding<string> GVB_SelectedCat;
    //static GetterValueBinding<AssetCat> GVB_MouserOnAssetCat;

    protected override void OnCreate()
    {
        Enabled = false;
        base.OnCreate();
        //AddBinding(VB_assetsCats = new ValueBinding<AssetCat[]>("el", "assetscat", [.. assetsCats], new ArrayWriter<AssetCat>(new ValueWriter<AssetCat>())));
        //AddBinding(GVB_ShowCatTab = new GetterValueBinding<bool>("el", "showcattab", () => showCatTab));
        //AddBinding(GVB_SelectedCat = new GetterValueBinding<string>("el", "selectedtab", () => selectedCat));
        //AddBinding(GVB_MouserOnAssetCat = new GetterValueBinding<AssetCat>("el", "mouseoverassetcat", () => mouseOverAssetCat));
        //AddBinding(new TriggerBinding<string>("el", "selectassetcat", new Action<string>(OnAssetCatClick)));
        //AddBinding(new TriggerBinding<AssetCat>("el", "mouseoverassetcat", new Action<AssetCat>((AssetCat) => { mouseOverAssetCat = AssetCat; GVB_MouserOnAssetCat.Update(); })));

    }

    //internal static void ShowCatsTab(bool value)
    //{
    //    showCatTab = value;
    //    if (showCatTab && string.IsNullOrEmpty(selectedCat)) OnAssetCatClick(assetsCats.First().name);
    //    GVB_ShowCatTab.Update();
    //}

    //private static void OnAssetCatClick(string assetCatName)
    //{
    //    bool first = true;
    //    foreach (string assetCat in categories.Keys)
    //    {
    //        foreach (UIAssetCategoryPrefab uIAssetCategoryPrefab in categories[assetCat])
    //        {
    //            Entity entity = EL.m_PrefabSystem.GetEntity(uIAssetCategoryPrefab);
    //            if (assetCat == assetCatName)
    //            {
    //                uIAssetCategoryPrefab.m_Menu.AddElement(entity);
    //                if (first)
    //                {
    //                    ToolbarUISystemPatch.SelectCatUI(entity);
    //                    first = false;
    //                }
    //            }
    //            else uIAssetCategoryPrefab.m_Menu.RemoveElement(entity);
    //        }
    //    }

    //    selectedCat = assetCatName;
    //    GVB_SelectedCat.Update();
    //    ToolbarUISystemPatch.UpdateMenuUI();
    //}

    //public static UIAssetChildCategoryPrefab GetOrCreateNewUIAssetChildCategoryPrefab(string name, UIAssetParentCategoryPrefab parentCategory)
    //{
    //    return PrefabsHelper.GetOrCreateUIAssetChildCategoryPrefab(parentCategory, $"{name} {parentCategory.name}");
    //}

    //public static UIAssetChildCategoryPrefab GetOrCreateNewUIAssetChildCategoryPrefab(string name, string parentCategoryName)
    //{
    //    return PrefabsHelper.GetOrCreateUIAssetChildCategoryPrefab(parentCategoryName, $"{name} {parentCategoryName}");
    //}

}
