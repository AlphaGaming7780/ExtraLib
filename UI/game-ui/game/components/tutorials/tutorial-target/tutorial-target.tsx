import React, { ReactElement } from "react"
import { getModule } from "cs2/modding"

const PATH = "game-ui/game/components/tutorials/tutorial-target/tutorial-target.tsx"

export type TutorialTargetProps = {
    uiTag: string
    active?: boolean
    disableBlinking?: boolean
    editor?: boolean
    children: ReactElement
}

const { TutorialTarget: InternalTutorialTarget } = getModule(
    PATH,
    "TutorialTarget"
) as {
    TutorialTarget: React.ForwardRefExoticComponent<
        TutorialTargetProps & React.RefAttributes<any>
    >
    }

export function TutorialTarget(props: TutorialTargetProps): ReactElement {
    var c = <InternalTutorialTarget {...props} />
    console.log(c)
    return c
}
