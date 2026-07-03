import { getModule } from "cs2/modding"

const path$ = "game-ui/editor/widgets/item/editor-item.module.scss"

export type PropsEditorItemSCSS = {
    pickerMinHeight: string
    editorItemBase: string
    centered: string
    editorItem: string
    row: string
    label: string
    control: string
    children: string
    toggle: string
    input: string
    pickerToggle: string
    dropdownToggle: string
    directoryButton: string
    directoryIcon: string
    vectorLabel: string
    vectorInput: string
    sliderContainer: string
    slider: string
    sliderInput: string
    picker: string
    pickerPopup: string
    swatch: string
    alpha: string
    groupChildren: string
    expandableHeader: string
    headerLabel: string
    headerSummary: string
    expandableChildren: string
    colorPickerContainer: string
    detailsBlock: string
    errorLabel: string
    errorBorder: string
    labelRight: string
}

export const EditorItemSCSS: PropsEditorItemSCSS = getModule(path$, "classes")
