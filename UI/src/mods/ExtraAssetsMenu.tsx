import { bindValue, trigger, useValue } from "cs2/api";
import { ModuleRegistryExtend } from "cs2/modding";
import { PropsAssetCategoryTabBar } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar";
import { AssetCategoryTabBarSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss";
import { CategoryItemSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/category-item.module.scss";
import { MouseEvent } from "react";

export const visible$ = bindValue<boolean>("el", 'showcattab');
export const SelectedTab$ = bindValue<string>("el", 'selectedtab');
export const AssetCats$ = bindValue<AssetCat[]>("el", 'assetscat');

export type AssetCat = {
	name: string
	icon: string
}

export function CustomAssetCategoryTabBar(AssetCats: AssetCat[], selectedTab: string, onClick : (value : MouseEvent) => void ) {
	return <div className={AssetCategoryTabBarSCSS.assetCategoryTabBar}>
		<div className={AssetCategoryTabBarSCSS.items}>
			{AssetCats && AssetCats.length && AssetCats.map((AssetCat, index) => {
				return <>
					<button id={AssetCat.name} className={AssetCat.name == selectedTab ? CategoryItemSCSS.button + " selected" : CategoryItemSCSS.button} onClick={onClick} onMouseEnter={() => trigger("audio", "playSound", "hover-item", 1) }>
						<img className={CategoryItemSCSS.icon} src={AssetCat.icon} />
						<div className={CategoryItemSCSS.itemInner} />
					</button>
				</>
			})}
		</div>
	</div>
}

export const ExtraAssetsMenu: ModuleRegistryExtend = (Component: any) => {
	return (props: PropsAssetCategoryTabBar) => {

		var visible: boolean = useValue(visible$);
		var selectedTab: string = useValue(SelectedTab$);
		var assetCats: AssetCat[] = useValue(AssetCats$);

		function OnClick(mouseEvent: MouseEvent) {
			trigger("audio", "playSound", "select-item", 1);
			trigger("el", "selectassetcat", mouseEvent.currentTarget.id)
		}

		// translation handling. Translates using locale keys that are defined in C# or fallback string here.
		// const { translate } = useLocalization();

		var result: JSX.Element = <>
			{visible && CustomAssetCategoryTabBar(assetCats, selectedTab, OnClick)}
			{Component(props)}
		</>


		return result;
	};
}

export const ExtraDetailingDetails: ModuleRegistryExtend = (Component: any) => {
	return (props: any) => {

		

		// translation handling. Translates using locale keys that are defined in C# or fallback string here.
		// const { translate } = useLocalization();


		var result: JSX.Element = <></>

		//if (Component) result = Component(props != undefined ? props : hezfiuehge)

		return result;
	};
}