import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/statistics-panel/menu/item/statistics-toggle.module.scss"

export type PropsStatisticsToggleSCSS = {
    size: string
    toggle: string
}

export const StatisticsToggleSCSS: PropsStatisticsToggleSCSS = getModule(path$, "classes")
