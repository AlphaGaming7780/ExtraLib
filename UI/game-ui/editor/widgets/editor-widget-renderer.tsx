import { getModule } from "cs2/modding"

const path$ = "game-ui/editor/widgets/editor-widget-renderer.tsx"

export type PropsEditorWidgetListRenderer = {
    parent: { group: string, path: any[] },
    data: any[],
    [key: string]: any;
}

const EditorWidgetListRendererModule = getModule(path$, "EditorWidgetListRenderer");

export function EditorWidgetListRenderer(props: PropsEditorWidgetListRenderer): JSX.Element {
    return <EditorWidgetListRendererModule {...props} />
}
