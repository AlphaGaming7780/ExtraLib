import { Panel } from "cs2/ui"
import { ExtraPanelType, SetFullScreenExtraPanel, SetPanelPosition } from "../ExtraPanelType"
import { ExtraPanelHeader } from "./Header/ExtraPanelHeader"
import { useState } from "react"
import { DragHandle } from "../../../../game-ui/common/input/drag-handle"
import classNames from "classnames"
import ExtraPanelSCSS from "./ExtraPanel.module.scss"

export interface propsExtraPanel {
    extraPanel: ExtraPanelType,
    children: any
}

export const ExtraPanel = ({ extraPanel, children }: propsExtraPanel) => {

    const [position, setPosition] = useState(extraPanel.panelLocation); 
    //const [ fullScreen , setFullScreen ] = useState(extraPanel.isFullScreen)

    const handleDragMouseUp = () => {
        //if (fullScreen) {
        //    SetFullScreenExtraPanel(extraPanel, true)
        //} else {
        //    SetPanelPosition(extraPanel, position);
        //    extraPanel.isFullScreen && SetFullScreenExtraPanel(extraPanel, false)
        //}
        SetPanelPosition(extraPanel, position);
        //extraPanel.isFullScreen && SetFullScreenExtraPanel(extraPanel, false)
    }

    const onDrag = (e: number, t: number, n: number, s: number) => {
        e = Math.min(Math.max(e, 0), window.innerWidth);
        t = Math.min(Math.max(t, 0), window.innerHeight);

        let x = e - n;
        let y = t - s;

        setPosition({ x: x, y: y })
        extraPanel.isFullScreen && SetFullScreenExtraPanel(extraPanel, false)
    }

    return <Panel
        header={DragHandle({ onDrag: onDrag, children: ExtraPanelHeader({ extraPanel, onMouseUp:handleDragMouseUp }) })}
        footer={"FOOTER"}
        className={classNames("draggable-panel", ExtraPanelSCSS.ExtraPanel, extraPanel.isFullScreen && ExtraPanelSCSS.FullScreen, !extraPanel.isExpanded && ExtraPanelSCSS.Collapsed ) }
        //showCloseHint={true }
        style={
            {
                left: `${extraPanel.isFullScreen ? 0 : position.x}px`,
                top: `${extraPanel.isFullScreen ? 0 : position.y }px`,
                visibility: extraPanel.visible ? "visible" : "hidden",
            }}
    >
        { extraPanel.isExpanded && children }
    </Panel>
}
