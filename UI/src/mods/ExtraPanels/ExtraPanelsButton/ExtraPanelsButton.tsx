import { FloatingButton, Panel, Tooltip } from "cs2/ui";
import ExtraPanelsSCSS from "./ExtraPanelsButton.module.scss"
import classNames from "classnames";
import { bindValue, trigger, useValue } from "cs2/api";
import { ExtraPanelType } from "../ExtraPanelType";


const ShowExtraPanelsButton$ = bindValue<boolean>("el", "ShowExtraPanelsButton");
const ExtraPanelsMenuOpened$ = bindValue<boolean>("el", 'ExtraPanelsMenuOpened');
const ShouldOpenExtraPanels$ = bindValue<boolean>("el", "ShouldOpenExtraPanels");
const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');

export const ExtraPanelsButton = () => {

    let showButton: boolean = useValue(ShowExtraPanelsButton$);
    let opened: boolean = useValue(ExtraPanelsMenuOpened$);
    let ShouldOpenExtraPanels: boolean = useValue(ShouldOpenExtraPanels$);
    let ExtraPanelsList: ExtraPanelType[] = useValue(ExtraPanelsList$)

    let IsOpened = opened || ShouldOpenExtraPanels
    function openExtraPanels(newValue: boolean) { if (!ShouldOpenExtraPanels) trigger("el", "ExtraPanelsMenuOpened", newValue) }

    return showButton ? <div className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorContainer)} >
        <Tooltip tooltip={ "ExtraPanels.Button" } >
            <FloatingButton
                className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorButton, IsOpened ? ExtraPanelsSCSS.selected : "")}
                selected={IsOpened}
                src={"coui://extralib/Icons/ExtraTools/ButtonIcon.svg"}
                onClick={() => openExtraPanels(!opened)}
            />
        </Tooltip>
        <div className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorPanelsContainer, IsOpened ? ExtraPanelsSCSS.selected : "" ) } >
            {ExtraPanelsList && ExtraPanelsList.length && ExtraPanelsList.map (
                (extraPanel: ExtraPanelType, index: number) => {
                    //if (!opened && extraPanel.visible) { openExtraPanels(true); opened = true }
                    if (!extraPanel.showInSelector) return <></>;
                    return <Tooltip tooltip={`ExtraPanel_Button[${extraPanel.__Type}]`} >
                        <FloatingButton
                            className={classNames(ExtraPanelsSCSS.ExtraPanelButton, extraPanel.visible ? ExtraPanelsSCSS.selected : "")}
                            selected={extraPanel.visible}
                            src={extraPanel.icon}
                            onClick={() => {
                                if (extraPanel.visible) {
                                    trigger("el", "CloseExtraPanel", extraPanel.__Type)
                                } else {
                                    trigger("el", "OpenExtraPanel", extraPanel.__Type)
                                }
                            }}
                        ></ FloatingButton>
                    </Tooltip>
                })
            }
        </div>

    </div> : <></>
}


