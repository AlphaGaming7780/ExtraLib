using Unity.Entities;

namespace ExtraLib.Prefabs
{
    public struct UIAssetMultiCategoryData : IComponentData, IQueryTypeParameter
    {
        public UIAssetMultiCategoryData(Entity parentCategory)
        {
            this.parentCategory = parentCategory;
        }

        public Entity parentCategory;
    }
}
