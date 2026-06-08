import { getModule } from "cs2/modding"
import { ReactNode } from "react"

const path$ = "game-ui/common/focus/focus-root.tsx"

const FocusRootModule = getModule(path$, "FocusRoot")

export interface FocusRootProps {
    children?: ReactNode,
}

export function FocusRoot(props: FocusRootProps): JSX.Element {
    return <FocusRootModule {...props} />
}
