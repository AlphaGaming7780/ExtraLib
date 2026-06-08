import { getModule } from "cs2/modding"
import { forwardRef, ReactNode } from "react"

const path$ = "game-ui/common/focus/focus-div.tsx"

const ActiveFocusDivModule = getModule(path$, "ActiveFocusDiv")
const PassiveFocusDivModule = getModule(path$, "PassiveFocusDiv")

export interface FocusDivProps {
    focusKey?: any,
    className?: string,
    children?: ReactNode,
    [key: string]: any,
}

export const ActiveFocusDiv = forwardRef<HTMLDivElement, FocusDivProps>((props, ref) => {
    return <ActiveFocusDivModule ref={ref} {...props} />
})

export const PassiveFocusDiv = forwardRef<HTMLDivElement, FocusDivProps>((props, ref) => {
    return <PassiveFocusDivModule ref={ref} {...props} />
})
