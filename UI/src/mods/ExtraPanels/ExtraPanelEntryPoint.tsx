import { ExtraPanelType } from "./ExtraPanelType"

// Single shared file for the per-panel-type component maps a consuming mod registers into,
// keyed by ExtraPanelType.__Type (registry.extend("ExtraLib/ExtraPanels/ExtraPanelsRoot/
// ExtraPanelsRoot", "<mapName>", ...), see index.tsx) - one file/module path to import from
// instead of one per slot. An unregistered type simply has no entry, which both ExtraPanel.tsx
// (content) and ExtraFooterRenderer.tsx (footer) already treat the same way: render nothing for
// that slot rather than an empty placeholder.
export type ExtraPanelComponentMap = { [x: string]: (extraPanel: ExtraPanelType) => any };

// Panel content - the Panel's `children`, rendered via TypedRenderer inside ExtraPanel.tsx.
export var extraPanelsComponents: ExtraPanelComponentMap = {};

// Panel footer - the Panel's `footer` slot (see Panel.tsx/ExtraPanel.tsx), same convention as
// content but opt-in per panel type: ExtraPanel.tsx only passes `footer` to <Panel> at all when a
// type has an entry here, so an unregistered type gets no footer row rendered, not an empty one.
export var extraPanelsFooterComponents: ExtraPanelComponentMap = {};
