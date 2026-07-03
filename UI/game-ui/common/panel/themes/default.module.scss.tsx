import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/themes/default.module.scss"

export type PropsDefaultPanelSCSS = {
    header: string
    content: string
    footer: string
    title: string
    floatingHint: string
    tooltipHint: string
    toggle: string
}

export const DefaultPanelSCSS: PropsDefaultPanelSCSS = getModule(path$, "classes")
