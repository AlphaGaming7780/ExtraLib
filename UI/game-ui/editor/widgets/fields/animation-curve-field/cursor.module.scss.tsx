import { getModule } from "cs2/modding"

const path$ = "game-ui/editor/widgets/fields/animation-curve-field/cursor.module.scss"

export type PropsCursorSCSS = {
    x: string
    canX: string
    y: string
    canY: string
    xy: string
    canXy: string
}

export const CursorSCSS: PropsCursorSCSS = getModule(path$, "classes")
