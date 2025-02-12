import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import { ExtraDetailingDetails, ExtraAssetsMenu } from "./mods/ExtraAssetsMenu";
import { AssetMultiCategory } from "./mods/AssetMultiCategory";
import { ExtraPanelsButton } from "./mods/ExtraPanels/ExtraPanelsButton";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx", 'AssetCategoryTabBar', ExtraAssetsMenu)
    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx", 'AssetCategoryTabBar', AssetMultiCategory)
    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-menu.tsx", "AssetMenu", ExtraDetailingDetails)
    moduleRegistry.append('GameTopLeft', ExtraPanelsButton);
    moduleRegistry.append('Menu', HelloWorldComponent);
}

export default register;