import { bindValue, useValue } from "cs2/api";
import { ExtraPanelType } from "../ExtraPanelType";
import { EditorWidgetListRenderer } from "../../../../game-ui/editor/widgets/editor-widget-renderer";

// Must match FileBrowserPanel.kGroup and the C# type full name.
const group$ = "elFileBrowser";
export const FileBrowserPanelType = "ExtraLib.Systems.UI.ExtraPanels.FileBrowserPanel";

const children$ = bindValue<any[]>(group$, "children", []);
const parent = { group: group$, path: [] };

// Renders the game DirectoryBrowserPanel widgets sent by the C# FileBrowserPanel.
const FileBrowserContent = () => {
    const children = useValue(children$);
    if (!children || children.length === 0) return null;
    return <EditorWidgetListRenderer parent={parent} data={children} />
}

export const FileBrowserPanel = (extraPanel: ExtraPanelType) => <FileBrowserContent />
