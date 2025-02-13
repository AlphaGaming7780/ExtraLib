import { ModRegistrar } from "cs2/modding";
import { extraPanelsComponentsExtended, HelloWorldComponent } from "mods/hello-world";
import { ExtraDetailingDetails, ExtraAssetsMenu } from "./mods/ExtraAssetsMenu";
import { AssetMultiCategory } from "./mods/AssetMultiCategory";
import { ExtraPanelsButton } from "./mods/ExtraPanels/ExtraPanelsButton/ExtraPanelsButton";
import { extraPanelsComponents, ExtraPanelsRoot } from "mods/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot"

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx", 'AssetCategoryTabBar', ExtraAssetsMenu)
    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx", 'AssetCategoryTabBar', AssetMultiCategory)
    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-menu.tsx", "AssetMenu", ExtraDetailingDetails)
    moduleRegistry.append('GameTopLeft', ExtraPanelsButton);
    moduleRegistry.append('Game', ExtraPanelsRoot)
    moduleRegistry.append('Menu', HelloWorldComponent);

    moduleRegistry.add("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot", { "extraPanelsComponents": extraPanelsComponents })

    moduleRegistry.extend("ExtraLib/ExtraPanels/ExtraPanelsRoot/ExtraPanelsRoot", "extraPanelsComponents", extraPanelsComponentsExtended)


}

export default register;