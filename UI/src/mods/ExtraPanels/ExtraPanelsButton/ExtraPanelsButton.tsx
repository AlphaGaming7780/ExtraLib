import { FloatingButton, Panel, Tooltip } from "cs2/ui";
import ExtraPanelsSCSS from "./ExtraPanelsButton.module.scss"
import classNames from "classnames";
import { bindValue, trigger, useValue } from "cs2/api";
import { ExtraPanelType } from "../ExtraPanelType";
import { ExtraPanel } from "../ExtraPanel/ExtraPanel";


const ShowExtraPanelsButton$ = bindValue<boolean>("el", "ShowExtraPanelsButton");
const ExtraPanelsMenuOpened$ = bindValue<boolean>("el", 'ExtraPanelsMenuOpened');
const ShouldOpenExtraPanels$ = bindValue<boolean>("el", "ShouldOpenExtraPanels");
const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');

export let doOnce = true

export const ExtraPanelsButton = () => {

    let showButton: boolean = useValue(ShowExtraPanelsButton$);
    let opened: boolean = useValue(ExtraPanelsMenuOpened$);
    let ShouldOpenExtraPanels: boolean = useValue(ShouldOpenExtraPanels$);
    let ExtraPanelsList: ExtraPanelType[] = useValue(ExtraPanelsList$)

    let IsOpened = opened // ShouldOpenExtraPanels
    function openExtraPanels(newValue: boolean) { trigger("el", "ExtraPanelsMenuOpened", newValue) } //if (!ShouldOpenExtraPanels)


    //let ExtraPanelsList: ExtraPanelType[] = []
    //ExtraPanelsList.push(...ExtraPanelsList2);
    //ExtraPanelsList.push(...ExtraPanelsList2);
    //ExtraPanelsList.push(...ExtraPanelsList2);
    //ExtraPanelsList.push(...ExtraPanelsList2);
    //ExtraPanelsList.push(...ExtraPanelsList2);
    //ExtraPanelsList.push(...ExtraPanelsList2);
    //ExtraPanelsList.push(...ExtraPanelsList2);

    function getShowInSelectorPanelList(): ExtraPanelType[] {

        let outVal: ExtraPanelType[] = []

        ExtraPanelsList.map((extraPanel: ExtraPanelType, index: number): void => {
            if (extraPanel.showInSelector) {
                outVal.push(extraPanel)
            }
        })

        return outVal;
    }

    var ShowInSelectorPanel = getShowInSelectorPanelList();

    function subButtonPanelsWidth(): number {

        if (ShowInSelectorPanel.length == 1) return 46;
        else if (ShowInSelectorPanel.length == 2) return 89;
        else if (ShowInSelectorPanel.length == 3) return 132;
        else if (ShowInSelectorPanel.length == 4) return 175;
        else return 218;
    }


    return showButton ? <div className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorContainer, IsOpened ? ExtraPanelsSCSS.selected : "")} >
        <Tooltip tooltip={ "ExtraPanels.Button" } >
            <FloatingButton
                className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorButton, IsOpened ? ExtraPanelsSCSS.selected : "")}
                selected={IsOpened}
                src={"coui://extralib/Icons/ExtraTools/ButtonIcon.svg"}
                onClick={() => openExtraPanels(!opened)}
            />
        </Tooltip>
        <div className={classNames(ExtraPanelsSCSS.ExtraPanelsSelectorPanelsContainer, IsOpened ? ExtraPanelsSCSS.selected : "")} style={{ "--subButtonPanelsWidth": `${subButtonPanelsWidth()}rem`, "--subButtonPanelsTransformX": 46/subButtonPanelsWidth() } as React.CSSProperties  } >
            {ShowInSelectorPanel && ShowInSelectorPanel.length && ShowInSelectorPanel.map (
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


