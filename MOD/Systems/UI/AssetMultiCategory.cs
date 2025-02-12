using Colossal.Entities;
using Colossal.UI.Binding;
using ExtraLib.Prefabs;
using Game.Prefabs;
using Game.UI;
using Game.UI.InGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace ExtraLib.Systems.UI
{

    internal partial class AssetMultiCategory : UISystemBase
    {
        public static AssetMultiCategory instance;
        private ToolbarUISystem _ToolbarUISystem;
        Traverse _ToolbarUISystemTraverse;

        private PrefabSystem _PrefabSystem;
        private ImageSystem _ImageSystem;

        private bool _alreadyUpdated = false;

        private readonly Dictionary<Entity, Entity> _LastSelectedCategories = new Dictionary<Entity, Entity>();

        private List<Entity> m_SelectedThemes => _ToolbarUISystemTraverse.Field("m_SelectedThemes").GetValue<List<Entity>>();
        private List<Entity> m_SelectedAssetPacks => _ToolbarUISystemTraverse.Field("m_SelectedAssetPacks").GetValue<List<Entity>>();

        private Dictionary<Entity, Entity> m_LastSelectedCategories => _ToolbarUISystemTraverse.Field("m_LastSelectedCategories").GetValue<Dictionary<Entity, Entity>>();

        private readonly List<Entity> _SelectedAssetMultiCategories = new List<Entity>();
        private Entity _SelectedAssetCategory = Entity.Null;

        private RawValueBinding _AssetMultiCategoriesBinding;
        private GetterValueBinding<List<Entity>> _SelectedAssetMultiCategoriesBinding;

        protected override void OnCreate()
        {
            base.OnCreate();
            instance = this;
            _PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            _ImageSystem = World.GetOrCreateSystemManaged<ImageSystem>();
            _ToolbarUISystem = World.GetOrCreateSystemManaged<ToolbarUISystem>();
            _ToolbarUISystemTraverse = Traverse.Create(_ToolbarUISystem);

            AddBinding(_AssetMultiCategoriesBinding = new RawValueBinding("el", "AssetMultiCategories", new Action<IJsonWriter>(this.WriteAssetMultiCategories)));
            AddBinding(_SelectedAssetMultiCategoriesBinding = new GetterValueBinding<List<Entity>>("el", "SelectedAssetMultiCategories", () => _SelectedAssetMultiCategories, new ListWriter<Entity>()));
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        private void WriteAssetMultiCategories(IJsonWriter writer)
        {

            if(_SelectedAssetMultiCategories.Count <= 1 )
            {
                writer.WriteEmptyArray();
                return;
            }

            writer.ArrayBegin(_SelectedAssetMultiCategories.Count - 1);

            for (int i = 0; i < _SelectedAssetMultiCategories.Count - 1; i++) // Why start at 0 and do -1, well because last selected cat, doesn't have more UI cats under it.
            {
                Entity entity = _SelectedAssetMultiCategories[i];
                if(base.EntityManager.TryGetBuffer(entity, true, out DynamicBuffer<UIGroupElement> elements))
                {
                    NativeList<UIObjectInfo> sortedCategories = this.GetSortedCategories(elements);
                    writer.ArrayBegin(sortedCategories.Length);
                    for (int j = 0; j < sortedCategories.Length; j++)
                    {
                        WriteAssetCategory(writer, sortedCategories[j]);
                    }
                    writer.ArrayEnd();
                    sortedCategories.Dispose();
                } 
                else
                {
                    writer.WriteEmptyArray();
                }
            }

            writer.ArrayEnd();
        }

        private void WriteAssetCategory(IJsonWriter writer, UIObjectInfo objectInfo)
        {
            Entity entity = objectInfo.entity;
            PrefabBase prefab = this._PrefabSystem.GetPrefab<PrefabBase>(objectInfo.prefabData);
            writer.TypeBegin("toolbar.AssetCategory");
            writer.PropertyName("entity");
            writer.Write(entity);
            writer.PropertyName("name");
            writer.Write(prefab.name);
            writer.PropertyName("icon");
            writer.Write(ImageSystem.GetIcon(prefab) ?? _ImageSystem.placeholderIcon);
            writer.PropertyName("locked");
            writer.Write(base.EntityManager.HasEnabledComponent<Locked>(entity));
            writer.PropertyName("uiTag");
            writer.Write(prefab.uiTag);
            writer.PropertyName("highlight");
            writer.Write(base.EntityManager.HasComponent<UIHighlight>(entity));
            writer.TypeEnd();
        }

        private void UpdateSelectedAssetCategories(Entity assetCategories)
        {
            _SelectedAssetCategory = assetCategories;
            UpdateSelectedAssetMultiCategories();
        }

        private void UpdateSelectedAssetMultiCategories()
        {
            _SelectedAssetMultiCategories.Clear();
            Entity entity = _SelectedAssetCategory;

            while (true)
            {

                if (!TryGetParentCategory(entity, out Entity parentEntity)) break;

                _SelectedAssetMultiCategories.Insert(0, entity);

                entity = parentEntity;

            }

            _SelectedAssetMultiCategoriesBinding.TriggerUpdate();
            _AssetMultiCategoriesBinding.Update();
        }

        internal void OnSelectAssetMenu(Entity assetMenu)
        {
            if(m_LastSelectedCategories.TryGetValue(assetMenu, out Entity lastCat))
            {
                if (!EntityManager.HasComponent<UIAssetParentCategoryData>(lastCat)) return;
            }
            Entity deepestAssetCategory = GetDeepestAssetCategory(assetMenu);
            _alreadyUpdated = true;
            SelectAssetCategory(deepestAssetCategory);
        }

        internal void OnSelectAssetCategory(Entity assetCategory)
        {

            if (_SelectedAssetCategory == assetCategory) return;

            if (_alreadyUpdated)
            {
                _alreadyUpdated = false;
                UpdateSelectedAssetCategories(assetCategory);
                return;
            }

            if (!TryGetParentCategory(assetCategory, out Entity parentCategory))
            {
                UpdateSelectedAssetCategories(assetCategory);
                return;
            }

            if (_LastSelectedCategories.ContainsKey(parentCategory)) _LastSelectedCategories[parentCategory] = assetCategory;
            else _LastSelectedCategories.Add(parentCategory, assetCategory);

            Entity deepestAssetCategory = GetDeepestAssetCategory(assetCategory);

            UpdateSelectedAssetCategories(deepestAssetCategory);

            if (deepestAssetCategory == assetCategory) return;

            SelectAssetCategory(deepestAssetCategory);
        }


        internal Entity GetDeepestAssetCategory(Entity assetParentCategoryOrMenu)
        {
            List<Entity> selectedThemes = m_SelectedThemes;
            List<Entity> selectedAssetPacks = m_SelectedAssetPacks;
            do
            {
                if (!_LastSelectedCategories.TryGetValue(assetParentCategoryOrMenu, out Entity firstItem))
                {
                    firstItem = GetFirstItem(assetParentCategoryOrMenu, selectedThemes, selectedAssetPacks);
                    _LastSelectedCategories.Add(assetParentCategoryOrMenu, firstItem);
                }

                if (firstItem == null) break;

                if (EntityManager.HasComponent<UIAssetChildCategoryData>(firstItem))
                {
                    assetParentCategoryOrMenu = firstItem;
                    break;
                }

                if (!EntityManager.HasComponent<UIAssetParentCategoryData>(firstItem)) break;

                assetParentCategoryOrMenu = firstItem;
            }
            while (true);

            if (assetParentCategoryOrMenu == null)
            {
                EL.Logger.Warn("parentItem is null");
            }

            return assetParentCategoryOrMenu;

        }

        private bool TryGetParentCategory(Entity assetCategory, out Entity parentCategory)
        {
            parentCategory = Entity.Null;
            if (EntityManager.TryGetComponent<UIAssetParentCategoryData>(assetCategory, out UIAssetParentCategoryData component))
            {
                parentCategory = component.parentCategoryOrMenu;
            }
            else if (EntityManager.TryGetComponent<UIAssetChildCategoryData>(assetCategory, out UIAssetChildCategoryData component1))
            {
                parentCategory = component1.parentCategory;
            }
            else
            {
                return false;
            }
            return true;
        }

        private Entity GetFirstItem(Entity groupEntity, List<Entity> themes, List<Entity> packs)
        {
            return _ToolbarUISystemTraverse.Method("GetFirstItem", [typeof(Entity), typeof(List<Entity>), typeof(List<Entity>)]).GetValue<Entity>(groupEntity, themes, packs);
        }

        private void SelectAssetCategory(Entity entity)
        {
            _ToolbarUISystemTraverse.Method("SelectAssetCategory", [typeof(Entity)]).GetValue(entity);
        }

        private NativeList<UIObjectInfo> GetSortedCategories(DynamicBuffer<UIGroupElement> elements)
        {
            return _ToolbarUISystemTraverse.Method("GetSortedCategories", [typeof(DynamicBuffer<UIGroupElement>)]).GetValue<NativeList<UIObjectInfo>>(elements);
        }

    }
}
