import { bindValue, useValue } from "cs2/api";
import { ExtraPanelType } from "../ExtraPanelType";
import { Panel } from "cs2/ui";
import { ExtraPanel } from "../ExtraPanel/ExtraPanel";
import { TypeFromMap } from "cs2/bindings";
import { TypedRenderer } from "../../../../game-ui/common/typed-renderer/typed-renderer";


const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');

export var extraPanelsComponents: {
    [x: string]: (extraPanel : ExtraPanelType) => any;
} = {};


export const ExtraPanelsRoot = () => {

    let ExtraPanelsList: ExtraPanelType[] = useValue(ExtraPanelsList$)

    return <div>
        {
            ExtraPanelsList && ExtraPanelsList.length > 0 && ExtraPanelsList.map((extraPanel: ExtraPanelType, index: number) => {

                if (!extraPanel.visible) return <></>                
                return <ExtraPanel extraPanel={extraPanel} >

                    < TypedRenderer components={extraPanelsComponents} data={extraPanel} props={ extraPanel } />

                </ExtraPanel>
            })
        }
    </div>
}