import { EditorToolbarSCSS } from "../../../../game-ui/editor/components/toolbar/toolbar.module.scss";
import { EditorToolButtonSCSS } from "../../../../game-ui/editor/themes/editor-tool-button.module.scss";
import { ExtraPanelsButton } from "./ExtraPanelsButton";
import classNames from "classnames";

export const ExtraPanelsButtonEditor = (Component: any) => {

    var extraPanelsButton = <ExtraPanelsButton placement="top" buttonClassName={classNames(EditorToolButtonSCSS.button, EditorToolbarSCSS.button)} />;

    return (props: any) =>
        <div {...props} style={{ display: "flex", flexDirection: "row", alignItems: "center", justifyContent: "flex-end", paddingRight: "810rem", pointerEvents: "none" }}>
            {extraPanelsButton}
            <Component />
        </div>;
}