import { bindValue, useValue } from "cs2/api";
import { TypedRenderer } from "../../../../game-ui/common/typed-renderer/typed-renderer";
import { ExtraPanel } from "../ExtraPanel/ExtraPanel";
import { ExtraPanelType } from "../ExtraPanelType";

const path$ = "game-ui/editor/data-binding/editor-tool-bindings.ts"

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