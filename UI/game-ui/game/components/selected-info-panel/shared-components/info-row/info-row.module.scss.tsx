import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/selected-info-panel/shared-components/info-row/info-row.module.scss"

export type PropsInfoRowSCSS = {
    infoRow: string
    disableFocusHighlight: string
    link: string
    tooltipRow: string
    left: string
    hasIcon: string
    right: string
    icon: string
    uppercase: string
    subRow: string
}

export const InfoRowSCSS: PropsInfoRowSCSS = getModule(path$, "classes")
