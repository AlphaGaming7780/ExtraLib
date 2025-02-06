import { getModule } from "cs2/modding"
import { Theme } from "cs2/bindings"
import { ComponentProps } from "react"

const path$ = "game-ui/common/input/button/icon-button.tsx"

//export interface PropsIconButton { src?: string, tinted?: boolean, theme?: Theme, className?: string, children?: any }

export function IconButton({ ...a }): JSX.Element {

	return getModule(path$, "IconButton").render(a)

}