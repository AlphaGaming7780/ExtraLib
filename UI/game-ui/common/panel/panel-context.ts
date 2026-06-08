import { getModule } from "cs2/modding"
import { Context } from "react"

const path$ = "game-ui/common/panel/panel-context.ts"

export const PanelContext: Context<any> = getModule(path$, "PanelContext")
export const CollapsiblePanelContext: Context<any> = getModule(path$, "CollapsiblePanelContext")
