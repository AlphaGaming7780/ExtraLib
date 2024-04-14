import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import { ExtraDetailingDetails, ExtraAssetsMenu } from "./mods/ExtraAssetsMenu";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx", 'AssetCategoryTabBar', ExtraAssetsMenu)
    moduleRegistry.extend("game-ui/game/components/asset-menu/asset-menu.tsx", "AssetMenu", ExtraDetailingDetails)

    moduleRegistry.append('Menu', HelloWorldComponent);
}

export default register;