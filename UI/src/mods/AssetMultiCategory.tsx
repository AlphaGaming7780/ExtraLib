import { bindValue, trigger, useValue } from "cs2/api";
import { ModuleRegistryExtend } from "cs2/modding";
import { CategoryItem, PropsAssetCategoryTabBar } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar";
import { AssetCategoryTabBarSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss";
import { CategoryItemSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/category-item.module.scss";
import { MouseEvent } from "react";
import { useLocalization } from "cs2/l10n";
import { AssetDetailPanelSCSS } from "../../game-ui/game/components/asset-menu/asset-detail-panel/asset-detail-panel.module.scss";
import { AssetMenuSCSS } from "../../game-ui/game/components/asset-menu/asset-menu.module.scss";
import { FormattedParagraphsSCSS } from "../../game-ui/common/text/formatted-paragraphs.module.scss";
import { FormattedTextSCSS } from "../../game-ui/common/text/formatted-text.module.scss";
import { AssetCategory, Entity, UISound, toolbar } from "cs2/bindings";
import { entityEquals } from "cs2/utils";

export const SelectedAssetMultiCategories$ = bindValue<Entity[]>("el", 'SelectedAssetMultiCategories');
export const AssetMultiCategories$ = bindValue <AssetCategory[][]>("el", 'AssetMultiCategories');

export function CustomAssetCategoryTabBar(AssetCats: AssetCategory[], selectedTab: Entity, onClick : (value : Entity) => void ) {
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
        var AssetMultiCategories: AssetCategory[][] = useValue(AssetMultiCategories$);

		if (SelectedAssetMultiCategories && SelectedAssetMultiCategories.length > 0) {
			props.selectedCategory = SelectedAssetMultiCategories[0];
		}

		var result: JSX.Element = <>
			<Component categories={props.categories} selectedCategory={props.selectedCategory} onChange={props.onChange} onClose={props.onClose} />
			{AssetMultiCategories && AssetMultiCategories.length > 0 && AssetMultiCategories.map((AssetCategories: AssetCategory[], index: number) => {
				return CustomAssetCategoryTabBar(AssetCategories, SelectedAssetMultiCategories[index + 1], toolbar.selectAssetCategory)
			})}
		</>



		return result;
	};
}