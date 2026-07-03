import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss"

export type PropsAssetCategoryTabBarSCSS = {
    assetCategoryTabBar: string
    tabIcon: string
    locked: string
    lock: string
    items: string
    closeButton: string
}

export const AssetCategoryTabBarSCSS: PropsAssetCategoryTabBarSCSS = getModule(path$, "classes")
