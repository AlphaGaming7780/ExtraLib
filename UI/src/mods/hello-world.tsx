import { ExtraPanelType } from "./ExtraPanels/ExtraPanelType";
import { bindValue, useValue } from "cs2/api";
import { NodeGraphEditor } from "./NodeGraph/NodeGraphEditor";


export const HelloWorldComponent = () => {
    // console.log("ExtraLib UI is loading...");
    return null
}

const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');

export const extraPanelsComponentsExtended = (ComponentList: { [x: string]: any; }): any => {
    ComponentList["ExtraLib.Systems.UI.ExtraPanels.TestExtraPanel"] = (extraPanel: ExtraPanelType) => {
        return <div style={{width: extraPanel.isFullScreen ? "100%" : "1000rem" }}>
            <NodeGraphEditor />
        </div>
    }
    return ComponentList;
}
    