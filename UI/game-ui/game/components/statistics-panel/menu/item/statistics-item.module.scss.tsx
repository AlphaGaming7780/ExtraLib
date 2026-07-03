import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/statistics-panel/menu/item/statistics-item.module.scss"

export type PropsStatisticsItemSCSS = {
    locked: string
    label: string
}

export const StatisticsItemSCSS: PropsStatisticsItemSCSS = getModule(path$, "classes")
