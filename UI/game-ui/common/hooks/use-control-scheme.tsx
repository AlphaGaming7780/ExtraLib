import { getModule } from "cs2/modding"

const path$ = "game-ui/common/hooks/use-control-scheme.tsx"

export const useGamepadActive: () => boolean = getModule(path$, "useGamepadActive")
export const useKeyboardAndMouseActive: () => boolean = getModule(path$, "useKeyboardAndMouseActive")
