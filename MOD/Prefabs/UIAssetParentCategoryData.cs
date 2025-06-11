using Unity.Entities;

namespace ExtraLib.Prefabs
{
    public struct UIAssetParentCategoryData : IComponentData, IQueryTypeParameter
    {
        public UIAssetParentCategoryData(Entity parentCategory)
        {
            this.parentCategoryOrMenu = parentCategory;
        }

        public Entity parentCategoryOrMenu;
    }
}
