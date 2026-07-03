import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/dialog/dialog.module.scss"

export type PropsDialogSCSS = {
    dialog: string
    wide: string
    row: string
    "pdx-title": string
    pdxTitle: string
    "pdx-button-row": string
    pdxButtonRow: string
    message: string
    "error-message": string
    errorMessage: string
    paragraphs: string
    buttons: string
    button: string
    "error-button": string
    errorButton: string
    footer: string
    "footer-label": string
    footerLabel: string
    "buttons-vertical": string
    buttonsVertical: string
    "button-ok": string
    buttonOk: string
    "error-dialog": string
    errorDialog: string
    "icon-layout": string
    iconLayout: string
    icon: string
    "main-column": string
    mainColumn: string
    "error-details": string
    errorDetails: string
    content: string
    "copy-button": string
    copyButton: string
    "scroll-hint": string
    scrollHint: string
    "scroll-hint-label": string
    scrollHintLabel: string
    hint: string
    "error-count": string
    errorCount: string
}

export const DialogSCSS: PropsDialogSCSS = getModule(path$, "classes")
