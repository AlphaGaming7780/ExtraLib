import { getModule } from "cs2/modding"

const path$ = "game-ui/common/animations/transition-sounds.tsx"

export const useTransitionSounds: (sounds?: { enter?: string, exit?: string }) => void = getModule(path$, "useTransitionSounds")
export const panelTransitionSounds: { enter: string, exit: string } = getModule(path$, "panelTransitionSounds")
export const menuTransitionSounds: { enter: string, exit: string } = getModule(path$, "menuTransitionSounds")
