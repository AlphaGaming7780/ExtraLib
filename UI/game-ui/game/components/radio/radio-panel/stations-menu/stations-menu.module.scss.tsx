import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss"

export type PropsStationsMenuSCSS = {
    stationsMenu: string
    networks: string
    stations: string
    networkItem: string
    stationItem: string
    icon: string
    column: string
    title: string
    program: string
    progress: string
}

export const StationsMenuSCSS: PropsStationsMenuSCSS = getModule(path$, "classes")
