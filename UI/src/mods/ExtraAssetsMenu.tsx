import { bindValue, trigger, useValue } from "cs2/api";
import { ModuleRegistryExtend } from "cs2/modding";
import { PropsAssetCategoryTabBar } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar";
import { AssetCategoryTabBarSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss";
import { CategoryItemSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/category-item.module.scss";
import { ForwardedRef, MouseEvent } from "react";
import { time } from "cs2/bindings";

export const visible$ = bindValue<boolean>("el", 'showcattab');
export const SelectedTab$ = bindValue<string>("el", 'selectedtab');
export const AssetCats$ = bindValue<AssetCat[]>("el", 'assetscat');
export const MouseOverAssetCats$ = bindValue<AssetCat>("el", 'mouseoverassetcat');

export type AssetCat = {
	name: string
	icon: string
}

export const NullAssetCat: AssetCat = {name:"null", icon:"null"}

export function CustomAssetCategoryTabBar(AssetCats: AssetCat[], selectedTab: string, onClick : (value : MouseEvent) => void ) {
	return <div className={AssetCategoryTabBarSCSS.assetCategoryTabBar}>
		<div className={AssetCategoryTabBarSCSS.items}>
			{AssetCats && AssetCats.length && AssetCats.map((AssetCat, index) => {
				return <>
                    <button id={AssetCat.name} className={AssetCat.name == selectedTab ? CategoryItemSCSS.button + " selected" : CategoryItemSCSS.button} onClick={onClick} onMouseEnter={() => { trigger("audio", "playSound", "hover-item", 1); trigger("el", "mouseoverassetcat", AssetCat) }} onMouseLeave={() => trigger("el", "mouseoverassetcat", NullAssetCat)} >   
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

export const ExtraDetailingDetails: ModuleRegistryExtend = (
    Component
) => {
    return (props) => {
        const { children, ...otherProps } = props || {};

        var mouseOverAssetCat : AssetCat = useValue(MouseOverAssetCats$);

        if (mouseOverAssetCat.name === NullAssetCat.name) {
            return <Component {...otherProps}>{children}</Component> 
        }

        return (
            <>
                <div
                    style={{
                        bottom: 0,
                        left: 0,
                        right: 0,
                        position: "absolute",
                        height: "255rem",
                    }}
                >
                    <div style={{ position: "relative" }}>
                        <div className="asset-detail-panel_hf8 detail-panel_izf">
                            <div className="title-bar_I7O child-opacity-transition_nkS">
                                <div className="title_qub">{mouseOverAssetCat.name}</div>
                            </div>
                            <div className="content_rep row_H0d child-opacity-transition_nkS">
                                <div className="preview-container_sPA">
                                    <img className="preview_MDY" src={mouseOverAssetCat.icon}/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <Component {...otherProps}>{children}</Component>
            </>
        );
    };
};