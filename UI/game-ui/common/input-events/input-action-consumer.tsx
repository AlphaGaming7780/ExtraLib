import { getModule } from "cs2/modding"
import { ReactNode } from "react"

const path$ = "game-ui/common/input-events/input-action-consumer.tsx"

const InputActionConsumerModule = getModule(path$, "InputActionConsumer")

export interface InputActionConsumerProps {
    onAction?: () => void,
    action?: string,
    actions?: Record<string, (() => void) | null>,
    actionContext?: any,
    disabled?: boolean,
    children?: ReactNode,
    [key: string]: any,
}

export function InputActionConsumer(props: InputActionConsumerProps): JSX.Element {
    return <InputActionConsumerModule {...props} />
}
