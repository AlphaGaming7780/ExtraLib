import { bindValue, useValue } from "cs2/api";
import { ModuleRegistryExtend } from "cs2/modding";
import { CategoryItem, PropsAssetCategoryTabBar } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar";
import { AssetCategoryTabBarSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss";
import { AssetCategory, Entity, toolbar } from "cs2/bindings";
import { entityEquals } from "cs2/utils";


export interface ExtraAssetCategory extends AssetCategory {
    ExtraTags: string[];
}

export const SelectedAssetMultiCategories$ = bindValue<Entity[]>("el", 'SelectedAssetMultiCategories');
export const AssetMultiCategories$ = bindValue<ExtraAssetCategory[][]>("el", 'AssetMultiCategories');

export function CustomAssetCategoryTabBar(AssetCats: ExtraAssetCategory[], selectedTab: Entity, onClick : (value : Entity) => void ) {
	return <div className={AssetCategoryTabBarSCSS.assetCategoryTabBar}>
		<div className={AssetCategoryTabBarSCSS.items}>
			{AssetCats && AssetCats.length > 0 && AssetCats.map((AssetCat, index) => {
				return CategoryItem(AssetCat, entityEquals(AssetCat.entity, selectedTab), AssetCats.length <= 1, onClick)
			})}
		</div>
	</div>
}

export const AssetMultiCategory: ModuleRegistryExtend = (Component: any) => {
    return (props: PropsAssetCategoryTabBar) => {

        var SelectedAssetMultiCategories: Entity[] = useValue(SelectedAssetMultiCategories$);
		var AssetMultiCategories: ExtraAssetCategory[][] = useValue(AssetMultiCategories$);

		if (SelectedAssetMultiCategories && SelectedAssetMultiCategories.length > 0) {
			props.selectedCategory = SelectedAssetMultiCategories[0];
		}

		var result: JSX.Element = <>
			<Component categories={props.categories} selectedCategory={props.selectedCategory} onChange={props.onChange} onClose={props.onClose} />
			{AssetMultiCategories && AssetMultiCategories.length > 0 && AssetMultiCategories.map((AssetCategories: ExtraAssetCategory[], index: number) => {
				return CustomAssetCategoryTabBar(AssetCategories, SelectedAssetMultiCategories[index + 1], toolbar.selectAssetCategory)
			})}
		</>

		return result;
	};
}