import { getModule } from "cs2/modding"
import { ReactNode } from "react"

const path$ = "game-ui/common/focus/focus-key-override.tsx"

const FocusKeyOverrideModule = getModule(path$, "FocusKeyOverride")

export interface FocusKeyOverrideProps {
    focusKey?: any,
    children?: ReactNode,
}

export function FocusKeyOverride(props: FocusKeyOverrideProps): JSX.Element {
    return <FocusKeyOverrideModule {...props} />
}
