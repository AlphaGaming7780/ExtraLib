import { getModule } from "cs2/modding"

const path$ = "game-ui/common/data-binding/input-bindings.ts"

export const ControlScheme: {
    gamepad: string,
    keyboardAndMouse: string,
} = getModule(path$, "ControlScheme")

export const useInputActionActive: (opts: { action: string, context?: any, controlScheme?: string }) => boolean = getModule(path$, "useInputActionActive")
