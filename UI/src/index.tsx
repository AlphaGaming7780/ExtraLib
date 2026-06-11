import { ModRegistrar } from "cs2/modding";
import { extraPanelsComponentsExtended, HelloWorldComponent } from "mods/hello-world";
//import { ExtraDetailingDetails, ExtraAssetsMenu } from "../REMOVED/ExtraAssetsMenu";
import { AssetMultiCategory } from "./mods/AssetMultiCategory";
import { ExtraPanelButtonsUniversalMod, ExtraPanelsButton } from "./mods/ExtraPanels/ExtraPanelsButton/ExtraPanelsButton";
import { extraPanelsComponents, ExtraPanelsRoot } from "mods/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot"
import { ExtraPanelsButtonEditor } from "./mods/ExtraPanels/ExtraPanelsButton/ExtraPanelsButtonEditor";

const register: ModRegistrar = (moduleRegistry) => {

    console.log("ExtraLib UI loading...")
    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx", 'AssetCategoryTabBar', AssetMultiCategory)

    moduleRegistry.append('UniversalModMenu', ExtraPanelButtonsUniversalMod);
    // moduleRegistry.append('GameTopLeft', ExtraPanelsButton);
    moduleRegistry.extend('game-ui/editor/components/toolbar/toolbar.tsx', 'Toolbar', ExtraPanelsButtonEditor);
    moduleRegistry.append('Editor', ExtraPanelsRoot);
    
    moduleRegistry.append('Game', ExtraPanelsRoot)
    moduleRegistry.append('Menu', HelloWorldComponent);

    if (moduleRegistry.registry.has("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot")) {
        const existing = moduleRegistry.registry.get("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot");
        if (existing?.extraPanelsComponents) {
            Object.assign(extraPanelsComponents, existing.extraPanelsComponents);
        }
        moduleRegistry.override("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot", "extraPanelsComponents", extraPanelsComponents);
    } else {
        moduleRegistry.add("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot", { "extraPanelsComponents": extraPanelsComponents })
    }

    moduleRegistry.extend("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot", "extraPanelsComponents", extraPanelsComponentsExtended)

    console.log("ExtraLib UI loaded")
}

export default register;