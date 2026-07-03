import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/statistics-panel/menu/item/statistics-checkbox.module.scss"

export type PropsStatisticsCheckboxSCSS = {
    toggle: string
}

export const StatisticsCheckboxSCSS: PropsStatisticsCheckboxSCSS = getModule(path$, "classes")
