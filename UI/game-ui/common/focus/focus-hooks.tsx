import { getModule } from "cs2/modding"

const path$ = "game-ui/common/focus/focus-hooks.tsx"

export const useFocused: (context: any) => boolean = getModule(path$, "useFocused")
export const useFocusedRef: (context: any) => any = getModule(path$, "useFocusedRef")
export const useFocusCallback: (...args: any[]) => any = getModule(path$, "useFocusCallback")
