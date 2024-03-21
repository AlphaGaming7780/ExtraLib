import { ExtraLibUI } from "../../../../../../ExtraLibUI"


const path = "game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.tsx"

export type PropsStationsMenu = {
    className? : string
}

export function StationsMenu(propsStationsMenu: PropsStationsMenu) : JSX.Element
{
    return ExtraLibUI.instance.registryData.registry.get(path)?.StationsMenu(propsStationsMenu)
}
