import { getModule } from "cs2/modding"
import type { UniqueFocusKey } from "cs2/bindings"

const path$ = "game-ui/common/focus/focus-key.ts"

export const FOCUS_DISABLED$: UniqueFocusKey = getModule(path$, "FOCUS_DISABLED")
export const FOCUS_AUTO$: UniqueFocusKey = getModule(path$, "FOCUS_AUTO")
export const useUniqueFocusKey: (key: UniqueFocusKey | undefined, debugName: string) => UniqueFocusKey | null = getModule(path$, "useUniqueFocusKey")