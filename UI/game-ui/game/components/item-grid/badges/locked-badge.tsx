import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/item-grid/badges/locked-badge.tsx"

export type PropsLockedBadge = { style?: any, className?: string }

export function LockedBadge(propsLockedBadge: PropsLockedBadge): JSX.Element {
	return getModule(path$, "LockedBadge")(propsLockedBadge)
}