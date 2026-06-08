import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/themes/panel-transition.module.scss"

export const PanelTransitionSCSS = {
    enter: getModule(path$, "classes").enter,
    enterActive: getModule(path$, "classes").enterActive,
    exit: getModule(path$, "classes").exit,
    exitActive: getModule(path$, "classes").exitActive,
}
