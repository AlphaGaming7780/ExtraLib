/* ===============================
   ExtraPanelsButton.tsx
   =============================== */

import { FloatingButton, Tooltip } from "cs2/ui";
import styles from "./ExtraPanelsButton.module.scss";
import classNames from "classnames";
import { bindValue, trigger, useValue } from "cs2/api";
import { CloseExtraPanel, ExtraPanelType, OpenExtraPanel } from "../ExtraPanelType";
import { useEffect, useRef } from "react";

const ShowExtraPanelsButton$ = bindValue<boolean>("el", "ShowExtraPanelsButton");
const ExtraPanelsMenuOpened$ = bindValue<boolean>("el", "ExtraPanelsMenuOpened");
const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", "ExtraPanels");

export type ExtraPanelsPlacement = "bottom" | "top" | "left" | "right";

export type ExtraPanelsButtonProps = {
    placement?: ExtraPanelsPlacement;
}

export const ExtraPanelsButton = ({ placement = "bottom" } : ExtraPanelsButtonProps): JSX.Element | null => {
    const showButton = useValue(ShowExtraPanelsButton$);
    const opened = useValue(ExtraPanelsMenuOpened$);
    const panels = useValue(ExtraPanelsList$);

    const buttonRef = useRef<HTMLDivElement>(null);
    const menuRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (!menuRef.current || !buttonRef.current) return;

        const menu = menuRef.current;
        const button = buttonRef.current;

        const menuStyle = getComputedStyle(menu);
        const paddingTop = parseFloat(menuStyle.paddingTop) || 0;
        const paddingBottom = parseFloat(menuStyle.paddingBottom) || 0;
        const paddingLeft = parseFloat(menuStyle.paddingLeft) || 0;
        const paddingRight = parseFloat(menuStyle.paddingRight) || 0;

        const totalHeight = menu.offsetHeight + paddingTop + paddingBottom;
        const totalWidth = menu.offsetWidth + paddingLeft + paddingRight;

        const btnRect = button.getBoundingClientRect();

        let translateX = 0;
        let translateY = 0;

        switch (placement) {
            case "top":
                translateY = -totalHeight;
                translateX = (btnRect.width / 2) - (totalWidth / 2);
                break;
            case "bottom":
                translateY = btnRect.height + paddingTop + paddingBottom;
                translateX = (btnRect.width / 2) - (totalWidth / 2);
                break;
            case "left":
                translateX = -totalWidth;
                translateY = (btnRect.height / 2) - (totalHeight / 2);
                break;
            case "right":
                translateX = btnRect.width;
                translateY = (btnRect.height / 2) - (totalHeight / 2);
                break;
        }

        menu.style.transform = `translate(${translateX}px, ${translateY}px) scale(${opened ? 1 : 0.6})`;
        menu.style.transition = "transform 0.25s ease, opacity 0.2s ease";
    }, [opened, placement]);


    //var pan2 = [...panels];
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //pan2.push(...panels);
    //panels = pan2;
    function toggleMenu() {
        trigger("el", "ExtraPanelsMenuOpened", !opened);
    }

    const selectorPanels = panels.filter(p => p.showInSelector);

    if (!showButton) return null;

    return (
        <div className={styles.root}>
            <div
                ref={buttonRef}
            >
                <FloatingButton
                    className={classNames(styles.mainButton, opened && styles.opened)}
                    selected={opened}
                    src="coui://extralib/Icons/ExtraTools/ButtonIcon.svg"
                    onClick={toggleMenu}
                />
            </div>


            <div
                ref={menuRef}
                className={classNames(
                    styles.menu,
                    styles[placement],
                    opened && styles.visible
                )}
            >
                {selectorPanels.map(panel => (
                    <Tooltip key={panel.__Type} tooltip={panel.__Type}>
                        <FloatingButton
                            className={classNames(
                                styles.panelButton,
                                panel.visible && styles.active
                            )}
                            selected={panel.visible}
                            src={panel.icon}
                            onClick={() =>
                                panel.visible
                                    ? CloseExtraPanel(panel)
                                    : OpenExtraPanel(panel)
                            }
                        />
                    </Tooltip>
                ))}
            </div>
        </div>
    );
};
