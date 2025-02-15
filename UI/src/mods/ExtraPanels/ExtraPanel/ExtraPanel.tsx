import { Panel, Number2, DraggablePanelProps } from "cs2/ui"
import { CloseExtraPanel, ExtraPanelType, SetPanelPosition } from "../ExtraPanelType"
import { trigger } from "cs2/api"
import { ExtraPanelHeader } from "./Header/ExtraPanelHeader"
import React, { FC, MouseEventHandler, useCallback, useEffect, useRef, useState } from "react"
import { useMouseDragEvents, DragEventData } from "../../../../game-ui/common/hooks/use-mouse-drag-events"
import { PanelTitleBar } from "../../../../game-ui/common/panel/panel-title-bar"
import { CollapsiblePanel } from "../../../../game-ui/common/panel/collapsible-panel"
import { DragHandle } from "../../../../game-ui/common/input/drag-handle"
import classNames from "classnames"
import { useElementRect } from "../../../../game-ui/common/hooks/resize-events"

export interface propsExtraPanel {
    extraPanel: ExtraPanelType,
    children: any
}

export const ExtraPanel = ({ extraPanel, children }: propsExtraPanel) => {

    const [position, setPosition] = useState(extraPanel.panelLocation); 

    const handleDragMouseUp = () => { SetPanelPosition(extraPanel, position) }

    const onDrag = (e: number, t: number, n: number, s: number) => {
        e = Math.min(Math.max(e, 0), window.innerWidth);
        t = Math.min(Math.max(t, 0), window.innerHeight);
        setPosition({
            x: e - n,
            y: t - s,
        })
    }

    return <Panel
        header={DragHandle({ onDrag: onDrag, children: ExtraPanelHeader({ extraPanel, onMouseUp:handleDragMouseUp }) })}
        footer={"FOOTER"}
        className={classNames("draggable-panel") }
        showCloseHint={true }
        style={
            {
                width: "auto",
                height: "auto",
                left: `${position.x}px`,
                top: `${position.y}px`,
                visibility: extraPanel.visible ? "visible" : "hidden",
            }}
    >
        { extraPanel.expanded && children }
    </Panel>
}
