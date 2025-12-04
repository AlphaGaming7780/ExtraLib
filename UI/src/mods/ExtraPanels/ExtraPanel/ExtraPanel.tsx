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

    const [translate, setTranslate] = useState({ x: 0, y: 0 }); 

    const getTranslate = (
        { x, y, startX, startY }: BetterDragEventData
    ): Number2 => {

        let transX = x - startX;
        let transY = y - startY;

        let finalPanelX = extraPanel.panelLocation.x + transX;
        let finalPanelY = extraPanel.panelLocation.y + transY;

        var clampedPanelPosx = Math.min(Math.max(finalPanelX, 0), window.innerWidth);
        var clampedPanelPosy = Math.min(Math.max(finalPanelY, 0), window.innerHeight);

        const translateX = clampedPanelPosx - extraPanel.panelLocation.x;
        const translateY = clampedPanelPosy - extraPanel.panelLocation.y;

        return { x: translateX, y: translateY };
    };


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

        extraPanel.panelLocation = { x: extraPanel.panelLocation.x + x, y: extraPanel.panelLocation.y + y }
        setTranslate({ x: 0.0, y: 0.0 })
        SetPanelPosition(extraPanel, extraPanel.panelLocation);
    }

    return <Panel
        header={BetterDragHandle({ onDragStart: onDragStart, onDrag: onDrag, onDragEnd: onDragEnd, children: ExtraPanelHeader({ extraPanel }) })}
        footer={"FOOTER"}
        className={classNames("draggable-panel", ExtraPanelSCSS.ExtraPanel, extraPanel.isFullScreen && ExtraPanelSCSS.FullScreen, !extraPanel.isExpanded && ExtraPanelSCSS.Collapsed ) }
        style={
            {
                left: `${extraPanel.isFullScreen ? 0 : extraPanel.panelLocation.x}px`,
                top: `${extraPanel.isFullScreen ? 0 : extraPanel.panelLocation.y}px`,
                visibility: extraPanel.visible ? "visible" : "hidden",
                transform: `translate(${translate.x}px, ${translate.y}px)`
            }}
    >
        {extraPanel.isExpanded && children}
        
    </Panel>
}
