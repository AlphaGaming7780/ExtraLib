using Unity.Entities;

namespace Extra.Lib.Prefabs
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
