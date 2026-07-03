import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/selected-info-panel/selected-info-sections/shared-sections/actions-section/action-button.module.scss"

export type PropsActionButtonSCSS = {
    button: string
    icon: string
}

export const ActionButtonSCSS: PropsActionButtonSCSS = getModule(path$, "classes")
