import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/button/themes/dialog-button.module.scss"

export type PropsDialogButtonSCSS = {
    button: string
    negative: string
}

export const DialogButtonSCSS: PropsDialogButtonSCSS = getModule(path$, "classes")
