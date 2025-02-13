import { bindValue, useValue } from "cs2/api";
import { ExtraPanelType } from "../ExtraPanelType";
import { Panel } from "cs2/ui";
import { ExtraPanel } from "../ExtraPanel/ExtraPanel";
import { TypeFromMap } from "cs2/bindings";


const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');

export var extraPanelsComponents: {
    [x: string]: any;
} = {};


//export type ExtraPanelsContent = TypeFromMap<test>;

export const ExtraPanelsRoot = () => {

    let ExtraPanelsList: ExtraPanelType[] = useValue(ExtraPanelsList$)

    return <div>
        {
            ExtraPanelsList && ExtraPanelsList.length > 0 && ExtraPanelsList.map((extraPanel: ExtraPanelType, index: number) => {
                if (!extraPanel.visible) return <></>
                return <ExtraPanel extraPanel={extraPanel} >
                    {extraPanelsComponents[extraPanel.__Type] != undefined ?
                        extraPanelsComponents[extraPanel.__Type] :
                        <div style={{ backgroundColor:"red", color:"yellow"}} >
                            Type : {extraPanel.__Type}
                        </div>
                    }
                </ExtraPanel>
            })
        }
    </div>
}