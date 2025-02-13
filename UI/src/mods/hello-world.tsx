import { ModuleRegistryExtend } from "cs2/modding";

export const HelloWorldComponent = () => {
    // This is a void component that does not output anynthing.
    // Cities: Skylines 2 UI is built with React and mods support outputting standard
    // React JSX elements!
    console.log("ExtraLib have been loaded.");

    return null;
}

export const extraPanelsComponentsExtended = (ComponentList: {[x: string]: any;}) : any => {

    console.log(ComponentList);

    return ComponentList["ExtraLib.Systems.UI.ExtraPanels.TestExtraPanel"] = <div>Let's GO</div>;

}
