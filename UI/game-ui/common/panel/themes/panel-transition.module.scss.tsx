import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/themes/panel-transition.module.scss"

export type PropsPanelTransitionSCSS = {
    enter: string
    enterActive: string
    exit: string
    exitActive: string
}

export const PanelTransitionSCSS: PropsPanelTransitionSCSS = getModule(path$, "classes")
