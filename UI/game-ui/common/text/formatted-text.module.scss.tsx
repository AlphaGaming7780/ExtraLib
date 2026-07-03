import { getModule } from "cs2/modding"

const path$ = "game-ui/common/text/formatted-text.module.scss"

export type PropsFormattedTextSCSS = {
    h1: string
    h2: string
    h3: string
    h4: string
    h5: string
    h6: string
    link: string
    p: string
    listItem: string
}

export const FormattedTextSCSS: PropsFormattedTextSCSS = getModule(path$, "classes")
