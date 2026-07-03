import { getModule } from "cs2/modding"

const path$ = "game-ui/common/text/formatted-paragraphs.module.scss"

export type PropsFormattedParagraphsSCSS = {
    paragraphs: string
}

export const FormattedParagraphsSCSS: PropsFormattedParagraphsSCSS = getModule(path$, "classes")
