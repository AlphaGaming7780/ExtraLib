import { getModule } from "cs2/modding"
import { Context } from "react"

const path$ = "game-ui/common/focus/controller/focus-controller.ts"

export const FocusContext: Context<any> = getModule(path$, "FocusContext")
export const FocusActivation: any = getModule(path$, "FocusActivation")
export const FocusLimits: any = getModule(path$, "FocusLimits")
export const disabledFocusController: any = getModule(path$, "disabledFocusController")
