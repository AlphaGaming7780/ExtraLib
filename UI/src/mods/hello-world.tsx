import { ExtraPanelType } from "./ExtraPanels/ExtraPanelType";
import { bindValue, useValue } from "cs2/api";
import { NodeGraphEditor } from "./NodeGraph/NodeGraphEditor";


export const HelloWorldComponent = () => {
    // This is a void component that does not output anynthing.
    // Cities: Skylines 2 UI is built with React and mods support outputting standard
    // React JSX elements!
    console.log("ExtraLib have been loaded.");

    return null

}

const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');

export const extraPanelsComponentsExtended = (ComponentList: { [x: string]: any; }): any => {

    return ComponentList["ExtraLib.Systems.UI.ExtraPanels.TestExtraPanel"] = (extraPanel: ExtraPanelType) => {

        let ExtraPanelsList: ExtraPanelType[] = useValue(ExtraPanelsList$)
        ExtraPanelsList.forEach((v) => console.log(v.__Type) )

        return <div style={{width: extraPanel.isFullScreen ? "100%" : "1000rem" } }>
            <NodeGraphEditor />
        </div>
        
    }
}
    