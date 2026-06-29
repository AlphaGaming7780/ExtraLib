import { FocusKey } from "cs2/bindings"
import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/tool-options/tool-button/tool-button.tsx"

// =========================
// ToolButton
// =========================

export type PropsToolButton = {
	focusKey?: FocusKey
	src?: string
	selected?: boolean
	multiSelect?: boolean
	disabled?: boolean
	tooltip?: any
	selectSound?: string
	uiTag?: string
	className?: string
	children?: JSX.Element
	onSelect?: (value: Event) => void
}

// =========================
// ValueToolButton
// =========================

export type PropsValueToolButton<T = any> = {
	focusKey?: FocusKey
	value: T

	src?: string
	selected?: boolean
	disabled?: boolean
	highlight?: boolean
	multiSelect?: boolean

	tooltip?: any
	uiTag?: string
	shortcut?: string
	className?: string

	children?: JSX.Element

	onSelect?: (value: T) => void
	onClick?: (value: T) => void
}

// =========================
// Modules
// =========================

const ToolButtonModule = getModule(path$, "ToolButton")
const ValueToolButtonModule = getModule(path$, "ValueToolButton")

// =========================
// Wrappers
// =========================

export function ToolButton(props: PropsToolButton): JSX.Element {
	return <ToolButtonModule {...props} />
}

export function ValueToolButton<T = any>(props: PropsValueToolButton<T>): JSX.Element {
	return <ValueToolButtonModule {...props} />
}