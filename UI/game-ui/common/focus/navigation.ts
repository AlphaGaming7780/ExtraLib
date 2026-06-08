import { getModule } from "cs2/modding"

const path$ = "game-ui/common/focus/navigation.ts"

export const NavigationDirection: {
    Horizontal: string,
    Vertical: string,
    Both: string,
    None: string,
} = getModule(path$, "NavigationDirection")
