import { EditorToolbarSCSS } from "../../../../game-ui/editor/components/toolbar/toolbar.module.scss";
import { EditorToolButtonSCSS } from "../../../../game-ui/editor/themes/editor-tool-button.module.scss";
import { ExtraPanelsButton } from "./ExtraPanelsButton";
import styles from "./ExtraPanelsButton.module.scss";
import classNames from "classnames";

// Vanilla Toolbar only takes `className` (its absolute bottom layout). The wrapper takes it over,
// and the vanilla toolbar is rendered as-is inside, so its own centering/padding is unchanged.
export const ExtraPanelsButtonEditor = (Component: any) => ({ className, ...props }: any) =>
    <div className={classNames(className, styles.editorToolbarWrapper)}>
        <div className={styles.editorButtonAnchor}>
            <ExtraPanelsButton
                className={styles.editorButton}
                placement="top"
                buttonClassName={classNames(EditorToolButtonSCSS.button, EditorToolbarSCSS.button)}
            />
        </div>
        <Component {...props} />
    </div>;
