import { EditorToolbarSCSS } from "../../../../game-ui/editor/components/toolbar/toolbar.module.scss";
import { EditorToolButtonSCSS } from "../../../../game-ui/editor/themes/editor-tool-button.module.scss";
import { ExtraPanelsButton } from "./ExtraPanelsButton";
import classNames from "classnames";

export const ExtraPanelsButtonEditor = (Component: any) => {
    return (props: any) => <div {...props} style={{ display: "flex", flexDirection: "row", alignItems: "center", justifyContent: "flex-end", paddingRight:"810rem", pointerEvents: "auto" }}>
        <ExtraPanelsButton placement="top" buttonClassName={ classNames(EditorToolButtonSCSS.button, EditorToolbarSCSS.button) } />
        <Component />
    </div>;
}