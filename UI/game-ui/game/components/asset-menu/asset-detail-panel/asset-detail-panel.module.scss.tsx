import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-detail-panel/asset-detail-panel.module.scss"

export type PropsAssetDetailPanelSCSS = {
    assetDetailPanel: string
    titleBar: string
    title: string
    constructionCostField: string
    notEnoughMoney: string
    constructionCostIcon: string
    row: string
    content: string
    previewContainer: string
    preview: string
    column: string
    description: string
    effects: string
    statsRow: string
    requirementsRow: string
    alreadyBuiltRow: string
}

export const AssetDetailPanelSCSS: PropsAssetDetailPanelSCSS = getModule(path$, "classes")
