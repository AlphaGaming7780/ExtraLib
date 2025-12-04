import { ModRegistrar } from "cs2/modding";
import { extraPanelsComponentsExtended, HelloWorldComponent } from "mods/hello-world";
//import { ExtraDetailingDetails, ExtraAssetsMenu } from "../REMOVED/ExtraAssetsMenu";
import { AssetMultiCategory } from "./mods/AssetMultiCategory";
import { ExtraPanelsButton } from "./mods/ExtraPanels/ExtraPanelsButton/ExtraPanelsButton";
import { extraPanelsComponents, ExtraPanelsRoot } from "mods/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot"
import { ExtraPanelsButtonEditor } from "./mods/ExtraPanels/ExtraPanelsButton/ExtraPanelsButtonEditor";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx", 'AssetCategoryTabBar', AssetMultiCategory)
    moduleRegistry.append('GameTopLeft', ExtraPanelsButton);
    moduleRegistry.extend('game-ui/editor/components/toolbar/toolbar.tsx', 'Toolbar', ExtraPanelsButtonEditor);
    moduleRegistry.append('Editor', ExtraPanelsRoot);
    

    moduleRegistry.append('Game', ExtraPanelsRoot)
    moduleRegistry.append('Menu', HelloWorldComponent);

    moduleRegistry.add("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot", { "extraPanelsComponents": extraPanelsComponents })

    moduleRegistry.extend("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot", "extraPanelsComponents", extraPanelsComponentsExtended)

}

export default register;