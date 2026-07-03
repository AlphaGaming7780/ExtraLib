import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/panel.module.scss"

export type PropsPanelSCSS = {
    closeIcon: string
    toggleIcon: string
    toggleIconExpanded: string
    panel: string
    header: string
    content: string
    footer: string
    titleBar: string
    title: string
    icon: string
    iconSpace: string
    closeButton: string
    toggle: string
}

export const PanelSCSS: PropsPanelSCSS = getModule(path$, "classes")
