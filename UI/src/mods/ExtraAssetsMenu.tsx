import { bindValue, trigger, useValue } from "cs2/api";
import { ModuleRegistryExtend } from "cs2/modding";
import { PropsAssetCategoryTabBar } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar";
import { AssetCategoryTabBarSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss";
import { CategoryItemSCSS } from "../../game-ui/game/components/asset-menu/asset-category-tab-bar/category-item.module.scss";
import { MouseEvent } from "react";
import { useLocalization } from "cs2/l10n";
import { AssetDetailPanelSCSS } from "../../game-ui/game/components/asset-menu/asset-detail-panel/asset-detail-panel.module.scss";
import { AssetMenuSCSS } from "../../game-ui/game/components/asset-menu/asset-menu.module.scss";
import { FormattedParagraphsSCSS } from "../../game-ui/common/text/formatted-paragraphs.module.scss";
import { FormattedTextSCSS } from "../../game-ui/common/text/formatted-text.module.scss";

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
		 const { translate } = useLocalization();

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
        const { translate } = useLocalization();

        const titleTranslated = translate("SubServices.NAME[" + mouseOverAssetCat.name + "]") ? translate("SubServices.NAME[" + mouseOverAssetCat.name + "]") : mouseOverAssetCat.name
        const descTranslated = translate("Assets.SUB_SERVICE_DESCRIPTION[" + mouseOverAssetCat.name + "]") ? translate("Assets.SUB_SERVICE_DESCRIPTION[" + mouseOverAssetCat.name + "]") : mouseOverAssetCat.name

        return (
            <>
                {(mouseOverAssetCat.name !== NullAssetCat.name &&
                    <div
                        style={{
                            bottom: 0,
                            left: 0,
                            right: 0,
                            position: "absolute",
                            height: "254rem",
                        }}
                    >
                        <div style={{ position: "relative" }}>
                            <div className={AssetDetailPanelSCSS.assetDetailPanel + " " + AssetMenuSCSS.detailPanel}>
                                <div className={AssetDetailPanelSCSS.titleBar}>
                                    <div className={AssetDetailPanelSCSS.title}>{titleTranslated}</div>
                                </div>
                                <div className={AssetDetailPanelSCSS.content}>
                                    <div className={AssetDetailPanelSCSS.previewContainer}>
                                        <img className={AssetDetailPanelSCSS.preview} src={mouseOverAssetCat.icon} />
                                    </div>
                                    <div className={AssetDetailPanelSCSS.column}>
                                        <div className={AssetDetailPanelSCSS.description + " " + FormattedParagraphsSCSS.paragraphs}>
                                            <p className={FormattedTextSCSS.p} cohinline="cohinline">{descTranslated}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                )}
                <Component {...otherProps}>{children}</Component>
            </>
        );
    };
};