import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-menu.module.scss"

export type PropsAssetMenuSCSS = {
    assetPanel: string
    gamepadActive: string
    detailContainer: string
    detailPanel: string
}

export const AssetMenuSCSS: PropsAssetMenuSCSS = getModule(path$, "classes")
