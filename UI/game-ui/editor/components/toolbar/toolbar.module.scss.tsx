import { getModule } from "cs2/modding"

const path$ = "game-ui/editor/components/toolbar/toolbar.module.scss"

export type PropsEditorToolbarSCSS = {
    editorToolbar: string
    button: string
}

export const EditorToolbarSCSS: PropsEditorToolbarSCSS = getModule(path$, "classes")
