import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/radio/radio-panel/station-detail/station-detail.module.scss"

export type PropsStationDetailSCSS = {
    stationDetail: string
    header: string
    stationName: string
    sectionTitle: string
    list: string
    programItem: string
    time: string
    column: string
    title: string
    description: string
    progress: string
}

export const StationDetailSCSS: PropsStationDetailSCSS = getModule(path$, "classes")
