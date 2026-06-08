import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input-events/action-hints/input-hint/floating-input-hint.tsx"

const FloatingInputHintModule = getModule(path$, "FloatingInputHint")

export interface FloatingInputHintProps {
    action?: string,
    active?: boolean,
    tooltip?: boolean,
    tooltipClassName?: string,
    className?: string,
    actionContext?: any,
    [key: string]: any,
}

export function FloatingInputHint(props: FloatingInputHintProps): JSX.Element {
    return <FloatingInputHintModule {...props} />
}
