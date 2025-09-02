using ExtraLib.Prefabs;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace ExtraLib.Components
{
    [RequireComponent(typeof(UIObject))]
    [ComponentMenu("Extra", new Type[] { })]
    public class ExtraUIObject : UIObject
    {
        public string[] ExtraTags = new string[] { };

        //public override void GetArchetypeComponents(HashSet<ComponentType> components)
        //{

        //}

        //public override void GetPrefabComponents(HashSet<ComponentType> components)
        //{

        //}

        public override void LateInitialize(EntityManager entityManager, Entity entity)
        {
            base.LateInitialize(entityManager, entity);

            if (!prefab.TryGet<UIObject>(out UIObject uiObject))
            {
                EL.Logger.Error("ExtraUIObject requires a UIObject component to be present on the same GameObject.");
                return;
            }

            if (uiObject.m_Group is not UIAssetChildCategoryPrefab uIAssetChildCategoryPrefab)
            {
                EL.Logger.Error("ExtraUIObject can only be used with UIAssetChildCategoryPrefab or its derived classes.");
                return;
            }

            ExtraTags = ExtraTags.Intersect(uIAssetChildCategoryPrefab.ExtraTags).ToArray();
        }
    }
}
