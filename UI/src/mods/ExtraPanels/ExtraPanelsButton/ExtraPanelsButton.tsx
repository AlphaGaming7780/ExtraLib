/* ===============================
   ExtraPanelsButton.tsx
   =============================== */

import { Button, FloatingButton, Tooltip } from "cs2/ui";
import styles from "./ExtraPanelsButton.module.scss";
import classNames from "classnames";
import { bindValue, trigger, useValue } from "cs2/api";
import { CloseExtraPanel, ExtraPanelType, OpenExtraPanel } from "../ExtraPanelType";
import { ScreenOrigin } from "../../Utilities/ScreenOrigin";
import { useLocalization } from "cs2/l10n";

// Must match .panelButton size + margins and .menu padding in the scss.
const MENU_BUTTON_REM = 44;
const MENU_PADDING_REM = 4;
const MENU_MAX_COLUMNS = 3;

const ShowExtraPanelsButton$ = bindValue<boolean>("el", "ShowExtraPanelsButton");
const ExtraPanelsMenuOpened$ = bindValue<boolean>("el", "ExtraPanelsMenuOpened");
const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", "ExtraPanels");

// Same text as the panel header.
const PanelTitle = ({ panel }: { panel: ExtraPanelType }) => {
    const { translate } = useLocalization();
    return <>{translate(panel.title, panel.title)}</>;
};

export type ExtraPanelsPlacement = "bottom" | "top" | "left" | "right";
export type ExtraPanelsAlign = "start" | "center" | "end";

export type ExtraPanelsButtonProps = {
    placement?: ExtraPanelsPlacement;
    // Alignment of the menu along the button's side (start = left/top edge of the button).
    align?: ExtraPanelsAlign;
    className?: string;
    buttonClassName?: string;
    menuClassName?: string;
}

const togglePanel = (panel: ExtraPanelType) =>
    panel.visible ? CloseExtraPanel(panel) : OpenExtraPanel(panel);

export const ExtraPanelsButton = ({ placement = "bottom", align = "center", className, buttonClassName, menuClassName }: ExtraPanelsButtonProps): JSX.Element | null => {
    const showButton = useValue(ShowExtraPanelsButton$);
    const opened = useValue(ExtraPanelsMenuOpened$);
    const panels = useValue(ExtraPanelsList$);

    if (!showButton) return null;

    // DEBUG: duplicate the list to test the menu with many panels. Set back to 1 when done.
    const DEBUG_DUPLICATE = 1;
    const selectorPanels = Array.from({ length: DEBUG_DUPLICATE }, () => panels.filter(p => p.showInSelector)).flat();

    return (
        <div className={classNames(styles.root, className)}>
            <FloatingButton
                className={buttonClassName ?? classNames(styles.mainButton, opened && styles.opened)}
                selected={opened}
                src="coui://extralib/Icons/ExtraTools/ButtonIcon.svg"
                onClick={() => trigger("el", "ExtraPanelsMenuOpened", !opened)}
            />

            {/* The anchor matches the button's side, so the menu is placed with flex alignment only (no JS measuring). */}
            <div className={classNames(styles.menuAnchor, styles[placement], styles[align])}>
                {/* Gameface doesn't size a wrapping flex box to its content, so the width is set from the column count. */}
                <div
                    className={classNames(styles.menu, opened && styles.visible)}
                    style={{ width: `${Math.max(1, Math.min(selectorPanels.length, MENU_MAX_COLUMNS)) * MENU_BUTTON_REM + MENU_PADDING_REM * 2}rem` }}
                >
                    {selectorPanels.map((panel, i) => (
                        <Tooltip key={`${panel.__Type}-${i}`} tooltip={<PanelTitle panel={panel} />}>
                            <FloatingButton
                                className={classNames(menuClassName, styles.panelButton, panel.visible && styles.active)}
                                selected={panel.visible}
                                src={panel.icon}
                                onClick={() => togglePanel(panel)}
                            />
                        </Tooltip>
                    ))}
                </div>
            </div>
        </div>
    );
};

// The "Menu" slot isn't full screen: ScreenOrigin brings it back to the real top-left corner.
export const ExtraPanelsButtonMenu = () =>
    <ScreenOrigin>
        <ExtraPanelsButton className={styles.mainMenuPosition} placement="bottom" align="start" />
    </ScreenOrigin>;

export const ExtraPanelButtonsUniversalMod = () => {

    const showButton = useValue(ShowExtraPanelsButton$);
    const panels = useValue(ExtraPanelsList$);

    if (!showButton) return <></>;

    const selectorPanels = panels.filter(p => p.showInSelector);

    return <>
        {selectorPanels.map(panel => (
            <Tooltip key={panel.__Type} tooltip={<PanelTitle panel={panel} />}>
                <Button
                    variant="floating"
                    className={classNames(styles.panelButtonUM, panel.visible && styles.active)}
                    selected={panel.visible}
                    src={panel.icon}
                    onClick={() => togglePanel(panel)}
                />
            </Tooltip>
        ))}
    </>
}
