using Unity.Entities;

namespace ExtraLib.Prefabs
{
    public struct UIAssetChildCategoryData : IComponentData, IQueryTypeParameter
    {
        public UIAssetChildCategoryData(Entity parentCategory)
        {
            this.parentCategory = parentCategory;
        }

        public Entity parentCategory;
    }
}
