import { trigger } from "cs2/api";
import { Typed } from "cs2/bindings";
import { Number2 } from "cs2/ui";

export interface ExtraPanelType extends Typed < "" > {
    //id: string;
    icon: string;
    visible: boolean;
    isExpanded: boolean;
    canFullScreen: boolean;
    isFullScreen: boolean;
    showInSelector: boolean;
    panelLocation: Number2;
    panelSize: Number2;
}

export function SetPanelPosition(extraPanel: ExtraPanelType, newPos: Number2) { trigger("el", "LocationChanged", extraPanel.__Type, newPos)}

export const OpenExtraPanel = (extraPanel: ExtraPanelType) => { trigger("el", "OpenExtraPanel", extraPanel.__Type) }
export const CloseExtraPanel = (extraPanel: ExtraPanelType) => { trigger("el", "CloseExtraPanel", extraPanel.__Type) }

export const CollapseExtraPanel = (extraPanel: ExtraPanelType) => { trigger("el", "CollapseExtraPanel", extraPanel.__Type) }
export const ExpandExtraPanel = (extraPanel: ExtraPanelType) => { trigger("el", "ExpandExtraPanel", extraPanel.__Type) }

export const SetFullScreenExtraPanel = (extraPanel: ExtraPanelType, fullScreen: boolean) => { trigger("el", "SetFullScreenExtraPanel", extraPanel.__Type, fullScreen) }