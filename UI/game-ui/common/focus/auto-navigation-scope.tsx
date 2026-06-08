import { getModule } from "cs2/modding"
import { ReactNode } from "react"

const path$ = "game-ui/common/focus/auto-navigation-scope.tsx"

const AutoNavigationScopeModule = getModule(path$, "AutoNavigationScope")

export interface AutoNavigationScopeProps {
    initialFocused?: any,
    allowLooping?: boolean,
    direction?: string,
    children?: ReactNode,
    [key: string]: any,
}

export function AutoNavigationScope(props: AutoNavigationScopeProps): JSX.Element {
    return <AutoNavigationScopeModule {...props} />
}
