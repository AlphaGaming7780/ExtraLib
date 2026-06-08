import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/panel-theme.ts"

export const usePanelTheme: (theme: any) => any = getModule(path$, "usePanelTheme")
