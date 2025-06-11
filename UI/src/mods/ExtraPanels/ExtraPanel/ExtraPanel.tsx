import { Panel } from "cs2/ui"
import { ExtraPanelType, SetFullScreenExtraPanel, SetPanelPosition } from "../ExtraPanelType"
import { ExtraPanelHeader } from "./Header/ExtraPanelHeader"
import { useState } from "react"
import { DragHandle } from "../../../../game-ui/common/input/drag-handle"
import classNames from "classnames"
import ExtraPanelSCSS from "./ExtraPanel.module.scss"
import { BetterDragEventData, BetterDragHandle, BetterDragHandleProps } from "../../Utilities/BetterDragHandle"
import { Number2 } from "cs2/bindings"

export interface propsExtraPanel {
    extraPanel: ExtraPanelType,
    children: any
}

export const ExtraPanel = ({ extraPanel, children }: propsExtraPanel) => {

    //const [position, setPosition] = useState(extraPanel.panelLocation); 
    const [translate, setTranslate] = useState({ x: 0, y: 0 }); 
    //const [ fullScreen , setFullScreen ] = useState(extraPanel.isFullScreen)

    //const handleDragMouseUp = () => {
    //    //if (fullScreen) {
    //    //    SetFullScreenExtraPanel(extraPanel, true)
    //    //} else {
    //    //    SetPanelPosition(extraPanel, position);
    //    //    extraPanel.isFullScreen && SetFullScreenExtraPanel(extraPanel, false)
    //    //}
    //    SetPanelPosition(extraPanel, { x: extraPanel.panelLocation.x + translate.x, y: extraPanel.panelLocation.y + translate.y });
    //    setTranslate({ x: 0, y: 0 })
    //    //extraPanel.isFullScreen && SetFullScreenExtraPanel(extraPanel, false)
    //}

    const getTranslate = ({ x, y, startX, startY }: BetterDragEventData) : Number2 => {
        x = Math.min(Math.max(x, 0), window.innerWidth);
        y = Math.min(Math.max(y, 0), window.innerHeight);

        x = x - startX - extraPanel.panelLocation.x;
        y = y - startY - extraPanel.panelLocation.y;
        return {x:x, y:y}
    }

    const onDragStart = (b: BetterDragEventData): boolean => {
        return !extraPanel.isFullScreen
    }

    const onDrag = (b: BetterDragEventData) => {
        extraPanel.isFullScreen && SetFullScreenExtraPanel(extraPanel, false)
        const { x, y } = getTranslate(b);
        setTranslate({ x: x, y: y })
    }

    const onDragEnd = (b: BetterDragEventData) => {
        const { x, y } = getTranslate(b);

        SetPanelPosition(extraPanel, { x: extraPanel.panelLocation.x + x, y: extraPanel.panelLocation.y + y });
        setTranslate({ x: 0, y: 0 })
    }

    return <Panel
        header={BetterDragHandle({ onDragStart: onDragStart, onDrag: onDrag, onDragEnd: onDragEnd, children: ExtraPanelHeader({ extraPanel }) })} //, onMouseUp:handleDragMouseUp 
        footer={"FOOTER"}
        className={classNames("draggable-panel", ExtraPanelSCSS.ExtraPanel, extraPanel.isFullScreen && ExtraPanelSCSS.FullScreen, !extraPanel.isExpanded && ExtraPanelSCSS.Collapsed ) }
        style={
            {
                // left: `${extraPanel.isFullScreen ? 0 : position.x}px`,
                // top: `${extraPanel.isFullScreen ? 0 : position.y}px`,
                left: `${extraPanel.isFullScreen ? 0 : extraPanel.panelLocation.x}px`,
                top: `${extraPanel.isFullScreen ? 0 : extraPanel.panelLocation.y}px`,
                visibility: extraPanel.visible ? "visible" : "hidden",
                transform: `translate(${translate.x}px, ${translate.y}px)`
            }}
    >
        {extraPanel.isExpanded && children}
        
    </Panel>
}
