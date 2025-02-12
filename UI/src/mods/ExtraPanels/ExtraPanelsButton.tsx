import { FloatingButton } from "cs2/ui";
import ExtraPanelsSCSS from "mods/ExtraPanels/ExtraPanels.module.scss"
import classNames from "classnames";
import { bindValue, trigger, useValue } from "cs2/api";
import { ExtraPanel } from "./ExtraPanel";


const ShowExtraPanelsButton$ = bindValue<boolean>("el", "ShowExtraPanelsButton");
const ExtraPanelsMenuOpened$ = bindValue<boolean>("el", 'ExtraPanelsMenuOpened');
const ExtraPanelsList$ = bindValue<ExtraPanel[]>("el", 'ExtraPanels');

export const ExtraPanelsButton = () => {

    let showButton: boolean = useValue(ShowExtraPanelsButton$);
    let opened: boolean = useValue(ExtraPanelsMenuOpened$);
    let ExtraPanelsList: ExtraPanel[] = useValue(ExtraPanelsList$)

    function openExtraPanels(newValue: boolean) { trigger("el", "ExtraPanelsMenuOpened", newValue) }

    return showButton ? <div className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorContainer)} >
        <FloatingButton
            className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorButton, "test")}
            selected={opened}
            src={"coui://extralib/Icons/ExtraTools/ButtonIcon.svg"}
            onClick={ () => openExtraPanels(!opened)}
        />
        <div className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorPanelsContainer, opened ? "selected" : "" ) } >
            {ExtraPanelsList && ExtraPanelsList.length && ExtraPanelsList.map (
                (extraPanel: ExtraPanel, index: number) => {
                    if (!opened && extraPanel.visible) { openExtraPanels(true); opened = true }
                    return <FloatingButton
                        selected={extraPanel.visible}
                        src={extraPanel.icon}
                        onClick={() => { trigger("el", "OpenExtraPanel", extraPanel.id) } } 
                    />
                })
            }
        </div>
    </div> : <></>
}


