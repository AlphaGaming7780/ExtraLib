import { getModule } from "cs2/modding"

const path$ = "game-ui/editor/themes/editor-tool-button.module.scss"

export type PropsEditorToolButtonSCSS = {
    button: string
    icon: string
    hint: string
}

export const EditorToolButtonSCSS: PropsEditorToolButtonSCSS = getModule(path$, "classes")
