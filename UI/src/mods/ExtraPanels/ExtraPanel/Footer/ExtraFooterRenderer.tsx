import { ExtraPanelType } from "../../ExtraPanelType";
import { extraPanelsFooterComponents } from "../../ExtraPanelEntryPoint";
import { TypedRenderer } from "../../../../../game-ui/common/typed-renderer/typed-renderer";

export interface propsExtraFooterRenderer {
    extraPanel: ExtraPanelType,
}

// Mirrors the main content's own TypedRenderer (ExtraPanel.tsx) but for the footer slot -
// deliberately its own component/file rather than inlined in ExtraPanel.tsx, same reasoning as
// Header/ExtraPanelHeader.tsx already being split out from it. Only ever mounted by ExtraPanel.tsx
// once it already confirmed extraPanel.__Type has an entry in extraPanelsFooterComponents (see
// there) - unlike the native title bar (ExtraPanelHeader), there's no generic chrome to fall back
// to here, this renders exactly whatever the registering mod supplied for this panel type.
export const ExtraFooterRenderer = ({ extraPanel }: propsExtraFooterRenderer): JSX.Element => {
    return <TypedRenderer components={extraPanelsFooterComponents} data={extraPanel} props={extraPanel} />
}
