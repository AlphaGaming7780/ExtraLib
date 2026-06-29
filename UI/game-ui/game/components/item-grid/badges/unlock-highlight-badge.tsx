import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/item-grid/badges/unlock-highlight-badge.tsx"

export type PropsUnlockHighlightBadge = { className: string }

export function UnlockHighlightBadge(propsUnlockHighlightBadge: PropsUnlockHighlightBadge): JSX.Element {
	return getModule(path$, "UnlockHighlightBadge")(propsUnlockHighlightBadge)
}