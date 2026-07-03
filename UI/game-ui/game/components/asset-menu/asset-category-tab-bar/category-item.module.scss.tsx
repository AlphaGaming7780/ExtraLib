import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-category-tab-bar/category-item.module.scss"

export type PropsCategoryItemSCSS = {
    button: string
    icon: string
    locked: string
    itemInner: string
    highlight: string
    lock: string
    singleTab: string
}

export const CategoryItemSCSS: PropsCategoryItemSCSS = getModule(path$, "classes")
