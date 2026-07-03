import { getModule } from "cs2/modding"

const path$ = "game-ui/menu/widgets/toggle-field/toggle-field.module.scss"

export type PropsToggleFieldSCSS = {
    toggle: string
    radioToggle: string
    bullet: string
}

export const ToggleFieldSCSS: PropsToggleFieldSCSS = getModule(path$, "classes")
