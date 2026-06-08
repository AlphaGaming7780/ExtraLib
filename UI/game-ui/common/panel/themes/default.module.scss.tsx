import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/themes/default.module.scss"

export const DefaultPanelSCSS = {
    header: getModule(path$, "classes").header,
    content: getModule(path$, "classes").content,
    footer: getModule(path$, "classes").footer,
    title: getModule(path$, "classes").title,
    floatingHint: getModule(path$, "classes").floatingHint,
    tooltipHint: getModule(path$, "classes").tooltipHint,
    toggle: getModule(path$, "classes").toggle,
}
