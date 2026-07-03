import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/selected-info-panel/shared-components/info-section/info-section.module.scss"

export type PropsInfoSectionSCSS = {
    infoSection: string
    content: string
    column: string
    divider: string
    noMargin: string
    disableFocusHighlight: string
    infoWrapBox: string
}

export const InfoSectionSCSS: PropsInfoSectionSCSS = getModule(path$, "classes")
