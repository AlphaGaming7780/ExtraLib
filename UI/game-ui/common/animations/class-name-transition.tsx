import { getModule } from "cs2/modding"
import { ReactNode } from "react"

const path$ = "game-ui/common/animations/class-name-transition.tsx"

const ClassNameTransitionModule = getModule(path$, "ClassNameTransition")

export const emptyStyles: any = getModule(path$, "emptyStyles")

export interface ClassNameTransitionProps {
    styles?: any,
    children?: ReactNode,
    [key: string]: any,
}

export function ClassNameTransition(props: ClassNameTransitionProps): JSX.Element {
    return <ClassNameTransitionModule {...props} />
}
